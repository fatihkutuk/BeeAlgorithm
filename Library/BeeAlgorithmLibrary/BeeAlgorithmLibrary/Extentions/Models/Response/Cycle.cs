using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BeeAlgorithmLibrary.BeeAlgorithm;

namespace BeeAlgorithmLibrary.Extentions.Models.Response
{
    public class Cycle
    {
        public int CycleIndex { get; set; }
        public double CycleGlobalMin { get; set; }
        public List<GlobalParameters> GlobalParameters { get; set; }
        public List<CycleData> CycleData { get; set; }

        public Cycle()
        {
            GlobalParameters = new List<GlobalParameters>();
            CycleData = new List<CycleData>();
        }
    }
}
