using iText.Kernel.Pdf;
using System.Reflection;

namespace jpg2pdf.Test
{
	public class ConverterTest
	{
		[Test]
		public void TestConvertJpgToPdf()
		{
			using var inputImageStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("jpg2pdf.Test.TestFiles.Test1.jpg");
			using var pdfStream = Converter.ConvertoToPdf(inputImageStream);
			{
				Assert.That(pdfStream, Is.Not.Null);
				Assert.That(pdfStream.Length, Is.GreaterThan(0));
				Assert.DoesNotThrow(() => new PdfReader(pdfStream));
			}
		}
	}
}