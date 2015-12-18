namespace MathNet.Numerics.Optimization
{
    public interface IEvaluation
    {
        double Point { get; }
        double Value { get; }
        double Derivative { get; }
        double SecondDerivative { get; }
    }

    public interface IObjectiveFunction
    {
        bool IsDerivativeSupported { get; }
        bool IsSecondDerivativeSupported { get; }

        IEvaluation Evaluate(double point);
    }
}
