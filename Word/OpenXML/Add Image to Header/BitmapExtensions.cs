using System.Drawing;


namespace PZone.Samples
{
    public static class BitmapExtensions
    {
        const int EMUsPerInch = 914400;


        public static byte[] ToByteArray(this Bitmap bmp)
        {
            return (byte[])new ImageConverter().ConvertTo(bmp, typeof(byte[]));
        }


        /// <summary>
        /// Получение ширины изображения в EMU (English Metric Units).
        /// </summary>
        public static long GetWidthInEMUs(this Bitmap img)
        {
            return (long)(img.Width / img.HorizontalResolution * EMUsPerInch);
        }


        /// <summary>
        /// Получение высоты изображения в EMU (English Metric Units).
        /// </summary>
        public static long GetHeightInEMUs(this Bitmap img)
        {
            return (long)(img.Height / img.VerticalResolution * EMUsPerInch);
        }
    }
}
