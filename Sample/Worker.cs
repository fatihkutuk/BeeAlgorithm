using BeeAlgorithm;
namespace Sample
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
            // Objective function to be minimized
            Func<double[], double> objectiveFunction = (x) =>
            {
                // For demonstration purposes, let's use the Sphere function
                return x[0] * x[0];
            };

            // Dimension of the optimization problem
            int dimension = 3;

            // Size of the bee colony
            int colonySize = 100;

            // Maximum number of iterations
            int maxIterations = 100;

            // Lower bounds of the search space
            double[] lowerBound = { -5, -5, -5 };

            // Upper bounds of the search space
            double[] upperBound = { 5, 5, 5 };

            // Optional: Define a custom random function
            Func<double> customRandomFunction = () =>
            {
                // Example custom random function
                return (new Random().NextDouble() * 2 - 1);
            };

            // Create an instance of the BeeAlgorithm class
            BeeAlgorithm.BeeAlgorithm beeAlgorithm = new BeeAlgorithm.BeeAlgorithm(objectiveFunction, dimension, colonySize, maxIterations, lowerBound, upperBound, customRandomFunction);

            // Solve the optimization problem
            double[] bestSolution = beeAlgorithm.Solve();

            // Output the best solution found
            Console.WriteLine("Best solution found:");
            foreach (var value in bestSolution)
            {
                Console.WriteLine(value);
            }
        }
    }
}
