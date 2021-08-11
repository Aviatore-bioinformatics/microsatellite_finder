using System;
using System.Collections.Generic;
using System.Text;

namespace microsatellite_finder.Models
{
    public class Transcript
    {
        private int lineLen = 60;
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
                output.Append(Sequence.Substring(start, position.Start - start));
                output.Append(Sequence.Substring(position.Start, position.End - position.Start + 1).ToLower());
                start = position.End + 1;
            }

            output.Append(Sequence.Substring(start));

            return output.ToString();
        }
    }
}