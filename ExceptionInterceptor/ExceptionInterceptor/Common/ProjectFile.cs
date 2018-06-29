using System;
using System.Collections.Generic;
using System.Text;

namespace ExceptionInterceptor.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class ProjectFile
    {
        #region Variables
        /// <summary>
        /// 
        /// </summary>
        private string _projectFileName = null;

        /// <summary>
        /// 
        /// </summary>
        private string _projectFilePath = null;
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public ProjectFile()
        {

        }
        #endregion

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public string ProjectFileName
        {
            get
            {
                return (_projectFileName);
            }
            set
            {
                _projectFileName = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ProjectFilePath
        {
            get
            {
                return (_projectFilePath);
            }
            set
            {
                _projectFilePath = value;
            }
        }


        #endregion

        #region Public Methods
        
        #endregion

        #region Private Methods
        
        #endregion
    }
}
