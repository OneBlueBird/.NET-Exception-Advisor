using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ExceptionInterceptor.UI
{
    public partial class DASolutionForm : Form
    {
        /// <summary>
        /// 
        /// </summary>
        private ExceptionInterceptor.Common.ProcessSolution _processSolution;

        /// <summary>
        /// This will store the current file context like Solution file / Project file / Source file(s)
        /// </summary>
        private ExceptionInterceptor.Common.FileContext _fileContext = null;

        /// <summary>
        /// This will store the list of Source file(s) that were selected from the UI
        /// </summary>
        private string[] fileNames = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connect"></param>
        public DASolutionForm(ExceptionInterceptor.Common.ProcessSolution processSolution)
        {
            _processSolution = processSolution;

            InitializeComponent();
        }

        /// <summary>
        /// This method captures Solution file through Open File Dialog.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowseSolution_Click(object sender, EventArgs e)
        {
            DAopenFileDialog.ShowDialog();
        }

        /// <summary>
        /// This method captures the Solution file name and displayes it in txtSolutonFile textbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DAopenFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            txtSolutionFile.Text = string.Empty;
            lstFiles.Items.Clear();

            fileNames = DAopenFileDialog.FileNames;

            if (fileNames.Length > 1)
            {
                for (int filesCount = 0; filesCount < fileNames.Length; filesCount++)
                {
                    CheckFileType(fileNames[filesCount]);
                    lstFiles.Items.Add(fileNames[filesCount]);
                }
            }
            else if (fileNames.Length == 1)
            {
                CheckFileType(fileNames[0]);
                txtSolutionFile.Text = fileNames[0];
            }
        }

        /// <summary>
        /// This is a delegate that will push the call to ParserFactory once the User clicks Process button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnProcessSolution_Click(object sender, EventArgs e)
        {
            if (txtSolutionFile.Text != string.Empty || lstFiles.Items.Count != 0)
            {
                _processSolution(_fileContext, fileNames);
                this.Close();
            }
            else
            {
                MessageBox.Show("Choose a Solution / Project / Source File(s)");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private void CheckFileType(string fileName)
        {
            if (Path.GetFileName(fileName).IndexOf(".sln") != -1)
            {
                _fileContext = new ExceptionInterceptor.Common.FileContext(ExceptionInterceptor.Common.FileType.VSSoln);
            }
            else if (Path.GetFileName(fileName).IndexOf(".csproj") != -1)
            {
                _fileContext = new ExceptionInterceptor.Common.FileContext(ExceptionInterceptor.Common.FileType.CSProj);
            }
            else if(Path.GetFileName(fileName).IndexOf(".vbproj") != -1)
            {
                _fileContext = new ExceptionInterceptor.Common.FileContext(ExceptionInterceptor.Common.FileType.VBFile);
            }
            else if (Path.GetFileName(fileName).IndexOf(".cs") != -1)
            {
                _fileContext = new ExceptionInterceptor.Common.FileContext(ExceptionInterceptor.Common.FileType.CSFile);
            }
            else if (Path.GetFileName(fileName).IndexOf(".vb") != -1)
            {
                _fileContext = new ExceptionInterceptor.Common.FileContext(ExceptionInterceptor.Common.FileType.VBFile);
            }
        }
    }
}