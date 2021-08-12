using System.IO;
using CommandLine;
using Serilog.Core;

namespace microsatellite_finder.Models
{
    public class CommandLineOptions
    {
        [Option(shortName: 'i', longName: "input", Required = true, HelpText = "FASTA file")]
        public string FastaFileName { get; set; }

        [Option(shortName: 'o', longName: "output", Required = false, HelpText = "Output file (default: output.fasta)")]
        public string OutputFileName { get; set; }

        [Option(shortName: 'k', HelpText = "Keep sequences with no micrsatellites")]
        public bool KeepSequenceWithNoMicrosatellites { get; set; }

        public bool CheckOptions()
        {
            return File.Exists(Path.Combine(Directory.GetCurrentDirectory(), FastaFileName));
        }
    }
}