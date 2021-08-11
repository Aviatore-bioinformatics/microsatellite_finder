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

        /*public override string ToString()
        {
            StringBuilder output = new StringBuilder();

            output.Append(Name);
            
        }*/
    }
}