using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.IO;

namespace LOLCodeLibrary.DataConversion
{
    public abstract class DataConversionAbstract
    {
        public string SourceFilePath  { get; set; }
        public string TargetFilePath  { get; set; }
        public string DecoderLocation { get; set; }
        public byte[] SourceData      { get; set; }
        public string DecoderName     { get; set; }

        public DataConversionAbstract(byte[] sourceData, string sourceFilePath, string targetFilePath, string decoderLocation, string decoderName)
        {
            this.SourceFilePath  = sourceFilePath;
            this.TargetFilePath  = targetFilePath;
            this.DecoderLocation = decoderLocation;
            this.SourceData = sourceData;
            this.DecoderName     = decoderName;
        }
        
        /// <summary>
        /// used by RunMainBody. Do not call directly. It is public so it can be overriden if required
        /// </summary>
        /// <returns></returns>
        public virtual bool RunConversionProcess()
        {
            bool result = true;
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.UseShellExecute = false;
                psi.CreateNoWindow = true;
                psi.WindowStyle = ProcessWindowStyle.Hidden;
                psi.FileName = this.DecoderName;
                psi.Arguments = this.BuildArgumentsString();
                Process p = Process.Start(psi);
                p.WaitForExit();
                p.Close();
                p.Dispose();
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

        public abstract string BuildArgumentsString();

        /// <summary>
        /// The main class method. Call it after the class constructor.
        /// </summary>
        /// <param name="applicationPhysicalPath">The physical location where the application is located</param>
        /// <param name="currentDirectory">The current execution folder. Will be changed and restored at the end of this method</param>
        /// <returns></returns>
        public virtual byte[] RunMainBody(string applicationPhysicalPath, string currentDirectory)
        {
            byte[] outputData = new byte[0];
            string path = Path.Combine(applicationPhysicalPath, this.DecoderLocation);
            Directory.SetCurrentDirectory(path);

            File.WriteAllBytes(this.SourceFilePath, this.SourceData);

            bool ProcessRanOK = this.RunConversionProcess();

            if (File.Exists(this.SourceFilePath))
                File.Delete(this.SourceFilePath);

            if (ProcessRanOK)
            {
                var targetPath = Path.Combine(path, this.TargetFilePath);
                var sourceData = this.ReadFile(this.TargetFilePath);

                if (sourceData.Count() > 0)
                {
                    Array.Resize(ref outputData, sourceData.Count());
                    sourceData.CopyTo(outputData, 0);
                }
                else
                {
                    Array.Resize(ref outputData, this.SourceData.Count());
                    this.SourceData.CopyTo(outputData, 0);
                }

                if (File.Exists(targetPath))
                    File.Delete(targetPath);
            }
            else
            {
                Array.Resize(ref outputData, this.SourceData.Count());
                this.SourceData.CopyTo(outputData, 0);
            }

            Directory.SetCurrentDirectory(currentDirectory);

            return outputData;
        }

        public virtual byte[] ReadFile(string path)
        {
            byte[] result = new byte[0];
            FileStream fs = File.OpenRead(path);
            try
            {
                result = new byte[fs.Length];
                fs.Read(result, 0, Convert.ToInt32(fs.Length));
                fs.Close();
            }
            catch (Exception ex)
            {
                //don't do anything, just return an empty byte[]
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }

            return result;
        }
    }
}
