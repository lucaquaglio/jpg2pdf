namespace jpg2pdf.Test
{
	sealed class Jpg2PdfConsoleTest : TestCase
	{
		[Test, TestCaseSource(nameof(GetTestDataImageFilePathCollection))]
		public void TestMain_WithManyDifferentFiles(string imageFilePath)
		{
			Console.Program.Main(new[] { imageFilePath });

			AssertFileExistsAndIsAValidPdf(imageFilePath);
		}

		[Test, TestCaseSource(nameof(GetTestDataImageFilePathCollection))]
		public void TestMain_OutputFilenameSpecified(string imageFilePath)
		{
			var outputFileNamePath = Path.Combine(TestFileDirectory, "file.pdf");

			Console.Program.Main(new[] { imageFilePath, $"-o{outputFileNamePath}" });
			FileAssert.Exists(outputFileNamePath);
		}

		[Test]
		public void TestMain_WithNoArguments()
		{
			Assert.Throws<InvalidOperationException>(() => Console.Program.Main(Array.Empty<string>()));
		}

		[Test]
		public void TestMain_WithMultipleFiles_NoOutputFilenameSpecified()
		{
			var imageFileCollection = GetTestDataImageFilePathCollection();

			Console.Program.Main(imageFileCollection.ToArray());

			AssertFileExistsAndIsAValidPdf(imageFileCollection.First());
		}

		[Test]
		public void TestMain_WithMultipleFile_OutputFilenameSpecified()
		{
			var args = GetTestDataImageFilePathCollection().ToList();
			var outputOutputFilename = Path.Combine(TestFileDirectory, "Result.pdf");

			args.Add($"-o {outputOutputFilename}");

			Console.Program.Main(args.ToArray());

			FileAssert.Exists(outputOutputFilename);
		}
	}
}