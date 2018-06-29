using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

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
    public class VSSourceFileParser : Parser
    {
        #region Variables

        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public VSSourceFileParser()
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
        protected override void OpenSolutionFile(string[] sourceFileName)
        {
            _IVSFileOpener.Open(sourceFileName);
        }
        #endregion
    }
}
