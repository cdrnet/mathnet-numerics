﻿namespace MathNet.Numerics.Optimization
{
    public class MinimizationWithLineSearchResult : MinimizationResult
    {
        public int TotalLineSearchIterations { get; private set; }
        public int IterationsWithNonTrivialLineSearch { get; private set; }

        public MinimizationWithLineSearchResult(IObjectiveVectorFunction functionInfo, int iterations, ExitCondition reasonForExit, int totalLineSearchIterations, int iterationsWithNonTrivialLineSearch)
            : base(functionInfo, iterations, reasonForExit)
        {
            TotalLineSearchIterations = totalLineSearchIterations;
            IterationsWithNonTrivialLineSearch = iterationsWithNonTrivialLineSearch;
        }
    }
}
