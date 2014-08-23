using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LOLCodeLibrary.LoggingSystem;

namespace ConsoleApplication1
{
    public interface ITestable
    {
        ILogger Logger { get; set; }        
    }
}
