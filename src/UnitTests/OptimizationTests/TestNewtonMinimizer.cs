﻿using System;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.Optimization;
using MathNet.Numerics.Optimization.ObjectiveFunctions;
using NUnit.Framework;

namespace MathNet.Numerics.UnitTests.OptimizationTests
{
    public class LazyRosenbrockObjectiveFunction : LazyObjectiveFunctionBase
    {
        public LazyRosenbrockObjectiveFunction() : base(true, true) { }

        public override IObjectiveVectorFunction CreateNew()
        {
            return new LazyRosenbrockObjectiveFunction();
        }

        protected override void EvaluateValue()
        {
            Value = RosenbrockFunction.Value(Point);
        }

        protected override void EvaluateGradient()
        {
            Gradient = RosenbrockFunction.Gradient(Point);
        }

        protected override void EvaluateHessian()
        {
            Hessian = RosenbrockFunction.Hessian(Point);
        }
    }

    public class RosenbrockObjectiveFunction : ObjectiveFunctionBase
    {
        public RosenbrockObjectiveFunction() : base(true, true) { }

        public override IObjectiveVectorFunction CreateNew()
        {
            return new RosenbrockObjectiveFunction();
        }

        protected override void Evaluate()
        {
            // here we could directly overwrite the existing matrix cells instead.
            // note: values must then be initialized manually first, if null.
            Value = RosenbrockFunction.Value(Point);
            Gradient = RosenbrockFunction.Gradient(Point);
            Hessian = RosenbrockFunction.Hessian(Point);
        }
    }

    [TestFixture]
    public class TestNewtonMinimizer
    {
        [Test]
        public void FindMinimum_Rosenbrock_Easy()
        {
            var obj = ObjectiveVectorFunction.GradientHessian(RosenbrockFunction.Value, RosenbrockFunction.Gradient, RosenbrockFunction.Hessian);
            var solver = new NewtonMinimizer(1e-5, 1000);
            var result = solver.FindMinimum(obj, new DenseVector(new[] { 1.2, 1.2 }));

            Assert.That(Math.Abs(result.MinimizingPoint[0] - 1.0), Is.LessThan(1e-3));
            Assert.That(Math.Abs(result.MinimizingPoint[1] - 1.0), Is.LessThan(1e-3));
        }

        [Test]
        public void FindMinimum_Rosenbrock_Hard()
        {
            var obj = ObjectiveVectorFunction.GradientHessian(point => Tuple.Create(RosenbrockFunction.Value(point), RosenbrockFunction.Gradient(point), RosenbrockFunction.Hessian(point)));
            var solver = new NewtonMinimizer(1e-5, 1000);
            var result = solver.FindMinimum(obj, new DenseVector(new[] { -1.2, 1.0 }));

            Assert.That(Math.Abs(result.MinimizingPoint[0] - 1.0), Is.LessThan(1e-3));
            Assert.That(Math.Abs(result.MinimizingPoint[1] - 1.0), Is.LessThan(1e-3));
        }

        [Test]
        public void FindMinimum_Rosenbrock_Overton()
        {
            var obj = new LazyRosenbrockObjectiveFunction();
            var solver = new NewtonMinimizer(1e-5, 1000);
            var result = solver.FindMinimum(obj, new DenseVector(new[] { -0.9, -0.5 }));

            Assert.That(Math.Abs(result.MinimizingPoint[0] - 1.0), Is.LessThan(1e-3));
            Assert.That(Math.Abs(result.MinimizingPoint[1] - 1.0), Is.LessThan(1e-3));
        }

        [Test]
        public void FindMinimum_Linesearch_Rosenbrock_Easy()
        {
            var obj = new RosenbrockObjectiveFunction();
            var solver = new NewtonMinimizer(1e-5, 1000, true);
            var result = solver.FindMinimum(obj, new DenseVector(new[] { 1.2, 1.2 }));

            Assert.That(Math.Abs(result.MinimizingPoint[0] - 1.0), Is.LessThan(1e-3));
            Assert.That(Math.Abs(result.MinimizingPoint[1] - 1.0), Is.LessThan(1e-3));
        }

        [Test]
        public void FindMinimum_Linesearch_Rosenbrock_Hard()
        {
            var obj = new LazyRosenbrockObjectiveFunction();
            var solver = new NewtonMinimizer(1e-5, 1000, true);
            var result = solver.FindMinimum(obj, new DenseVector(new[] { -1.2, 1.0 }));

            Assert.That(Math.Abs(result.MinimizingPoint[0] - 1.0), Is.LessThan(1e-3));
            Assert.That(Math.Abs(result.MinimizingPoint[1] - 1.0), Is.LessThan(1e-3));
        }

        [Test]
        public void FindMinimum_Linesearch_Rosenbrock_Overton()
        {
            var obj = new LazyRosenbrockObjectiveFunction();
            var solver = new NewtonMinimizer(1e-5, 1000, true);
            var result = solver.FindMinimum(obj, new DenseVector(new[] { -0.9, -0.5 }));

            Assert.That(Math.Abs(result.MinimizingPoint[0] - 1.0), Is.LessThan(1e-3));
            Assert.That(Math.Abs(result.MinimizingPoint[1] - 1.0), Is.LessThan(1e-3));
        }
    }
}
