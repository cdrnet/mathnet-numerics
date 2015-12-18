namespace MathNet.Numerics.Optimization
{
    public interface IEvaluation1D
    {
        double Point { get; }
        double Value { get; }
        double Derivative { get; }
        double SecondDerivative { get; }
    }

    public interface IObjectiveFunction1D
    {
        bool DerivativeSupported { get; }
        bool SecondDerivativeSupported { get; }
        IEvaluation1D Evaluate(double point);
    }
}
