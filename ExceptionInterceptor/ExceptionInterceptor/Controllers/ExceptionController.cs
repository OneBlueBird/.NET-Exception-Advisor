using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.SymbolStore;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace ExceptionInterceptor.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class ExceptionController
    {
        #region Variables
        /// <summary>
        /// 
        /// </summary>
        private DataSet bclTypesUsedinFilesDS;

        /// <summary>
        /// 
        /// </summary>
        private DataTable projectsTable;

        /// <summary>
        /// 
        /// </summary>
        private DataTable bclTypesinFilesTable;

        /// <summary>
        /// 
        /// </summary>
        private ExceptionInterceptor.Reports.XMLReport xmlReport;

        /// <summary>
        /// 
        /// </summary>
        private Interceptor.BCLInterceptor bclManager = null;

        #endregion

        #region Constructor
        /// <summary>
        /// Default Contructor
        /// </summary>
        public ExceptionController()
        {
            InitializeTypesUsedinFilesDS();

            InitializeDataTables();

            InitializeBCLManager();
        }
        #endregion

        #region Properties

        #endregion

        #region Public Methods

        /// <summary>
        /// THIS METHOD IS NOT USED ANY WHERE. HENCE CAN BE DELETED
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="fileName"></param>
        public void AddNewProject(string projectName, string fileName)
        {
            try
            {
                DataRow projectRow;
                projectsTable = bclTypesUsedinFilesDS.Tables["ProjectsDataTable"];

                projectRow = projectsTable.NewRow();
                projectRow["ProjectName"] = projectName;
                projectRow["FileName"] = fileName;

                projectsTable.Rows.Add(projectRow);
            }
            catch (Exception ex)
            {
                // Do not throw any exceptions that are getting generated in this block and just proceed
                // further
                //Console.WriteLine("Exception : " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="className"></param>
        /// <param name="methodName"></param>
        /// <param name="exceptionsUsed"></param>
        public void AddNewClass(string projectFileName, string fileName, string className, string methodName, 
                                string lineNos, string bclTypeUsed, string exceptionsUsed)
        {
            try
            {
                if (methodName.Length != 0 && lineNos.Length != 0)
                {
                    DataRow classRow;
                    bclTypesinFilesTable = bclTypesUsedinFilesDS.Tables["BCLTypesDataTable"];

                    classRow = bclTypesinFilesTable.NewRow();
                    classRow["ProjectName"] = projectFileName;
                    classRow["FileName"] = fileName;
                    classRow["ClassName"] = className;
                    classRow["MethodSignature"] = methodName;
                    classRow["LineNos"] = lineNos;
                    classRow["BCLTypeUsed"] = bclTypeUsed;
                    classRow["ExceptionsUsed"] = exceptionsUsed;

                    bclTypesinFilesTable.Rows.Add(classRow);
                }
                else
                {
                    Trace.WriteLine("Method Name or LineNos is null");
                }
            }
            catch (Exception ex)
            {
                // Do not throw any exceptions that are getting generated in this block and just proceed
                // further
                //Console.WriteLine("Exception : " + ex.Message);
                Trace.WriteLine("Exception : " + ex.Message);
            }
        }

        /// <summary>
        /// This method captures the Project Name, File Name, Class Name, MethodName, Method Line Nos,
        /// BCL Type Used in methods and Exceptions handled in methods
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="className"></param>
        /// <param name="methodName"></param>
        /// <param name="LineNos"></param>
        /// <param name="bclTypeUsed"></param>
        /// <param name="bclExpectedException"></param>
        public void StoreTypeUsedinMethod(ExceptionInterceptor.Common.ProjectFile projectFile,
                                          ExceptionInterceptor.Common.SourceFile sourceFile,
                                          ExceptionInterceptor.Common.Method method, 
                                          string bclTypeUsed,
                                          string bclExceptionUsed)
        {
            try
            {
                AddNewClass(projectFile.ProjectFileName, sourceFile.SourceFileName, sourceFile.SourceClassName, method.MethodName,
                            method.MethodBeginLineNumber + "-" + method.MethodEndLineNumber, bclTypeUsed,
                            bclExceptionUsed);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method will pass the bclTypesinFilesTable to XMLReport to generate XML output.
        /// </summary>
        public void PrintReport()
        {
            try
            {
                xmlReport.GenerateReport();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// This method will map all the BCL Types used in the source files and create an XML report document
        /// as output.
        /// </summary>
        /// <param name="bclTypesDS"></param>
        public void MapBCLTypes(string totalFiles)
        {
            try
            {
                DataTable bclTypesinFilesTable = bclTypesUsedinFilesDS.Tables["BCLTypesDataTable"];
                string tempProjectName = null;
                string tempSourceFileName = null;
                string suggestedExceptions = null;

                int tempProjectFlag = 0;

                xmlReport = new ExceptionInterceptor.Reports.XMLReport();

                Trace.WriteLine("Creating XML Tag....");
                xmlReport.CreateXMLDocumentTag();               //  XML BEGIN BLOCK

                Trace.WriteLine("Creating Root Tag....");
                xmlReport.CreateXMLRootTag();                   //  DAExceptionCodeAudit BEGIN BLOCK

                Trace.WriteLine("Creating Projects Tag....");
                //xmlReport.CreateStartElement("Projects");      //  PROJECTS BEGIN BLOCK
                xmlReport.CreateStartElement("Projects", "TotalSourceFiles", totalFiles);

                for (int projectCount = 0; projectCount < bclTypesinFilesTable.Rows.Count; projectCount++)
                {   // OUTER FOR LOOP STARTS HERE

                    suggestedExceptions = SearchBCLExceptionForBCLType(bclTypesinFilesTable.Rows[projectCount]["BCLTypeUsed"].ToString());

                    //if (suggestedExceptions != "No Exception Match Found")
                    //{   
                        Trace.WriteLine("tempProjectName : " + tempProjectName + ", Project Name : " + bclTypesinFilesTable.Rows[projectCount]["ProjectName"].ToString());
                        Trace.WriteLine("tempSourceFileName : " + tempSourceFileName + ", Source FileName : " + bclTypesinFilesTable.Rows[projectCount]["FileName"].ToString());
                        if (tempProjectName != bclTypesinFilesTable.Rows[projectCount]["ProjectName"].ToString())
                        {
                            if (tempProjectFlag == 0)    // First time in the iteration do not create </Project> tag
                            {
                                // Create Project Tag
                                xmlReport.CreateStartElement("Project", "Name", bclTypesinFilesTable.Rows[projectCount]["ProjectName"].ToString());          //  PROJECT BEGIN BLOCK

                                // Create SourceFiles Tag
                                Trace.WriteLine("Creating SourceFiles Tag....");
                                xmlReport.CreateStartElement("SourceFiles");          //  SOURCEFILES BEGIN BLOCK

                                tempProjectFlag = 1;
                            }
                            else if (tempProjectFlag == 1)
                            {
                                // Create Class End Tag
                                xmlReport.CreateEndElement();

                                // Create SourceFiles End Tag
                                xmlReport.CreateEndElement();

                                // Create SourceFile End Tag
                                xmlReport.CreateEndElement();

                                // Create Project End Tag
                                xmlReport.CreateEndElement();

                                // Create Project Tag
                                xmlReport.CreateStartElement("Project", "Name", bclTypesinFilesTable.Rows[projectCount]["ProjectName"].ToString());          //  PROJECT BEGIN BLOCK

                                // Create SourceFiles Tag
                                Trace.WriteLine("Creating SourceFiles Tag....");
                                xmlReport.CreateStartElement("SourceFiles");          //  SOURCEFILES BEGIN BLOCK

                                //  tempProjectFlag = 0;
                            }

                            if (tempSourceFileName != bclTypesinFilesTable.Rows[projectCount]["FileName"].ToString())
                            {
                                // Create SourceFile Tag
                                xmlReport.CreateStartElement("SourceFile", "Name", bclTypesinFilesTable.Rows[projectCount]["FileName"].ToString());          //  SOURCE BEGIN BLOCK

                                // Create Class Tag
                                xmlReport.CreateStartElement("Class", "Name", bclTypesinFilesTable.Rows[projectCount]["ClassName"].ToString());

                                // CreateMethodBlock(.....)
                                if (suggestedExceptions != "No Exception Match Found")
                                {
                                    CreateMethodBlock(bclTypesinFilesTable.Rows[projectCount]["MethodSignature"].ToString(),
                                                  bclTypesinFilesTable.Rows[projectCount]["LineNos"].ToString(),
                                                  bclTypesinFilesTable.Rows[projectCount]["BCLTypeUsed"].ToString(),
                                                  bclTypesinFilesTable.Rows[projectCount]["ProjectName"].ToString(),
                                                  bclTypesinFilesTable.Rows[projectCount]["FileName"].ToString(),
                                                  bclTypesinFilesTable.Rows[projectCount]["ClassName"].ToString(),
                                                  suggestedExceptions);
                                }
                            }
                            else if (tempSourceFileName == bclTypesinFilesTable.Rows[projectCount]["FileName"].ToString())
                            {
                            }
                        }
                        else if (tempProjectName == bclTypesinFilesTable.Rows[projectCount]["ProjectName"].ToString())
                        {
                            if (tempSourceFileName != bclTypesinFilesTable.Rows[projectCount]["FileName"].ToString())
                            {
                                // Create End element for Class Tag
                                xmlReport.CreateEndElement();

                                // Create End element for SourceFile Tag
                                xmlReport.CreateEndElement();

                                // Create SourceFile Tag
                                xmlReport.CreateStartElement("SourceFile", "Name", bclTypesinFilesTable.Rows[projectCount]["FileName"].ToString());          //  SOURCE BEGIN BLOCK

                                // Create Class Tag
                                xmlReport.CreateStartElement("Class", "Name", bclTypesinFilesTable.Rows[projectCount]["ClassName"].ToString());

                                // CreateMethodBlock(.....)
                                if (suggestedExceptions != "No Exception Match Found")
                                {
                                    CreateMethodBlock(bclTypesinFilesTable.Rows[projectCount]["MethodSignature"].ToString(),
                                                  bclTypesinFilesTable.Rows[projectCount]["LineNos"].ToString(),
                                                  bclTypesinFilesTable.Rows[projectCount]["BCLTypeUsed"].ToString(),
                                                  bclTypesinFilesTable.Rows[projectCount]["ProjectName"].ToString(),
                                                  bclTypesinFilesTable.Rows[projectCount]["FileName"].ToString(),
                                                  bclTypesinFilesTable.Rows[projectCount]["ClassName"].ToString(),
                                                  suggestedExceptions);
                                }
                            }
                            else if (tempSourceFileName == bclTypesinFilesTable.Rows[projectCount]["FileName"].ToString())
                            {
                                // CreateMethodBlock(.....)
                                if (suggestedExceptions != "No Exception Match Found")
                                {
                                    CreateMethodBlock(bclTypesinFilesTable.Rows[projectCount]["MethodSignature"].ToString(),
                                                  bclTypesinFilesTable.Rows[projectCount]["LineNos"].ToString(),
                                                  bclTypesinFilesTable.Rows[projectCount]["BCLTypeUsed"].ToString(),
                                                  bclTypesinFilesTable.Rows[projectCount]["ProjectName"].ToString(),
                                                  bclTypesinFilesTable.Rows[projectCount]["FileName"].ToString(),
                                                  bclTypesinFilesTable.Rows[projectCount]["ClassName"].ToString(),
                                                  suggestedExceptions);
                                }
                            }
                        }
                    //}       // This needs to be corrected

                    tempProjectName = bclTypesinFilesTable.Rows[projectCount]["ProjectName"].ToString();
                    tempSourceFileName = bclTypesinFilesTable.Rows[projectCount]["FileName"].ToString();
                }   // OUTER FOR LOOP ENDS HERE

                xmlReport.CreateEndElement();               // PROJECTS END BLOCK

                xmlReport.CreateEndElement();               // Root Node closing

                xmlReport.FlushXMLContents();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
                throw (ex);
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// 
        /// </summary>
        private void CreateMethodBlock(string methodName, string lineNos, string bclTypeUsed,
                                       string projectName, string fileName, string className,
                                       string suggestedExceptions)
        {
            string exceptionsUsed = null;

            NameValueCollection methodSignature = new NameValueCollection();
            methodSignature.Add("Name", methodName);
            methodSignature.Add("LineNos", lineNos);

            Trace.WriteLine("Creating Method Tag....with enumeration values as attributes");
            xmlReport.CreateStartElement("Method", methodSignature);       //  METHOD START BLOCK

                Trace.WriteLine("Creating BCLTypesUsed Tag...." + bclTypeUsed);
                xmlReport.CreateStartElement("BCLTypesUsed", bclTypeUsed);
                xmlReport.CreateEndElement();

                // Group all the Exception Types used as comma separated string
                exceptionsUsed = CaptureUniqueExceptionsUsed(projectName, fileName,
                                                             className,
                                                             methodName);

                Trace.WriteLine("Creating ExceptionsUsed Tag...." + exceptionsUsed);
                xmlReport.CreateStartElement("ExceptionsUsed", exceptionsUsed);
                xmlReport.CreateEndElement();

                Trace.WriteLine("Creating ExpectedExceptions Tag...." + exceptionsUsed);
                xmlReport.CreateStartElement("ExpectedExceptions", suggestedExceptions);
                xmlReport.CreateEndElement();

            xmlReport.CreateEndElement();   //  METHOD END BLOCK
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bclTypeUsed"></param>
        /// <returns></returns>
        private string SearchBCLExceptionForBCLType(string bclTypeUsed)
        {
            NameValueCollection suggestedExceptions = new NameValueCollection();
            string[] typesUsed = bclTypeUsed.Split(new char[] { ',' });
            string[] typesSuggested = null;
            string[] exceptionKeys = null;
            StringBuilder exceptionBuilder = new StringBuilder();
            int i = 0;

            for (int typesCount = 0; typesCount < typesUsed.Length; typesCount++)
            {
                // For each BCL Types available in the array, find out the possible exceptions
                typesSuggested = bclManager.SearchBCLExceptionForBCLType(typesUsed[typesCount]);

                if (typesSuggested != null)
                {
                    // Store all the suggested exceptions as a NameValueCollection
                    for (int suggestedCount = 0; suggestedCount < typesSuggested.Length; suggestedCount++)
                    {
                        // Add the Suggested Exception iff that Exception is not stored already
                        if (suggestedExceptions.Get(typesSuggested[suggestedCount]) == null)
                        {
                            suggestedExceptions.Add(typesSuggested[suggestedCount], null);
                        }
                    }
                }

                typesSuggested = null;
            }

            // Return the Name Value Collection as a comma separated string
            exceptionKeys = suggestedExceptions.AllKeys;
            if (exceptionKeys.Length != 0)
            {
                foreach (string strExceptions in exceptionKeys)
                {
                    if (i == 0)
                    {
                        exceptionBuilder.Append(strExceptions);
                        i++;
                    }
                    else if (i > 0)
                    {
                        exceptionBuilder.Append(", " + strExceptions);
                    }
                }
            }

            if (exceptionBuilder.ToString().Length != 0)
            {
                return (exceptionBuilder.ToString());
            }
            else
            {
                return ("No Exception Match Found");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitializeTypesUsedinFilesDS()
        {
            bclTypesUsedinFilesDS = new DataSet("BCLTypesInFilesDS");

            // Create a BCL Types table that will hold all the types used in each class file(s)
            bclTypesinFilesTable = bclTypesUsedinFilesDS.Tables.Add("BCLTypesDataTable");

            bclTypesinFilesTable.Columns.Add("ProjectName", typeof(string));
            bclTypesinFilesTable.Columns.Add("FileName", typeof(string));
            bclTypesinFilesTable.Columns.Add("ClassName", typeof(string));
            bclTypesinFilesTable.Columns.Add("MethodSignature", typeof(string));
            bclTypesinFilesTable.Columns.Add("LineNos", typeof(string));
            bclTypesinFilesTable.Columns.Add("BCLTypeUsed", typeof(string));
            bclTypesinFilesTable.Columns.Add("ExceptionsUsed", typeof(string));
        }

        /// <summary>
        /// 
        /// </summary>
        private DataRow[] GetClassesForFiles(string fileName)
        {
            string filterExpression = "FileName = '" + fileName + "'";

            DataRow[] dr = null;
            dr = bclTypesinFilesTable.Select(filterExpression);
            
            if(dr.Length != 0 && dr!= null)
            {
                return (dr);
            }
            else
            {
                return (null);
            }
        }

        /// <summary>
        /// THIS METHOD IS NOT USED. HENCE CAN BE DELETED
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="className"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        private string CaptureUniqueBCLTypesUsed(string fileName, string className, string methodName)
        {
            string filterExpression = "FileName = '" + fileName + "' AND ClassName='" + className + "' AND MethodSignature='" + methodName + "'";
            StringBuilder methodBuilder = new StringBuilder();
            int i = 0;

            foreach (DataRow filteredRow in bclTypesinFilesTable.Select(filterExpression))
            {
                if (i == 0)
                {
                    methodBuilder.Append(filteredRow["BCLTypeUsed"].ToString());
                    i++;
                }
                else if (i > 0)
                {
                    methodBuilder.Append(", " + filteredRow["BCLTypeUsed"].ToString());
                }
            }

            return (methodBuilder.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="className"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        private string CaptureUniqueExceptionsUsed(string projectName, string fileName, string className, 
                                                   string methodName)
        {
            string filterExpression = "ProjectName = '" + projectName + "' AND FileName = '" + fileName + "' AND ClassName='" + className + "' AND MethodSignature='" + methodName + "'";
            StringBuilder exceptionBuilder = new StringBuilder();
            int i = 0;

            foreach (DataRow filteredRow in bclTypesinFilesTable.Select(filterExpression))
            {
                if (i == 0)
                {
                    if (filteredRow["ExceptionsUsed"].ToString() != string.Empty)
                    {
                        exceptionBuilder.Append(filteredRow["ExceptionsUsed"].ToString());
                        i++;
                    }
                }
                else if (i > 0)
                {
                    if (filteredRow["ExceptionsUsed"].ToString() != string.Empty)
                    {
                        exceptionBuilder.Append(", " + filteredRow["ExceptionsUsed"].ToString());
                    }
                }
            }

            if (exceptionBuilder.ToString().Length == 0)
            {
                return "No Exceptions handled";
            }
            else
            {
                return (exceptionBuilder.ToString());
            }            
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitializeDataTables()
        {
            //  projectsTable = bclTypesUsedinFilesDS.Tables[0];
            bclTypesinFilesTable = bclTypesUsedinFilesDS.Tables["BCLTypesDataTable"];
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitializeBCLManager()
        {
            bclManager = new Interceptor.BCLInterceptor();
            bclManager.LoadBCLTypes();
        }
        #endregion
    }
}
