using System;
using System.Collections.Generic;
using System.Text;

using EnvDTE;
using EnvDTE80;

using VSLangProj;
using VSLangProj2;
using VSLangProj80;

namespace ExceptionInterceptor.Parser
{
    /// <summary>
    /// 
    /// </summary>
    public class SolutionFileOpener : ExceptionInterceptor.Parser.IVSFileOpener
    {
        #region Variables
        /// <summary>
        /// 
        /// </summary>
        private DTE2 _dte2;

        /// <summary>
        /// 
        /// </summary>
        private ExceptionInterceptor.Common.FileContext _fileContext;
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public SolutionFileOpener(ExceptionInterceptor.Common.FileContext fileContext, DTE2 dte2)
        {
            _dte2 = dte2;
            _fileContext = fileContext;
        }
        #endregion

        #region Properties

        #endregion

        #region Public Methods

        #endregion

        #region Private Methods

        #endregion

        #region IVSFileOpener Members
        /// <summary>
        /// 
        /// </summary>
        /// <param name="solutionFileName"></param>
        public void Open(string[] solutionFileName)
        {
            try
            {
                _dte2.Solution.Open(solutionFileName[0]);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        #endregion
    }
}
