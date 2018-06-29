using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Windows.Forms;

using Extensibility;
using EnvDTE;
using EnvDTE80;

using ExceptionInterceptor.Properties;

namespace ExceptionInterceptor.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class AddInController
    {
        #region Variables
        
        #endregion

        #region Constructor

        #endregion

        #region Properties

        #endregion

        #region Public Methods
        /// <summary>
        /// This method helps to detect the exact Exception Interceptor AddIn based on configuration.
        /// </summary>
        public void InitializeExceptionInterceptor(DTE2 dte2, ext_ConnectMode connectMode, AddIn addIn)
        {
            try
            {
                IEnumerator addInsEnum = addIn.Collection.GetEnumerator();
                AddIn tempAddIn;

                while (addInsEnum.MoveNext())
                {
                    tempAddIn = (AddIn)addInsEnum.Current;

                    if (tempAddIn != null)
                    {
                        if (tempAddIn.Name == Settings.Default.AddInName)
                        {
                            if (connectMode.ToString() == "ext_cm_AfterStartup")
                            {
                                // Display Form UI
                                LoadExceptionInterceptorUI(dte2);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// This method will load the Exception Interceptor UI screen to the user
        /// </summary>
        private void LoadExceptionInterceptorUI(DTE2 dte2)
        {
            ExceptionInterceptor.Common.ProcessSolution processSolution = null;
            ExceptionInterceptor.Factories.ParserFactory parserFactory = null;

            parserFactory = new ExceptionInterceptor.Factories.ParserFactory(dte2);
            processSolution += new ExceptionInterceptor.Common.ProcessSolution(parserFactory.LoadParser);

            ExceptionInterceptor.UI.DASolutionForm daSolutionForm = new ExceptionInterceptor.UI.DASolutionForm(processSolution);
            daSolutionForm.Show();
        }
        #endregion
    }
}
