// <copyright file="ArrayStatistics.cs" company="Math.NET">
// Math.NET Numerics, part of the Math.NET Project
// http://numerics.mathdotnet.com
// http://github.com/mathnet/mathnet-numerics
// http://mathnetnumerics.codeplex.com
//
// Copyright (c) 2009-2015 Math.NET
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

using System;
using MathNet.Numerics.LinearAlgebra;

namespace MathNet.Numerics.Statistics
{
    /// <summary>
    /// Pointwise vector statistics on arrays of vectors of the same dimension.
    /// </summary>
    public static class VectorArrayStatistics
    {
        /// <summary>
        /// Returns the pointwise smallest value of the vectors
        /// Returns NaN if data is empty or any entry is NaN.
        /// </summary>
        /// <param name="data">Vector array, where all vectors have the same length.</param>
        public static Vector<double> Minimum(this Vector<double>[] data)
        {
            return data.TransposeArrayMap(ArrayStatistics.Minimum);
        }

        /// <summary>
        /// Returns the pointwise smallest value from the vectors.
        /// Returns NaN if data is empty or any entry is NaN.
        /// </summary>
        /// <param name="data">Vector array, where all vectors have the same length.</param>
        public static Vector<float> Minimum(this Vector<float>[] data)
        {
            return data.TransposeArrayMap(ArrayStatistics.Minimum);
        }

        /// <summary>
        /// Returns the pointwise smallest value from the vectors.
        /// Returns NaN if data is empty or any entry is NaN.
        /// </summary>
        /// <param name="data">Vector array, where all vectors have the same length.</param>
        public static Vector<double> Maximum(this Vector<double>[] data)
        {
            return data.TransposeArrayMap(ArrayStatistics.Maximum);
        }

        /// <summary>
        /// Returns the pointwise smallest value from the vectors.
        /// Returns NaN if data is empty or any entry is NaN.
        /// </summary>
        /// <param name="data">Vector array, where all vectors have the same length.</param>
        public static Vector<float> Maximum(this Vector<float>[] data)
        {
            return data.TransposeArrayMap(ArrayStatistics.Maximum);
        }

        /// <summary>
        /// Estimates the pointwise arithmetic sample mean from the vectors.
        /// Returns NaN if data is empty or any entry is NaN.
        /// </summary>
        /// <param name="data">Vector array, where all vectors have the same length.</param>
        public static Vector<double> Mean(this Vector<double>[] data)
        {
            return data.TransposeArrayMap(ArrayStatistics.Mean);
        }

        /// <summary>
        /// Estimates the pointwise unbiased population variance from the provided samples as vectors.
        /// On a dataset of size N will use an N-1 normalizer (Bessel's correction).
        /// Returns NaN if data has less than two entries or if any entry is NaN.
        /// </summary>
        /// <param name="samples">Vector array, where all vectors have the same length.</param>
        public static Vector<double> Variance(this Vector<double>[] samples)
        {
            return samples.TransposeArrayMap(ArrayStatistics.Variance);
        }

        /// <summary>
        /// Evaluates the pointwise population variance from the full population provided as vectors.
        /// On a dataset of size N will use an N normalizer and would thus be biased if applied to a subset.
        /// Returns NaN if data is empty or if any entry is NaN.
        /// </summary>
        /// <param name="population">Vector array, where all vectors have the same length.</param>
        public static Vector<double> PopulationVariance(this Vector<double>[] population)
        {
            return population.TransposeArrayMap(ArrayStatistics.PopulationVariance);
        }

        /// <summary>
        /// Estimates the pointwise unbiased population standard deviation from the provided samples as vectors.
        /// On a dataset of size N will use an N-1 normalizer (Bessel's correction).
        /// Returns NaN if data has less than two entries or if any entry is NaN.
        /// </summary>
        /// <param name="samples">Vector array, where all vectors have the same length.</param>
        public static Vector<double> StandardDeviation(this Vector<double>[] samples)
        {
            return samples.TransposeArrayMap(ArrayStatistics.StandardDeviation);
        }

        /// <summary>
        /// Evaluates the pointwise population standard deviation from the full population provided as vectors.
        /// On a dataset of size N will use an N normalizer and would thus be biased if applied to a subset.
        /// Returns NaN if data is empty or if any entry is NaN.
        /// </summary>
        /// <param name="population">Vector array, where all vectors have the same length.</param>
        public static Vector<double> PopulationStandardDeviation(this Vector<double>[] population)
        {
            return population.TransposeArrayMap(ArrayStatistics.PopulationStandardDeviation);
        }

        /// <summary>
        /// Estimates the pointwise unbiased population skewness from the provided samples.
        /// Uses a normalizer (Bessel's correction; type 2).
        /// Returns NaN if data has less than three entries or if any entry is NaN.
        /// </summary>
        /// <param name="samples">Vector array, where all vectors have the same length.</param>
        public static Vector<double> Skewness(this Vector<double>[] samples)
        {
            return samples.TransposeArrayMap(array => new RunningStatistics(array).Skewness);
        }

        /// <summary>
        /// Evaluates the pointwise skewness from the full population.
        /// Does not use a normalizer and would thus be biased if applied to a subset (type 1).
        /// Returns NaN if data has less than two entries or if any entry is NaN.
        /// </summary>
        /// <param name="population">Vector array, where all vectors have the same length.</param>
        public static Vector<double> PopulationSkewness(this Vector<double>[] population)
        {
            return population.TransposeArrayMap(array => new RunningStatistics(array).PopulationSkewness);
        }

        /// <summary>
        /// Estimates the pointwise unbiased population kurtosis from the provided samples.
        /// Uses a normalizer (Bessel's correction; type 2).
        /// Returns NaN if data has less than four entries or if any entry is NaN.
        /// </summary>
        /// <param name="samples">Vector array, where all vectors have the same length.</param>
        public static Vector<double> Kurtosis(this Vector<double>[] samples)
        {
            return samples.TransposeArrayMap(array => new RunningStatistics(array).Kurtosis);
        }

        /// <summary>
        /// Evaluates the pointwise kurtosis from the full population.
        /// Does not use a normalizer and would thus be biased if applied to a subset (type 1).
        /// Returns NaN if data has less than three entries or if any entry is NaN.
        /// </summary>
        /// <param name="population">Vector array, where all vectors have the same length.</param>
        public static Vector<double> PopulationKurtosis(this Vector<double>[] population)
        {
            return population.TransposeArrayMap(array => new RunningStatistics(array).PopulationKurtosis);
        }

        /// <summary>
        /// Estimates the pointwise root mean square (RMS) also known as quadratic mean from the vectors.
        /// Returns NaN if data is empty or any entry is NaN.
        /// </summary>
        /// <param name="data">Vector array, where all vectors have the same length.</param>
        public static Vector<double> RootMeanSquare(this Vector<double>[] data)
        {
            return data.TransposeArrayMap(ArrayStatistics.RootMeanSquare);
        }

        /// <summary>
        /// Returns the pointwise order statistic (order 1..N) from the vectors.
        /// </summary>
        /// <param name="data">Vector array, where all vectors have the same length.</param>
        /// <param name="order">One-based order of the statistic, must be between 1 and N (inclusive).</param>
        public static Vector<double> OrderStatistic(this Vector<double>[] data, int order)
        {
            return data.TransposeArrayMap(array => ArrayStatistics.OrderStatisticInplace(array, order));
        }

        /// <summary>
        /// Estimates the pointwise median value from the vectors.
        /// </summary>
        /// <param name="data">Vector array, where all vectors have the same length.</param>
        public static Vector<double> Median(this Vector<double>[] data)
        {
            return data.TransposeArrayMap(ArrayStatistics.MedianInplace);
        }

        /// <summary>
        /// Estimates the pointwise p-Percentile value from the vectors.
        /// If a non-integer Percentile is needed, use Quantile instead.
        /// Approximately median-unbiased regardless of the sample distribution (R8).
        /// </summary>
        /// <param name="data">Vector array, where all vectors have the same length.</param>
        /// <param name="p">Percentile selector, between 0 and 100 (inclusive).</param>
        public static Vector<double> Percentile(this Vector<double>[] data, int p)
        {
            return data.TransposeArrayMap(array => ArrayStatistics.PercentileInplace(array, p));
        }

        /// <summary>
        /// Estimates the pointwise first quartile value from the vectors.
        /// Approximately median-unbiased regardless of the sample distribution (R8).
        /// </summary>
        /// <param name="data">Vector array, where all vectors have the same length.</param>
        public static Vector<double> LowerQuartile(this Vector<double>[] data)
        {
            return data.TransposeArrayMap(ArrayStatistics.LowerQuartileInplace);
        }

        /// <summary>
        /// Estimates the pointwise third quartile value from the vectors.
        /// Approximately median-unbiased regardless of the sample distribution (R8).
        /// </summary>
        /// <param name="data">Vector array, where all vectors have the same length.</param>
        public static Vector<double> UpperQuartile(this Vector<double>[] data)
        {
            return data.TransposeArrayMap(ArrayStatistics.UpperQuartileInplace);
        }

        /// <summary>
        /// Estimates the pointwise inter-quartile range from the vectors.
        /// Approximately median-unbiased regardless of the sample distribution (R8).
        /// </summary>
        /// <param name="data">Vector array, where all vectors have the same length.</param>
        public static Vector<double> InterquartileRange(this Vector<double>[] data)
        {
            return data.TransposeArrayMap(ArrayStatistics.InterquartileRangeInplace);
        }

        /// <summary>
        /// Estimates the pointwise tau-th quantile from the vectors.
        /// The tau-th quantile is the data value where the cumulative distribution
        /// function crosses tau.
        /// Approximately median-unbiased regardless of the sample distribution (R8).
        /// </summary>
        /// <param name="data">Vector array, where all vectors have the same length.</param>
        /// <param name="tau">Quantile selector, between 0.0 and 1.0 (inclusive).</param>
        /// <remarks>
        /// R-8, SciPy-(1/3,1/3):
        /// Linear interpolation of the approximate medians for order statistics.
        /// When tau &lt; (2/3) / (N + 1/3), use x1. When tau &gt;= (N - 1/3) / (N + 1/3), use xN.
        /// </remarks>
        public static Vector<double> Quantile(this Vector<double>[] data, double tau)
        {
            return data.TransposeArrayMap(array => ArrayStatistics.QuantileInplace(array, tau));
        }

        /// <summary>
        /// Estimates the pointwise tau-th quantile from the vectors.
        /// The tau-th quantile is the data value where the cumulative distribution
        /// function crosses tau. The quantile definition can be specified
        /// by 4 parameters a, b, c and d, consistent with Mathematica.
        /// </summary>
        /// <param name="data">Vector array, where all vectors have the same length.</param>
        /// <param name="tau">Quantile selector, between 0.0 and 1.0 (inclusive)</param>
        /// <param name="a">a-parameter</param>
        /// <param name="b">b-parameter</param>
        /// <param name="c">c-parameter</param>
        /// <param name="d">d-parameter</param>
        public static Vector<double> QuantileCustom(this Vector<double>[] data, double tau, double a, double b, double c, double d)
        {
            return data.TransposeArrayMap(array => ArrayStatistics.QuantileCustomInplace(array, tau, a, b, c, d));
        }

        /// <summary>
        /// Estimates the pointwise tau-th quantile from the vectors.
        /// The tau-th quantile is the data value where the cumulative distribution
        /// function crosses tau. The quantile definition can be specified to be compatible
        /// with an existing system.
        /// </summary>
        /// <param name="data">Vector array, where all vectors have the same length.</param>
        /// <param name="tau">Quantile selector, between 0.0 and 1.0 (inclusive)</param>
        /// <param name="definition">Quantile definition, to choose what product/definition it should be consistent with</param>
        public static Vector<double> QuantileCustom(this Vector<double>[] data, double tau, QuantileDefinition definition)
        {
            return data.TransposeArrayMap(array => ArrayStatistics.QuantileCustomInplace(array, tau, definition));
        }

        /// <summary>
        /// Estimates the pointwise quantile tau from the vectors.
        /// The tau-th quantile is the data value where the cumulative distribution
        /// function crosses tau. The quantile definition can be specified to be compatible
        /// with an existing system.
        /// </summary>
        /// <param name="data">Vector array, where all vectors have the same length.</param>
        /// <param name="x">Quantile value.</param>
        /// <param name="definition">Rank definition, to choose how ties should be handled and what product/definition it should be consistent with</param>
        public static Vector<double> QuantileRank(this Vector<double>[] data, double x, RankDefinition definition = RankDefinition.Default)
        {
            return data.TransposeArrayMap(array =>
            {
                Array.Sort(array);
                return SortedArrayStatistics.QuantileRank(array, x, definition);
            });
        }

        /// <summary>
        /// Estimates the pointwise empirical cumulative distribution function (CDF) at x from the provided samples.
        /// </summary>
        /// <param name="data">Vector array, where all vectors have the same length.</param>
        /// <param name="x">The value where to estimate the CDF at.</param>
        public static Vector<double> EmpiricalCDF(this Vector<double>[] data, double x)
        {
            return data.TransposeArrayMap(array =>
            {
                Array.Sort(array);
                return SortedArrayStatistics.EmpiricalCDF(array, x);
            });
        }

        /// <summary>
        /// Estimates the pointwise empirical inverse CDF at tau from the provided samples.
        /// </summary>
        /// <param name="data">Vector array, where all vectors have the same length.</param>
        /// <param name="tau">Quantile selector, between 0.0 and 1.0 (inclusive).</param>
        public static Vector<double> EmpiricalInvCDF(this Vector<double>[] data, double tau)
        {
            return data.TransposeArrayMap(array => ArrayStatistics.QuantileCustomInplace(array, tau, QuantileDefinition.EmpiricalInvCDF));
        }

        /// <summary>
        /// Calculates the pointwise entropy of the vectors in bits.
        /// Returns NaN if any of the values in the stream are NaN.
        /// </summary>
        /// <param name="data">Vector array, where all vectors have the same length.</param>
        public static Vector<double> Entropy(this Vector<double>[] data)
        {
            return data.TransposeArrayMap(StreamingStatistics.Entropy);
        }
    }
}
