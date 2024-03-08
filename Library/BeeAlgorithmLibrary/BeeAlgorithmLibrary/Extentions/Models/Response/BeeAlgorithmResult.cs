using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeAlgorithmLibrary.Extentions.Models.Response
{
    public class BeeAlgorithmResult
    {
        public List<BeeRunResult> Results { get; set; }
        public double Mean { get; set; }

        public BeeAlgorithmResult()
        {
            Results = new List<BeeRunResult>();
        }
    }
}
