namespace jpg2pdf.Test
{
	sealed class Jpg2PdfConsoleTest : TestCase
	{
		[Test, TestCaseSource(nameof(GetTestDataImageFilePathCollection))]
		public void TestMain_WithManyDifferentFiles(string imageFilePath)
		{
			Console.Program.Main(new[] { imageFilePath });

			AssertFileExists(imageFilePath);
		}

		[Test, TestCaseSource(nameof(GetTestDataResourceNames))]
		public void TestMain_OutputFilenameSpecified(string resourceName)
		{
			var outputFilename = "file.pdf";

			Console.Program.Main(new[] { resourceName, $"-o{outputFilename}" });
			Assert.That(File.Exists(outputFilename), Is.True);
		}

		[Test]
		public void TestMain_WithNoArguments()
		{
			Assert.Throws<InvalidOperationException>(() => Console.Program.Main(Array.Empty<string>()));
		}

		[Test]
		public void TestMain_WithMultipleFiles_NoOutputFilenameSpecified()
		{
			var imageFileNameCollection = GetTestDataResourceNames();
			var expectedOutputFilename = TestHelper.GetPdfFileNameGivenImageFileName(imageFileNameCollection.ElementAt(0));

			Console.Program.Main(imageFileNameCollection.ToArray());

			Assert.That(File.Exists(expectedOutputFilename), Is.True);
		}

		[Test]
		public void TestMain_WithMultipleFile_OutputFilenameSpecified()
		{
			var args = GetTestDataResourceNames().ToList();
			var outputOutputFilename = "Result.pdf";

			args.Add($"-o {outputOutputFilename}");

			Console.Program.Main(args.ToArray());

			Assert.That(File.Exists(outputOutputFilename), Is.True);
		}

		[SetUp]
		public void SetUp()
		{
			foreach (var file in GetTestDataResourceNames())
			{
				TestHelper.WriteFileGivenResourceName(file);
			}
		}
	}
}