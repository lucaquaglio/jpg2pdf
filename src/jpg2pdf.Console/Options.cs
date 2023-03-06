using CommandLine;

namespace jpg2pdf.Console
{
	sealed class Options
	{
		[Value(0, Required = true, HelpText = "Input files to be processed.")]
		public IEnumerable<string> InputFiles { get; set; }

		[Option('o', "output", Required = false, HelpText = "Output file.")]
		public string OutputFile { get; set; }
	}
}