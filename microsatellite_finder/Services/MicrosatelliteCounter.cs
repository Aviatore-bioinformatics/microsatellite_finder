using System;
using System.Collections.Generic;
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
                        }
                        else
                        {
                            //Console.Out.WriteLine(repeatCount);
                            if (repeatCount >= Options.MinRepeatNumber - 1)
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
                        }

                        indexStart += repeatedSequenceLength;
                        indexEnd = indexStart + repeatedSequenceLength;
                    }
                }
            }
            

            return positions;
        }
    }
}