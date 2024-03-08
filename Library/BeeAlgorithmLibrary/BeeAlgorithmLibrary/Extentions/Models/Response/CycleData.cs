using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeAlgorithmLibrary.Extentions.Models.Response
{
    public class CycleData
    {
        public int Iteration { get; set; }
        public List<double> Parameters { get; set; }

        public CycleData()
        {
            Parameters = new List<double>();
        }
    }

}
