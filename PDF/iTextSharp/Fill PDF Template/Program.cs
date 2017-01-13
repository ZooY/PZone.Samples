using System.IO;
using iTextSharp.text.pdf;


namespace PZone.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            var pdfTemplate = @"tmp.pdf";
            var pdfFile = @"result.pdf";

            using (var templateReader = new PdfReader(pdfTemplate))
            {
                using (var resultStamper = new PdfStamper(templateReader, new FileStream(pdfFile, FileMode.Create)))
                {
                    // Получаем ссылку на форму с полями.
                    var form = resultStamper.AcroFields;
                    // Получаем все шрифты формы.
                    var fonts = templateReader.GetFormFonts();

                    // Установка значений полей формы.
                    form.SetFieldWithFont(templateReader, fonts, "LastName", "Копаев");
                    form.SetFieldWithFont(templateReader, fonts, "FormerLastName", "Новиков");
                    form.SetField("Embassyconsulate", "On");

                    // Установка запрета на редактирование полей.
                    resultStamper.FormFlattening = true;

                    resultStamper.Close();
                }
                templateReader.Close();
            }
        }
    }
}