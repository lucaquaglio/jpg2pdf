using Ardalis.GuardClauses;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace jpg2pdf
{
	public class ImageConverter
	{
		public static Stream ToPdf(Stream imageStream)
		{
			Guard.Against.Null(imageStream, nameof(imageStream));

			var pdfStream = new MemoryStream();
			using var pdfWriter = new PdfWriter(pdfStream);
			pdfWriter.SetCloseStream(false);

			var pdfDoc = new PdfDocument(pdfWriter);

			var document = new Document(pdfDoc);

			var buffer = ConvertStreamInByteArray(imageStream);
			var image = new Image(ImageDataFactory.Create(buffer));

			document.Add(image);
			document.Close();

			pdfStream.Position = 0;
			return pdfStream;
		}

		public static void ToPdf(string fileName)
		{
			Guard.Against.NullOrEmpty(fileName, nameof(fileName));

			var outputFilename = $"{fileName}.pdf";

			using var reader = new FileStream(fileName, FileMode.Open);
			using var pdfStream = ToPdf(reader);
			using var output = new FileStream(outputFilename, FileMode.Create);
			pdfStream.CopyTo(output);
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