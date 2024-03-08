using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeAlgorithmLibrary.Extentions
{
    public class TestFunctions
    {
        public static double Sphere(double[] sol)
        {
            int j;
            double top = 0;
            for (j = 0; j < sol.Length; j++)
            {
                top = top + sol[j] * sol[j];
            }
            return top;
        }

        public static double Rosenbrock(double[] sol)
        {
            int j;
            double top = 0;
            for (j = 0; j < sol.Length - 1; j++)
            {
                top = top + 100 * Math.Pow((sol[j + 1] - Math.Pow((sol[j]), (double)2)), (double)2) + Math.Pow((sol[j] - 1), (double)2);
            }
            return top;
        }

        public static double Griewank(double[] sol)
        {
            int j;
            double top1, top2, top;
            top = 0;
            top1 = 0;
            top2 = 1;
            for (j = 0; j < sol.Length; j++)
            {
                top1 = top1 + Math.Pow((sol[j]), (double)2);
                top2 = top2 * Math.Cos((((sol[j]) / Math.Sqrt((double)(j + 1))) * Math.PI) / 180);
            }
            top = (1 / (double)4000) * top1 - top2 + 1;
            return top;
        }

        public static double Rastrigin(double[] sol)
        {
            int j;
            double top = 0;
            for (j = 0; j < sol.Length; j++)
            {
                top = top + (Math.Pow(sol[j], (double)2) - 10 * Math.Cos(2 * Math.PI * sol[j]) + 10);
            }
            return top;
        }


    }
}
