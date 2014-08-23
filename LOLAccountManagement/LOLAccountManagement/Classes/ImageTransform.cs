using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.IO;

namespace LOLAccountManagement.Classes
{
    public class ImageTransform
    {
        private byte[] ImageData;
        private int Height;
        private int Width;
        private Color BackgroundColor;
        private System.Drawing.Imaging.ImageFormat OutputImageFormat;

        public ImageTransform()
        {
            this.ImageData = null;
            this.Height = 0;
            this.Width = 0;
            this.BackgroundColor = Color.Black;
            this.OutputImageFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
        }

        public ImageTransform(byte[] inputData, int height, int width, Color backgroundColor, System.Drawing.Imaging.ImageFormat outputFormat)
        {
            this.ImageData = inputData;
            this.Height = height;
            this.Width = width;
            this.BackgroundColor = backgroundColor;
            this.OutputImageFormat = outputFormat;
        }

        /// <summary>
        /// scaling happens in memory using the instance variables
        /// </summary>
        /// <returns></returns>
        public byte[] ScaleImageCenter()
        {
            MemoryStream msInput = new MemoryStream(this.ImageData);
            Bitmap OriginalImage = (Bitmap)Bitmap.FromStream(msInput);

            Bitmap result = new Bitmap(this.Width, this.Height);

            // Get Graphics from result image
            Graphics g = Graphics.FromImage(result);

            // Clear to background color
            g.Clear(this.BackgroundColor);

            if (OriginalImage.Width / OriginalImage.Height >= this.Width / this.Height)
            {
                // Fit to width
                double proportion = (double)OriginalImage.Width / this.Width;

                // Calculate offset
                int offset = (this.Height - (int)(OriginalImage.Height / proportion)) / 2;
                g.DrawImage(OriginalImage, 0, offset, this.Width, (int)(OriginalImage.Height / proportion));
            }
            else
            {
                // Fit to height
                double proportion = (double)OriginalImage.Height / this.Height;

                // Calculate offset
                int offset = (this.Width - (int)(OriginalImage.Width / proportion)) / 2;
                g.DrawImage(OriginalImage, offset, 0, (int)(OriginalImage.Width / proportion), this.Height);
            }

            // Save picture

            using (MemoryStream ms = new MemoryStream())
            {
                OriginalImage.Save(ms, this.OutputImageFormat);
                result.Dispose();
                OriginalImage.Dispose();
                return ms.ToArray();
            }
        }

        /// <summary>
        /// scaling happens in memory using the instance variables
        /// </summary>
        /// <returns></returns>
        public byte[] ScaleImage()
        {
            //load data in memory
            MemoryStream msInput = new MemoryStream(this.ImageData);
            Bitmap OriginalImage = (Bitmap)Bitmap.FromStream(msInput);
            Bitmap result = new Bitmap(this.Width, this.Height);            
            Graphics g = Graphics.FromImage(result);

            //setup all the encoding parameters
            System.Drawing.Imaging.ImageCodecInfo imageCodecEncoder = GetEncoder(this.OutputImageFormat);
            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Compression;
            System.Drawing.Imaging.EncoderParameters myEncoderParameters = new System.Drawing.Imaging.EncoderParameters(1);
            System.Drawing.Imaging.EncoderParameter myEncoderParameter = new System.Drawing.Imaging.EncoderParameter(myEncoder, 100L);
            myEncoderParameters.Param[0] = myEncoderParameter;

            g.Clear(this.BackgroundColor);

            if (OriginalImage.Width / OriginalImage.Height >= this.Width / this.Height)
            {
                // Fit to width
                double proportion = (double)OriginalImage.Width / this.Width;
                g.DrawImage(OriginalImage, 0, 0, this.Width, (int)(OriginalImage.Height / proportion));
            }
            else
            {
                // Fit to height
                double proportion = (double)OriginalImage.Height / this.Height;
                g.DrawImage(OriginalImage, 0, 0, (int)(OriginalImage.Width / proportion), this.Height);
            }

            // return the processed result  
            using ( MemoryStream ms = new MemoryStream())
            {
                OriginalImage.Save(ms, imageCodecEncoder, myEncoderParameters);
                //OriginalImage.Save(ms, this.OutputImageFormat);
                result.Dispose();
                OriginalImage.Dispose();
                return ms.ToArray();
            }
        }

        private System.Drawing.Imaging.ImageCodecInfo GetEncoder(System.Drawing.Imaging.ImageFormat format)
        {
            System.Drawing.Imaging.ImageCodecInfo[] codecs = System.Drawing.Imaging.ImageCodecInfo.GetImageDecoders();

            foreach (System.Drawing.Imaging.ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
    }
}