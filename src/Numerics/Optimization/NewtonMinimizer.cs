﻿using System;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Optimization.LineSearch;

namespace MathNet.Numerics.Optimization
{
    public class NewtonMinimizer
    {
        public double GradientTolerance { get; set; }
        public int MaximumIterations { get; set; }
        public bool UseLineSearch { get; set; }

        public NewtonMinimizer(double gradientTolerance, int maximumIterations, bool useLineSearch = false)
        {
            GradientTolerance = gradientTolerance;
            MaximumIterations = maximumIterations;
            UseLineSearch = useLineSearch;
        }

        public MinimizationResult FindMinimum(IObjectiveVectorFunction objective, Vector<double> initialGuess)
        {
            if (!objective.IsGradientSupported)
            {
                throw new IncompatibleObjectiveException("Gradient not supported in objective function, but required for Newton minimization.");
            }

            if (!objective.IsHessianSupported)
            {
                throw new IncompatibleObjectiveException("Hessian not supported in objective function, but required for Newton minimization.");
            }

            // Check that we're not already done
            objective.EvaluateAt(initialGuess);
            ValidateGradient(objective);
            if (ExitCriteriaSatisfied(objective.Gradient))
            {
                return new MinimizationResult(objective, 0, MinimizationResult.ExitCondition.AbsoluteGradient);
            }

            // Set up line search algorithm
            var lineSearcher = new WeakWolfeLineSearch(1e-4, 0.9, 1e-4, maxIterations: 1000);

            // Subsequent steps
            int iterations = 0;
            int totalLineSearchSteps = 0;
            int iterationsWithNontrivialLineSearch = 0;
            bool tmpLineSearch = false;
            while (!ExitCriteriaSatisfied(objective.Gradient) && iterations < MaximumIterations)
            {
                ValidateHessian(objective);

                var searchDirection = objective.Hessian.LU().Solve(-objective.Gradient);
                if (searchDirection * objective.Gradient >= 0)
                {
                    searchDirection = -objective.Gradient;
                    tmpLineSearch = true;
                }

                if (UseLineSearch || tmpLineSearch)
                {
                    LineSearchResult result;
                    try
                    {
                        result = lineSearcher.FindConformingStep(objective, searchDirection, 1.0);
                    }
                    catch (Exception e)
                    {
                        throw new InnerOptimizationException("Line search failed.", e);
                    }

                    iterationsWithNontrivialLineSearch += result.Iterations > 0 ? 1 : 0;
                    totalLineSearchSteps += result.Iterations;
                    objective = result.FunctionInfoAtMinimum;
                }
                else
                {
                    objective.EvaluateAt(objective.Point + searchDirection);
                }

                ValidateGradient(objective);

                tmpLineSearch = false;
                iterations += 1;
            }

            if (iterations == MaximumIterations)
            {
                throw new MaximumIterationsException(String.Format("Maximum iterations ({0}) reached.", MaximumIterations));
            }

            return new MinimizationWithLineSearchResult(objective, iterations, MinimizationResult.ExitCondition.AbsoluteGradient, totalLineSearchSteps, iterationsWithNontrivialLineSearch);
        }

        bool ExitCriteriaSatisfied(Vector<double> gradient)
        {
            return gradient.Norm(2.0) < GradientTolerance;
        }

        static void ValidateGradient(IObjectiveVectorFunction eval)
        {
            foreach (var x in eval.Gradient)
            {
                if (Double.IsNaN(x) || Double.IsInfinity(x))
                {
                    throw new EvaluationException("Non-finite gradient returned.", eval);
                }
            }
        }

        private void ValidateObjective(IObjectiveVectorFunction eval)
        {
            if (Double.IsNaN(eval.Value) || Double.IsInfinity(eval.Value))
                throw new EvaluationException("Non-finite objective function returned.", eval);
        }

        private void ValidateHessian(IObjectiveVectorFunction eval)
        {
            for (int ii = 0; ii < eval.Hessian.RowCount; ++ii)
            {
                for (int jj = 0; jj < eval.Hessian.ColumnCount; ++jj)
                {
                    if (Double.IsNaN(eval.Hessian[ii, jj]) || Double.IsInfinity(eval.Hessian[ii, jj]))
                    {
                        throw new EvaluationException("Non-finite Hessian returned.", eval);
                    }
                }
            }
        }
    }
}
