using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web.Hosting;

namespace LOLCodeLibrary.DataConversion
{
    /// <summary>
    /// Make sure the passed in decoder name exists in the right location - which is where the main program is executed combined with the decoder location folder
    /// </summary>
    public class JpegConversion : DataConversionAbstract
    {
        public JpegConversion(byte[] sourceData, string sourceFilePath, string targetFilePath, string decoderLocation, string decoderName)
            : base( sourceData, sourceFilePath, targetFilePath, decoderLocation, decoderName )
        {
        }
        
        public override string BuildArgumentsString()
        {
            return this.SourceFilePath + " /silent /resize=(512,512) /aspectratio /jpgq=70 /convert=" + this.TargetFilePath;
        }        
    }
}
