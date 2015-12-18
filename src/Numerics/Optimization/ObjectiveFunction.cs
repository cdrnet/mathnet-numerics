using System;
using MathNet.Numerics.Optimization.ObjectiveFunctions;

namespace MathNet.Numerics.Optimization
{
    public static class ObjectiveFunction
    {
        /// <summary>
        /// Objective function where neither first nor second derivative is available. Lazy evaluation.
        /// </summary>
        public static IObjectiveFunction Value(Func<double, double> function)
        {
            return new SimpleObjectiveFunction(function);
        }

        /// <summary>
        /// Objective function where the first derivative is available. Lazy evaluation.
        /// </summary>
        public static IObjectiveFunction FirstDerivative(Func<double, double> function, Func<double, double> derivative)
        {
            return new SimpleObjectiveFunction(function, derivative);
        }

        /// <summary>
        /// Objective function where the first and second derivative are available. Lazy evaluation.
        /// </summary>
        public static IObjectiveFunction FirstSecondDerivative(Func<double, double> function, Func<double, double> derivative, Func<double, double> secondDerivative)
        {
            return new SimpleObjectiveFunction(function, derivative, secondDerivative);
        }
    }
}
