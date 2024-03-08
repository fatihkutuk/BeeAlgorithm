using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeAlgorithmLibrary.Extentions.Models.Response
{
    public class Runtime
    {
        public int RuntimeIndex { get; set; }
        public List<Cycle> Cycles { get; set; }

        public Runtime()
        {
            Cycles = new List<Cycle>();
        }
    }
}
