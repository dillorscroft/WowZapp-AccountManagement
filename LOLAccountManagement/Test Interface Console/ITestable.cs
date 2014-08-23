using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LOLAccountManagement.Classes;
using LOLCodeLibrary.LoggingSystem;

namespace Test_Interface_Console
{
    public interface ITestable
    {
        LOLConnect.LOLConnectClient _ws { get; set; }
        ILogger Logger { get; set; }        
    }
}
