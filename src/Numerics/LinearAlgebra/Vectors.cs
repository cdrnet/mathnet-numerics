// <copyright file="Vectors.cs" company="Math.NET">
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
using System.Collections.Generic;
using MathNet.Numerics.Properties;

namespace MathNet.Numerics.LinearAlgebra
{
    /// <summary>
    /// Operations on a set of vectors
    /// </summary>
    public static class Vectors
    {
        /// <summary>
        /// Reduces all vectors by applying a vector function between two of them, until only a single vector is left.
        /// </summary>
        public static Vector<T> Reduce<T>(this Vector<T>[] vectors, Func<Vector<T>, Vector<T>, Vector<T>> f) where T : struct, IEquatable<T>, IFormattable
        {
            if (vectors.Length == 0)
            {
                throw new ArgumentException(Resources.CollectionEmpty);
            }
            Vector<T> state = vectors[0];
            for (int i = 1; i < vectors.Length; i++)
            {
                state = f(state, vectors[i]);
            }
            return state;
        }

        /// <summary>
        /// Reduce all vectors by applying an in-place vector function between two of them, until only a single vector is left.
        /// After each invocation of the reduction function, the routine expects the new state to be written to the first argument (in-place).
        /// Assuming that the reduction function does not change the second argument, none of the provided vectors are modified.
        /// </summary>
        public static Vector<T> ReduceInplace<T>(this Vector<T>[] vectors, Action<Vector<T>, Vector<T>> f) where T : struct, IEquatable<T>, IFormattable
        {
            if (vectors.Length == 0)
            {
                throw new ArgumentException(Resources.CollectionEmpty);
            }
            if (vectors.Length == 1)
            {
                return vectors[0];
            }
            Vector<T> state = vectors[0].Clone();
            for (int i = 1; i < vectors.Length; i++)
            {
                f(state, vectors[i]);
            }
            return state;
        }

        /// <summary>
        /// Reduces all vectors by applying a scalar function pointwise between two of them, until only a single vector is left.
        /// </summary>
        public static Vector<T> ReducePointwise<T>(this Vector<T>[] vectors, Func<T, T, T> f, Zeros zeros = Zeros.AllowSkip) where T : struct, IEquatable<T>, IFormattable
        {
            if (vectors.Length == 0)
            {
                throw new ArgumentException(Resources.CollectionEmpty);
            }
            Vector<T> state = vectors[0].Clone();
            for (int i = 1; i < vectors.Length; i++)
            {
                state.Map2(f, vectors[i], state, zeros);
            }
            return state;
        }

        /// <summary>
        /// Reduces all vectors by applying a vector function between two of them, until only a single vector is left.
        /// </summary>
        public static Vector<T> Reduce<T>(this IEnumerable<Vector<T>> vectors, Func<Vector<T>, Vector<T>, Vector<T>> f) where T : struct, IEquatable<T>, IFormattable
        {
            Vector<T> state;
            using (var iterator = vectors.GetEnumerator())
            {
                if (iterator.MoveNext())
                {
                    state = iterator.Current;
                }
                else
                {
                    throw new ArgumentException(Resources.CollectionEmpty);
                }
                while (iterator.MoveNext())
                {
                    state = f(state, iterator.Current);
                }
            }
            return state;
        }

        /// <summary>
        /// Reduce all vectors by applying an in-place vector function between two of them, until only a single vector is left.
        /// After each invocation of the reduction function, the routine expects the new state to be written to the first argument (in-place).
        /// Assuming that the reduction function does not change the second argument, none of the provided vectors are modified.
        /// </summary>
        public static Vector<T> ReduceInplace<T>(this IEnumerable<Vector<T>> vectors, Action<Vector<T>, Vector<T>> f) where T : struct, IEquatable<T>, IFormattable
        {
            Vector<T> state;
            using (var iterator = vectors.GetEnumerator())
            {
                if (iterator.MoveNext())
                {
                    state = iterator.Current.Clone();
                }
                else
                {
                    throw new ArgumentException(Resources.CollectionEmpty);
                }
                while (iterator.MoveNext())
                {
                    f(state, iterator.Current);
                }
            }
            return state;
        }

        /// <summary>
        /// Reduce all vectors by applying a scalar function pointwise between two of them, until only a single vector is left.
        /// </summary>
        public static Vector<T> ReducePointwise<T>(this IEnumerable<Vector<T>> vectors, Func<T, T, T> f, Zeros zeros = Zeros.AllowSkip) where T : struct, IEquatable<T>, IFormattable
        {
            Vector<T> state;
            using (var iterator = vectors.GetEnumerator())
            {
                if (iterator.MoveNext())
                {
                    state = iterator.Current.Clone();
                }
                else
                {
                    throw new ArgumentException(Resources.CollectionEmpty);
                }
                while (iterator.MoveNext())
                {
                    state.Map2(f, iterator.Current, state, zeros);
                }
            }
            return state;
        }

        /// <summary>
        /// Applies a vector function f to each column vector, threading an accumulator vector argument through the computation.
        /// Returns the resulting accumulator vector.
        /// </summary>
        public static Vector<T> Fold<T>(this Vector<T>[] vectors, Func<Vector<T>, Vector<T>, Vector<T>> f, Vector<T> state) where T : struct, IEquatable<T>, IFormattable
        {
            for (int i = 0; i < vectors.Length; i++)
            {
                state = f(state, vectors[i]);
            }
            return state;
        }

        /// <summary>
        /// Applies an in-place vector function f to each column vector, threading an accumulator vector argument through the computation.
        /// After each invocation of the fold function, the routine expects the new state to be written to the first argument (in-place).
        /// Assuming that the reduction function does not change the second argument, none of the provided vectors are modified (except the state vector).
        /// The resulting accumulator vector is written back to the provided state vector.
        /// </summary>
        public static void FoldInplace<T>(this Vector<T>[] vectors, Action<Vector<T>, Vector<T>> f, Vector<T> state) where T : struct, IEquatable<T>, IFormattable
        {
            for (int i = 0; i < vectors.Length; i++)
            {
                f(state, vectors[i]);
            }
        }

        /// <summary>
        /// Applies a scalar function f pointwise to each column vector, threading an accumulator vector argument through the computation.
        /// </summary>
        public static Vector<T> FoldPointwise<T>(this Vector<T>[] vectors, Func<T, T, T> f, Vector<T> state, Zeros zeros = Zeros.AllowSkip) where T : struct, IEquatable<T>, IFormattable
        {
            state = state.Clone();
            for (int i = 0; i < vectors.Length; i++)
            {
                state.Map2(f, vectors[i], state, zeros);
            }
            return state;
        }

        /// <summary>
        /// Applies a vector function f to each column vector, threading an accumulator vector argument through the computation.
        /// Returns the resulting accumulator vector.
        /// </summary>
        public static Vector<T> Fold<T>(this IEnumerable<Vector<T>> vectors, Func<Vector<T>, Vector<T>, Vector<T>> f, Vector<T> state) where T : struct, IEquatable<T>, IFormattable
        {
            foreach (var vector in vectors)
            {
                state = f(state, vector);
            }
            return state;
        }

        /// <summary>
        /// Applies an in-place vector function f to each column vector, threading an accumulator vector argument through the computation.
        /// After each invocation of the fold function, the routine expects the new state to be written to the first argument (in-place).
        /// Assuming that the reduction function does not change the second argument, none of the provided vectors are modified (except the state vector).
        /// The resulting accumulator vector is written back to the provided state vector.
        /// </summary>
        public static void FoldInplace<T>(this IEnumerable<Vector<T>> vectors, Action<Vector<T>, Vector<T>> f, Vector<T> state) where T : struct, IEquatable<T>, IFormattable
        {
            foreach (var vector in vectors)
            {
                f(state, vector);
            }
        }

        /// <summary>
        /// Applies a a scalar function f pointwise to each column vector, threading an accumulator vector argument through the computation.
        /// </summary>
        public static Vector<T> FoldPointwise<T>(this IEnumerable<Vector<T>> vectors, Func<T, T, T> f, Vector<T> state, Zeros zeros = Zeros.AllowSkip) where T : struct, IEquatable<T>, IFormattable
        {
            state = state.Clone();
            foreach (var vector in vectors)
            {
                state.Map2(f, vector, state, zeros);
            }
            return state;
        }

        public static Vector<T>[] Transpose<T>(this Vector<T>[] vectors) where T : struct, IEquatable<T>, IFormattable
        {
            if (vectors.Length == 0)
            {
                return vectors;
            }

            var transposed = new Vector<T>[vectors[0].Count];
            for (int i = 0; i < transposed.Length; i++)
            {
                transposed[i] = Vector<T>.Build.SameAs(vectors[0], vectors.Length);
            }
            for (int i = 0; i < vectors.Length; i++)
            {
                foreach (var entry in vectors[i].EnumerateIndexed(Zeros.AllowSkip))
                {
                    transposed[entry.Item1].At(i, entry.Item2);
                }
            }
            return transposed;
        }

        public static Vector<TU> TransposeArrayMap<T, TU>(this Vector<T>[] vectors, Func<T[], TU> map)
            where T : struct, IEquatable<T>, IFormattable
            where TU : struct, IEquatable<TU>, IFormattable
        {
            if (vectors.Length == 0)
            {
                throw new ArgumentException(Resources.CollectionEmpty);
            }

            var transposed = new T[vectors[0].Count][];
            for (int i = 0; i < transposed.Length; i++)
            {
                transposed[i] = new T[vectors.Length];
            }
            for (int i = 0; i < vectors.Length; i++)
            {
                foreach (var entry in vectors[i].EnumerateIndexed(Zeros.AllowSkip))
                {
                    transposed[entry.Item1][i] = entry.Item2;
                }
            }
            return Vector<TU>.Build.Dense(transposed.Length, i => map(transposed[i]));
        }

        public static Vector<TU> TransposeArrayMapIndexed<T, TU>(this Vector<T>[] vectors, Func<int, T[], TU> map)
            where T : struct, IEquatable<T>, IFormattable
            where TU : struct, IEquatable<TU>, IFormattable
        {
            if (vectors.Length == 0)
            {
                throw new ArgumentException(Resources.CollectionEmpty);
            }

            var transposed = new T[vectors[0].Count][];
            for (int i = 0; i < transposed.Length; i++)
            {
                transposed[i] = new T[vectors.Length];
            }
            for (int i = 0; i < vectors.Length; i++)
            {
                foreach (var entry in vectors[i].EnumerateIndexed(Zeros.AllowSkip))
                {
                    transposed[entry.Item1][i] = entry.Item2;
                }
            }
            return Vector<TU>.Build.Dense(transposed.Length, i => map(i, transposed[i]));
        }

        public static Vector<T> Sum<T>(this Vector<T>[] vectors) where T : struct, IEquatable<T>, IFormattable
        {
            return ReduceInplace(vectors, (a, b) => a.Add(b, a));
        }

        public static Vector<T> Sum<T>(this IEnumerable<Vector<T>> vectors) where T : struct, IEquatable<T>, IFormattable
        {
            return ReduceInplace(vectors, (a, b) => a.Add(b, a));
        }
    }
}
