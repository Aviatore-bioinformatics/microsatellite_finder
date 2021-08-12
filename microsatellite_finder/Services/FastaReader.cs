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
            
            using (var sr = new StreamReader(FastaFilePath))
            {
                var line = sr.ReadLine();
                while (line is not null)
                {
                    if (line.StartsWith('>'))
                    {
                        transcriptCount++;
                        if (sequenceTmp.Length > 0)
                        {
                            var transcript = new Transcript()
                            {
                                Name = nameTmp.ToString(),
                                Sequence = sequenceTmp.ToString()
                            };
                            
                            Transcripts.Add(transcript);
                            
                            nameTmp.Clear();
                            sequenceTmp.Clear();
                        }
                        
                        nameTmp.Append(line);
                    }
                    else
                    {
                        sequenceTmp.Append(line.TrimEnd());
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