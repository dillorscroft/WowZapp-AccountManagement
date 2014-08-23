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
    public sealed class Test_JpegConversion : TestBase, ITestable
    {
        #region ITestable
        
        public ILogger Logger { get; set; }

        public override void RunTests()
        {
            this.JpegConversion_ValidInput_ShouldSucceed();
        }
        #endregion

        #region Constructor
        public Test_JpegConversion(ILogger logger)
            : base()
        {
            this.Logger = logger;
        }
        #endregion

        #region Test Methods

        private void JpegConversion_ValidInput_ShouldSucceed()
        {
            this.Logger.LogMessage("Testing JpegConversion_ValidInput_ShouldSucceed ...", true);
            Image profileImage = Image.FromFile(this.ImageFilePath);
            byte[] pictureData = ImageHandler.imageToByteArray(profileImage);
            
            var elapsed = Stopwatch.StartNew();
            JpegConversion jgc = new JpegConversion(pictureData, Guid.NewGuid() + ".jpg", Guid.NewGuid() + ".jpg", "ImageDecoder", "i_view32.exe");
            pictureData = jgc.RunMainBody(this.ApplicationExecutionPath, Directory.GetCurrentDirectory());
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (pictureData != null)
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest();
        }

        #endregion  
    }
}
