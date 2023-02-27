using EnsureThat;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace jpg2pdf
{
	public class Converter
	{
		public static Stream ConvertImageToPdf(Stream imageStream)
		{
			Ensure.That(imageStream is not null, nameof(imageStream));
			Ensure.That(imageStream!.Length > 0);

			var buffer = ConvertStreamInByteArray(imageStream);

			var pdfStream = new MemoryStream();

			var pdfDoc = new PdfDocument(new PdfWriter(pdfStream));
			var image = new Image(ImageDataFactory.Create(buffer));

			var document = new Document(pdfDoc);
			document.Add(image);

			pdfStream.Position = 0;
			return pdfStream;
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