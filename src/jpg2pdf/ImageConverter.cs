using Ardalis.GuardClauses;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace jpg2pdf
{
	public class ImageConverter
	{
		public static Stream ToPdf(params Stream[] imageStreamCollection)
		{
			Guard.Against.NullOrEmpty(imageStreamCollection, nameof(imageStreamCollection));

			var pdfStream = new MemoryStream();
			using var pdfWriter = new PdfWriter(pdfStream);
			pdfWriter.SetCloseStream(false);

			var pdfDoc = new PdfDocument(pdfWriter);
			var document = new Document(pdfDoc);

			foreach (var imageStream in imageStreamCollection)
			{
				var buffer = ConvertStreamInByteArray(imageStream);
				var image = new Image(ImageDataFactory.Create(buffer));

				document.Add(image);
			}
			document.Close();

			pdfStream.Position = 0;
			return pdfStream;
		}

		public static void ToPdf(string fileName, string outputFileName = null)
		{
			Guard.Against.NullOrEmpty(fileName.Trim(), nameof(fileName));

			outputFileName ??= $"{fileName}.pdf";

			using var reader = new FileStream(fileName, FileMode.Open);
			using var pdfStream = ToPdf(reader);
			using var output = new FileStream(outputFileName, FileMode.Create);
			pdfStream.CopyTo(output);
		}

		public static void ToPdf(string[] inputFiles, string outputFileName = null)
		{
			Guard.Against.NullOrEmpty(inputFiles, nameof(inputFiles));

			outputFileName ??= $"{inputFiles[0]}.pdf";
			outputFileName = outputFileName.Trim();

			var inputStreamCollection = inputFiles
				.Select(x => new FileStream(x, FileMode.Open))
				.Cast<Stream>()
				.ToArray();

			using var pdfStream = ToPdf(inputStreamCollection);
			using var output = new FileStream(outputFileName, FileMode.Create);
			pdfStream.CopyTo(output);

			inputStreamCollection
				.ToList()
				.ForEach(x => x.Dispose());
		}

		static byte[] ConvertStreamInByteArray(Stream stream)
		{
			var buffer = new byte[stream.Length];
			stream.Seek(0, SeekOrigin.Begin);
			stream.Read(buffer, 0, (int)stream.Length);
			return buffer;
		}
	}
}