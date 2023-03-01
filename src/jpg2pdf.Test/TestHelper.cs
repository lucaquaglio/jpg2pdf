using System.Reflection;

namespace jpg2pdf.Test
{
	static class TestHelper
	{
		public static string GetPdfFileNameGivenImageFileName(string imageFilename) => $"{imageFilename}.pdf";

		public static void WriteFileGivenResourceName(string resourceName)
		{
			using var resourceStream = GetResourceStream(resourceName);
			using var fileStream = new FileStream(resourceName, FileMode.Create);
			resourceStream.CopyTo(fileStream);
		}

		public static Stream GetResourceStream(string resourceName)
		{
			return Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName) ?? throw new InvalidOperationException();
		}

		public const string Test1ResourceName = "jpg2pdf.Test.TestFiles.Test1.jpg";
		public const string Test2ResourceName = "jpg2pdf.Test.TestFiles.Test2.jpg";
		public const string Test3ResourceName = "jpg2pdf.Test.TestFiles.Test3.jpg";
	}
}