using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

class Program
{
    static void Main(string[] args)
    {
        using (Stream inputPdfStream = new FileStream("Sample.pdf", FileMode.Open, FileAccess.Read, FileShare.Read))
        using (Stream inputImageStream = new FileStream("barcode.png", FileMode.Open, FileAccess.Read, FileShare.Read))
        using (Stream outputPdfStream = new FileStream("Result.pdf", FileMode.Create, FileAccess.Write, FileShare.None))
        {
            var reader = new PdfReader(inputPdfStream);
            var stamper = new PdfStamper(reader, outputPdfStream);
            
            Image image = Image.GetInstance(inputImageStream);

            var pdfContentByte = stamper.GetOverContent(1);
            var pageSize = reader.GetPageSize(1);

            // Добавляем картинку в правый верхний угол, с отступом 20 от верха и правого края
            image.SetAbsolutePosition(pageSize.Width - image.Width - 20, pageSize.Height - image.Height - 20);

            pdfContentByte.AddImage(image);
            stamper.Close();
        }
    }
}