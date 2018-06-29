using System;
using System.Collections.Generic;
using System.Text;

namespace ExceptionInterceptor.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class SourceFile
    {
        #region Variables
        /// <summary>
        /// 
        /// </summary>
        private string _sourceFileName;

        /// <summary>
        /// 
        /// </summary>
        private string _sourceClassName;

        /// <summary>
        /// Can be Class / Interface / Delegate
        /// </summary>
        private string _sourceFileType;
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public SourceFile()
        {
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceFileName"></param>
        /// <param name="sourceClassName"></param>
        public SourceFile(string sourceFileName, string sourceClassName, string sourceFileType)
        {
            _sourceFileName = sourceFileName;
            _sourceClassName = sourceClassName;
            _sourceFileType = sourceFileType;
        }        
        #endregion

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public string SourceFileName
        {
            get
            {
                return (_sourceFileName);
            }
            set
            {
                _sourceFileName = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SourceClassName
        {
            get
            {
                return (_sourceClassName);
            }
            set
            {
                _sourceClassName = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SourceFileType
        {
            get
            {
                return (_sourceFileType);
            }
            set
            {
                _sourceFileType = value;
            }
        }
        #endregion

        #region Public Methods
        
        #endregion

        #region Private Methods
        
        #endregion
    }
}
