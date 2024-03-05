/* Fatih Kütük */

using BeeAlgorithm;

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
                Func<double[], double> objectiveFunction = (x) =>
                {
                    // For demonstration purposes, let's use the Sphere function
                    return x[0] * x[0];
                };

                // Dimension of the optimization problem
                int dimension = 3;

                // Size of the bee colony
                int colonySize = 20;

                // Maximum number of iterations
                int maxIterations = 100;

                // Lower bounds of the search space
                double[] lowerBound = { -10, -10, -10 };

                // Upper bounds of the search space
                double[] upperBound = { 10, 10, 10 };

                // Optional: Define a custom random function
                Func<double> customRandomFunction = () =>
                {
                    // Example custom random function
                    return (new Random().NextDouble() * 2 - 1);
                };

                // Create an instance of the BeeAlgorithm class
                BeeAlgorithm.BeeAlgorithm beeAlgorithm = new BeeAlgorithm.BeeAlgorithm(objectiveFunction, dimension, colonySize, maxIterations, lowerBound, upperBound, customRandomFunction);
                // use default random function -> BeeAlgorithm.BeeAlgorithm beeAlgorithm = new BeeAlgorithm.BeeAlgorithm(objectiveFunction, dimension, colonySize, maxIterations, lowerBound, upperBound);

                // Solve the optimization problem
                double[] bestSolution = beeAlgorithm.Solve();

                // Output the best solution found
                Console.WriteLine("Best solution found:");
                foreach (var value in bestSolution)
                {
                    Console.WriteLine(value);
                }
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
