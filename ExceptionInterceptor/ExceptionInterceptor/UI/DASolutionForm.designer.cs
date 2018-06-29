namespace ExceptionInterceptor.UI
{
    partial class DASolutionForm
    {
        private System.Windows.Forms.OpenFileDialog DAopenFileDialog;
        private System.Windows.Forms.TextBox txtSolutionFile;
        private System.Windows.Forms.Button btnBrowseSolution;
        private System.Windows.Forms.Button btnProcessSolution;
        private System.Windows.Forms.Label lblDATitle;

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
            this.DAopenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.txtSolutionFile = new System.Windows.Forms.TextBox();
            this.btnBrowseSolution = new System.Windows.Forms.Button();
            this.btnProcessSolution = new System.Windows.Forms.Button();
            this.lblDATitle = new System.Windows.Forms.Label();
            this.lblSolution = new System.Windows.Forms.Label();
            this.lstFiles = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // DAopenFileDialog
            // 
            this.DAopenFileDialog.Multiselect = true;
            this.DAopenFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.DAopenFileDialog_FileOk);
            // 
            // txtSolutionFile
            // 
            this.txtSolutionFile.Location = new System.Drawing.Point(12, 46);
            this.txtSolutionFile.Name = "txtSolutionFile";
            this.txtSolutionFile.Size = new System.Drawing.Size(401, 20);
            this.txtSolutionFile.TabIndex = 0;
            // 
            // btnBrowseSolution
            // 
            this.btnBrowseSolution.Location = new System.Drawing.Point(419, 46);
            this.btnBrowseSolution.Name = "btnBrowseSolution";
            this.btnBrowseSolution.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseSolution.TabIndex = 1;
            this.btnBrowseSolution.Text = "Browse...";
            this.btnBrowseSolution.UseVisualStyleBackColor = true;
            this.btnBrowseSolution.Click += new System.EventHandler(this.btnBrowseSolution_Click);
            // 
            // btnProcessSolution
            // 
            this.btnProcessSolution.Location = new System.Drawing.Point(419, 75);
            this.btnProcessSolution.Name = "btnProcessSolution";
            this.btnProcessSolution.Size = new System.Drawing.Size(75, 23);
            this.btnProcessSolution.TabIndex = 2;
            this.btnProcessSolution.Text = "Process";
            this.btnProcessSolution.UseVisualStyleBackColor = true;
            this.btnProcessSolution.Click += new System.EventHandler(this.btnProcessSolution_Click);
            // 
            // lblDATitle
            // 
            this.lblDATitle.AutoSize = true;
            this.lblDATitle.Location = new System.Drawing.Point(16, 7);
            this.lblDATitle.Name = "lblDATitle";
            this.lblDATitle.Size = new System.Drawing.Size(154, 13);
            this.lblDATitle.TabIndex = 3;
            this.lblDATitle.Text = "Exception Management Profiler";
            // 
            // lblSolution
            // 
            this.lblSolution.AutoSize = true;
            this.lblSolution.Location = new System.Drawing.Point(16, 27);
            this.lblSolution.Name = "lblSolution";
            this.lblSolution.Size = new System.Drawing.Size(121, 13);
            this.lblSolution.TabIndex = 4;
            this.lblSolution.Text = "Choose a .NET Solution";
            // 
            // lstFiles
            // 
            this.lstFiles.FormattingEnabled = true;
            this.lstFiles.Location = new System.Drawing.Point(12, 73);
            this.lstFiles.Name = "lstFiles";
            this.lstFiles.ScrollAlwaysVisible = true;
            this.lstFiles.Size = new System.Drawing.Size(400, 30);
            this.lstFiles.TabIndex = 5;
            // 
            // DASolutionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 114);
            this.Controls.Add(this.lstFiles);
            this.Controls.Add(this.lblSolution);
            this.Controls.Add(this.lblDATitle);
            this.Controls.Add(this.btnProcessSolution);
            this.Controls.Add(this.btnBrowseSolution);
            this.Controls.Add(this.txtSolutionFile);
            this.Name = "DASolutionForm";
            this.Text = "Exception Management Profiler";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Label lblSolution;
        private System.Windows.Forms.ListBox lstFiles;
    }
}