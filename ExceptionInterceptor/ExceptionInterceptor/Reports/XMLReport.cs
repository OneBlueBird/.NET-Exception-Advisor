using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

using System.Text;
using System.Xml;
using System.Windows.Forms;


namespace ExceptionInterceptor.Reports
{
    /// <summary>
    /// 
    /// </summary>
    public class XMLReport
    {
        #region Variables
        /// <summary>
        /// 
        /// </summary>
        private string _xmlOutput = null;

        /// <summary>
        /// 
        /// </summary>
        private XmlTextWriter _xmlTextWriter = null;
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public XMLReport()
        {
            SetXMLOutput();
        }
        #endregion

        #region Properties
        
        #endregion

        #region Public Methods
        /// <summary>
        /// 
        /// </summary>
        public void CreateXMLDocumentTag()
        {
            _xmlTextWriter = new XmlTextWriter(_xmlOutput, System.Text.Encoding.UTF8);
            _xmlTextWriter.WriteStartDocument();
        }

        /// <summary>
        /// 
        /// </summary>
        public void CreateXMLRootTag()
        {
            _xmlTextWriter.WriteStartElement("DAExceptionCodeAudit");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementName"></param>
        public void CreateStartElement(string elementName)
        {
            try
            {
                _xmlTextWriter.WriteStartElement(elementName);
            }
            catch (System.Xml.XmlException ex)
            {
                MessageBox.Show("Element Name : " + elementName + ", Exception : " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementName"></param>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        public void CreateStartElement(string elementName, string attributeName, string attributeValue)
        {
            try
            {
                _xmlTextWriter.WriteStartElement(elementName);
                _xmlTextWriter.WriteAttributeString(attributeName, attributeValue);
            }
            catch (System.Xml.XmlException ex)
            {
                MessageBox.Show("Element Name : " + elementName + ", Attribute Name : " + attributeName +
                                "Exception : " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementName"></param>
        public void CreateStartElement(string elementName, string elementInnerText)
        {
            try
            {
                _xmlTextWriter.WriteStartElement(elementName);
                _xmlTextWriter.WriteString(elementInnerText);
            }
            catch (System.Xml.XmlException ex)
            {
                MessageBox.Show("Element Name : " + elementName + ", Element Inner Text : " + elementInnerText +
                                "Exception : " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementName"></param>
        /// <param name="attribute"></param>
        public void CreateStartElement(string elementName, NameValueCollection attribute)
        {
            string tempStr = null;
            try
            {
                _xmlTextWriter.WriteStartElement(elementName);

                IEnumerator enumerator = attribute.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    tempStr = tempStr + enumerator.Current.ToString() + "-->" + attribute[enumerator.Current.ToString()];
                    _xmlTextWriter.WriteAttributeString(enumerator.Current.ToString(),
                                                       attribute[enumerator.Current.ToString()]);
                }
            }
            catch (System.Xml.XmlException ex)
            {
                MessageBox.Show("Element Name : " + elementName + "Exception : " + ex.Message + "tempStr : " + tempStr);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void CreateEndElement()
        {
            _xmlTextWriter.WriteEndElement();
        }

        /// <summary>
        /// 
        /// </summary>
        public void FlushXMLContents()
        {
            _xmlTextWriter.Flush();
            _xmlTextWriter.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        public void GenerateReport()
        {
            ReportTransformer reportTransformer = new ReportTransformer();
            reportTransformer.TransformHTMLReport();
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// 
        /// </summary>
        private void SetXMLOutput()
        {
            _xmlOutput = ExceptionInterceptor.Common.ExceptionConfigurator.GetXMLOutputPath();
        }
        #endregion
    }
}
