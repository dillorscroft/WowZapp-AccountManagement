using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using LOLCodeLibrary.LoggingSystem;

namespace ConsoleApplication1
{
    public abstract class TestBase
    {
        public virtual string ImageFilePath { get; set; }
        public virtual string TestSuccessMessage { get; set; }
        public virtual string TestFailMessage { get; set; }
        public virtual string Delimiter { get; set; }
        public virtual string ApplicationExecutionPath { get; set; }
        public virtual string WavFilePath { get; set; }

        /// <summary>
        /// base constructor setting the default values to be used by all Test classes
        /// </summary>
        public TestBase()
        {
            this.ImageFilePath    = @"E:\Andi-Small.jpg";
            this.WavFilePath = @"J:\test.wav";
            this.TestSuccessMessage = "Test Succesfull !";
            this.TestFailMessage    = "Test Failed !";
            this.ApplicationExecutionPath = @"C:\Development\SatoriIT\LOL Account Management\LOLAccountManagement\LolCodeLibrary_TestInterface";

            this.Delimiter = "--------------------------------------";
        }

        /// <summary>
        /// generates a random email address to be used by functions like UserCreate.
        /// </summary>
        /// <returns></returns>
        public virtual string GetRandomEmail()
        {
            return Guid.NewGuid().ToString() + "@random.com";
        }

        /// <summary>
        /// formats the Elapsed time output for use by all Test classes. This is the only place that needs to be changed should the format require changing
        /// </summary>
        /// <param name="swObject"></param>
        /// <returns></returns>
        public virtual string PrepareElapsedTimeOutput(Stopwatch swObject)
        {
            return "Execution time was - hours :" + swObject.Elapsed.Hours + ", minutes :" + swObject.Elapsed.Minutes + ", seconds :" + swObject.Elapsed.Seconds + ", milliseconds :" + swObject.ElapsedMilliseconds;
        }

        /// <summary>
        /// override in the implementing class and call the specific test methods required
        /// </summary>
        public virtual void RunTests()
        {
        }

        public virtual void CleanAfterTest()
        {
        }
    }
}
