using System;
using System.IO;
using System.Linq;
using microsatellite_finder.Models;
using microsatellite_finder.Services;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Configuration;

namespace microsatellite_finder
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
            
            var commandLineOptions = CommandLine.Parser.Default.ParseArguments<CommandLineOptions>(args);
            if (commandLineOptions.Errors.Any())
            {
                return;
            }

            if (!commandLineOptions.Value.CheckOptions())
            {
                logger.Error($"The file {commandLineOptions.Value.FastaFileName} does not exists");
                return;
            }

            var fr = new FastaReader(Path.Combine(Directory.GetCurrentDirectory(), commandLineOptions.Value.FastaFileName));
            fr.ReadFasta();

            var mco = new MicrosatelliteCounterOptions()
            {
                MinRepeatedSequenceLength = 3,
                MaxRepeatedSequenceLength = 10,
                MinRepeatNumber = 3,
                MaxRepeatNumber = 3 // not used yet
            };
            var mc = new MicrosatelliteCounter(mco, fr.Transcripts, logger);
            mc.FindMicrosatellites(fr.transcriptCount);

            using (var sw = new StreamWriter(Path.Combine(Directory.GetCurrentDirectory(), commandLineOptions.Value.OutputFileName)))
            {
                foreach (var transcript in mc.Transcripts.Where(p => p.Positions.Any()).OrderByDescending(p => p.Positions.OrderByDescending(pp => pp.MerLen).First().MerLen))
                {
                    if (transcript.Positions.Length > 0)
                    {
                        /*Console.Out.WriteLine($"Transcript name: {transcript.Name}");
                        Console.Out.WriteLine($"Microsatellites:");
                        foreach (var transcriptPosition in transcript.Positions.OrderByDescending(p => p.MerLen))
                        {
                            Console.Out.WriteLine(
                                $"Start: {transcriptPosition.Start} End: {transcriptPosition.End} MerLen: {transcriptPosition.MerLen}");
                        }*/

                        sw.Write(transcript.ToString() + '\n');
                    }
                }
            }
            
        }
    }
}