

<h1>BeeAlgorithmLibrary -- Artificial Bee Colony (ABC) Algorithm </h1>

<p>BeeAlgorithmLibrary is a C# library for solving optimization problems using the  Artificial Bee Colony (ABC) Algorithm introduced by Derviş Karaboğa to the literature.

<h3>IMPORTANT :</h3> This library does not use the standard bee algorithm.
It provides the Artificial Bee Colony (ABC) Algorithm </p>

<h2>More</h2>
<p>You can learn more about how the algorithm works here See the <a href="https://abc.erciyes.edu.tr/">Artificial Bee Colony (ABC) Algorithm Homepage</a>.</p>
<p>You can learn more about how the algorithm works on wikipedia <a href="https://en.wikipedia.org/wiki/Artificial_bee_colony_algorithm">Artificial Bee Colony (ABC) Algorithm Wikipedia</a>.</p>
<h2>Installation</h2>

<p>To use this library, you can add it to your project as a <a href="https://www.nuget.org/packages/BeeAlgorithmLibrary">NuGet Package</a>.</p>

<pre>
<code>dotnet add package BeeAlgorithmLibrary --version 3.0.2</code></pre>
or
<pre>
<code>NuGet\Install-Package BeeAlgorithmLibrary -Version 3.0.2</code>
</pre>

<h2>How to Use</h2>
You can follow these steps to solve an optimization problem using BeeAlgorithmLibrary:

Define the Objective Function: Create an objective function that represents the problem you want to solve. This function takes a vector of inputs and returns a score. The goal is to minimize this score.

Create a BeeAlgorithm Object: Define the optimization problem using the BeeAlgorithm class. Specify parameters such as the objective function, dimension, colony size, maximum number of iterations, lower and upper bounds of the search space.

Solve the Optimization Problem: Solve the optimization problem using the BeeAlgorithm object. You can obtain the best solution by calling the Solve() method.


```csharp
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

```


<h2>Contribution</h2>

<p>This library is open for contributions. If you find any bugs or have suggestions for improvement, please open a <a href="https://github.com/example/example/issues">GitHub issue</a> or submit a pull request.</p>

<h2>License</h2>

<p>This project is licensed under the MIT License. See the <a href="LICENSE">LICENSE</a> file for more information.</p>


