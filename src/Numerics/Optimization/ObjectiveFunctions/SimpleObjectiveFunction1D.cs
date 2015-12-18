using System;

namespace MathNet.Numerics.Optimization.ObjectiveFunctions
{
    public class SimpleObjectiveFunction1D : IObjectiveFunction1D
    {
        public Func<double, double> Objective { get; private set; }
        public Func<double, double> Derivative { get; private set; }
        public Func<double, double> SecondDerivative { get; private set; }

        public SimpleObjectiveFunction1D(Func<double, double> objective)
        {
            Objective = objective;
            Derivative = null;
            SecondDerivative = null;
        }

        public SimpleObjectiveFunction1D(Func<double, double> objective, Func<double, double> derivative)
        {
            Objective = objective;
            Derivative = derivative;
            SecondDerivative = null;
        }

        public SimpleObjectiveFunction1D(Func<double, double> objective, Func<double, double> derivative, Func<double,double> secondDerivative)
        {
            Objective = objective;
            Derivative = derivative;
            SecondDerivative = secondDerivative;
        }

        public bool DerivativeSupported
        {
            get { return Derivative != null; }
        }

        public bool SecondDerivativeSupported
        {
            get { return SecondDerivative != null; }
        }

        public IEvaluation1D Evaluate(double point)
        {
            return new CachedEvaluation1D(this, point);
        }
    }

    internal class CachedEvaluation1D : IEvaluation1D
    {
        private readonly SimpleObjectiveFunction1D _objective;
        private readonly double _point;
        private double? _value;
        private double? _derivative;
        private double? _secondDerivative;

        public CachedEvaluation1D(SimpleObjectiveFunction1D objective, double point)
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
