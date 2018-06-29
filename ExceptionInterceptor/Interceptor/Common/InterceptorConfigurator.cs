using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace Interceptor.Common
{
    public class InterceptorConfigurator
    {
        #region Variables

        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public InterceptorConfigurator()
        {

        }
        #endregion

        #region Properties

        #endregion

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetDotNetversion()
        {
            return (ConfigurationManager.AppSettings["DotNetVersion"]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IList<string> GetBCLAssemblies()
        {
            IList<string> config = (IList<string>)System.Configuration.ConfigurationManager.GetSection("BCLGroup/BCLTypes");
            return (config);
        }
        #endregion

        #region Private Methods

        #endregion
    }
}
