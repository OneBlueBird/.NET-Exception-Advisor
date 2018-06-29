using System;
using System.Collections.Generic;
using System.Text;

namespace ExceptionInterceptor.Parser
{
    public interface IVSFileOpener
    {
        /// <summary>
        /// This is a Strategy method and the methodology to open a Solution file / Project file / 
        /// Source file varies based on the File Context being processed.
        /// </summary>
        void Open(string[] fileName);
    }
}
