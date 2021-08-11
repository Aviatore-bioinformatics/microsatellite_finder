using System;
using microsatellite_finder.Models;
using microsatellite_finder.Services;

namespace microsatellite_finder
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var fr = new FastaReader("Data/Trinity.fasta");
            fr.ReadFasta();

            var mco = new MicrosatelliteCounterOptions()
            {
                MinRepeatedSequenceLength = 3,
                MaxRepeatedSequenceLength = 10,
                MinRepeatNumber = 3,
                MaxRepeatNumber = 10
            };
            var mc = new MicrosatelliteCounter(mco, fr.Transcripts);
            var result = mc.ScanSequence(fr.Transcripts[0], mco);

            Console.Out.WriteLine($"Microsatellites Count: {result.Count}");
            foreach (var position in result)
            {
                Console.Out.WriteLine($"start: {position.Start} stop: {position.End}");
            }
        }
    }
}