using System;
using MathNet.Numerics.Optimization.ObjectiveFunctions;

namespace MathNet.Numerics.Optimization
{
    public static class ObjectiveFunction1D
    {
        /// <summary>
        /// Objective function where neither first nor second derivative is available. Lazy evaluation.
        /// </summary>
        public static IObjectiveFunction1D Value(Func<double, double> function)
        {
            return new SimpleObjectiveFunction1D(function);
        }

        /// <summary>
        /// Objective function where the first derivative is available. Lazy evaluation.
        /// </summary>
        public static IObjectiveFunction1D FirstDerivative(Func<double, double> function, Func<double, double> derivative)
        {
            return new SimpleObjectiveFunction1D(function, derivative);
        }

        /// <summary>
        /// Objective function where the first and second derivative are available. Lazy evaluation.
        /// </summary>
        public static IObjectiveFunction1D FirstSecondDerivative(Func<double, double> function, Func<double, double> derivative, Func<double, double> secondDerivative)
        {
            return new SimpleObjectiveFunction1D(function, derivative, secondDerivative);
        }
    }
}
