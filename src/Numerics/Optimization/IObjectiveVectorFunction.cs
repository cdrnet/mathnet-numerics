﻿using MathNet.Numerics.LinearAlgebra;

namespace MathNet.Numerics.Optimization
{
    /// <summary>
    /// Objective function with a frozen evaluation that must not be changed from the outside.
    /// </summary>
    public interface IVectorEvaluation
    {
        /// <summary>Create a new unevaluated and independent copy of this objective function</summary>
        IObjectiveVectorFunction CreateNew();

        /// <summary>Create a new independent copy of this objective function, evaluated at the same point.</summary>
        IObjectiveVectorFunction Fork();

        Vector<double> Point { get; }

        double Value { get; }

        Vector<double> Gradient { get; }

        Matrix<double> Hessian { get; }
    }

    /// <summary>
    /// Objective function with a mutable evaluation.
    /// </summary>
    public interface IObjectiveVectorFunction : IVectorEvaluation
    {
        bool IsGradientSupported { get; }
        bool IsHessianSupported { get; }

        void Evaluate(Vector<double> point);
    }
}
