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

		[Test, TestCaseSource(nameof(GetTestDataFileNamePaths))]
		public void TestToPdf_ExportToFile(string filename)
		{
			ImageConverter.ToPdf(filename);

			var expectedFilenameResult = $"{filename}.pdf";
			Assert.That(File.Exists(expectedFilenameResult), Is.True);
			Assert.DoesNotThrow(() => new PdfReader(expectedFilenameResult));

			File.Delete(expectedFilenameResult);
		}

		[Test]
		public void TestGuardClause()
		{
			Assert.Throws<ArgumentNullException>(() => ImageConverter.ToPdf(imageStream: null));
			Assert.Throws<ArgumentNullException>(() => ImageConverter.ToPdf(string.Empty));

			Assert.Throws<iText.IO.Exceptions.IOException>(() => ImageConverter.ToPdf(Stream.Null));
			Assert.Throws<System.IO.FileNotFoundException>(() => ImageConverter.ToPdf("<invalid path>"));
		}

		static IEnumerable<string> GetTestDataResourceFileNames()
		{
			yield return "jpg2pdf.Test.TestFiles.Test1.jpg";
			yield return "jpg2pdf.Test.TestFiles.Test2.png";
			yield return "jpg2pdf.Test.TestFiles.Test3.gif";
		}

		static IEnumerable<string> GetTestDataFileNamePaths()
		{
			yield return "TestFiles\\Test.jpg";
		}
	}
}