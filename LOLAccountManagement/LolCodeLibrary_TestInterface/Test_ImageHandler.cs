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
    public sealed class Test_ImageHandler : TestBase, ITestable
    {
        #region ITestable
        
        public ILogger Logger { get; set; }

        public override void RunTests()
        {
            this.ImageHandler_ValidInputSizeBig_ShouldSucceed();
        }
        #endregion

        #region Constructor
        public Test_ImageHandler(ILogger logger)
            : base()
        {
            this.Logger = logger;
        }
        #endregion

        #region Test Methods

        private void ImageHandler_ValidInputSizeBig_ShouldSucceed()
        {
            this.Logger.LogMessage("Testing ImageHandler_ValidInputSizeBig_ShouldSucceed ... Size is 2.7mb", true);
            Image profileImage = Image.FromFile(this.ImageFilePath);
            
            var elapsed = Stopwatch.StartNew();
            byte[] image = ImageHandler.imageToByteArray(profileImage);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (image.Length > 0)
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest();
        }

        #endregion  
    }
}
