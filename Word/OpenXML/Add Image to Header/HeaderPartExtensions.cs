using System.IO;
using DocumentFormat.OpenXml.Packaging;


namespace PZone.Samples
{
    public static class HeaderPartExtensions
    {
        public static ImagePart AddImagePart(this HeaderPart headerPart, Stream stream)
        {
            var imagePart = headerPart.AddImagePart(ImagePartType.Jpeg);
            imagePart.FeedData(stream);
            return imagePart;
        }
    }
}