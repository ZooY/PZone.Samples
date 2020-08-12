using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;


namespace PZone.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            var wordBytes = File.ReadAllBytes("Sample.docx");
            var image = new Bitmap("Sample.png");
            var imageWidth = image.GetWidthInEMUs() / 10;
            var imageHeight = image.GetHeightInEMUs() / 10;

            using (MemoryStream docStream = new MemoryStream())
            {
                docStream.Write(wordBytes, 0, wordBytes.Length);
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(docStream, true))
                {
                    MainDocumentPart mainPart = wordDoc.MainDocumentPart;

                    var parts = new List<OpenXmlPart>() { mainPart };
                    parts.AddRange(mainPart.HeaderParts);
                    parts.AddRange(mainPart.FooterParts);

                    foreach (var part in parts)
                        InsertImage(part, "BARCODE", image, imageWidth, imageHeight);
                }
                wordBytes = docStream.ToArray();
            }
            File.WriteAllBytes("Result.docx", wordBytes);
        }

        
        private static void InsertImage(OpenXmlPart part, string fieldName, Bitmap image, long imageWidth, long imageHeight)
        {
            var mergeFields = part.RootElement.Descendants<FieldCode>().Where(f => f.InnerText.Contains(fieldName));
            if (!mergeFields.Any())
                return;
            using (MemoryStream imageStream = new MemoryStream(image.ToByteArray()))
            {
                ImagePart imagePart;
                if (part is MainDocumentPart)
                    imagePart = ((MainDocumentPart)part).AddImagePart(imageStream);
                else if (part is HeaderPart)
                    imagePart = ((HeaderPart)part).AddImagePart(imageStream);
                else
                    imagePart = ((FooterPart)part).AddImagePart(imageStream);
                foreach (var field in mergeFields)
                {
                    Run rFldCode = (Run)field.Parent;
                    Run rBegin = rFldCode.PreviousSibling<Run>();
                    Run rEnd = rFldCode.NextSibling<Run>();
                    rFldCode.RemoveAllChildren();
                    rBegin.Remove();
                    rEnd.Remove();
                    Drawing imageElement = part.CreateElement(imagePart, "Image", imageWidth, imageHeight);
                    rFldCode.AppendChild(imageElement);
                }
            }
        }
    }
}