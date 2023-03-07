using iText.Kernel.Pdf;
using iText.Layout;

namespace jpg2pdf.Test
{
	sealed class ImageConverterTest : TestCase
	{
		[Test, TestCaseSource(nameof(GetTestDataResourceNames))]
		public void TesToPdf(string resourceName)
		{
			using Stream inputImageStream = GetResourceStream(resourceName);
			{
				Assert.That(inputImageStream, Is.Not.Null);

				using var pdfStream = ImageConverter.ToPdf(inputImageStream);
				{
					AssertStreamIsAValidPdf(pdfStream);
				}
			}
		}

		[Test, TestCaseSource(nameof(GetTestDataImageFilePathCollection))]
		public void TestToPdfFile(string imageFilePath)
		{
			ImageConverter.ToPdfFile(imageFilePath);

			AssertFileExistsAndIsAValidPdf(imageFilePath);
		}

		[Test, TestCaseSource(nameof(GetTestDataImageFilePathCollection))]
		public void TestToPdfFile_OutputFileNameSpecified(string imageFilePath)
		{
			var expectedFilenameResult = Path.Combine(TestFileDirectory, "result.pdf");

			ImageConverter.ToPdfFile(imageFilePath, expectedFilenameResult);

			AssertFileExistsAndIsAValidPdf(expectedFilenameResult);
		}

		[Test]
		public void TestToPdfToFile_MultipleImagesInput()
		{
			var imageFileCollection = GetTestDataImageFilePathCollection();

			ImageConverter.ToPdfFile(imageFileCollection.ToArray());

			AssertFileExistsAndIsAValidPdf(imageFileCollection.First());
		}

		[Test]
		public void TestToPdfToFile_MultipleImagesInput_OutputFilenameSpecified()
		{
			var outputFilename = Path.Combine(TestFileDirectory, "result.pdf");
			var imageFileCollection = GetTestDataImageFilePathCollection();

			ImageConverter.ToPdfFile(imageFileCollection.ToArray(), outputFilename);

			AssertFileExistsAndIsAValidPdf(outputFilename);
		}


		[Test]
		public void TestGuardClause()
		{
			Assert.Throws<ArgumentNullException>(() => ImageConverter.ToPdf(imageStreamCollection: null));
			Assert.Throws<ArgumentException>(() => ImageConverter.ToPdfFile(string.Empty));
			Assert.Throws<ArgumentException>(() => ImageConverter.ToPdf(Enumerable.Empty<Stream>().ToArray()));

			Assert.Throws<iText.IO.Exceptions.IOException>(() => ImageConverter.ToPdf(Stream.Null));
			Assert.Throws<FileNotFoundException>(() => ImageConverter.ToPdfFile("file not exists"));
		}

		[Test]
		public void TestToPdf_UsingMultipleImages()
		{
			var imageStreamCollection = GetTestDataResourceNames()
				.Select(x => GetResourceStream(x))
				.ToArray();

			using var pdfResultStream = ImageConverter.ToPdf(imageStreamCollection);
			{
				AssertStreamIsAValidPdf(pdfResultStream);
			}
		}

		void AssertStreamIsAValidPdf(Stream stream)
		{
			using var reader = new PdfReader(stream);
			using var pdfDoc = new PdfDocument(reader);

			Assert.That(pdfDoc.GetNumberOfPages(), Is.GreaterThan(0));
		}
	}
}