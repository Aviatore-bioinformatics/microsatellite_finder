using System;
using System.Collections.Generic;
using System.Linq;
using microsatellite_finder.Models;
using Serilog.Core;

namespace microsatellite_finder.Services
{
    public class MicrosatelliteCounter
    {
        public MicrosatelliteCounterOptions Options { get; }
        public List<Transcript> Transcripts { get; set; }
        private Logger _logger;
        
        public MicrosatelliteCounter(MicrosatelliteCounterOptions options, List<Transcript> transcripts, Logger logger)
        {
            Options = options;
            Transcripts = transcripts;
            _logger = logger;
        }

        public void FindMicrosatellites(int transcriptCount)
        {
            int count = 0;
            foreach (var transcript in Transcripts)
            {
                count++;
                //Console.Clear();
                //Console.Out.WriteLine($"Transcript: {count}/{transcriptCount}");
                if (count % 1000 == 0)
                {
                    _logger.Debug($"Transcript: {count}/{transcriptCount}");
                }
                transcript.Positions = ScanSequence(transcript).ToArray();
            }
        }

        public List<Position> ScanSequence(Transcript transcript)
        {
            List<Position> positions = new List<Position>();

            for (int repeatedSequenceLength = Options.MinRepeatedSequenceLength;
                repeatedSequenceLength < Options.MaxRepeatedSequenceLength;
                repeatedSequenceLength++)
            {
                for (int i = 0; i < 3; i++)
                {
                    //int repeatedSequenceLength = options.MinRepeatedSequenceLength; 
                    int indexStart = i;
                    int indexEnd = indexStart + repeatedSequenceLength;
                    int firstMerStart = i;

                    int repeatCount = 0;
                
                    while (transcript.Sequence.Length >= indexEnd)
                    {
                        if (transcript.Sequence.Substring(indexStart, repeatedSequenceLength)
                            .Equals(transcript.Sequence.Substring(firstMerStart, repeatedSequenceLength)))
                        {
                            repeatCount++;
                            //Console.Out.WriteLine($"ok {indexStart} ::: {repeatedSequenceLength} ::: {repeatCount} ::: {firstMerStart} ::: {indexStart - 1} {transcript.Sequence.Substring(indexStart, repeatedSequenceLength)} ::: {transcript.Sequence.Substring(firstMerStart, repeatedSequenceLength)}");
                        }
                        else
                        {
                            //Console.Out.WriteLine($"fail {indexStart} ::: {repeatedSequenceLength} ::: {repeatCount} ::: {firstMerStart} ::: {indexStart - 1} {transcript.Sequence.Substring(indexStart, repeatedSequenceLength)} ::: {transcript.Sequence.Substring(firstMerStart, repeatedSequenceLength)}");
                            /*if (repeatCount > 0)
                            {
                                Console.Out.WriteLine($"count: {repeatCount} ::: {firstMerStart} ::: {indexStart}");
                            }*/
                            //Console.Out.WriteLine(repeatCount);
                            if (repeatCount >= Options.MinRepeatNumber)
                            {
                                //Console.Out.WriteLine(repeatCount);
                                var position = new Position()
                                {
                                    Start = firstMerStart,
                                    End = indexStart - 1,
                                    MerLen = repeatedSequenceLength
                                };
                                //Console.Out.WriteLine($"Added: {position.Start} <> {position.End} <> {position.MerLen}");
                                positions.Add(position);
                            }
                            repeatCount = 0;

                            firstMerStart = indexStart;
                            
                            indexStart -= repeatedSequenceLength;
                        }

                        indexStart += repeatedSequenceLength;
                        indexEnd = indexStart + repeatedSequenceLength;
                    }
                    
                    if (repeatCount >= Options.MinRepeatNumber)
                    {
                        //Console.Out.WriteLine(repeatCount);
                        var position = new Position()
                        {
                            Start = firstMerStart,
                            End = indexStart - 1,
                            MerLen = repeatedSequenceLength
                        };
                        //Console.Out.WriteLine($"Added: {position.Start} <> {position.End} <> {position.MerLen}");
                        positions.Add(position);
                    }
                }
            }

            //var pos = positions.OrderBy(p => p.MerLen).ThenBy(p => p.Start).ToList();
            var pos = positions.OrderBy(p => p.Start).ToList();
            for (int i = pos.Count() - 1; i >= 1; i--)
            {
                if (pos[i].Start + 1 < pos[i - 1].End && pos[i - 1].Start + 1 < pos[i].End)
                {
                    pos.RemoveAt(i);
                }
            }

            return pos;
            /*Console.Out.WriteLine($"positions count: {positions.Count}");
            return positions.OrderBy(p => p.Start).ToList();*/
        }
    }
}