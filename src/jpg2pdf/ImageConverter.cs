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
			var pdfDoc = new PdfDocument(new PdfWriter(pdfStream));

			var document = new Document(pdfDoc);

			var buffer = ConvertStreamInByteArray(imageStream);
			var image = new Image(ImageDataFactory.Create(buffer));
			document.Add(image);

			pdfStream.Position = 0;
			return pdfStream;
		}

		public static void ToPdf(string fileName)
		{
			Guard.Against.

			var outputFilename = $"{fileName}.pdf";			
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