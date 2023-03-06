using CommandLine;

namespace jpg2pdf.Console
{
	public class Program
	{
		public static void Main(string[] args)
		{
			if (args.Length is 0)
			{
				throw new InvalidOperationException();
			}

			var option = Parser.Default.ParseArguments<Options>(args).Value;

			ImageConverter.ToPdf(option.InputFiles.ToArray(), option.OutputFile);
		}
	}
}