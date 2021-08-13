using System;
using System.Collections.Generic;
using System.Text;

namespace microsatellite_finder.Models
{
    public class Transcript
    {
        public int LineLen { get; set; }
        public string Name { get; set; }
        public string Sequence { get; set; }
        public Position[] Positions { get; set; }

        public override string ToString()
        {
            StringBuilder output = new StringBuilder();

            int start = 0;
            output.Append(Name + '\n');

            foreach (var position in Positions)
            {
                //Console.Out.WriteLine($"{position.Start} ::: {position.End}");
                try
                {
                    if (position.Start - start > 0)
                    {
                        output.Append(Sequence.Substring(start, position.Start - start));
                        output.Append(Sequence.Substring(position.Start, position.End - position.Start + 1).ToLower());
                    }
                    else
                    {
                        output.Append(Sequence.Substring(position.Start + Math.Abs(position.Start - start), position.End - position.Start + 1 - Math.Abs(position.Start - start)).ToLower());
                    }
                    start = position.End + 1;
                }
                catch (ArgumentOutOfRangeException e)
                {
                    Console.Out.WriteLine(Name);
                    Console.Out.WriteLine($"{position.Start} ::: {start}");

                    foreach (var p in Positions)
                    {
                        Console.Out.WriteLine($"{p.Start} ::: {p.End}");
                    }
                    throw;
                }
            }

            output.Append(Sequence.Substring(start));

            return FastaFormatter(output.ToString());
        }

        public string FastaFormatter(string input)
        {
            StringBuilder output = new StringBuilder();

            var inputSplitted = input.Split('\n');
            output.Append(inputSplitted[0] + '\n');

            int indexStart = 0;

            while (inputSplitted[1].Length >= indexStart + LineLen)
            {
                output.Append(inputSplitted[1].Substring(indexStart, LineLen) + '\n');

                indexStart += LineLen;
            }

            if (inputSplitted[1].Length > indexStart)
            {
                output.Append(inputSplitted[1].Substring(indexStart) + '\n');
            }

            return output.ToString();
        }
    }
}