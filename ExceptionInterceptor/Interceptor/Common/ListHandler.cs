using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Xml;

namespace Interceptor.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class ListHandler : IConfigurationSectionHandler
    {
        #region Variables
        
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public ListHandler()
        {

        }
        #endregion

        #region Properties

        #endregion

        #region Public Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="configContext"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        object IConfigurationSectionHandler.Create(object parent, object configContext, XmlNode section)
        {
            System.Collections.Generic.IList<string> iList = new System.Collections.Generic.List<string>();

            // Gets the child element names and attributes.
            foreach (XmlNode child in section.ChildNodes)
            {
                foreach (XmlAttribute childAttrib in child.Attributes)
                {
                    if (XmlNodeType.Attribute == childAttrib.NodeType)
                    {
                        iList.Add(childAttrib.Value);
                    }
                }
            }

            return (iList);
        }
        #endregion

        #region Private Methods

        #endregion
    }
}
