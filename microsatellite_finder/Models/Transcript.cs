using System.Collections.Generic;

namespace microsatellite_finder.Models
{
    public class Transcript
    {
        public string Name { get; set; }
        public string Sequence { get; set; }
        public Position[] Positions { get; set; }
    }
}