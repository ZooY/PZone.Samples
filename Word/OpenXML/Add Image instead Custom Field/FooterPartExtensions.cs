using System.IO;
using DocumentFormat.OpenXml.Packaging;


namespace PZone.Samples
{
    public static class FooterPartExtensions
    {
        public static ImagePart AddImagePart(this FooterPart footerPart, Stream stream)
        {
            var imagePart = footerPart.AddImagePart(ImagePartType.Jpeg);
            imagePart.FeedData(stream);
            return imagePart;
        }
    }
}