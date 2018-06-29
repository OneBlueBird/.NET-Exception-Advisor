using System;
using System.Collections.Generic;
using System.Text;

namespace ExceptionInterceptor.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="solutionFileName"></param>
    public delegate void ProcessSolution(ExceptionInterceptor.Common.FileContext fileContext,
                                         string[] fileName);
}

