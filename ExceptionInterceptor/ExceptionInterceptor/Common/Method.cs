using System;
using System.Collections.Generic;
using System.Text;

namespace ExceptionInterceptor.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class Method
    {
        #region Variables
        /// <summary>
        /// 
        /// </summary>
        private string _methodName;

        /// <summary>
        /// 
        /// </summary>
        private string _methodBeginLineNumber;

        /// <summary>
        /// 
        /// </summary>
        private string _methodEndLineNumber;
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public Method()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="methodBeginLineNumber"></param>
        public Method(string methodName, string methodBeginLineNumber, string methodEndLineNumber)
        {
            _methodName = methodName;
            _methodBeginLineNumber = methodBeginLineNumber;
            _methodEndLineNumber = methodEndLineNumber;
        }
        #endregion

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public string MethodName
        {
            get
            {
                return (_methodName);
            }
            set
            {
                _methodName = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string MethodBeginLineNumber
        {
            get
            {
                return (_methodBeginLineNumber);
            }
            set
            {
                _methodBeginLineNumber = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string MethodEndLineNumber
        {
            get
            {
                return (_methodEndLineNumber);
            }
            set
            {
                _methodEndLineNumber = value;
            }
        }
        #endregion

        #region Public Methods

        #endregion

        #region Private Methods

        #endregion
    }
}
