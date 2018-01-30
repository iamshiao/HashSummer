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

            cmbxHashType.SelectedIndex = 0;
        }

        private void Checksum(string hashFilePath)
        {
            var lines = File.ReadAllLines(hashFilePath);
            var locatedPath = Path.GetDirectoryName(hashFilePath);

            Task.Run(() => Vertify(locatedPath, lines));
        }

        private void Vertify(string locatedPath, string[] lines)
        {
            var checkList = lines.Select(line => new
            {
                FileName = line.Split(" *")[1],
                HashValue = line.Split(" *")[0]
            }).ToDictionary(pair => pair.FileName, pair => pair.HashValue);

            var filesToCheck = Directory.GetFiles(locatedPath, "*.*", SearchOption.AllDirectories)
                .Where(fileName => !fileName.EndsWith(".sha256"))
                .Select(path => new
                {
                    FileName = Path.GetFileName(path),
                    FilePath = path
                }).ToDictionary(pair => pair.FileName, pair => pair.FilePath);

            var knownFiles = checkList.Keys.Union(filesToCheck.Keys);
            foreach (var fileName in knownFiles)
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
                    if (hash != checkList[fileName])
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
            }
        }

        private void btnCreateHashFile_Click(object sender, EventArgs e)
        {
            if (folderSelector.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(folderSelector.SelectedPath))
            {
                dataGridView.Rows.Clear();

                fileSaver.InitialDirectory = folderSelector.SelectedPath;
                fileSaver.FileName = Path.GetFileName(folderSelector.SelectedPath);
                Task.Run(() => LoadFilesToGridView(folderSelector.SelectedPath)).ContinueWith(
                    (antecedent) => SaveHashFile(antecedent.Result));
            }
        }

        private void btnChecksum_Click(object sender, EventArgs e)
        {
            if (fileSelector.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(fileSelector.FileName))
            {
                dataGridView.Rows.Clear();

                Checksum(fileSelector.FileName);
            }
        }

        private void SaveHashFile(List<string> linesToHashFile)
        {
            this.Invoke(new Action(() =>
            {
                if (fileSaver.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(fileSaver.FileName))
                {
                    File.WriteAllLines(fileSaver.FileName, linesToHashFile);
                }
            }));
        }

        private List<string> LoadFilesToGridView(string folderPath)
        {
            var filePaths = Directory.GetFiles(folderPath, "*.*",
                SearchOption.AllDirectories).Where(fileName => !fileName.EndsWith(".sha256"));
            List<string> linesToHashFile = new List<string>();

            foreach (var filePath in filePaths)
            {
                string hash = "";
                string fileName = "";
                string caption = "";
                Image status;
                try
                {
                    fileName = Path.GetFileName(filePath);
                    hash = GetHashString(filePath);
                    status = Resources.green;
                    linesToHashFile.Add($"{hash} *{fileName}");
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
            }

            return linesToHashFile;
        }

        private string GetHashString(string filePath)
        {
            using (FileStream stream = File.OpenRead(filePath))
            {
                SHA256 sha256 = new SHA256CryptoServiceProvider();
                byte[] hash = sha256.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", "");
            }
        }

        private void dataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            dataGridView.FirstDisplayedScrollingRowIndex = e.RowIndex;
        }
    }
}
