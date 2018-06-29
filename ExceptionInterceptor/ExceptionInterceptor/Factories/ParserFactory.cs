using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using EnvDTE;
using EnvDTE80;

using VSLangProj;
using VSLangProj2;
using VSLangProj80;

namespace ExceptionInterceptor.Factories
{
    /// <summary>
    /// This class will help to instantiate the appropriate factory class at runtime based on the type of
    /// file being processed.
    /// </summary>
    public class ParserFactory
    {
        #region Variables
        /// <summary>
        /// 
        /// </summary>
        private DTE2 _dte2;
        #endregion

        #region Constructor
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ParserFactory(DTE2 dte2)
        {
            _dte2 = dte2;
        }
        #endregion

        #region Properties

        #endregion

        #region Public Methods
        /// <summary>
        /// 
        /// </summary>
        public void LoadParser(ExceptionInterceptor.Common.FileContext fileContext, 
                               string[] fileName)
        {
            ExceptionInterceptor.Parser.Parser parser = null;

            try
            {
                if (fileContext.CurrentFileType == ExceptionInterceptor.Common.FileType.VSSoln)
                {
                    parser = new ExceptionInterceptor.Parser.VSSolutionParser();
                    parser.SetVSFileOpener(new ExceptionInterceptor.Parser.SolutionFileOpener(fileContext, _dte2));

                    parser.Initialize(_dte2, fileName);
                }
                else if (fileContext.CurrentFileType == ExceptionInterceptor.Common.FileType.CSFile)
                {
                    parser = new ExceptionInterceptor.Parser.VSSourceFileParser();
                    parser.SetVSFileOpener(new ExceptionInterceptor.Parser.SourceFileOpener(fileContext, _dte2));

                    parser.Initialize(_dte2, fileName);
                }
                else if (fileContext.CurrentFileType == ExceptionInterceptor.Common.FileType.VBFile)
                {
                    MessageBox.Show("Start processing VB Files");
                }
                else if (fileContext.CurrentFileType == ExceptionInterceptor.Common.FileType.CSProj)
                {
                    parser = new ExceptionInterceptor.Parser.VSProjectParser();
                    parser.SetVSFileOpener(new ExceptionInterceptor.Parser.ProjectFileOpener(fileContext, _dte2));

                    parser.Initialize(_dte2, fileName);
                }
                else if (fileContext.CurrentFileType == ExceptionInterceptor.Common.FileType.VBProj)
                {
                    MessageBox.Show("Start processing VB Project");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Private Methods

        #endregion
    }
}
