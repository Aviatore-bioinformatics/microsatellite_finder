using System;
using System.Collections.Generic;
using System.Linq;
using microsatellite_finder.Models;

namespace microsatellite_finder.Services
{
    public class MicrosatelliteCounter
    {
        public MicrosatelliteCounterOptions Options { get; }
        public List<Transcript> Transcripts { get; set; }
        
        public MicrosatelliteCounter(MicrosatelliteCounterOptions options, List<Transcript> transcripts)
        {
            Options = options;
            Transcripts = transcripts;
        }

        public void FindMicrosatellites()
        {
            foreach (var transcript in Transcripts)
            {
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
                            //Console.Out.WriteLine($"ok {transcript.Sequence.Substring(indexStart, repeatedSequenceLength)} ::: {transcript.Sequence.Substring(firstMerStart, repeatedSequenceLength)}");
                        }
                        else
                        {
                            //Console.Out.WriteLine($"fail {transcript.Sequence.Substring(indexStart, repeatedSequenceLength)} ::: {transcript.Sequence.Substring(firstMerStart, repeatedSequenceLength)}");
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
                                positions.Add(position);
                            }
                            repeatCount = 0;

                            firstMerStart = indexStart;
                            
                            indexStart -= repeatedSequenceLength;
                        }

                        indexStart += repeatedSequenceLength;
                        indexEnd = indexStart + repeatedSequenceLength;
                    }
                }
            }

            //var pos = positions.OrderBy(p => p.MerLen).ThenBy(p => p.Start).ToList();
            var pos = positions.OrderBy(p => p.Start).ToList();
            for (int i = pos.Count() - 1; i >= 1; i--)
            {
                if (pos[i].Start < pos[i - 1].End && pos[i - 1].Start < pos[i].End)
                {
                    pos.RemoveAt(i);
                }
            }

            return pos;
            //return positions;
        }
    }
}