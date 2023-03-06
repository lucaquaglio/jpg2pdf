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

			ImageConverter.ToPdf(resourceName);

			Assert.That(File.Exists(expectedFilenameResult), Is.True);
			Assert.DoesNotThrow(() => new PdfReader(expectedFilenameResult).Close());
		}

		[Test, TestCaseSource(nameof(GetTestDataResourceFileNames))]
		public void TestToPdf_ExportToFile_OutputFileNameSpecified(string resourceName)
		{
			TestHelper.WriteFileGivenResourceName(resourceName);
			var expectedFilenameResult = "result.pdf";

			ImageConverter.ToPdf(resourceName, expectedFilenameResult);

			Assert.That(File.Exists(expectedFilenameResult), Is.True);
			Assert.DoesNotThrow(() => new PdfReader(expectedFilenameResult).Close());
		}

		[Test]
		public void TestToPdf_ExportToFile_MultipleImagesInput()
		{
			var inputResourceFileNames = GetTestDataResourceFileNames();
			foreach (var resourceName in inputResourceFileNames)
			{
				TestHelper.WriteFileGivenResourceName(resourceName);
			}
			var expectedFilenameResult = TestHelper.GetPdfFileNameGivenImageFileName(inputResourceFileNames.ElementAt(0));

			ImageConverter.ToPdf(inputResourceFileNames.ToArray());

			Assert.That(File.Exists(expectedFilenameResult), Is.True);
			Assert.DoesNotThrow(() => new PdfReader(expectedFilenameResult).Close());
		}

		[Test]
		public void TestToPdf_ExportToFile_MultipleImagesInput_OutputFilenameSpecified()
		{
			var inputResourceFileNames = GetTestDataResourceFileNames();
			foreach (var resourceName in inputResourceFileNames)
			{
				TestHelper.WriteFileGivenResourceName(resourceName);
			}
			var outputFilename = "result.pdf";

			ImageConverter.ToPdf(inputResourceFileNames.ToArray(), outputFilename);

			Assert.That(File.Exists(outputFilename), Is.True);
			Assert.DoesNotThrow(() => new PdfReader(outputFilename).Close());

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

		[TearDown]
		public void TearDown()
		{
			var currentDirectory = Directory.GetCurrentDirectory();
			var allFiles = Directory.GetFiles(currentDirectory)
				.Where(x =>
				{
					return x.EndsWith(".jpg")
					|| x.EndsWith(".png")
					|| x.EndsWith(".gif")
					|| x.EndsWith(".pdf");
				});

			foreach (var item in allFiles)
			{
				File.Delete(item);
			}
		}

		static IEnumerable<string> GetTestDataResourceFileNames() => TestHelper.GetTestDataResourceFileNames();

	}
}