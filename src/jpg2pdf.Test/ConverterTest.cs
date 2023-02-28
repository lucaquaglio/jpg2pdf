using iText.Kernel.Pdf;
using System.Reflection;

namespace jpg2pdf.Test
{
	sealed class ImageConverterTest
	{
		[Test, TestCaseSource(nameof(GetTestDataResourceFileNames))]
		public void TesToPdf(string resourceFilename)
		{
			using var inputImageStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceFilename);
			{
				Assert.That(inputImageStream, Is.Not.Null);

				using var pdfStream = ImageConverter.ToPdf(inputImageStream);
				{
					Assert.That(pdfStream, Is.Not.Null);
					Assert.That(pdfStream.Length, Is.GreaterThan(0));
					Assert.DoesNotThrow(() => new PdfReader(pdfStream));
				}
			}
		}

		[Test]
		public void TestGuardClause()
		{
			Assert.Throws<ArgumentNullException>(() => ImageConverter.ToPdf(null));
			Assert.Throws<iText.IO.Exceptions.IOException>(() => ImageConverter.ToPdf(Stream.Null));
		}

		static IEnumerable<string> GetTestDataResourceFileNames()
		{
			yield return "jpg2pdf.Test.TestFiles.Test1.jpg";
			yield return "jpg2pdf.Test.TestFiles.Test2.png";
			yield return "jpg2pdf.Test.TestFiles.Test3.gif";
		}
	}
}