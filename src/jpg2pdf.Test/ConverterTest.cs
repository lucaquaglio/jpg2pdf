using iText.Kernel.Pdf;
using System.Reflection;

namespace jpg2pdf.Test
{
	sealed class ImageConverterTest
	{
		[Test, TestCaseSource(nameof(GetTestDataResourceFileNames))]
		public void TesToPdf(string resourceName)
		{
			using Stream? inputImageStream = GetResourceStream(resourceName);
			{
				Assert.That(inputImageStream, Is.Not.Null);

				using var pdfStream = ImageConverter.ToPdf(inputImageStream);
				{
					Assert.That(pdfStream, Is.Not.Null);
					Assert.That(pdfStream.Length, Is.GreaterThan(0));
					Assert.DoesNotThrow(() => new PdfReader(pdfStream).Close());
				}
			}
		}

		[Test, TestCaseSource(nameof(GetTestDataResourceFileNames))]
		public void TestToPdf_ExportToFile(string resourceName)
		{
			var expectedFilenameResult = $"{resourceName}.pdf";

			using (var resourceStream = GetResourceStream(resourceName))
			using (var fileStream = new FileStream(resourceName, FileMode.Create))
			{
				resourceStream.CopyTo(fileStream);
			}

			try
			{
				ImageConverter.ToPdf(resourceName);

				Assert.That(File.Exists(expectedFilenameResult), Is.True);
				Assert.DoesNotThrow(() => new PdfReader(expectedFilenameResult).Close());
			}
			finally
			{
				File.Delete(resourceName);
				File.Delete(expectedFilenameResult);
			}
		}

		[Test]
		public void TestGuardClause()
		{
			Assert.Throws<ArgumentNullException>(() => ImageConverter.ToPdf(imageStream: null));
			Assert.Throws<ArgumentException>(() => ImageConverter.ToPdf(string.Empty));

			Assert.Throws<iText.IO.Exceptions.IOException>(() => ImageConverter.ToPdf(Stream.Null));
			Assert.Throws<System.IO.FileNotFoundException>(() => ImageConverter.ToPdf("file not exists"));
		}

		static IEnumerable<string> GetTestDataResourceFileNames()
		{
			yield return "jpg2pdf.Test.TestFiles.Test1.jpg";
			yield return "jpg2pdf.Test.TestFiles.Test2.png";
			yield return "jpg2pdf.Test.TestFiles.Test3.gif";
		}

		static Stream GetResourceStream(string resourceName)
		{
			return Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName) ?? throw new InvalidOperationException();
		}
	}
}