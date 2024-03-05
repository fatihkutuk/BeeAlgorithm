namespace BeeAlgorithm
{
    public class BeeAlgorithm
    {
        private Random random;
        private Func<double[], double> objectiveFunction;
        private Func<double> randomFunction;

        private int dimension;
        private int colonySize;
        private int maxIterations;
        private double[] lowerBound;
        private double[] upperBound;

        /// <summary>
        /// Constructor for BeeAlgorithm class.
        /// </summary>
        /// <param name="objectiveFunction">The objective function to be minimized.</param>
        /// <param name="dimension">The dimension of the optimization problem.</param>
        /// <param name="colonySize">The size of the bee colony.</param>
        /// <param name="maxIterations">The maximum number of iterations.</param>
        /// <param name="lowerBound">The lower bounds of the search space.</param>
        /// <param name="upperBound">The upper bounds of the search space.</param>
        /// <param name="randomFunction">Optional. The random function to generate random numbers.</param>
        public BeeAlgorithm(Func<double[], double> objectiveFunction, int dimension, int colonySize, int maxIterations, double[] lowerBound, double[] upperBound, Func<double> randomFunction = null)
        {
            this.random = new Random();
            this.objectiveFunction = objectiveFunction;
            this.dimension = dimension;
            this.colonySize = colonySize;
            this.maxIterations = maxIterations;
            this.lowerBound = lowerBound;
            this.upperBound = upperBound;

            // Use the provided random function if available, otherwise use default.
            if (randomFunction != null)
                this.randomFunction = randomFunction;
            else
                this.randomFunction = () => DefaultRandomFunction();
        }

        /// <summary>
        /// Solves the optimization problem using the Bee Algorithm.
        /// </summary>
        /// <returns>The best solution found by the algorithm.</returns>
        public double[] Solve()
        {
            double[][] bees = new double[colonySize][];
            double[] fitnessValues = new double[colonySize];
            double[] globalBestPosition = new double[dimension];
            double globalBestFitness = double.MaxValue;

            // Initialize bees with random positions
            for (int i = 0; i < colonySize; i++)
            {
                bees[i] = GenerateRandomSolution(lowerBound, upperBound);
                fitnessValues[i] = objectiveFunction(bees[i]);

                if (fitnessValues[i] < globalBestFitness)
                {
                    globalBestFitness = fitnessValues[i];
                    Array.Copy(bees[i], globalBestPosition, dimension);
                }
            }

            // Perform iterations
            for (int iteration = 0; iteration < maxIterations; iteration++)
            {
                for (int i = 0; i < colonySize; i++)
                {
                    double[] newPosition = GenerateNewPosition(bees[i]);
                    double newFitness = objectiveFunction(newPosition);

                    if (newFitness < fitnessValues[i])
                    {
                        Array.Copy(newPosition, bees[i], dimension);
                        fitnessValues[i] = newFitness;

                        if (newFitness < globalBestFitness)
                        {
                            globalBestFitness = newFitness;
                            Array.Copy(newPosition, globalBestPosition, dimension);
                        }
                    }
                }
            }

            return globalBestPosition;
        }

        private double DefaultRandomFunction()
        {
            return random.NextDouble() * 2 - 1;
        }

        private double[] GenerateRandomSolution(double[] lowerBound, double[] upperBound)
        {
            double[] solution = new double[dimension];
            for (int i = 0; i < dimension; i++)
            {
                solution[i] = lowerBound[i] + randomFunction() * (upperBound[i] - lowerBound[i]);
            }
            return solution;
        }

        private double[] GenerateNewPosition(double[] currentPosition)
        {
            double[] newPosition = new double[dimension];
            for (int i = 0; i < dimension; i++)
            {
                double r = randomFunction(); // Generate a random number between -1 and 1
                newPosition[i] = currentPosition[i] + r;
                newPosition[i] = Math.Max(lowerBound[i], Math.Min(upperBound[i], newPosition[i])); // Ensure newPosition is within bounds
            }
            return newPosition;
        }
    }
}
