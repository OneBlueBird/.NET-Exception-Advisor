using System;
using System.Collections.Generic;
using System.Text;

namespace ExceptionInterceptor.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class FileContext
    {
        #region Variables
        /// <summary>
        /// 
        /// </summary>
        private FileType _fileType;
        #endregion

        #region Constructor
        /// <summary>
        /// Default Constructor
        /// </summary>
        public FileContext()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileType"></param>
        public FileContext(FileType fileType)
        {
            _fileType = fileType;
        }
        #endregion

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public FileType CurrentFileType
        {
            get
            {
                return (_fileType);
            }
            set
            {
                _fileType = value;
            }
        }
        #endregion

        #region Public Methods

        #endregion

        #region Private Methods

        #endregion
    }
}
