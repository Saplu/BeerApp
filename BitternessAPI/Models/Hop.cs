using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace BitternessAPI.Models
{
    public class Hop
    {
        public long Id { get; set; }
        public double Weight { get; set; }
        public double Alpha { get; set; }
        public double BoilingTime { get; set; }
        public string Name { get; set; }
    }
}
