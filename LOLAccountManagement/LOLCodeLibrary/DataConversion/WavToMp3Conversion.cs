using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LOLCodeLibrary.DataConversion
{
    public class WavToMp3Conversion : DataConversionAbstract
    {
        public WavToMp3Conversion(byte[] sourceData, string sourceFilePath, string targetFilePath, string decoderLocation, string decoderName)
            : base(sourceData, sourceFilePath, targetFilePath, decoderLocation, decoderName)
        {
        }
        
        public override string BuildArgumentsString()
        {
            return "-b 48 --resample 22.05 -m j " + this.SourceFilePath + " " + this.TargetFilePath;
        }
    }
}
