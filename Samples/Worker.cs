/* Fatih Kütük */

using BeeAlgorithmLibrary.Extentions.Models.Response;

namespace Samples
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Func<double[], double> TestFunction = (sol) =>
                {
                    int j;
                    double top = 0;

                    for (j = 0; j < sol.Length; j++)
                    {
                        top = top + (Math.Pow(sol[j], (double)2) - 10 * Math.Cos(2 * Math.PI * sol[j]) + 10);
                    }
                    return top;
                };

                int NP = 20; // The number of colony size (employed bees+onlooker bees)

                int maxCycle = 2500;// The number of cycles for foraging {a stopping criteria}

                int limit = 100; // A food source which could not be improved through "limit" trials is abandoned by its employed bee

                int D = 100; // The number of parameters of the problem to be optimized

                double lb = -5.12; // lower bound of the parameters.

                double ub = 5.12; // upper bound of the parameters. lb and ub can be defined as arrays for the problems of which parameters have different bounds

                int runtime = 30; // Algorithm can be run many times in order to see its robustness

                //Func<double[], double> customTestFunction = BeeAlgorithmLibrary.Extentions.TestFunctions.Rastrigin; if u want test with global functions u can use extensions like this block
                BeeAlgorithmLibrary.Extentions.Types.OptimizationType optimizationType = BeeAlgorithmLibrary.Extentions.Types.OptimizationType.Minimize; // if u want minimize the function use Minimize, if u want maximize function use Maximize
               
                Func<double[], double> customTestFunction = TestFunction;
                
                BeeAlgorithmLibrary.BeeAlgorithm beeAlgorithm = new BeeAlgorithmLibrary.BeeAlgorithm(optimizationType, NP, maxCycle, limit, D, lb, ub, runtime,customTestFunction);
                //BeeAlgorithmLibrary.BeeAlgorithm beeAlgorithm = new BeeAlgorithmLibrary.BeeAlgorithm(optimizationType, NP, maxCycle, limit, D, lb, ub, runtime); if u dont give custom test function it will run with default Sphere function
                
                BeeAlgorithmResult solve = beeAlgorithm.Solve();
                //BeeAlgorithmResult solve = await beeAlgorithm.SolveAsync(); for async 


                // example write to optimization datas 
                foreach (var result in solve.Results.Select((value, i) => new { i, value }))
                {
                    Console.WriteLine($"Runtim({result.i+1})  Global Min  = {result.value.GlobalMin}");
                    foreach (var param in result.value.GlobalParams.Select((value, i) => new { i, value }))
                    {
                        Console.WriteLine($"  Runtim({result.i+1}) Param({param.i+1})  = {param.value}");

                    }

                }
                Console.WriteLine($"Mean of 30 Runtime = {solve.Mean}");

            }
        }
    }
}
