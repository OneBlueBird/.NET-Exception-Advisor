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
    public class VSProjectParser : Parser
    {
        #region Variables

        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public VSProjectParser()
        {

        }
        #endregion

        #region Properties

        #endregion

        #region Public Methods

        #endregion

        #region Private Methods

        #endregion

        #region Protected Methods
        /// <summary>
        /// This method open a soltion file and its related projects for processing
        /// </summary>
        protected override void OpenSolutionFile(string[] projectFileName)
        {
            _IVSFileOpener.Open(projectFileName);
        }        
        #endregion
    }
}
