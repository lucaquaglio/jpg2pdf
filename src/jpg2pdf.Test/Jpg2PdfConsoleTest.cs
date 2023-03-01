namespace jpg2pdf.Test
{
	sealed class Jpg2PdfConsoleTest
	{
		[Test, TestCaseSource(nameof(GetTestDataResourceFileNames))]
		public void TestConvertFile(string resourceName)
		{
			Console.Program.Main(new[] { resourceName });

			var expectedOutputFilename = TestHelper.GetPdfFileNameGivenImageFileName(resourceName);
			Assert.That(File.Exists(expectedOutputFilename), Is.True);
		}


		[SetUp]
		public void SetUp()
		{
			foreach (var file in GetTestDataResourceFileNames())
			{
				TestHelper.WriteFileGivenResourceName(file);
			}
		}

		[TearDown]
		public void TearDown()
		{
			foreach (var file in GetTestDataResourceFileNames())
			{
				File.Delete(file);
				File.Delete(TestHelper.GetPdfFileNameGivenImageFileName(file));
			}
		}

		static IEnumerable<string> GetTestDataResourceFileNames() => TestHelper.GetTestDataResourceFileNames();
	}
}