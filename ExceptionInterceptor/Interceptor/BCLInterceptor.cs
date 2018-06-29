using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;

namespace Interceptor
{
    /// <summary>
    /// 
    /// </summary>
    public class BCLInterceptor
    {
        #region Variables
        /// <summary>
        /// DataSet that will hold all the .NET BCL Types in DataTable(s)
        /// </summary>
        private DataSet bclTypesMasterDS;

        /// <summary>
        /// DataTable that will hold all the .NET BCL Types and its respective Exceptions in memory
        /// </summary>
        private DataTable bclTypesMasterTable;

        /// <summary>
        /// 
        /// </summary>
        private DataRow bclTypeDataRow;

        /// <summary>
        /// This is the key to search for all Types that ends with Exception keyword in .NET BCL Types
        /// </summary>
        private const string exceptionPattern = ".*exception$";
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public BCLInterceptor()
        {
            InitializeBCLTypesMasterDataSet();
        }        
        #endregion

        #region Properties
        
        #endregion

        #region Public Methods
        /// <summary>
        /// 
        /// </summary>
        public void LoadBCLTypes()
        {
            System.Reflection.Assembly assembly;
            ArrayList defaultAssemblyList = DefaultAssemblyList();

            SortedList slist = new SortedList();
            ArrayList assmList = new ArrayList();

            InitializeBCLDataTable();

            try
            {
                foreach (string strAssemblyName in defaultAssemblyList)
                {
                    assembly = System.Reflection.Assembly.Load(strAssemblyName);

                    foreach (System.Reflection.Module module in assembly.GetModules())
                    {
                        foreach (Type type in module.GetTypes())
                        {
                            if (Regex.IsMatch(type.FullName, exceptionPattern, RegexOptions.IgnoreCase))
                            {
                                AppendBCLTypes(type.Name, type.Namespace, type.Name, true);
                            }
                            else
                            {
                                AppendBCLTypes(type.Name, type.Namespace, string.Empty, false);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string[] SearchBCLExceptionForBCLType(string bclType)
        {
            string strExpr = string.Empty;
            string strSort = string.Empty;
            string[] exceptionName = null;
            DataRow[] dr;

            try
            {
                // Discover the Exception Namespace for the corresponding type
                strExpr = "TypeName = '" + bclType + "'";
                strSort = "TypeName DESC";
                dr = bclTypesMasterTable.Select(strExpr, strSort, DataViewRowState.Added);

                if (dr.Length != 0)
                {
                    // Discover the Actual Exception for the given type
                    strExpr = "Namespace='" + dr[0]["Namespace"].ToString() + "' AND ExceptionAvailable=True";

                    dr = bclTypesMasterTable.Select(strExpr, strSort, DataViewRowState.Added);

                    if (dr.Length != 0)
                    {
                        if (dr.Length >= 1)
                        {
                            //exceptionName = dr[0]["Exception"].ToString();
                            exceptionName = new string[dr.Length];
                            int counter = 0;
                            foreach (DataRow filteredRow in dr)
                            {
                                exceptionName[counter] = filteredRow["Exception"].ToString();
                                counter++;
                            }
                        }
                    }
                }                

                return (exceptionName);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bclName"></param>
        /// <param name="bclNamespace"></param>
        /// <param name="exception"></param>
        /// <param name="exceptionAvailable"></param>
        private void AppendBCLTypes(string bclName, string bclNamespace, string exception, 
                                    bool exceptionAvailable)
        {
            bclTypeDataRow = bclTypesMasterTable.NewRow();

            bclTypeDataRow["TypeName"] = bclName;
            bclTypeDataRow["Namespace"] = bclNamespace;
            bclTypeDataRow["Exception"] = exception;
            bclTypeDataRow["ExceptionAvailable"] = exceptionAvailable;

            bclTypesMasterTable.Rows.Add(bclTypeDataRow);
        }

        /// <summary>
        /// 
        /// </summary>
        private DataSet InitializeBCLTypesMasterDataSet()
        {
            bclTypesMasterDS = new DataSet("BCLTypesMasterDataSet");

            bclTypesMasterTable = bclTypesMasterDS.Tables.Add("BCLTypesMasterTable");

            bclTypesMasterTable.Columns.Add("TypeName", typeof(string));
            bclTypesMasterTable.Columns.Add("Namespace", typeof(string));
            bclTypesMasterTable.Columns.Add("Exception", typeof(string));
            bclTypesMasterTable.Columns.Add("ExceptionAvailable", typeof(bool));

            return (bclTypesMasterDS);
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitializeBCLDataTable()
        {
            bclTypesMasterTable = bclTypesMasterDS.Tables[0];
        }

        /// <summary>
        /// This method should be configurable based on the .NET version that is currently targeted for.
        /// </summary>
        /// <returns></returns>
        private static ArrayList DefaultAssemblyList()
        {
            ArrayList al = new ArrayList();

            al.Add("mscorlib, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
            al.Add("System.Data, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
            al.Add("System, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
            al.Add("System.Runtime.Remoting, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
            al.Add("System.Windows.Forms, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
            al.Add("System.Xml, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
            al.Add("System.Design, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
            al.Add("System.DirectoryServices, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
            al.Add("System.Drawing.Design, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
            al.Add("System.Drawing, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
            al.Add("System.Messaging, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
            al.Add("System.Runtime.Serialization.Formatters.Soap, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
            al.Add("System.Security, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
            al.Add("System.ServiceProcess, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
            al.Add("System.Web, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
            al.Add("System.Web.Services, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
            al.Add("System.Management, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");

            return al;
        }
        #endregion
    }
}
