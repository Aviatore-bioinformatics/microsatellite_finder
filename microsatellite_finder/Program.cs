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

            var fr = new FastaReader("Data/Trinity.fasta");
            fr.ReadFasta();

            var mco = new MicrosatelliteCounterOptions()
            {
                MinRepeatedSequenceLength = 3,
                MaxRepeatedSequenceLength = 10,
                MinRepeatNumber = 6,
                MaxRepeatNumber = 3 // not used yet
            };
            var mc = new MicrosatelliteCounter(mco, fr.Transcripts);
            mc.FindMicrosatellites(fr.transcriptCount);

            using (var sw = new StreamWriter("Data/output.fasta"))
            {
                foreach (var transcript in mc.Transcripts)
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