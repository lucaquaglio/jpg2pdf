namespace jpg2pdf.Console
{
	public class Program
	{
		public static void Main(string[] args)
		{
			switch (args.Length)
			{
				case 0:
					throw new InvalidOperationException("Any file to convert");
				case > 1:
					throw new InvalidOperationException("Jpg2Pdf can convert only one file at a time");
				default:
					break;
			}

			ImageConverter.ToPdf(args[0]);
		}
	}
}