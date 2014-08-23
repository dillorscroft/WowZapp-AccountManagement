using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleApplication1;
using LOLCodeLibrary.LoggingSystem;
using LOLCodeLibrary.DataConversion;
using System.Web.Hosting;
using System.IO;
using System.Drawing;
using LOLCodeLibrary;
using System.Diagnostics;

namespace LolCodeLibrary_TestInterface
{
    public sealed class Test_WavToMp3Conversion : TestBase, ITestable
    {
        #region ITestable
        
        public ILogger Logger { get; set; }

        public override void RunTests()
        {
            this.WavToMp3Conversion_ValidInput_ShouldSucceed();
        }
        #endregion

        #region Constructor
        public Test_WavToMp3Conversion(ILogger logger)
            : base()
        {
            this.Logger = logger;
        }
        #endregion

        #region Test Methods

        private void WavToMp3Conversion_ValidInput_ShouldSucceed()
        {
            this.Logger.LogMessage("Testing WavToMp3Conversion_ValidInput_ShouldSucceed ...", true);

            FileStream fs = File.OpenRead(this.WavFilePath);            
            byte[] bytes = new byte[fs.Length];
            fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
            fs.Close();
            
            var elapsed = Stopwatch.StartNew();
            WavToMp3Conversion wc = new WavToMp3Conversion(bytes, Guid.NewGuid() + ".wav", Guid.NewGuid() + ".mp3", "SoundDecoder", "lame.exe");
            byte[] processedData = wc.RunMainBody(this.ApplicationExecutionPath, Directory.GetCurrentDirectory());
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (processedData != null)
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest();
        }

        #endregion  
    }
}
