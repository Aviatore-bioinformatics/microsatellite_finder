using System;
using System.Linq;
using microsatellite_finder.Models;
using microsatellite_finder.Services;

namespace microsatellite_finder
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var fr = new FastaReader("Data/Trinity2.fasta");
            fr.ReadFasta();

            var mco = new MicrosatelliteCounterOptions()
            {
                MinRepeatedSequenceLength = 3,
                MaxRepeatedSequenceLength = 10,
                MinRepeatNumber = 3,
                MaxRepeatNumber = 10
            };
            var mc = new MicrosatelliteCounter(mco, fr.Transcripts);
            mc.FindMicrosatellites();

            foreach (var transcript in mc.Transcripts)
            {
                if (transcript.Positions.Length > 0)
                {
                    Console.Out.WriteLine($"Transcript name: {transcript.Name}");
                    Console.Out.WriteLine($"Microsatellites:");
                    foreach (var transcriptPosition in transcript.Positions.OrderByDescending(p => p.MerLen))
                    {
                        Console.Out.WriteLine(
                            $"Start: {transcriptPosition.Start} End: {transcriptPosition.End} MerLen: {transcriptPosition.MerLen}");
                    }

                    Console.Out.WriteLine(transcript.ToString());
                }
            }
        }
    }
}