using System;
using System.Collections.Generic;
using System.Configuration;
using ExceptionInterceptor.Properties;
using System.Text;

namespace ExceptionInterceptor.Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExceptionConfigurator
    {
        #region Variables
        
        #endregion

        #region Constructor
              
        #endregion

        #region Properties
        
        #endregion

        #region Public Methods
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetXMLOutputPath()
        {
            return (Settings.Default.XMLOutputPath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetHTMLOutputPath()
        {
            return (Settings.Default.HTMLOutputPath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetXSLTPath()
        {
            return (Settings.Default.XSLTPath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetExcelOutputPath()
        {
            return (Settings.Default.ExcelOutputPath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetTemporaryProjectPath()
        {
            return (Settings.Default.TempProjectPath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetCSEmptyProject()
        {
            return (Settings.Default.CSEmptyProject);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetCSEmptyProjectType()
        {
            return (Settings.Default.CSEmptyProjectType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetVBEmptyProject()
        {
            return (Settings.Default.VBEmptyProject);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetVBEmptyProjectType()
        {
            return (Settings.Default.VBEmptyProjectType);
        }
        #endregion

        #region Private Methods

        #endregion
    }
}
