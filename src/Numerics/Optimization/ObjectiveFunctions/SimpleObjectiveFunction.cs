using System;

namespace MathNet.Numerics.Optimization.ObjectiveFunctions
{
    internal class SimpleObjectiveFunction : IObjectiveFunction
    {
        public Func<double, double> Objective { get; private set; }
        public Func<double, double> Derivative { get; private set; }
        public Func<double, double> SecondDerivative { get; private set; }

        public SimpleObjectiveFunction(Func<double, double> objective)
        {
            Objective = objective;
            Derivative = null;
            SecondDerivative = null;
        }

        public SimpleObjectiveFunction(Func<double, double> objective, Func<double, double> derivative)
        {
            Objective = objective;
            Derivative = derivative;
            SecondDerivative = null;
        }

        public SimpleObjectiveFunction(Func<double, double> objective, Func<double, double> derivative, Func<double,double> secondDerivative)
        {
            Objective = objective;
            Derivative = derivative;
            SecondDerivative = secondDerivative;
        }

        public bool IsDerivativeSupported
        {
            get { return Derivative != null; }
        }

        public bool IsSecondDerivativeSupported
        {
            get { return SecondDerivative != null; }
        }

        public IEvaluation Evaluate(double point)
        {
            return new CachedEvaluation(this, point);
        }
    }

    internal class CachedEvaluation : IEvaluation
    {
        private readonly SimpleObjectiveFunction _objective;
        private readonly double _point;
        private double? _value;
        private double? _derivative;
        private double? _secondDerivative;

        public CachedEvaluation(SimpleObjectiveFunction objective, double point)
        {
            _objective = objective;
            _point = point;
        }

        public double Point
        {
            get { return _point; }
        }

        public double Value
        {
            get { return _value ?? SetValue(); }
        }

        public double Derivative
        {
            get { return _derivative ?? SetDerivative(); }
        }

        public double SecondDerivative
        {
            get { return _secondDerivative ?? SetSecondDerivative(); }
        }

        private double SetValue()
        {
            _value = _objective.Objective(_point);
            return _value.Value;
        }

        private double SetDerivative()
        {
            _derivative = _objective.Derivative(_point);
            return _derivative.Value;
        }

        private double SetSecondDerivative()
        {
            _secondDerivative = _objective.SecondDerivative(_point);
            return _secondDerivative.Value;
        }
    }
}
