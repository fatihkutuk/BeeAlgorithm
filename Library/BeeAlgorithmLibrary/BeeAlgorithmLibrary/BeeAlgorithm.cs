using BeeAlgorithmLibrary.Extentions.Models.Response;
using BeeAlgorithmLibrary.Extentions.Types;

namespace BeeAlgorithmLibrary
{
    public class BeeAlgorithm
    {

        public Func<double[], double> TestFunction { get; set; }
        public OptimizationType OptimizationType { get; set; }
        public int NP;
        public int FoodNumber;
        public int limit;
        public int maxCycle;
        public int D;
        public double lb;
        public double ub;
        public int runtime;
        public double[][] Foods;
        public double[] f;
        public double[] fitness;
        public double[] trial;
        public double[] prob;
        public double[] solution;
        public double ObjValSol;
        public double FitnessSol;
        public int neighbour, param2change;
        public double GlobalMin;
        public double[] GlobalParams;
        public double[] GlobalMins;
        public double r;

        public BeeAlgorithm( OptimizationType optimizationType, int NP, int maxCycle, int limit, int D, double lb, double ub, int runtime, Func<double[], double> testFunction = null)
        {

            TestFunction = testFunction ?? Sphere;
            OptimizationType = optimizationType;
            this.NP = NP;
            this.maxCycle = maxCycle;
            this.limit = limit;
            this.D = D;
            this.lb = lb;
            this.ub = ub;
            this.runtime = runtime;
            FoodNumber = NP / 2;
            Foods = new double[FoodNumber][];
            for (int i = 0; i < FoodNumber; i++)
            {
                Foods[i] = new double[D];
            }
            f = new double[FoodNumber];
            fitness = new double[FoodNumber];
            trial = new double[FoodNumber];
            prob = new double[FoodNumber];
            solution = new double[D];
            GlobalParams = new double[D];
            GlobalMins = new double[runtime];
            TestFunction = testFunction;
        }

        public void MemorizeBestSource()
        {
            int i, j;
            for (i = 0; i < FoodNumber; i++)
            {
                if (f[i] < GlobalMin)
                {
                    GlobalMin = f[i];
                    for (j = 0; j < D; j++)
                    {
                        GlobalParams[j] = Foods[i][j];
                    }
                }
            }
        }


        public BeeAlgorithmResult Solve()
        {
            BeeAlgorithmResult algorithmResult = new BeeAlgorithmResult();
            algorithmResult.Results = new List<BeeRunResult>();
            double mean = 0;

            for (int run = 0; run < runtime; run++)
            {
                initial();
                MemorizeBestSource();
                BeeRunResult runResult = new BeeRunResult();
                runResult.GlobalParams = new List<double>();

                for (int iter = 0; iter < maxCycle; iter++)
                {
                    SendEmployedBees();
                    CalculateProbabilities();
                    SendOnlookerBees();
                    MemorizeBestSource();
                    SendScoutBees();
                    runResult.CycleParams.Add(GlobalMin);
                }

                for (int j = 0; j < D; j++)
                {
                    runResult.GlobalParams.Add(GlobalParams[j]);
                }
                runResult.GlobalMin = GlobalMin;
                mean += GlobalMin;

                algorithmResult.Results.Add(runResult);
            }

            mean /= runtime;
            algorithmResult.Mean = mean;

            return algorithmResult;
        }
        public async Task<BeeAlgorithmResult> SolveAsync()
        {
            BeeAlgorithmResult algorithmResult = new BeeAlgorithmResult();
            algorithmResult.Results = new List<BeeRunResult>();
            double mean = 0;

            await Task.Run(() =>
            {
                for (int run = 0; run < runtime; run++)
                {
                    initial();
                    MemorizeBestSource();
                    BeeRunResult runResult = new BeeRunResult();
                    runResult.GlobalParams = new List<double>();

                    for (int iter = 0; iter < maxCycle; iter++)
                    {
                        SendEmployedBees();
                        CalculateProbabilities();
                        SendOnlookerBees();
                        MemorizeBestSource();
                        SendScoutBees();
                        runResult.CycleParams.Add(GlobalMin);
                    }

                    for (int j = 0; j < D; j++)
                    {
                        runResult.GlobalParams.Add(GlobalParams[j]);
                    }
                    runResult.GlobalMin = GlobalMin;
                    mean += GlobalMin;

                    algorithmResult.Results.Add(runResult);
                }

                mean /= runtime;
                algorithmResult.Mean = mean;
            });

            return algorithmResult;
        }


        public void init(int index)
        {
            int j;
            Random rand = new Random();
            for (j = 0; j < D; j++)
            {
                r = rand.NextDouble() * (ub - lb) + lb;
                Foods[index][j] = r;
                solution[j] = Foods[index][j];
            }
            f[index] = calculateFunction(solution);
            fitness[index] = CalculateFitness(f[index]);
            trial[index] = 0;
        }

        public void initial()
        {
            int i;
            for (i = 0; i < FoodNumber; i++)
            {
                init(i);
            }
            GlobalMin = f[0];
            for (i = 0; i < D; i++)
            {
                GlobalParams[i] = Foods[0][i];
            }
        }

        public void SendEmployedBees()
        {
            int i, j;
            Random rand = new Random();
            for (i = 0; i < FoodNumber; i++)
            {
                r = rand.NextDouble() * D;
                param2change = (int)r;

                r = rand.NextDouble() * FoodNumber;
                neighbour = (int)r;

                for (j = 0; j < D; j++)
                {
                    solution[j] = Foods[i][j];
                }

                r = rand.NextDouble() * 2 - 1;
                solution[param2change] = Foods[i][param2change] + (Foods[i][param2change] - Foods[neighbour][param2change]) * (r - 0.5) * 2;

                if (solution[param2change] < lb)
                {
                    solution[param2change] = lb;
                }
                if (solution[param2change] > ub)
                {
                    solution[param2change] = ub;
                }
                ObjValSol = calculateFunction(solution);
                FitnessSol = CalculateFitness(ObjValSol);

                if (FitnessSol > fitness[i])
                {
                    trial[i] = 0;
                    for (j = 0; j < D; j++)
                    {
                        Foods[i][j] = solution[j];
                    }
                    f[i] = ObjValSol;
                    fitness[i] = FitnessSol;
                }
                else
                {
                    trial[i]++;
                }
            }
        }

        public void CalculateProbabilities()
        {
            int i;
            double maxfit;
            maxfit = fitness[0];
            for (i = 1; i < FoodNumber; i++)
            {
                if (fitness[i] > maxfit)
                {
                    maxfit = fitness[i];
                }
            }
            for (i = 0; i < FoodNumber; i++)
            {
                prob[i] = (0.9 * (fitness[i] / maxfit)) + 0.1;
            }
        }

        public void SendOnlookerBees()
        {
            int i, j, t;
            i = 0;
            t = 0;
            Random rand = new Random();
            while (t < FoodNumber)
            {
                r = rand.NextDouble() * 32767 / (32767 + 1);
                if (r < prob[i])
                {
                    t++;
                    r = rand.NextDouble() * D;
                    param2change = (int)r;

                    r = rand.NextDouble() * FoodNumber;
                    neighbour = (int)r;

                    while (neighbour == i)
                    {
                        r = rand.NextDouble() * FoodNumber;
                        neighbour = (int)r;
                    }
                    for (j = 0; j < D; j++)
                    {
                        solution[j] = Foods[i][j];
                    }
                    r = rand.NextDouble() * 32767 / (32767 + 1);
                    solution[param2change] = Foods[i][param2change] + (Foods[i][param2change] - Foods[neighbour][param2change]) * (r - 0.5) * 2;

                    if (solution[param2change] < lb)
                    {
                        solution[param2change] = lb;
                    }
                    if (solution[param2change] > ub)
                    {
                        solution[param2change] = ub;
                    }
                    ObjValSol = calculateFunction(solution);
                    FitnessSol = CalculateFitness(ObjValSol);

                    if (FitnessSol > fitness[i])
                    {
                        trial[i] = 0;
                        for (j = 0; j < D; j++)
                        {
                            Foods[i][j] = solution[j];
                        }
                        f[i] = ObjValSol;
                        fitness[i] = FitnessSol;
                    }
                    else
                    {
                        trial[i]++;
                    }
                }
                i++;
                if (i == FoodNumber)
                {
                    i = 0;
                }
            }
        }

        public void SendScoutBees()
        {
            int maxtrialindex, i;
            maxtrialindex = 0;
            for (i = 1; i < FoodNumber; i++)
            {
                if (trial[i] > trial[maxtrialindex])
                {
                    maxtrialindex = i;
                }
            }
            if (trial[maxtrialindex] >= limit)
            {
                init(maxtrialindex);
            }
        }

        public double calculateFunction(double[] sol)
        {
            if(TestFunction != null)
            {
                return TestFunction(sol);
            }
            else
            {
                return Sphere(sol);
            }
        }

        public double Sphere(double[] sol)
        {
            int j;
            double top = 0;
            for (j = 0; j < D; j++)
            {
                top = top + sol[j] * sol[j];
            }
            return top;
        }
        public double CalculateFitness(double fun)
        {
            double result = 0;

            if (OptimizationType == OptimizationType.Minimize)
            {
                if (fun >= 0)
                {
                    result = 1 / (fun + 1);
                }
                else
                {
                    result = 1 + Math.Abs(fun);
                }
            }
            else if (OptimizationType == OptimizationType.Maximize)
            {
                if (fun >= 0)
                {
                    result = -1 / (fun + 1); // Negatifini alarak maksimize edilmiş halini döndürür
                }
                else
                {
                    result = -1 - Math.Abs(fun); // Negatifini alarak maksimize edilmiş halini döndürür
                }
            }
            else
            {
                throw new ArgumentException("Invalid optimization type.");
            }

            return result;
        }
    }
}
