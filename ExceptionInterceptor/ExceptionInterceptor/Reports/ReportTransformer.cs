using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Xsl;

namespace ExceptionInterceptor.Reports
{
    /// <summary>
    /// 
    /// </summary>
    public class ReportTransformer
    {
        #region Variables
        /// <summary>
        /// 
        /// </summary>
        private string xmlOutput = null;

        /// <summary>
        /// 
        /// </summary>
        private string htmlOutput = null;

        /// <summary>
        /// 
        /// </summary>
        private string xsltTransformer = null;        
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public ReportTransformer()
        {
            SetXMLOutput();

            SetHTMLOutput();

            SetXSLTTransformer();
        }
        #endregion

        #region Properties
        
        #endregion

        #region Public Methods
        /// <summary>
        /// This method transforms the XML output to HTML output
        /// </summary>
        public void TransformHTMLReport()
        {
            using (FileStream stream = File.Open(htmlOutput, FileMode.Create))
            {
                //Create XsltCommand and compile stylesheet.
                XslCompiledTransform processor = new XslCompiledTransform();

                processor.Load(xsltTransformer);

                //Transform the file.
                processor.Transform(xmlOutput, null, stream);
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// 
        /// </summary>
        private void SetXMLOutput()
        {
            xmlOutput = ExceptionInterceptor.Common.ExceptionConfigurator.GetXMLOutputPath();
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetHTMLOutput()
        {
            htmlOutput = ExceptionInterceptor.Common.ExceptionConfigurator.GetHTMLOutputPath();
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetXSLTTransformer()
        {
            xsltTransformer = ExceptionInterceptor.Common.ExceptionConfigurator.GetXSLTPath();
        }
        #endregion
    }
}
