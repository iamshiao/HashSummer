namespace CircleHsiao.HashSummer.GUI
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.Status = new System.Windows.Forms.DataGridViewImageColumn();
            this.FileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Hash = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Caption = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.folderSelector = new System.Windows.Forms.FolderBrowserDialog();
            this.cmbxHashType = new System.Windows.Forms.ComboBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.description = new System.Windows.Forms.Label();
            this.fileSaver = new System.Windows.Forms.SaveFileDialog();
            this.hint = new System.Windows.Forms.ToolTip(this.components);
            this.btnChecksum = new System.Windows.Forms.Button();
            this.btnCreateHashFile = new System.Windows.Forms.Button();
            this.fileSelector = new System.Windows.Forms.OpenFileDialog();
            this.summary = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Status,
            this.FileName,
            this.Hash,
            this.Caption});
            this.dataGridView.Location = new System.Drawing.Point(12, 48);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.RowTemplate.Height = 24;
            this.dataGridView.Size = new System.Drawing.Size(985, 255);
            this.dataGridView.TabIndex = 0;
            this.dataGridView.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dataGridView_RowsAdded);
            // 
            // Status
            // 
            this.Status.DataPropertyName = "Status";
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            this.Status.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Status.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Status.Width = 57;
            // 
            // FileName
            // 
            this.FileName.DataPropertyName = "FileName";
            this.FileName.HeaderText = "FileName";
            this.FileName.Name = "FileName";
            this.FileName.ReadOnly = true;
            this.FileName.Width = 74;
            // 
            // Hash
            // 
            this.Hash.DataPropertyName = "Hash";
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Hash.DefaultCellStyle = dataGridViewCellStyle1;
            this.Hash.HeaderText = "Hash";
            this.Hash.Name = "Hash";
            this.Hash.ReadOnly = true;
            this.Hash.Width = 53;
            // 
            // Caption
            // 
            this.Caption.HeaderText = "Caption";
            this.Caption.Name = "Caption";
            this.Caption.ReadOnly = true;
            this.Caption.Width = 67;
            // 
            // cmbxHashType
            // 
            this.cmbxHashType.Enabled = false;
            this.cmbxHashType.FormattingEnabled = true;
            this.cmbxHashType.Items.AddRange(new object[] {
            "SHA256"});
            this.cmbxHashType.Location = new System.Drawing.Point(12, 14);
            this.cmbxHashType.Name = "cmbxHashType";
            this.cmbxHashType.Size = new System.Drawing.Size(121, 20);
            this.cmbxHashType.TabIndex = 2;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(481, 311);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(480, 23);
            this.progressBar.Step = 1;
            this.progressBar.TabIndex = 3;
            // 
            // description
            // 
            this.description.AutoSize = true;
            this.description.Location = new System.Drawing.Point(967, 317);
            this.description.Name = "description";
            this.description.Size = new System.Drawing.Size(20, 12);
            this.description.TabIndex = 4;
            this.description.Text = "0%";
            // 
            // fileSaver
            // 
            this.fileSaver.Filter = "SHA256|*.sha256|MD5|*.md5";
            this.fileSaver.RestoreDirectory = true;
            this.fileSaver.Title = "Save to";
            // 
            // btnChecksum
            // 
            this.btnChecksum.BackgroundImage = global::CircleHsiao.HashSummer.Properties.Resources.compare;
            this.btnChecksum.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnChecksum.Location = new System.Drawing.Point(193, 5);
            this.btnChecksum.Name = "btnChecksum";
            this.btnChecksum.Size = new System.Drawing.Size(45, 37);
            this.btnChecksum.TabIndex = 5;
            this.hint.SetToolTip(this.btnChecksum, "Checksum the hash file with files in the path it belongs");
            this.btnChecksum.UseVisualStyleBackColor = true;
            this.btnChecksum.Click += new System.EventHandler(this.btnChecksum_Click);
            // 
            // btnCreateHashFile
            // 
            this.btnCreateHashFile.BackgroundImage = global::CircleHsiao.HashSummer.Properties.Resources.file;
            this.btnCreateHashFile.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnCreateHashFile.Location = new System.Drawing.Point(139, 5);
            this.btnCreateHashFile.Name = "btnCreateHashFile";
            this.btnCreateHashFile.Size = new System.Drawing.Size(48, 37);
            this.btnCreateHashFile.TabIndex = 1;
            this.hint.SetToolTip(this.btnCreateHashFile, "Create a hash file from files in selected path recursively");
            this.btnCreateHashFile.UseVisualStyleBackColor = true;
            this.btnCreateHashFile.Click += new System.EventHandler(this.btnCreateHashFile_Click);
            // 
            // fileSelector
            // 
            this.fileSelector.Filter = "SHA256 files (*.sha256)|*.sha256";
            // 
            // summary
            // 
            this.summary.AutoSize = true;
            this.summary.Location = new System.Drawing.Point(12, 317);
            this.summary.Name = "summary";
            this.summary.Size = new System.Drawing.Size(0, 12);
            this.summary.TabIndex = 6;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 346);
            this.Controls.Add(this.summary);
            this.Controls.Add(this.btnChecksum);
            this.Controls.Add(this.description);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.cmbxHashType);
            this.Controls.Add(this.btnCreateHashFile);
            this.Controls.Add(this.dataGridView);
            this.Font = new System.Drawing.Font("PMingLiU", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.Text = "HashSummer";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Button btnCreateHashFile;
        private System.Windows.Forms.FolderBrowserDialog folderSelector;
        private System.Windows.Forms.ComboBox cmbxHashType;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label description;
        private System.Windows.Forms.SaveFileDialog fileSaver;
        private System.Windows.Forms.Button btnChecksum;
        private System.Windows.Forms.ToolTip hint;
        private System.Windows.Forms.OpenFileDialog fileSelector;
        private System.Windows.Forms.DataGridViewImageColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Hash;
        private System.Windows.Forms.DataGridViewTextBoxColumn Caption;
        private System.Windows.Forms.Label summary;
    }
}

