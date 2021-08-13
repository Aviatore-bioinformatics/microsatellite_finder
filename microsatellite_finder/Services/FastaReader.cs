using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using microsatellite_finder.Models;

namespace microsatellite_finder.Services
{
    public class FastaReader
    {
        public List<Transcript> Transcripts { get; }
        public string FastaFilePath { get; }
        public int transcriptCount;

        public FastaReader(string filePath)
        {
            FastaFilePath = filePath;
            Transcripts = new List<Transcript>();
        }

        public void ReadFasta()
        {
            var nameTmp = new StringBuilder();
            var sequenceTmp = new StringBuilder();
            int? lineLength = null;
            
            using (var sr = new StreamReader(FastaFilePath))
            {
                var line = sr.ReadLine();
                while (line is not null)
                {
                    if (line.StartsWith('>'))
                    {
                        transcriptCount++;
                        if (sequenceTmp.Length > 0 && lineLength is not null)
                        {
                            var transcript = new Transcript()
                            {
                                Name = nameTmp.ToString(),
                                Sequence = sequenceTmp.ToString(),
                                LineLen = lineLength ?? default
                            };
                            
                            Transcripts.Add(transcript);
                            
                            nameTmp.Clear();
                            sequenceTmp.Clear();
                        }
                        
                        nameTmp.Append(line);
                        lineLength = null;
                    }
                    else
                    {
                        sequenceTmp.Append(line.TrimEnd());
                        lineLength ??= line.TrimEnd().Length;
                    }
                    
                    line = sr.ReadLine();
                }
                
                var trans = new Transcript()
                {
                    Name = nameTmp.ToString(),
                    Sequence = sequenceTmp.ToString()
                };
                            
                Transcripts.Add(trans);
            }
        }
    }
}