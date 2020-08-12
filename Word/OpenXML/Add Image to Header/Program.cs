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
            using (MemoryStream imageStream = new MemoryStream(image.ToByteArray()))
            using (MemoryStream docStream = new MemoryStream())
            {
                docStream.Write(wordBytes, 0, wordBytes.Length);
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(docStream, true))
                {
                    MainDocumentPart mainPart = wordDoc.MainDocumentPart;
                    var headerPart = mainPart.HeaderParts.First();
                                        
                    ImagePart imagePart = headerPart.AddImagePart(imageStream);
                    var imageElement = headerPart.CreateElement(imagePart, "Image", image.GetWidthInEMUs()/3, image.GetHeightInEMUs()/3, "right", 0, 0, 0);
                    headerPart.Header.AppendChild(new Paragraph(new Run(imageElement)));
                }
                wordBytes = docStream.ToArray();
            }
            File.WriteAllBytes("Result.docx", wordBytes);
        }
    }
}