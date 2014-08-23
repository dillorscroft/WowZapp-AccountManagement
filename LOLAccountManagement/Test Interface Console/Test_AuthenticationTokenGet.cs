using System;
using System.Diagnostics;
using LOLAccountManagement.Classes;
using LOLCodeLibrary.LoggingSystem;

namespace Test_Interface_Console
{
    public sealed class Test_AuthenticationTokenGet : TestBase, ITestable
    {
        //AuthenticationTokenGet(string DeviceID)
        //we are testing the following scenarios : 
        //1. pass a null deviceid  - should return Guid.Empty        
        //2. pass a proper DeviceID - should return valid Guid

        #region ITestable        
        public LOLConnect.LOLConnectClient _ws { get;set;}
        public ILogger Logger { get; set; }

        public override void RunTests()
        {
            AuthenticationTokenGet_EmptyDeviceID_ShouldFail();
            AuthenticationTokenGet_ValidInput_ShouldSucceed();
        }
        #endregion

        #region Constructor
        public Test_AuthenticationTokenGet(LOLConnect.LOLConnectClient ws, ILogger logger) : base()
        {
            this._ws = ws;
            this.Logger = logger;
        }
        #endregion

        #region Test Methods

        private void AuthenticationTokenGet_EmptyDeviceID_ShouldFail()
        {
            this.Logger.LogMessage("Testing AuthenticationTokenGet_EmptyDeviceID_ShouldFail ...", true);
            
            var elapsed = Stopwatch.StartNew();
            Guid result = _ws.AuthenticationTokenGet(string.Empty);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);
            
            if (result.Equals(Guid.Empty))            
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void AuthenticationTokenGet_ValidInput_ShouldSucceed()
        {
            this.Logger.LogMessage("Testing AuthenticationTokenGet_ValidInput_ShouldSucceed ...", true);

            var elapsed = Stopwatch.StartNew();
            Guid result = _ws.AuthenticationTokenGet(this.RandomDeviceID);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (!result.Equals(Guid.Empty))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        #endregion
    }
}
