using System.Reflection;

namespace jpg2pdf.Test
{
	class TestCase
	{

		[SetUp]
		public virtual void SetUp()
		{
			WriteAllFilesInTempFolder();
		}

		[TearDown]
		public virtual void TearDown()
		{
			Directory.Delete(TestFileDirectory, recursive: true);
		}

		protected static string GetPdfFileNameGivenImageFileName(string imageFilename) => $"{imageFilename}.pdf";

		protected void AssertFileExists(string imageFilePath)
		{
			var pdfFilePath = GetPdfFileNameGivenImageFileName(imageFilePath);
			FileAssert.Exists(pdfFilePath);
		}

		protected static void WriteFileGivenResourceName(string resourceName)
		{
			using var resourceStream = GetResourceStream(resourceName);
			using var fileStream = new FileStream(resourceName, FileMode.Create);
			resourceStream.CopyTo(fileStream);
		}

		protected static Stream GetResourceStream(string resourceName)
		{
			return Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
		}

		protected static IEnumerable<string> GetTestDataResourceNames()
		{
			yield return TestHelper.Test1ResourceName;
			yield return TestHelper.Test2ResourceName;
			yield return TestHelper.Test3ResourceName;
		}

		protected static IEnumerable<string> GetTestDataImageFilePathCollection()
		{
			var testFileDirectory = TestFileDirectory;

			foreach (var item in GetTestDataResourceNames())
			{
				yield return Path.Combine(testFileDirectory, item);
			}
		}

		protected const string Test1ResourceName = "jpg2pdf.Test.TestFiles.Test1.jpg";
		protected const string Test2ResourceName = "jpg2pdf.Test.TestFiles.Test2.png";
		protected const string Test3ResourceName = "jpg2pdf.Test.TestFiles.Test3.gif";

		void WriteAllFilesInTempFolder()
		{
			var testFilesDirectory = TestFileDirectory;
			if (!Directory.Exists(testFilesDirectory))
			{
				Directory.CreateDirectory(testFilesDirectory);
			}

			foreach (var resourceName in GetTestDataResourceNames())
			{
				var testFilePath = Path.Combine(testFilesDirectory, resourceName);
				using var resourceStream = GetResourceStream(resourceName);
				using var fileStream = new FileStream(testFilePath, FileMode.Create);
				resourceStream.CopyTo(fileStream);
			}
		}

		static string TestFileDirectory { get; } = Path.Combine(TestContext.CurrentContext.TestDirectory, "Files");
	}
}