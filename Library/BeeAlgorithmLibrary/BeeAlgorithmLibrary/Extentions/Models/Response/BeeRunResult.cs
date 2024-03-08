using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeAlgorithmLibrary.Extentions.Models.Response
{
    public class BeeRunResult
    {
        public List<double> GlobalParams { get; set; }
        public double GlobalMin { get; set; }
        public List<double> CycleParams { get; set; }

        public BeeRunResult()
        {
            GlobalParams = new List<double>();
            CycleParams = new List<double>();
        }
    }
}
