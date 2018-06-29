using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
    public class SourceFileOpener : ExceptionInterceptor.Parser.IVSFileOpener
    {
        #region Variables
        /// <summary>
        /// 
        /// </summary>
        private DTE2 _dte2;

        /// <summary>
        /// 
        /// </summary>
        private ExceptionInterceptor.Common.FileContext _fileContext;
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public SourceFileOpener(ExceptionInterceptor.Common.FileContext fileContext, DTE2 dte2)
        {
            _dte2 = dte2;
            _fileContext = fileContext;
        }
        #endregion

        #region Properties

        #endregion

        #region Public Methods

        #endregion

        #region Private Methods
        /// <summary>
        /// 
        /// </summary>
        private string GetTargetProjectPath()
        {
            return (ExceptionInterceptor.Common.ExceptionConfigurator.GetTemporaryProjectPath());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetCSEmptyProject()
        {
            return (ExceptionInterceptor.Common.ExceptionConfigurator.GetCSEmptyProject());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetCSEmptyProjectType()
        {
            return (ExceptionInterceptor.Common.ExceptionConfigurator.GetCSEmptyProjectType());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetVBEmptyProject()
        {
            return (ExceptionInterceptor.Common.ExceptionConfigurator.GetVBEmptyProject());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetVBEmptyProjectType()
        {
            return (ExceptionInterceptor.Common.ExceptionConfigurator.GetVBEmptyProjectType());
        }
        #endregion

        #region IVSFileOpener Members
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        public void Open(string[] sourceFileName)
        {
            string templatePath = null;
            string targetProjectPath = null;
            string emptyProject = null;
            string emptyProjectType = null;
            Solution2 soln = null;
            Project prj = null;

            try
            {
                targetProjectPath = GetTargetProjectPath();

                // DA - To create a new project from a template and add source files to it
                Directory.Delete(targetProjectPath, true);
                Directory.CreateDirectory(targetProjectPath);

                soln = (Solution2)_dte2.Solution;

                if (_fileContext.CurrentFileType == ExceptionInterceptor.Common.FileType.CSFile)
                {
                    emptyProject = GetCSEmptyProject();
                    emptyProjectType = GetCSEmptyProjectType();
                    templatePath = soln.GetProjectTemplate(emptyProject, emptyProjectType);
                }
                else if (_fileContext.CurrentFileType == ExceptionInterceptor.Common.FileType.VBFile)
                {
                    emptyProject = GetVBEmptyProject();
                    emptyProjectType = GetVBEmptyProjectType();
                    templatePath = soln.GetProjectTemplate(emptyProject, emptyProjectType);
                }

                soln.AddFromTemplate(templatePath, targetProjectPath, "EmptyProjectName", false);

                // Point to the first project
                prj = soln.Projects.Item(1);

                for (int fileCount = 0; fileCount < sourceFileName.Length; fileCount++)
                {
                    prj.ProjectItems.AddFromFileCopy(sourceFileName[fileCount]);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion
    }
}
