using System;
using MathNet.Numerics.LinearAlgebra;

namespace MathNet.Numerics.Optimization.ObjectiveFunctions
{
    internal class GradientObjectiveFunction : IObjectiveVectorFunction
    {
        readonly Func<Vector<double>, Tuple<double, Vector<double>>> _function;

        public GradientObjectiveFunction(Func<Vector<double>, Tuple<double, Vector<double>>> function)
        {
            _function = function;
        }

        public IObjectiveVectorFunction CreateNew()
        {
            return new GradientObjectiveFunction(_function);
        }

        public IObjectiveVectorFunction Fork()
        {
            // no need to deep-clone values since they are replaced on evaluation
            return new GradientObjectiveFunction(_function)
            {
                Point = Point,
                Value = Value,
                Gradient = Gradient
            };
        }

        public bool IsGradientSupported
        {
            get { return true; }
        }

        public bool IsHessianSupported
        {
            get { return false; }
        }

        public void Evaluate(Vector<double> point)
        {
            Point = point;

            var result = _function(point);
            Value = result.Item1;
            Gradient = result.Item2;
        }

        public Vector<double> Point { get; private set; }
        public double Value { get; private set; }
        public Vector<double> Gradient { get; private set; }

        public Matrix<double> Hessian
        {
            get { throw new NotSupportedException(); }
        }
    }
}