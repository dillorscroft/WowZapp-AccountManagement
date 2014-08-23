using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.Serialization;

namespace ContentUploader
{
    
    public class General
    {

        public struct Error
        {
            public string ErrorNumber { get; set; }
            public string ErrorTitle { get; set; }
            public string ErrorDescription { get; set; }
            public string ErrorLocation { get; set; }
        }

    }
}
