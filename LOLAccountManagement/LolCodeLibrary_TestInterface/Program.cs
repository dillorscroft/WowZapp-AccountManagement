using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LOLCodeLibrary.LoggingSystem;


namespace LolCodeLibrary_TestInterface
{
    class Program
    {
        private static void ShowMenu()
        {
            Console.WriteLine("Which tests do you want to run ?");
            Console.WriteLine("0 - All");
            Console.WriteLine("1 - JpegConversion");
            Console.WriteLine("2 - WavToMp3Conversion");
            Console.WriteLine("3 - ImageHandler");
            Console.WriteLine("X - Exit");

            int testID = 0;
            var read = Console.ReadLine();
            
            if (!read.ToLower().Equals("x"))
            {
                int.TryParse(read, out testID);
                RunBattery(testID);
            }
        }

        private static void RunBattery(int id)
        {
            ILogger logger = new LoggerManager(new ConsoleLogger()).GetManager();

            switch (id)
            {
                case 0:
                    Test_JpegConversion testJpegConversion = new Test_JpegConversion(logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for JpegConversion", true);
                    testJpegConversion.RunTests();
                    logger.LogMessage("================================", true);

                    Test_WavToMp3Conversion testWavToMp3Conversion = new Test_WavToMp3Conversion(logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for WavToMp3Conversion", true);
                    testWavToMp3Conversion.RunTests();
                    logger.LogMessage("================================", true);

                    Test_ImageHandler testImageHandler = new Test_ImageHandler(logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for ImageHandler", true);
                    testImageHandler.RunTests();
                    logger.LogMessage("================================", true);

                    break;
                case 1:
                    Test_JpegConversion testJpegConversion1 = new Test_JpegConversion(logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for JpegConversion", true);
                    testJpegConversion1.RunTests();
                    logger.LogMessage("================================", true);
                    break;
                case 2:
                    Test_WavToMp3Conversion testWavToMp3Conversion1 = new Test_WavToMp3Conversion(logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for WavToMp3Conversion", true);
                    testWavToMp3Conversion1.RunTests();
                    logger.LogMessage("================================", true);
                    break;
                case 3:
                    Test_ImageHandler testImageHandler1 = new Test_ImageHandler(logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for ImageHandler", true);
                    testImageHandler1.RunTests();
                    logger.LogMessage("================================", true);
                    break;
                default:
                    logger.LogMessage("Please pass a valid option ( 0-3 )", true);
                    break;
            }

            ShowMenu();
        }

        static void Main(string[] args)
        {            
            ShowMenu();
        }
    }
}
