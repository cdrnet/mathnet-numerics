using MathNet.Numerics.LinearAlgebra;

namespace MathNet.Numerics.Optimization
{
    public interface IUnconstrainedMinimizer
    {
        MinimizationResult FindMinimum(IObjectiveVectorFunction objective, Vector<double> initialGuess);
    }
}
