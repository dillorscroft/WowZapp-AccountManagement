using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ContentUploader.Classes
{
    public class ErrorReport
    {

        
        public Guid ErrorReportId { get; set; }

        
        public string ApplicationName { get; set; }

        
        public string MachineName { get; set; }

        
        public string CommandLine { get; set; }

        
        public string OsVersion { get; set; }

        
        public string SystemUserName { get; set; }

        
        public string ClrVersion { get; set; }

        
        public string ErrorMessage { get; set; }

        
        public string StackTrace { get; set; }

        
        public List<string> LoadedAssemblies { get; set; }

        
        public string Comments { get; set; }

        
        public string ExceptionType { get; set; }

        
        public string SubSystem { get; set; }

    }
}