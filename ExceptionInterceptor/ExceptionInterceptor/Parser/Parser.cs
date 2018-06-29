using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using EnvDTE;
using EnvDTE80;

using VSLangProj;
using VSLangProj2;
using VSLangProj80;

namespace ExceptionInterceptor.Parser
{
    /// <summary>
    /// 
    /// </summary>
    public class Parser
    {
        #region Variables
        /// <summary>
        /// This will store the current file context like Solution file / Project file / Source file(s)
        /// </summary>
        private ExceptionInterceptor.Common.FileContext _fileContext = null;

        /// <summary>
        /// 
        /// </summary>
        protected ExceptionInterceptor.Parser.IVSFileOpener _IVSFileOpener = null;

        /// <summary>
        /// 
        /// </summary>
        protected DTE2 _dte2;

        /// <summary>
        /// 
        /// </summary>
        protected ExceptionInterceptor.Controllers.ExceptionController _exceptionController = null;

        /// <summary>
        /// 
        /// </summary>
        private NameValueCollection _methodsTable = null;

        /// <summary>
        /// 
        /// </summary>
        private ExceptionInterceptor.Common.SourceFile _sourceFile = null;

        /// <summary>
        /// 
        /// </summary>
        protected ExceptionInterceptor.Common.ProjectFile _projectFile = null;

        /// <summary>
        /// 
        /// </summary>
        protected ExceptionInterceptor.Common.Method _method = null;

        /// <summary>
        /// 
        /// </summary>
        protected Solution2 _soln = null;

        #endregion

        #region Constructor
        /// <summary>
        /// Default Constructor
        /// </summary>
        public Parser()
        {
            InitializeSource();

            InitializeExceptionController();
        }
        #endregion

        #region Properties

        #endregion

        #region Public Methods
        /// <summary>
        /// 
        /// </summary>
        public virtual void Initialize(DTE2 dte2, string[] fileName)
        {
            _dte2 = dte2;

            try
            {
                OpenSolutionFile(fileName);

                DiscoverProjects();

                MapBCLTypes();

                PrintReport();

                CloseSolutionFile();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ExceptionInterceptor.Common.FileContext GetCurrentFileContext()
        {
            return (_fileContext);
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetCurrentFileContext(ExceptionInterceptor.Common.FileContext fileContext)
        {
            _fileContext = fileContext;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ParseClassFile(ExceptionInterceptor.Common.ProjectFile projectFile)
        {
            _projectFile = projectFile;

            FileCodeModel fileCodeModel;
            CodeElements elts;
            CodeElement elt;

            InitializeMethodsTableCollection();

            // Set the Source File Name that is currently in process.
            _sourceFile.SourceFileName = _dte2.ActiveDocument.Name;

            try
            {
                fileCodeModel = _dte2.ActiveDocument.ProjectItem.FileCodeModel;
                elts = null;
                elts = fileCodeModel.CodeElements;
                elt = null;
                int i = 0;

                for (i = 1; i <= fileCodeModel.CodeElements.Count; i++)
                {
                    elt = elts.Item(i);
                    CollapseElt(elt, elts, i);
                }

                DiscoverBCLTypesUsedInMethods(_dte2);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ", " + ex.StackTrace);
                //  throw (ex);
            }
            finally
            {
                fileCodeModel = null;
                elts = null;
                elt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void GenerateReport()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ivsFileOpener"></param>
        public void SetVSFileOpener(IVSFileOpener ivsFileOpener)
        {
            _IVSFileOpener = ivsFileOpener;
        }


        #endregion

        #region Private Methods
        /// <summary>
        /// 
        /// </summary>
        private void InitializeExceptionController()
        {
            _exceptionController = new ExceptionInterceptor.Controllers.ExceptionController();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elt"></param>
        /// <param name="elts"></param>
        /// <param name="loc"></param>
        private void CollapseElt(CodeElement elt, CodeElements elts, long loc)
        {
            EditPoint epStart = null;
            EditPoint epEnd = null;
            epStart = elt.StartPoint.CreateEditPoint();
            // Do this because we move it later.
            epEnd = elt.EndPoint.CreateEditPoint();
            epStart.EndOfLine();
            if (((elt.IsCodeType) & 
                 (elt.Kind != vsCMElement.vsCMElementDelegate) & 
                 (elt.Kind != vsCMElement.vsCMElementInterface) & 
                 (elt.Kind != vsCMElement.vsCMElementEnum)
                ))
            {
                // Set the Source Class Name that is currently in process.
                _sourceFile.SourceClassName = elt.Name;
                Trace.WriteLine("DA Type Name : " + elt.Name);
                _sourceFile.SourceFileType = "Class";

                //MessageBox.Show("got type but not a delegate, named : " + elt.Name);
                DiscoverMethods(elt);

                CodeType ct = null;
                ct = ((EnvDTE.CodeType)(elt));
                CodeElements mems = null;
                mems = ct.Members;
                int i = 0;
                for (i = 1; i <= ct.Members.Count; i++)
                {
                    CollapseElt(mems.Item(i), mems, i);
                }
            }
            else if ((elt.Kind == vsCMElement.vsCMElementNamespace))
            {
                //MessageBox.Show("got a namespace, named: " + elt.Name);
                CodeNamespace cns = null;
                cns = ((EnvDTE.CodeNamespace)(elt));
                //MessageBox.Show("set cns = elt, named: " + cns.Name);

                CodeElements mems_vb = null;
                mems_vb = cns.Members;
                //MessageBox.Show("got cns.members");
                int i = 0;

                for (i = 1; i <= cns.Members.Count; i++)
                {
                    CollapseElt(mems_vb.Item(i), mems_vb, i);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codeClass"></param>
        private void DiscoverMethods(CodeElement codeClass)
        {
            //MessageBox.Show("Inside DiscoverMethods of FileParser");

            CodeClass2 codeClass2 = (CodeClass2)codeClass;

            //StringBuilder sb = new StringBuilder(codeClass2.Name + " has the following members:\n");

            foreach (CodeElement ce in codeClass2.Members)
            {
                //sb.AppendLine(ce.Name + ", " + ce.StartPoint.Line.ToString() + " , " + ce.EndPoint.Line.ToString());

                if (ce.Name != null)
                {
                    _method = new ExceptionInterceptor.Common.Method(ce.Name, ce.StartPoint.Line.ToString(), ce.EndPoint.Line.ToString());

                    _methodsTable.Add(ce.StartPoint.Line.ToString() + "#" + ce.EndPoint.Line.ToString(),
                                 ce.Name);
                }
            }
            // Display the name and members elements of the CodeClass.
            //MessageBox.Show(sb.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dte"></param>
        private void DiscoverBCLTypesUsedInMethods(DTE2 dte)
        {
            // TextSelection textSelection = (TextSelection)(dte.ActiveDocument.Selection);

            IEnumerator enumKeys = _methodsTable.Keys.GetEnumerator();
            string[] keys;

            while (enumKeys.MoveNext())
            {
                //MessageBox.Show("FileName : " + dte.ActiveDocument.Name + ", Key : " + enumKeys.Current.ToString() + " , Value : " +
                //                           methodsTable[enumKeys.Current.ToString()]);

                keys = enumKeys.Current.ToString().Split(new char[] { '#' });
                _method = new ExceptionInterceptor.Common.Method(_methodsTable.Get(enumKeys.Current.ToString()), keys[0].ToString(), keys[1].ToString());

                CaptureMethodBody(int.Parse(keys[0]), int.Parse(keys[1]));
            }
        }

        /// <summary>
        /// This method is responsible to detect all the .NET BCL Types that are used in the code and 
        /// build a collection on the same to generate report on Exception that were not declared.
        /// </summary>
        /// <param name="textSelection"></param>
        /// <param name="startLine"></param>
        /// <param name="endLine"></param>
        private void CaptureMethodBody(int startLine, int endLine)
        {
            TextSelection textSelection = (TextSelection)(_dte2.ActiveDocument.Selection);

            try
            {
                textSelection.MoveToLineAndOffset(startLine, 1, false);

                textSelection.MoveToLineAndOffset(startLine, 1, true);

                textSelection.MoveToLineAndOffset(endLine, 1, true);

                textSelection.Copy();

                // MessageBox.Show("StartLine : " + startLine + "EndLine : " + endLine + "Contents : " + Clipboard.GetText());

                ParseMethods(Clipboard.GetText());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
                throw (ex);
            }
            finally
            {
                textSelection = null;
            }
        }

        /// <summary>
        /// This method parses all the contents inside the methodBody and groups them as single collection.
        /// </summary>
        /// <param name="methodBody"></param>
        private void ParseMethods(string methodBody)
        {
            string[] methodContents = null;
            const string pattern = "^[A-Z]";
            const string strExceptionPattern = ".*exception$";

            try
            {
                methodContents = methodBody.Split(new char[] { ' ' });

                for (int i = 0; i < methodContents.Length; i++)
                {
                    if (Regex.IsMatch(methodContents[i], pattern))
                    {
                        if (Regex.IsMatch(methodContents[i], strExceptionPattern, RegexOptions.IgnoreCase))
                        {
                            // The search text is of type BCL Exception Type
                            _exceptionController.StoreTypeUsedinMethod(_projectFile, _sourceFile, _method,
                                                                     string.Empty, methodContents[i]);
                        }
                        else
                        {
                            // The search text if of type BCL Type
                            _exceptionController.StoreTypeUsedinMethod(_projectFile, _sourceFile, _method,
                                                                      methodContents[i], string.Empty);
                        }
                    }
                }

                Clipboard.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
                throw (ex);
            }
            finally
            {
                methodContents = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitializeMethodsTableCollection()
        {
            _methodsTable = new NameValueCollection();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitializeSource()
        {
            _sourceFile = new ExceptionInterceptor.Common.SourceFile();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitializeMethod()
        {
            _method = new ExceptionInterceptor.Common.Method();
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// This method open a soltion file and its related projects for processing
        /// </summary>
        protected virtual void OpenSolutionFile(string[] fileName)
        {
            throw new Exception("Method not implemented");
        }

        /// <summary>
        /// This method captures the list the projects and its related source files associated with it
        /// </summary>
        protected virtual void DiscoverProjects()
        {
            Projects projects = null;
            Project project = null;
            ProjectItems projectItems = null;
            string projectFolderPath = null;

            _projectFile = new ExceptionInterceptor.Common.ProjectFile();
            int tempFileIncr = 0;
            _soln = (Solution2)_dte2.Solution;

            try
            {
                // The below code snippet gives me list of projects available in a solution.
                projects = _dte2.Solution.Projects;

                // For each project available in a solution, discover .cs file and process the same.
                for (int projectsCount = 1; projectsCount <= _dte2.Solution.Projects.Count; projectsCount++)
                {
                    project = projects.Item(projectsCount);

                    _projectFile.ProjectFileName = project.FullName.Substring(project.FullName.LastIndexOf('\\') + 1);

                    projectFolderPath = project.FullName.Substring(0, project.FullName.LastIndexOf('\\') + 1);

                    _projectFile.ProjectFilePath = projectFolderPath;

                    projectItems = project.ProjectItems;

                    for (int pi = 1; pi <= projectItems.Count; pi++)
                    {
                        if (projectItems.Item(pi).Properties.Item(1).Value.ToString() == ".cs" ||
                            projectItems.Item(pi).Properties.Item(1).Value.ToString() == ".vb")
                        {
                            if (_soln.Globals.get_VariableExists("TotalFiles") == false)
                            {
                                _soln.Globals["TotalFiles"] = "1";
                            }
                            else
                            {
                                tempFileIncr = int.Parse(_soln.Globals["TotalFiles"].ToString());
                                tempFileIncr = tempFileIncr + 1;
                                _soln.Globals["TotalFiles"] = tempFileIncr.ToString();
                            }

                            // Below line opens a .cs source file for processing
                            OpenSourceFile(projectFolderPath + projectItems.Item(pi).Name);

                            // Process the currently opened file here
                            ParseClassFile(_projectFile);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void MapBCLTypes()
        {
            _exceptionController.MapBCLTypes(_soln.Globals["TotalFiles"].ToString());
        }

        /// <summary>
        /// This method prints the result of processing as HTML output
        /// </summary>
        protected virtual void PrintReport()
        {
            _exceptionController.PrintReport();
        }

        /// <summary>
        /// This method closes a soltion file after processing
        /// </summary>
        protected virtual void CloseSolutionFile()
        {
            _dte2.Solution.Close(false);
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void GetReportContext()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void SetReportContext()
        {

        }

        /// <summary>
        /// This method opens a Source File from a project.
        /// </summary>
        /// <param name="projectFolderPath"></param>
        /// <param name="sourceFileName"></param>
        protected virtual void OpenSourceFile(string fileName)
        {
            _dte2.ItemOperations.OpenFile(fileName, EnvDTE.Constants.vsViewKindCode);
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void CloseSourceFile()
        {
            _dte2.Solution.DTE.ActiveDocument.Close(vsSaveChanges.vsSaveChangesNo);
        }
        #endregion
    }
}
