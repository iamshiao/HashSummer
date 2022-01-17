using CircleHsiao.HashSummer.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Forms;
using CircleHsiao.HashSummer.Extension;
using System.Runtime.InteropServices;

namespace CircleHsiao.HashSummer.GUI
{
    public partial class FormMain : Form
    {
        public FormMain(string[] args)
        {
            InitializeComponent();

            if (args != null && args.Any() && !string.IsNullOrEmpty(args[0]))
            {
                Checksum(args[0]);
            }
            else
            {
                hashType.SelectedIndex = 0;
            }

            if (!string.IsNullOrEmpty(Settings.Default.DefaultPath))
            {
                textBox_selectedPath.Text = Settings.Default.DefaultPath;
            }
            else
            {
                SendMessage(textBox_selectedPath.Handle, 0x1501, 1, @"C:\Users\UserName\FolderName");
            }
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

        private void SetHashType(string hashFilePath)
        {
            var extension = Path.GetExtension(hashFilePath);
            if (string.Equals(extension, ".sha256", StringComparison.OrdinalIgnoreCase))
            {
                hashType.SelectedIndex = hashType.FindStringExact("SHA256");
            }
            else if (string.Equals(extension, ".md5", StringComparison.OrdinalIgnoreCase))
            {
                hashType.SelectedIndex = hashType.FindStringExact("MD5");
            }
        }

        private string GetHashType()
        {
            return (string)this.Invoke(new Func<string>(() =>
            {
                return hashType.SelectedItem.ToString();
            }));
        }

        private void Checksum(string hashFilePath)
        {
            SetHashType(hashFilePath);

            var lines = File.ReadAllLines(hashFilePath);
            var locatedPath = Path.GetDirectoryName(hashFilePath);

            Task.Run(() => Vertify(locatedPath, lines)).ContinueWith((antecedent) => Summarize());
        }

        private void Vertify(string locatedPath, string[] lines)
        {
            var checkList = lines.Select(line => new
            {
                FileName = line.Split(" *")[1],
                HashValue = line.Split(" *")[0]
            }).ToDictionary(pair => pair.FileName.Replace(@"\", "/"), pair => pair.HashValue);
            var hashExtensions = GetHashExtensions();
            var filesToCheck = Directory.GetFiles(locatedPath, "*.*", SearchOption.AllDirectories)
                .Where(fileName =>
                !hashExtensions.Any(ext => string.Equals(ext, Path.GetExtension(fileName), StringComparison.OrdinalIgnoreCase)))
                .Select(path => new
                {
                    FileName = MakeRelative(path, $"{locatedPath}\\"),
                    FilePath = path
                }).ToDictionary(pair => pair.FileName, pair => pair.FilePath);

            var knownFiles = checkList.Keys.Union(filesToCheck.Keys);
            Parallel.ForEach(knownFiles, fileName =>
            {
                string hash = "";
                string caption = "";
                Image status;
                try
                {
                    if (checkList.ContainsKey(fileName) && !filesToCheck.ContainsKey(fileName))
                    {
                        throw new Exception("The file is missing.");
                    }
                    else if (!checkList.ContainsKey(fileName) && filesToCheck.ContainsKey(fileName))
                    {
                        throw new Exception("The file isn't on the checklist.");
                    }

                    hash = GetHashString(filesToCheck[fileName]);
                    if (!string.Equals(hash, checkList[fileName], StringComparison.OrdinalIgnoreCase))
                    {
                        throw new Exception("Checksum value mismatched.");
                    }
                    else
                    {
                        status = Resources.green;
                    }
                }
                catch (Exception ex)
                {
                    status = Resources.red;
                    caption = ex.Message;
                }
                var row = new object[] { status, fileName, hash, caption };

                this.Invoke(new Action(() =>
                {
                    dataGridView.Rows.Add(row);
                    progressBar.Value = dataGridView.Rows.Count * 100 / knownFiles.Count();
                    description.Text = $"{progressBar.Value}%";
                }));
            });
        }

        private void btnCreateHashFile_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox_selectedPath.Text))
            {
                if (folderSelector.ShowDialog() == DialogResult.OK)
                {
                    textBox_selectedPath.Text = folderSelector.SelectedPath;

                    dataGridView.Rows.Clear();
                }
            }

            fileSaver.InitialDirectory = textBox_selectedPath.Text;
            fileSaver.FileName = Path.GetFileName(textBox_selectedPath.Text);
            Task.Run(() => LoadFilesToGridView(textBox_selectedPath.Text)).ContinueWith(
                (antecedent) =>
                {
                    Summarize();
                    SaveHashFile(antecedent.Result);
                });
        }

        private void Summarize()
        {
            this.Invoke(new Action(() =>
            {
                var rows = dataGridView.Rows.OfType<DataGridViewRow>();
                var errorCount = rows.Count(row => !string.IsNullOrEmpty(row.Cells["Caption"].Value.ToString()));
            }));
        }

        private void btnChecksum_Click(object sender, EventArgs e)
        {
            if (fileSelector.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(fileSelector.FileName))
            {
                dataGridView.Rows.Clear();

                Checksum(fileSelector.FileName);
            }
        }

        private void SaveHashFile(IEnumerable<string> outputLines)
        {
            this.Invoke(new Action(() =>
            {
                fileSaver.FilterIndex = hashType.SelectedIndex + 1;
                if (fileSaver.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(fileSaver.FileName))
                {
                    File.WriteAllLines(fileSaver.FileName, outputLines);
                }
            }));

            Settings.Default.DefaultPath = Path.GetDirectoryName(fileSaver.FileName);
            Settings.Default.Save();
        }

        private IEnumerable<string> GetHashExtensions()
        {
            return (IEnumerable<string>)this.Invoke(new Func<IEnumerable<string>>(() =>
            {
                return hashType.Items.Cast<string>().Select(item => $".{item}");
            }));
        }

        private IEnumerable<string> LoadFilesToGridView(string folderPath)
        {
            var hashExtensions = GetHashExtensions();
            var filePaths = Directory.GetFiles(folderPath, "*.*",
                SearchOption.AllDirectories).Where(fileName =>
                !hashExtensions.Any(ext => string.Equals(ext, Path.GetExtension(fileName), StringComparison.OrdinalIgnoreCase)));
            Dictionary<string, string> outputLines = new Dictionary<string, string>();

            Parallel.ForEach(filePaths, filePath =>
            {
                string hash = "";
                string fileName = "";
                string caption = "";
                Image status;
                try
                {
                    hash = GetHashString(filePath);
                    fileName = MakeRelative(filePath, $"{folderPath}\\");

                    if (outputLines.ContainsKey(hash))
                    {
                        throw new Exception("Duplicated file, will not be included.");
                    }

                    status = Resources.green;
                    outputLines[hash] = $"{hash} *{fileName}";
                }
                catch (Exception ex)
                {
                    status = Resources.red;
                    caption = ex.Message;
                }
                var row = new object[] { status, fileName, hash, caption };

                this.Invoke(new Action(() =>
                {
                    dataGridView.Rows.Add(row);
                    progressBar.Value = dataGridView.Rows.Count * 100 / filePaths.Count();
                    description.Text = $"{progressBar.Value}%";
                }));
            });

            return outputLines.Values;
        }

        private string MakeRelative(string filePath, string refPath)
        {
            var fileUri = new Uri(filePath);
            var refUri = new Uri(refPath);
            return refUri.MakeRelativeUri(fileUri).ToString();
        }

        private string GetHashString(string filePath)
        {
            using (FileStream stream = File.OpenRead(filePath))
            {
                var algorithm = GetHashAlgorithm();
                byte[] hash = algorithm.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", "");
            }
        }

        private void dataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            dataGridView.FirstDisplayedScrollingRowIndex = e.RowIndex;
        }

        private HashAlgorithm GetHashAlgorithm()
        {
            switch (GetHashType())
            {
                case "MD5":
                    return new MD5CryptoServiceProvider();
                case "SHA256":
                default:
                    return new SHA256CryptoServiceProvider();
            }
        }

        private void button_cleanPathCache_Click(object sender, EventArgs e)
        {
            Settings.Default.DefaultPath = "";
            textBox_selectedPath.Text = "";
        }
    }
}
