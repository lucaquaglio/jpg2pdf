using iText.Kernel.Pdf;
using iText.Layout;

namespace jpg2pdf.Test
{
	sealed class ImageConverterTest
	{
		[Test, TestCaseSource(nameof(GetTestDataResourceFileNames))]
		public void TesToPdf(string resourceName)
		{
			using Stream inputImageStream = TestHelper.GetResourceStream(resourceName);
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
			TestHelper.WriteFileGivenResourceName(resourceName);
			var expectedFilenameResult = TestHelper.GetPdfFileNameGivenImageFileName(resourceName);

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

		[Test, TestCaseSource(nameof(GetTestDataResourceFileNames))]
		public void TestToPdf_ExportToFile_OutputFileNameSpecified(string resourceName)
		{
			TestHelper.WriteFileGivenResourceName(resourceName);
			var expectedFilenameResult = "result.pdf";

			try
			{
				ImageConverter.ToPdf(resourceName, expectedFilenameResult);

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
			Assert.Throws<ArgumentNullException>(() => ImageConverter.ToPdf(imageStreamCollection: null));
			Assert.Throws<ArgumentException>(() => ImageConverter.ToPdf(string.Empty));
			Assert.Throws<ArgumentException>(() => ImageConverter.ToPdf(Enumerable.Empty<Stream>().ToArray()));

			Assert.Throws<iText.IO.Exceptions.IOException>(() => ImageConverter.ToPdf(Stream.Null));
			Assert.Throws<FileNotFoundException>(() => ImageConverter.ToPdf("file not exists"));
		}

		[Test]
		public void TestToPdf_UsingMultipleImages()
		{
			var images = TestHelper.GetTestDataResourceFileNames()
				.Select(x => TestHelper.GetResourceStream(x))
				.ToArray();

			using var pdfResultStream = ImageConverter.ToPdf(images);
			{
				Assert.That(pdfResultStream, Is.Not.Null);
				Assert.That(pdfResultStream.Length, Is.GreaterThan(0));

				using var pdfReader = new PdfReader(pdfResultStream);
				using var document = new PdfDocument(pdfReader);

				Assert.That(document.GetNumberOfPages(), Is.EqualTo(3));
			}
		}

		static IEnumerable<string> GetTestDataResourceFileNames() => TestHelper.GetTestDataResourceFileNames();

	}
}