using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using EnvDTE;
using EnvDTE80;
using Extensibility;

using VSLangProj;
using VSLangProj2;
using VSLangProj80;

namespace ExceptionInterceptor.Parser
{
    /// <summary>
    /// 
    /// </summary>
    public class VSSolutionParser : Parser
    {
        #region Variables

        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public VSSolutionParser()
        {
            
        }
        #endregion

        #region Properties

        #endregion

        #region Public Methods
        /// <summary>
        /// 
        /// </summary>
        public void GetProjects()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public void GetSourceFiles()
        {

        }
        #endregion

        #region Private Methods

        #endregion

        #region Protected Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        protected override void OpenSolutionFile(string[] solutionFileName)
        {
            _IVSFileOpener.Open(solutionFileName);
        }        
        #endregion
    }
}
