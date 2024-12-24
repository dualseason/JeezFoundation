﻿using System.ComponentModel;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace JeezFoundation.Algorithm;

/// <summary>Root type of the static functional methods in JeezFoundation.Algorithm.</summary>
public static partial class Statics
{
    #region Internals

    #region Constants

    /// <summary>Related: https://github.com/dotnet/roslyn/issues/49568</summary>
    internal const string NotIntended = "This member is not intended to be used.";

    /// <summary>The max <see cref="byte"/>s to stackalloc.</summary>
    internal const int Stackalloc = 256;

    #endregion Constants

    #region Optimizations

    // These are some shared internal optimizations that I don't want to expose because it might confuse people.
    // If you need to use it, just copy this code into your own project.

    internal static class MultiplyAddImplementation<T>
    {
        /// <summary>a * b + c</summary>
        internal static Func<T, T, T, T> Function = (a, b, c) =>
        {
            var A = Expression.Parameter(typeof(T));
            var B = Expression.Parameter(typeof(T));
            var C = Expression.Parameter(typeof(T));
            var BODY = Expression.Add(Expression.Multiply(A, B), C);
            Function = Expression.Lambda<Func<T, T, T, T>>(BODY, A, B, C).Compile();
            return Function(a, b, c);
        };
    }

    internal static class D_subtract_A_multiply_B_divide_C<T>
    {
        /// <summary>d - a * b / c</summary>
        internal static Func<T, T, T, T, T> Function = (a, b, c, d) =>
        {
            var A = Expression.Parameter(typeof(T));
            var B = Expression.Parameter(typeof(T));
            var C = Expression.Parameter(typeof(T));
            var D = Expression.Parameter(typeof(T));
            var BODY = Expression.Subtract(D, Expression.Divide(Expression.Multiply(A, B), C));
            Function = Expression.Lambda<Func<T, T, T, T, T>>(BODY, A, B, C, D).Compile();
            return Function(a, b, c, d);
        };
    }

    #endregion Optimizations

    #region Helpers

    internal static T OperationOnStepper<T>(Action<Action<T>> stepper, Func<T, T, T> operation)
    {
        if (stepper is null) throw new ArgumentNullException(nameof(stepper));
        T? result = default;
        bool assigned = false;
        stepper(a =>
        {
            if (assigned)
            {
                result = operation(result!, a);
            }
            else
            {
                result = a;
                assigned = true;
            }
        });
        return assigned
            ? result!
            : throw new ArgumentException($"{nameof(stepper)} is empty.", nameof(stepper));
    }

    #endregion Helpers

    #endregion Internals

    #region Swap

    /// <summary>Swaps two values.</summary>
    /// <typeparam name="T">The type of values to swap.</typeparam>
    /// <param name="a">The first value of the swap.</param>
    /// <param name="b">The second value of the swap.</param>
    public static void Swap<T>(ref T a, ref T b) => (a, b) = (b, a);

    /// <summary>Swaps two values.</summary>
    /// <typeparam name="T">The type of values to swap.</typeparam>
    /// <typeparam name="TGet">The type of the get method.</typeparam>
    /// <typeparam name="TSet">The type of the set method.</typeparam>
    /// <param name="a">The index of the first value to swap.</param>
    /// <param name="b">The index of the second value to swap.</param>
    /// <param name="get">The the get method.</param>
    /// <param name="set">The the set method.</param>
    public static void Swap<T, TGet, TSet>(int a, int b, TGet get, TSet set)
        where TGet : struct, IFunc<int, T>
        where TSet : struct, IAction<int, T>
    {
        T temp = get.Invoke(a);
        set.Invoke(a, get.Invoke(b));
        set.Invoke(b, temp);
    }

    #endregion Swap

    #region source...

#pragma warning disable IDE1006 // Naming Styles

    /// <summary>Gets the directory path of the current location in source code.</summary>
    /// <param name="default">Intended to leave default. This value is set by the compiler via <see cref="CallerFilePathAttribute"/>.</param>
    /// <returns>The directory path of the current location in source code.</returns>
    public static string? sourcedirectory([CallerFilePath] string @default = default!) => Path.GetDirectoryName(@default);

    /// <summary>Gets the file path of the current location in source code.</summary>
    /// <param name="default">Intended to leave default. This value is set by the compiler via <see cref="CallerFilePathAttribute"/>.</param>
    /// <returns>The file path of the current location in source code.</returns>
    public static string sourcefilepath([CallerFilePath] string @default = default!) => @default;

    /// <summary>Gets the member name of the current location in source code.</summary>
    /// <param name="default">Intended to leave default. This value is set by the compiler via <see cref="CallerMemberNameAttribute"/>.</param>
    /// <returns>The member name of the current location in source code.</returns>
    public static string sourcemembername([CallerMemberName] string @default = default!) => @default;

    /// <summary>Gets the line number of the current location in source code.</summary>
    /// <param name="default">Intended to leave default. This value is set by the compiler via <see cref="CallerLineNumberAttribute"/>.</param>
    /// <returns>The line number of the current location in source code.</returns>
    public static int sourcelinenumber([CallerLineNumber] int @default = default) => @default;

    /// <summary>Gets the source code and evaluation of an expression.</summary>
    /// <typeparam name="T">The type the expression will evaluate to.</typeparam>
    /// <param name="expression">The expression to evaluate and get the source of.</param>
    /// <param name="default">Intended to leave default. This value is set by the compiler via <see cref="CallerArgumentExpressionAttribute"/>.</param>
    /// <returns>The source code and evaluation of the expression.</returns>
    public static (T Value, string Source) sourceof<T>(T expression, [CallerArgumentExpression("expression")] string? @default = default) => (expression, @default!);

    /// <summary>Gets the source code and evaluation of an expression.</summary>
    /// <typeparam name="T">The type the expression will evaluate to.</typeparam>
    /// <param name="expression">The expression to evaluate and get the source of.</param>
    /// <param name="source">The source code of the expression.</param>
    /// <param name="default">Intended to leave default. This value is set by the compiler via <see cref="CallerArgumentExpressionAttribute"/>.</param>
    /// <returns>The evaluation of the expression.</returns>
    public static T sourceof<T>(T expression, out string source, [CallerArgumentExpression("expression")] string? @default = default)
    {
        source = @default!;
        return expression;
    }

#pragma warning restore IDE1006 // Naming Styles

    #endregion source...

    #region TryParse

    /// <summary>Tries to parse a <see cref="string"/> into a value of the type <typeparamref name="T"/>.</summary>
    /// <typeparam name="T">The type to parse the <see cref="string"/> into a value of.</typeparam>
    /// <param name="string">The <see cref="string"/> to parse into a value ot type <typeparamref name="T"/>.</param>
    /// <returns>
    /// - <see cref="bool"/> Success: true if the parse was successful or false if not<br/>
    /// - <typeparamref name="T"/> Value: the value if the parse was successful or default if not
    /// </returns>
    public static (bool Success, T? Value) TryParse<T>(string @string) =>
        (TryParseImplementation<T>.Function(@string, out T? value), value);

    internal static class TryParseImplementation<T>
    {
        internal delegate TResult TryParse<T1, T2, TResult>(T1 arg1, out T2? arg2);

        internal static TryParse<string, T, bool> Function = (string @string, out T? value) =>
        {
            static bool Fail(string @string, out T? value)
            {
                value = default;
                return false;
            }
            if (typeof(T).IsEnum)
            {
                foreach (MethodInfo methodInfo in typeof(Enum).GetMethods(
                    BindingFlags.Static |
                    BindingFlags.Public))
                {
                    if (methodInfo.Name == nameof(Enum.TryParse) &&
                        methodInfo.IsGenericMethod &&
                        methodInfo.IsStatic &&
                        methodInfo.IsPublic &&
                        methodInfo.ReturnType == typeof(bool))
                    {
                        MethodInfo genericMethodInfo = methodInfo.MakeGenericMethod(typeof(T));
                        ParameterInfo[] parameters = genericMethodInfo.GetParameters();
                        if (parameters.Length is 2 &&
                            parameters[0].ParameterType == typeof(string) &&
                            parameters[1].ParameterType == typeof(T).MakeByRefType())
                        {
                            Function = genericMethodInfo.CreateDelegate<TryParse<string, T, bool>>();
                            return Function(@string, out value);
                        }
                    }
                }
                throw new TowelBugException("The System.Enum.TryParse method was not found via reflection.");
            }
            else
            {
                MethodInfo? methodInfo = Meta.GetTryParseMethod<T>();
                Function = methodInfo is null
                    ? Fail
                    : methodInfo.CreateDelegate<TryParse<string, T, bool>>();
                return Function(@string, out value);
            }
        };
    }

    #endregion TryParse

    #region Hash

    /// <summary>Static wrapper for the instance based "object.GetHashCode" function.</summary>
    /// <typeparam name="T">The generic type of the hash operation.</typeparam>
    /// <param name="value">The item to get the hash code of.</param>
    /// <returns>The computed hash code using the base GetHashCode instance method.</returns>
    public static int Hash<T>(T value)
    {
        if (value is null) throw new ArgumentNullException(nameof(value));
        return value.GetHashCode();
    }

    #endregion Hash

    #region Convert

    /// <summary>Converts <paramref name="a"/> from <typeparamref name="TA"/> to <typeparamref name="TB"/>.</summary>
    /// <typeparam name="TA">The type of the value to convert.</typeparam>
    /// <typeparam name="TB">The type to convert the value to.</typeparam>
    /// <param name="a">The value to convert.</param>
    /// <returns>The <paramref name="a"/> value of <typeparamref name="TB"/> type.</returns>
    public static TB Convert<TA, TB>(TA a) =>
        ConvertImplementation<TA, TB>.Function(a);

    internal static class ConvertImplementation<TA, TB>
    {
        internal static Func<TA, TB> Function = a =>
        {
            ParameterExpression A = Expression.Parameter(typeof(TA));
            Expression BODY = Expression.Convert(A, typeof(TB));
            Function = Expression.Lambda<Func<TA, TB>>(BODY, A).Compile();
            return Function(a);
        };
    }

    #endregion Convert

    #region Join

    /// <summary>Iterates a <see cref="System.Range"/> and joins the results of a System.Func&lt;int, string&gt; seperated by a <see cref="string"/> <paramref name="seperator"/>.</summary>
    /// <param name="range">The range of values to use use on the &lt;System.Func{int, string&gt; <paramref name="func"/>.</param>
    /// <param name="func">The System.Func&lt;int, string&gt;.</param>
    /// <param name="seperator">The <see cref="string"/> seperator to join the values with.</param>
    /// <returns>The resulting <see cref="string"/> of the join.</returns>
    public static string Join(Range range, Func<int, string> func, string seperator) =>
        string.Join(seperator, range.ToIEnumerable().Select(func));

    #endregion Join

    #region Equate

#if false
		/// <summary>Checks for equality of two values [<paramref name="a"/> == <paramref name="b"/>].</summary>
		/// <typeparam name="A">The type of the left operand.</typeparam>
		/// <typeparam name="B">The type of the right operand.</typeparam>
		/// <typeparam name="C">The type of the return.</typeparam>
		/// <param name="a">The left operand.</param>
		/// <param name="b">The right operand.</param>
		/// <returns>The result of the equality.</returns>
		public static C Equate<A, B, C>(A a, B b) =>
			EquateImplementation<A, B, C>.Function(a, b);
#endif

    /// <summary>Checks for equality of two values [<paramref name="a"/> == <paramref name="b"/>].</summary>
    /// <typeparam name="T">The type of the operation.</typeparam>
    /// <param name="a">The left operand.</param>
    /// <param name="b">The right operand.</param>
    /// <returns>The result of the equality check.</returns>
    public static bool Equate<T>(T a, T b) =>
        EquateImplementation<T, T, bool>.Function(a, b);

    /// <summary>Checks for equality among multiple values [<paramref name="a"/> == <paramref name="b"/> == <paramref name="c"/> == ...].</summary>
    /// <typeparam name="T">The numeric type of the operation.</typeparam>
    /// <param name="a">The first operand of the equality check.</param>
    /// <param name="b">The second operand of the equality check.</param>
    /// <param name="c">The remaining operands of the equality check.</param>
    /// <returns>True if all operands are equal or false if not.</returns>
    public static bool Equate<T>(T a, T b, params T[] c)
    {
        if (c is null) throw new ArgumentNullException(nameof(c));
        if (c.Length is 0) throw new ArgumentException("The array is empty.", nameof(c));
        if (!Equate(a, b))
        {
            return false;
        }
        for (int i = 0; i < c.Length; i++)
        {
            if (!Equate(a, c[i]))
            {
                return false;
            }
        }
        return true;
    }

    internal static class EquateImplementation<TA, TB, TC>
    {
        internal static Func<TA, TB, TC> Function = (a, b) =>
        {
#warning TODO: kill this try catch
            try
            {
                var A = Expression.Parameter(typeof(TA));
                var B = Expression.Parameter(typeof(TB));
                var BODY = Expression.Equal(A, B);
                Function = Expression.Lambda<Func<TA, TB, TC>>(BODY, A, B).Compile();
                return Function(a, b);
            }
            catch
            {
            }

            if (typeof(TC) == typeof(bool))
            {
                EquateImplementation<TA, TB, bool>.Function =
                    (typeof(TA).IsValueType, typeof(TB).IsValueType) switch
                    {
                        (true, true) => (A, B) => A!.Equals(B),
                        (true, false) => (A, B) => A!.Equals(B),
                        (false, true) => (A, B) => B!.Equals(A),
                        (false, false) =>
                            (A, B) =>
                                (A, B) switch
                                {
                                    (null, null) => true,
                                    (_, null) => false,
                                    (null, _) => false,
                                    _ => A.Equals(B),
                                },
                    };
                return Function!(a, b);
            }
#warning TODO
            throw new NotImplementedException();
        };
    }

    #endregion Equate

    #region Inequate

    /// <summary>Checks for inequality of two values [<paramref name="a"/> != <paramref name="b"/>].</summary>
    /// <typeparam name="TA">The type of the left operand.</typeparam>
    /// <typeparam name="TB">The type of the right operand.</typeparam>
    /// <typeparam name="TC">The type of the return.</typeparam>
    /// <param name="a">The left operand.</param>
    /// <param name="b">The right operand.</param>
    /// <returns>The result of the inequality.</returns>
    public static TC Inequate<TA, TB, TC>(TA a, TB b) =>
        InequateImplementation<TA, TB, TC>.Function(a, b);

    /// <summary>Checks for inequality of two values [<paramref name="a"/> != <paramref name="b"/>].</summary>
    /// <typeparam name="T">The numeric type of the operation.</typeparam>
    /// <param name="a">The first operand of the inequality check.</param>
    /// <param name="b">The second operand of the inequality check.</param>
    /// <returns>The result of the inequality check.</returns>
    public static bool Inequate<T>(T a, T b) =>
        Inequate<T, T, bool>(a, b);

    internal static class InequateImplementation<TA, TB, TC>
    {
        internal static Func<TA, TB, TC> Function = (a, b) =>
        {
            var A = Expression.Parameter(typeof(TA));
            var B = Expression.Parameter(typeof(TB));
            var BODY = Expression.NotEqual(A, B);
            Function = Expression.Lambda<Func<TA, TB, TC>>(BODY, A, B).Compile();
            return Function(a, b);
        };
    }

    #endregion Inequate

    #region LessThan

    /// <summary>Checks if one value is less than another [<paramref name="a"/> &lt; <paramref name="b"/>].</summary>
    /// <typeparam name="TA">The type of the left operand.</typeparam>
    /// <typeparam name="TB">The type of the right operand.</typeparam>
    /// <typeparam name="TC">The type of the return.</typeparam>
    /// <param name="a">The left operand.</param>
    /// <param name="b">The right operand.</param>
    /// <returns>The result of the less than operation.</returns>
    public static TC LessThan<TA, TB, TC>(TA a, TB b) =>
        LessThanImplementation<TA, TB, TC>.Function(a, b);

    /// <summary>Checks if one value is less than another [<paramref name="a"/> &lt; <paramref name="b"/>].</summary>
    /// <typeparam name="T">The numeric type of the operation.</typeparam>
    /// <param name="a">The first operand of the less than check.</param>
    /// <param name="b">The second operand of the less than check.</param>
    /// <returns>The result of the less than check.</returns>
    public static bool LessThan<T>(T a, T b) =>
        LessThan<T, T, bool>(a, b);

    internal static class LessThanImplementation<TA, TB, TC>
    {
        internal static Func<TA, TB, TC> Function = (a, b) =>
        {
            var A = Expression.Parameter(typeof(TA));
            var B = Expression.Parameter(typeof(TB));
            var BODY = Expression.LessThan(A, B);
            Function = Expression.Lambda<Func<TA, TB, TC>>(BODY, A, B).Compile();
            return Function(a, b);
        };
    }

    #endregion LessThan

    #region GreaterThan

    /// <summary>Checks if one value is greater than another [<paramref name="a"/> &gt; <paramref name="b"/>].</summary>
    /// <typeparam name="TA">The type of the left operand.</typeparam>
    /// <typeparam name="TB">The type of the right operand.</typeparam>
    /// <typeparam name="TC">The type of the return.</typeparam>
    /// <param name="a">The left operand.</param>
    /// <param name="b">The right operand.</param>
    /// <returns>The result of the greater than operation.</returns>
    public static TC GreaterThan<TA, TB, TC>(TA a, TB b) =>
        GreaterThanImplementation<TA, TB, TC>.Function(a, b);

    /// <summary>Checks if one value is greater than another [<paramref name="a"/> &gt; <paramref name="b"/>].</summary>
    /// <typeparam name="T">The numeric type of the operation.</typeparam>
    /// <param name="a">The first operand of the greater than check.</param>
    /// <param name="b">The second operand of the greater than check.</param>
    /// <returns>The result of the greater than check.</returns>
    public static bool GreaterThan<T>(T a, T b) =>
        GreaterThan<T, T, bool>(a, b);

    internal static class GreaterThanImplementation<TA, TB, TC>
    {
        internal static Func<TA, TB, TC> Function = (a, b) =>
        {
            var A = Expression.Parameter(typeof(TA));
            var B = Expression.Parameter(typeof(TB));
            var BODY = Expression.GreaterThan(A, B);
            Function = Expression.Lambda<Func<TA, TB, TC>>(BODY, A, B).Compile();
            return Function(a, b);
        };
    }

    #endregion GreaterThan

    #region LessThanOrEqual

    /// <summary>Checks if one value is less than or equal to another [<paramref name="a"/> &lt;= <paramref name="b"/>].</summary>
    /// <typeparam name="TA">The type of the left operand.</typeparam>
    /// <typeparam name="TB">The type of the right operand.</typeparam>
    /// <typeparam name="TC">The type of the return.</typeparam>
    /// <param name="a">The left operand.</param>
    /// <param name="b">The right operand.</param>
    /// <returns>The result of the less than or equal to operation.</returns>
    public static TC LessThanOrEqual<TA, TB, TC>(TA a, TB b) =>
        LessThanOrEqualImplementation<TA, TB, TC>.Function(a, b);

    /// <summary>Checks if one value is less than or equal to another [<paramref name="a"/> &lt;= <paramref name="b"/>].</summary>
    /// <typeparam name="T">The numeric type of the operation.</typeparam>
    /// <param name="a">The first operand of the less than or equal to check.</param>
    /// <param name="b">The second operand of the less than or equal to check.</param>
    /// <returns>The result of the less than or equal to check.</returns>
    public static bool LessThanOrEqual<T>(T a, T b) =>
        LessThanOrEqual<T, T, bool>(a, b);

    internal static class LessThanOrEqualImplementation<TA, TB, TC>
    {
        internal static Func<TA, TB, TC> Function = (a, b) =>
        {
            var A = Expression.Parameter(typeof(TA));
            var B = Expression.Parameter(typeof(TB));
            var BODY = Expression.LessThanOrEqual(A, B);
            Function = Expression.Lambda<Func<TA, TB, TC>>(BODY, A, B).Compile();
            return Function(a, b);
        };
    }

    #endregion LessThanOrEqual

    #region GreaterThanOrEqual

    /// <summary>Checks if one value is less greater or equal to another [<paramref name="a"/> &gt;= <paramref name="b"/>].</summary>
    /// <typeparam name="TA">The type of the left operand.</typeparam>
    /// <typeparam name="TB">The type of the right operand.</typeparam>
    /// <typeparam name="TC">The type of the return.</typeparam>
    /// <param name="a">The left operand.</param>
    /// <param name="b">The right operand.</param>
    /// <returns>The result of the greater than or equal to operation.</returns>
    public static TC GreaterThanOrEqual<TA, TB, TC>(TA a, TB b) =>
        GreaterThanOrEqualImplementation<TA, TB, TC>.Function(a, b);

    /// <summary>Checks if one value is greater than or equal to another [<paramref name="a"/> &gt;= <paramref name="b"/>].</summary>
    /// <typeparam name="T">The numeric type of the operation.</typeparam>
    /// <param name="a">The first operand of the greater than or equal to check.</param>
    /// <param name="b">The second operand of the greater than or equal to check.</param>
    /// <returns>The result of the greater than or equal to check.</returns>
    public static bool GreaterThanOrEqual<T>(T a, T b) =>
        GreaterThanOrEqual<T, T, bool>(a, b);

    internal static class GreaterThanOrEqualImplementation<TA, TB, TC>
    {
        internal static Func<TA, TB, TC> Function = (a, b) =>
        {
            var A = Expression.Parameter(typeof(TA));
            var B = Expression.Parameter(typeof(TB));
            var BODY = Expression.GreaterThanOrEqual(A, B);
            Function = Expression.Lambda<Func<TA, TB, TC>>(BODY, A, B).Compile();
            return Function(a, b);
        };
    }

    #endregion GreaterThanOrEqual

    #region Compare

#if false
		/// <summary>Compares two values.</summary>
		/// <typeparam name="A">The type of the left operand.</typeparam>
		/// <typeparam name="B">The type of the right operand.</typeparam>
		/// <param name="a">The left operand.</param>
		/// <param name="b">The right operand.</param>
		/// <returns>The result of the comparison.</returns>
		public static C Compare<A, B, C>(A a, B b) =>
			CompareImplementation<A, B, C>.Function(a, b);
#endif

    /// <summary>Compares two values.</summary>
    /// <typeparam name="T">The type of values to compare.</typeparam>
    /// <param name="a">The first value of the comparison.</param>
    /// <param name="b">The second value of the comparison.</param>
    /// <returns>The result of the comparison.</returns>
    public static CompareResult Compare<T>(T a, T b) =>
        CompareImplementation<T, T, CompareResult>.Function(a, b);

    internal static class CompareImplementation<TA, TB, TC>
    {
        internal static Func<TA, TB, TC> Function = (a, b) =>
        {
            if (typeof(TC) == typeof(CompareResult))
            {
                if (typeof(TA) == typeof(TB) && a is IComparable<TB> &&
                    !(typeof(TA).IsPrimitive && typeof(TB).IsPrimitive))
                {
                    CompareImplementation<TA, TA, CompareResult>.Function =
                        (a, b) => System.Collections.Generic.Comparer<TA>.Default.Compare(a, b).ToCompareResult();
                }
                else
                {
                    var A = Expression.Parameter(typeof(TA));
                    var B = Expression.Parameter(typeof(TB));

                    var lessThanPredicate =
                        typeof(TA).IsPrimitive && typeof(TB).IsPrimitive
                        ? Expression.LessThan(A, B)
                        : Meta.GetLessThanMethod<TA, TB, bool>() is not null
                            ? Expression.LessThan(A, B)
                            : Meta.GetGreaterThanMethod<TB, TA, bool>() is not null
                                ? Expression.GreaterThan(B, A)
                                : null;

                    var greaterThanPredicate =
                        typeof(TA).IsPrimitive && typeof(TB).IsPrimitive
                        ? Expression.GreaterThan(A, B)
                        : Meta.GetGreaterThanMethod<TA, TB, bool>() is not null
                            ? Expression.GreaterThan(A, B)
                            : Meta.GetLessThanMethod<TB, TA, bool>() is not null
                                ? Expression.LessThan(B, A)
                                : null;

                    if (lessThanPredicate is null || greaterThanPredicate is null)
                    {
                        throw new NotSupportedException("You attempted a comparison operation with unsupported types.");
                    }

                    var RETURN = Expression.Label(typeof(CompareResult));
                    var BODY = Expression.Block(
                        Expression.IfThen(
                            lessThanPredicate,
                            Expression.Return(RETURN, Expression.Constant(Less, typeof(CompareResult)))),
                        Expression.IfThen(
                            greaterThanPredicate,
                            Expression.Return(RETURN, Expression.Constant(Greater, typeof(CompareResult)))),
                        Expression.Return(RETURN, Expression.Constant(Equal, typeof(CompareResult))),
                        Expression.Label(RETURN, Expression.Constant(default(CompareResult), typeof(CompareResult))));
                    CompareImplementation<TA, TB, CompareResult>.Function = Expression.Lambda<Func<TA, TB, CompareResult>>(BODY, A, B).Compile();
                }
                return Function!(a, b);
            }
#warning TODO
            throw new NotImplementedException();
        };
    }

    #endregion Compare

    #region Negation

    /// <summary>Negates a value [-<paramref name="a"/>].</summary>
    /// <typeparam name="TA">The type of the value to negate.</typeparam>
    /// <typeparam name="TB">The resulting type of the negation.</typeparam>
    /// <param name="a">The value to negate.</param>
    /// <returns>The result of the negation [-<paramref name="a"/>].</returns>
    public static TB Negation<TA, TB>(TA a) =>
        NegationImplementation<TA, TB>.Function(a);

    /// <summary>Negates a value [-<paramref name="a"/>].</summary>
    /// <typeparam name="T">The type of the value to negate.</typeparam>
    /// <param name="a">The value to negate.</param>
    /// <returns>The result of the negation [-<paramref name="a"/>].</returns>
    public static T Negation<T>(T a) =>
        Negation<T, T>(a);

    internal static class NegationImplementation<TA, TB>
    {
        internal static Func<TA, TB> Function = a =>
        {
            var A = Expression.Parameter(typeof(TA));
            var BODY = Expression.Negate(A);
            Function = Expression.Lambda<Func<TA, TB>>(BODY, A).Compile();
            return Function(a);
        };
    }

    #endregion Negation

    #region Addition

    /// <summary>Adds two values [<paramref name="a"/> + <paramref name="b"/>].</summary>
    /// <typeparam name="TA">The type of the left operand.</typeparam>
    /// <typeparam name="TB">The type of the right operand.</typeparam>
    /// <typeparam name="TC">The type of the return.</typeparam>
    /// <param name="a">The left operand.</param>
    /// <param name="b">The right operand.</param>
    /// <returns>The result of the addition [<paramref name="a"/> + <paramref name="b"/>].</returns>
    public static TC Addition<TA, TB, TC>(TA a, TB b) =>
        AdditionImplementation<TA, TB, TC>.Function(a, b);

    /// <summary>Adds two values [<paramref name="a"/> + <paramref name="b"/>].</summary>
    /// <typeparam name="T">The numeric type of the operation.</typeparam>
    /// <param name="a">The left operand.</param>
    /// <param name="b">The right operand.</param>
    /// <returns>The result of the addition [<paramref name="a"/> + <paramref name="b"/>].</returns>
    public static T Addition<T>(T a, T b) =>
        Addition<T, T, T>(a, b);

    /// <summary>Adds multiple values [<paramref name="a"/> + <paramref name="b"/> + <paramref name="c"/> + ...].</summary>
    /// <typeparam name="T">The type of the operation.</typeparam>
    /// <param name="a">The first operand of the addition.</param>
    /// <param name="b">The second operand of the addition.</param>
    /// <param name="c">The third operand of the addition.</param>
    /// <param name="d">The remaining operands of the addition.</param>
    /// <returns>The result of the addition [<paramref name="a"/> + <paramref name="b"/> + <paramref name="c"/> + ...].</returns>
    public static T Addition<T>(T a, T b, T c, params T[] d) =>
        Addition<T>(step => { step(a); step(b); step(c); d.ToStepper()(step); });

    /// <summary>Adds multiple values [step1 + step2 + step3 + ...].</summary>
    /// <typeparam name="T">The type of the operation.</typeparam>
    /// <param name="stepper">The stepper of the values to add.</param>
    /// <returns>The result of the addition [step1 + step2 + step3 + ...].</returns>
    public static T Addition<T>(Action<Action<T>> stepper) =>
        OperationOnStepper(stepper, Addition);

    internal static class AdditionImplementation<TA, TB, TC>
    {
        internal static Func<TA, TB, TC> Function = (a, b) =>
        {
            var A = Expression.Parameter(typeof(TA));
            var B = Expression.Parameter(typeof(TB));
            var BODY = Expression.Add(A, B);
            Function = Expression.Lambda<Func<TA, TB, TC>>(BODY, A, B).Compile();
            return Function(a, b);
        };
    }

    #endregion Addition

    #region Subtraction

    /// <summary>Subtracts two values [<paramref name="a"/> - <paramref name="b"/>].</summary>
    /// <typeparam name="TA">The type of the left operand.</typeparam>
    /// <typeparam name="TB">The type of the right operand.</typeparam>
    /// <typeparam name="TC">The type of the return.</typeparam>
    /// <param name="a">The left operand.</param>
    /// <param name="b">The right operand.</param>
    /// <returns>The result of the subtraction [<paramref name="a"/> - <paramref name="b"/>].</returns>
    public static TC Subtraction<TA, TB, TC>(TA a, TB b) =>
        SubtractionImplementation<TA, TB, TC>.Function(a, b);

    /// <summary>Subtracts two values [<paramref name="a"/> - <paramref name="b"/>].</summary>
    /// <typeparam name="T">The type of the operation.</typeparam>
    /// <param name="a">The left operand.</param>
    /// <param name="b">The right operand.</param>
    /// <returns>The result of the subtraction [<paramref name="a"/> - <paramref name="b"/>].</returns>
    public static T Subtraction<T>(T a, T b) =>
        Subtraction<T, T, T>(a, b);

    /// <summary>Subtracts multiple values [<paramref name="a"/> - <paramref name="b"/> - <paramref name="c"/> - ...].</summary>
    /// <typeparam name="T">The type of the operation.</typeparam>
    /// <param name="a">The first operand.</param>
    /// <param name="b">The second operand.</param>
    /// <param name="c">The third operand.</param>
    /// <param name="d">The remaining values.</param>
    /// <returns>The result of the subtraction [<paramref name="a"/> - <paramref name="b"/> - <paramref name="c"/> - ...].</returns>
    public static T Subtraction<T>(T a, T b, T c, params T[] d) =>
        Subtraction<T>(step => { step(a); step(b); step(c); d.ToStepper()(step); });

    /// <summary>Subtracts multiple numeric values [step1 - step2 - step3 - ...].</summary>
    /// <typeparam name="T">The numeric type of the operation.</typeparam>
    /// <param name="stepper">The stepper containing the values.</param>
    /// <returns>The result of the subtraction [step1 - step2 - step3 - ...].</returns>
    public static T Subtraction<T>(Action<Action<T>> stepper) =>
        OperationOnStepper(stepper, Subtraction);

    internal static class SubtractionImplementation<TA, TB, TC>
    {
        internal static Func<TA, TB, TC> Function = (a, b) =>
        {
            var A = Expression.Parameter(typeof(TA));
            var B = Expression.Parameter(typeof(TB));
            var BODY = Expression.Subtract(A, B);
            Function = Expression.Lambda<Func<TA, TB, TC>>(BODY, A, B).Compile();
            return Function(a, b);
        };
    }

    #endregion Subtraction

    #region Multiplication

    /// <summary>Multiplies two values [<paramref name="a"/> * <paramref name="b"/>].</summary>
    /// <typeparam name="TA">The type of the left operand.</typeparam>
    /// <typeparam name="TB">The type of the right operand.</typeparam>
    /// <typeparam name="TC">The type of the return.</typeparam>
    /// <param name="a">The left operand.</param>
    /// <param name="b">The right operand.</param>
    /// <returns>The result of the multiplication [<paramref name="a"/> * <paramref name="b"/>].</returns>
    public static TC Multiplication<TA, TB, TC>(TA a, TB b) =>
        MultiplicationImplementation<TA, TB, TC>.Function(a, b);

    /// <summary>Multiplies two values [<paramref name="a"/> * <paramref name="b"/>].</summary>
    /// <typeparam name="T">The type of the operation.</typeparam>
    /// <param name="a">The left operand.</param>
    /// <param name="b">The right operand.</param>
    /// <returns>The result of the multiplication [<paramref name="a"/> * <paramref name="b"/>].</returns>
    public static T Multiplication<T>(T a, T b) =>
        Multiplication<T, T, T>(a, b);

    /// <summary>Multiplies multiple values [<paramref name="a"/> * <paramref name="b"/> * <paramref name="c"/> * ...].</summary>
    /// <typeparam name="T">The type of the operation.</typeparam>
    /// <param name="a">The first operand.</param>
    /// <param name="b">The second operand.</param>
    /// <param name="c">The third operand.</param>
    /// <param name="d">The remaining values.</param>
    /// <returns>The result of the multiplication [<paramref name="a"/> * <paramref name="b"/> * <paramref name="c"/> * ...].</returns>
    public static T Multiplication<T>(T a, T b, T c, params T[] d) =>
        Multiplication<T>(step => { step(a); step(b); step(c); d.ToStepper()(step); });

    /// <summary>Multiplies multiple values [step1 * step2 * step3 * ...].</summary>
    /// <typeparam name="T">The type of the operation.</typeparam>
    /// <param name="stepper">The stepper containing the values.</param>
    /// <returns>The result of the multiplication [step1 * step2 * step3 * ...].</returns>
    public static T Multiplication<T>(Action<Action<T>> stepper) =>
        OperationOnStepper(stepper, Multiplication);

    internal static class MultiplicationImplementation<TA, TB, TC>
    {
        internal static Func<TA, TB, TC> Function = (a, b) =>
        {
            var A = Expression.Parameter(typeof(TA));
            var B = Expression.Parameter(typeof(TB));
            var BODY = Expression.Multiply(A, B);
            Function = Expression.Lambda<Func<TA, TB, TC>>(BODY, A, B).Compile();
            return Function(a, b);
        };
    }

    #endregion Multiplication

    #region Division

    /// <summary>Divides two values [<paramref name="a"/> / <paramref name="b"/>].</summary>
    /// <typeparam name="TA">The type of the left operand.</typeparam>
    /// <typeparam name="TB">The type of the right operand.</typeparam>
    /// <typeparam name="TC">The type of the return.</typeparam>
    /// <param name="a">The left operand.</param>
    /// <param name="b">The right operand.</param>
    /// <returns>The result of the division [<paramref name="a"/> / <paramref name="b"/>].</returns>
    public static TC Division<TA, TB, TC>(TA a, TB b) =>
        DivisionImplementation<TA, TB, TC>.Function(a, b);

    /// <summary>Divides two values [<paramref name="a"/> / <paramref name="b"/>].</summary>
    /// <typeparam name="T">The type of the operation.</typeparam>
    /// <param name="a">The left operand.</param>
    /// <param name="b">The right operand.</param>
    /// <returns>The result of the division [<paramref name="a"/> / <paramref name="b"/>].</returns>
    public static T Division<T>(T a, T b) =>
        Division<T, T, T>(a, b);

    /// <summary>Divides multiple values [<paramref name="a"/> / <paramref name="b"/> / <paramref name="c"/> / ...].</summary>
    /// <typeparam name="T">The type of the operation.</typeparam>
    /// <param name="a">The first operand of the division.</param>
    /// <param name="b">The second operand of the division.</param>
    /// <param name="c">The third operand of the division.</param>
    /// <param name="d">The remaining values of the division.</param>
    /// <returns>The result of the division [<paramref name="a"/> / <paramref name="b"/> / <paramref name="c"/> / ...].</returns>
    public static T Division<T>(T a, T b, T c, params T[] d) =>
        Division<T>(step => { step(a); step(b); step(c); d.ToStepper()(step); });

    /// <summary>Divides multiple values [step1 / step2 / step3 / ...].</summary>
    /// <typeparam name="T">The type of the operation.</typeparam>
    /// <param name="stepper">The stepper containing the values.</param>
    /// <returns>The result of the division [step1 / step2 / step3 / ...].</returns>
    public static T Division<T>(Action<Action<T>> stepper) =>
        OperationOnStepper(stepper, Division);

    internal static class DivisionImplementation<TA, TB, TC>
    {
        internal static Func<TA, TB, TC> Function = (a, b) =>
        {
            var A = Expression.Parameter(typeof(TA));
            var B = Expression.Parameter(typeof(TB));
            var BODY = Expression.Divide(A, B);
            Function = Expression.Lambda<Func<TA, TB, TC>>(BODY, A, B).Compile();
            return Function(a, b);
        };
    }

    #endregion Division

    #region Remainder

    /// <summary>Remainders two values [<paramref name="a"/> % <paramref name="b"/>].</summary>
    /// <typeparam name="TA">The type of the left operand.</typeparam>
    /// <typeparam name="TB">The type of the right operand.</typeparam>
    /// <typeparam name="TC">The type of the return.</typeparam>
    /// <param name="a">The left operand.</param>
    /// <param name="b">The right operand.</param>
    /// <returns>The result of the remainder operation [<paramref name="a"/> % <paramref name="b"/>].</returns>
    public static TC Remainder<TA, TB, TC>(TA a, TB b) =>
        RemainderImplementation<TA, TB, TC>.Function(a, b);

    /// <summary>Modulos two numeric values [<paramref name="a"/> % <paramref name="b"/>].</summary>
    /// <typeparam name="T">The numeric type of the operation.</typeparam>
    /// <param name="a">The first operand of the modulation.</param>
    /// <param name="b">The second operand of the modulation.</param>
    /// <returns>The result of the modulation.</returns>
    public static T Remainder<T>(T a, T b) =>
        Remainder<T, T, T>(a, b);

    /// <summary>Modulos multiple numeric values [<paramref name="a"/> % <paramref name="b"/> % <paramref name="c"/> % ...].</summary>
    /// <typeparam name="T">The numeric type of the operation.</typeparam>
    /// <param name="a">The first operand of the modulation.</param>
    /// <param name="b">The second operand of the modulation.</param>
    /// <param name="c">The third operand of the modulation.</param>
    /// <param name="d">The remaining values of the modulation.</param>
    /// <returns>The result of the modulation.</returns>
    public static T Remainder<T>(T a, T b, T c, params T[] d) =>
        Remainder<T>(step => { step(a); step(b); step(c); d.ToStepper()(step); });

    /// <summary>Modulos multiple numeric values [step_1 % step_2 % step_3...].</summary>
    /// <typeparam name="T">The numeric type of the operation.</typeparam>
    /// <param name="stepper">The stepper containing the values.</param>
    /// <returns>The result of the modulation.</returns>
    public static T Remainder<T>(Action<Action<T>> stepper) =>
        OperationOnStepper(stepper, Remainder);

    internal static class RemainderImplementation<TA, TB, TC>
    {
        internal static Func<TA, TB, TC> Function = (a, b) =>
        {
            var A = Expression.Parameter(typeof(TA));
            var B = Expression.Parameter(typeof(TB));
            var BODY = Expression.Modulo(A, B);
            Function = Expression.Lambda<Func<TA, TB, TC>>(BODY, A, B).Compile();
            return Function(a, b);
        };
    }

    #endregion Remainder

    #region Inversion

    /// <summary>Inverts a numeric value [1 / a].</summary>
    /// <typeparam name="T">The numeric type of the operation.</typeparam>
    /// <param name="a">The numeric value to invert.</param>
    /// <returns>The result of the inversion.</returns>
    public static T Inversion<T>(T a) =>
        Division(Constant<T>.One, a);

    #endregion Inversion

    #region Power

    /// <summary>Powers two numeric values [a ^ b].</summary>
    /// <typeparam name="T">The numeric type of the operation.</typeparam>
    /// <param name="a">The first operand of the power.</param>
    /// <param name="b">The second operand of the power.</param>
    /// <returns>The result of the power.</returns>
    public static T Power<T>(T a, T b) =>
        PowerImplementation<T>.Function(a, b);

    /// <summary>Powers multiple numeric values [a ^ b ^ c...].</summary>
    /// <typeparam name="T">The numeric type of the operation.</typeparam>
    /// <param name="a">The first operand of the power.</param>
    /// <param name="b">The second operand of the power.</param>
    /// <param name="c">The third operand of the power.</param>
    /// <param name="d">The remaining values of the power.</param>
    /// <returns>The result of the power.</returns>
    public static T Power<T>(T a, T b, T c, params T[] d) =>
        Power<T>(step => { step(a); step(b); step(c); d.ToStepper()(step); });

    /// <summary>Powers multiple numeric values [step_1 ^ step_2 ^ step_3...].</summary>
    /// <typeparam name="T">The numeric type of the operation.</typeparam>
    /// <param name="stepper">The stepper containing the values.</param>
    /// <returns>The result of the power.</returns>
    public static T Power<T>(Action<Action<T>> stepper) =>
        OperationOnStepper(stepper, Power);

    internal static class PowerImplementation<T>
    {
        internal static Func<T, T, T> Function = (a, b) =>
        {
            // Note: this code needs to die.. but this works until it gets a better version

            // optimization for specific known types
            if (TypeDescriptor.GetConverter(typeof(T)).CanConvertTo(typeof(double)))
            {
                var A = Expression.Parameter(typeof(T));
                var B = Expression.Parameter(typeof(T));
                var Math_Pow = typeof(Math).GetMethod(nameof(Math.Pow));
                if (Math_Pow is not null)
                {
                    Expression BODY = Expression.Convert(Expression.Call(Math_Pow, Expression.Convert(A, typeof(double)), Expression.Convert(B, typeof(double))), typeof(T));
                    Function = Expression.Lambda<Func<T, T, T>>(BODY, A, B).Compile();
                    return Function(a, b);
                }
            }

            Function = (A, B) =>
            {
                if (IsInteger(B) && IsPositive(B) && LessThan(B, Convert<int, T>(int.MaxValue)))
                {
                    T result = A;
                    int power = Convert<T, int>(B);
                    for (int i = 0; i < power; i++)
                    {
                        result = Multiplication(result, A);
                    }
                    return result;
                }
                else
                {
#warning TODO
                    throw new NotImplementedException("This feature is still in development.");
                }
            };

            return Function(a, b);
        };
    }

    #endregion Power

    #region SquareRoot

    /// <summary>Square roots a numeric value [√a].</summary>
    /// <typeparam name="T">The numeric type of the operation.</typeparam>
    /// <param name="a">The numeric value to square root.</param>
    /// <returns>The result of the square root.</returns>
    public static T SquareRoot<T>(T a) =>
        SquareRootImplementation<T>.Function(a);

    internal static class SquareRootImplementation<T>
    {
        internal static Func<T, T> Function = a =>
        {
            #region Optimization(int)

            if (typeof(T) == typeof(int))
            {
                static int SquareRoot(int x)
                {
                    if (x is 0 || x is 1)
                    {
                        return x;
                    }
                    int start = 1, end = x, ans = 0;
                    while (start <= end)
                    {
                        int mid = (start + end) / 2;
                        if (mid * mid == x)
                        {
                            return mid;
                        }
                        if (mid * mid < x)
                        {
                            start = mid + 1;
                            ans = mid;
                        }
                        else
                        {
                            end = mid - 1;
                        }
                    }
                    return ans;
                }
                SquareRootImplementation<int>.Function = SquareRoot;
                return Function!(a);
            }

            #endregion Optimization(int)

            return Root(a, Constant<T>.Two);
        };
    }

    #endregion SquareRoot

    #region Root

    /// <summary>Roots two numeric values [a ^ (1 / b)].</summary>
    /// <typeparam name="T">The numeric type of the operation.</typeparam>
    /// <param name="a">The base of the root.</param>
    /// <param name="b">The root of the operation.</param>
    /// <returns>The result of the root.</returns>
    public static T Root<T>(T a, T b) =>
        Power(a, Inversion(b));

    #endregion Root

    #region Logarithm

    /// <summary>Computes the logarithm of a value.</summary>
    /// <typeparam name="T">The numeric type of the operation.</typeparam>
    /// <param name="value">The value to compute the logarithm of.</param>
    /// <param name="base">The base of the logarithm to compute.</param>
    /// <returns>The computed logarithm value.</returns>
    public static T Logarithm<T>(T value, T @base) =>
        LogarithmImplementation<T>.Function(value, @base);

    internal static class LogarithmImplementation<T>
    {
        internal static Func<T, T, T> Function = (a, b) =>
        {
#warning TODO
            throw new NotImplementedException();

            // ParameterExpression A = Expression.Parameter(typeof(T));
            // ParameterExpression B = Expression.Parameter(typeof(T));
            // Expression BODY = ;
            // Function = Expression.Lambda<Func<T, T, T>>(BODY, A, B).Compile();
            // return Function(a, b);
        };
    }

    #endregion Logarithm

    #region IsInteger

    /// <summary>Determines if a numerical value is an integer.</summary>
    /// <typeparam name="T">The numeric type of the operation.</typeparam>
    /// <param name="a">The value to determine integer status of.</param>
    /// <returns>Whether or not the value is an integer.</returns>
    public static bool IsInteger<T>(T a) =>
        IsIntegerImplementation<T>.Function(a);

    internal static class IsIntegerImplementation<T>
    {
        internal static Func<T, bool> Function = a =>
        {
            var methodInfo = Meta.GetIsIntegerMethod<T>();
            if (methodInfo is not null)
            {
                Function = methodInfo.CreateDelegate<Func<T, bool>>();
                return Function(a);
            }

            var A = Expression.Parameter(typeof(T));
            var BODY = Expression.Equal(
                Expression.Modulo(A, Expression.Constant(Constant<T>.One)),
                Expression.Constant(Constant<T>.Zero));
            Function = Expression.Lambda<Func<T, bool>>(BODY, A).Compile();
            return Function(a);
        };
    }

    #endregion IsInteger

    #region IsNonNegative

    /// <summary>Determines if a numerical value is non-negative.</summary>
    /// <typeparam name="T">The numeric type of the operation.</typeparam>
    /// <param name="a">The value to determine non-negative status of.</param>
    /// <returns>Whether or not the value is non-negative.</returns>
    public static bool IsNonNegative<T>(T a) =>
        IsNonNegativeImplementation<T>.Function(a);

    internal static class IsNonNegativeImplementation<T>
    {
        internal static Func<T, bool> Function = a =>
        {
            var methodInfo = Meta.GetIsNonNegativeMethod<T>();
            if (methodInfo is not null)
            {
                Function = methodInfo.CreateDelegate<Func<T, bool>>();
                return Function(a);
            }

            var A = Expression.Parameter(typeof(T));
            var BODY = Expression.GreaterThanOrEqual(A, Expression.Constant(Constant<T>.Zero));
            Function = Expression.Lambda<Func<T, bool>>(BODY, A).Compile();
            return Function(a);
        };
    }

    #endregion IsNonNegative

    #region IsNegative

    /// <summary>Determines if a numerical value is negative.</summary>
    /// <typeparam name="T">The numeric type of the operation.</typeparam>
    /// <param name="a">The value to determine negative status of.</param>
    /// <returns>Whether or not the value is negative.</returns>
    public static bool IsNegative<T>(T a) =>
        IsNegativeImplementation<T>.Function(a);

    internal static class IsNegativeImplementation<T>
    {
        internal static Func<T, bool> Function = a =>
        {
            MethodInfo? methodInfo = Meta.GetIsNegativeMethod<T>();
            if (methodInfo is not null)
            {
                Function = methodInfo.CreateDelegate<Func<T, bool>>();
                return Function(a);
            }

            ParameterExpression A = Expression.Parameter(typeof(T));
            LabelTarget RETURN = Expression.Label(typeof(bool));
            Expression BODY = Expression.LessThan(A, Expression.Constant(Constant<T>.Zero));
            Function = Expression.Lambda<Func<T, bool>>(BODY, A).Compile();
            return Function(a);
        };
    }

    #endregion IsNegative

    #region IsPositive

    /// <summary>Determines if a numerical value is positive.</summary>
    /// <typeparam name="T">The numeric type of the operation.</typeparam>
    /// <param name="a">The value to determine positive status of.</param>
    /// <returns>Whether or not the value is positive.</returns>
    public static bool IsPositive<T>(T a) =>
        IsPositiveImplementation<T>.Function(a);

    internal static class IsPositiveImplementation<T>
    {
        internal static Func<T, bool> Function = a =>
        {
            var methodInfo = Meta.GetIsPositiveMethod<T>();
            if (methodInfo is not null)
            {
                Function = methodInfo.CreateDelegate<Func<T, bool>>();
                return Function(a);
            }

            var A = Expression.Parameter(typeof(T));
            var BODY = Expression.GreaterThan(A, Expression.Constant(Constant<T>.Zero));
            Function = Expression.Lambda<Func<T, bool>>(BODY, A).Compile();
            return Function(a);
        };
    }

    #endregion IsPositive

    #region IsEven

    /// <summary>Determines if a numerical value is even.</summary>
    /// <typeparam name="T">The numeric type of the operation.</typeparam>
    /// <param name="a">The value to determine even status of.</param>
    /// <returns>Whether or not the value is even.</returns>
    public static bool IsEven<T>(T a) =>
        IsEvenImplementation<T>.Function(a);

    internal static class IsEvenImplementation<T>
    {
        internal static Func<T, bool> Function = a =>
        {
            MethodInfo? methodInfo = Meta.GetIsEvenMethod<T>();
            if (methodInfo is not null)
            {
                Function = methodInfo.CreateDelegate<Func<T, bool>>();
                return Function(a);
            }

            var A = Expression.Parameter(typeof(T));
            var BODY = Expression.Equal(Expression.Modulo(A, Expression.Constant(Constant<T>.Two)), Expression.Constant(Constant<T>.Zero));
            Function = Expression.Lambda<Func<T, bool>>(BODY, A).Compile();
            return Function(a);
        };
    }

    #endregion IsEven

    #region IsOdd

    /// <summary>Determines if a numerical value is odd.</summary>
    /// <typeparam name="T">The numeric type of the operation.</typeparam>
    /// <param name="a">The value to determine odd status of.</param>
    /// <returns>Whether or not the value is odd.</returns>
    public static bool IsOdd<T>(T a) =>
        IsOddImplementation<T>.Function(a);

    internal static class IsOddImplementation<T>
    {
        internal static Func<T, bool> Function = a =>
        {
            var methodInfo = Meta.GetIsOddMethod<T>();
            if (methodInfo is not null)
            {
                Function = methodInfo.CreateDelegate<Func<T, bool>>();
                return Function(a);
            }

            var A = Expression.Parameter(typeof(T));
            Expression BODY =
                Expression.Block(
                    Expression.IfThen(
                        Expression.LessThan(A, Expression.Constant(Constant<T>.Zero)),
                        Expression.Assign(A, Expression.Negate(A))),
                    Expression.Equal(Expression.Modulo(A, Expression.Constant(Constant<T>.Two)), Expression.Constant(Constant<T>.One)));
            Function = Expression.Lambda<Func<T, bool>>(BODY, A).Compile();
            return Function(a);
        };
    }

    #endregion IsOdd

    #region IsPrime

    /// <summary>Determines if a numerical value is prime.</summary>
    /// <typeparam name="T">The numeric type of the operation.</typeparam>
    /// <param name="a">The value to determine prime status of.</param>
    /// <returns>Whether or not the value is prime.</returns>
    public static bool IsPrime<T>(T a) =>
        IsPrimeImplementation<T>.Function(a);

    internal static class IsPrimeImplementation<T>
    {
        internal static Func<T, bool> Function = a =>
        {
            var methodInfo = Meta.GetIsPrimeMethod<T>();
            if (methodInfo is not null)
            {
                Function = methodInfo.CreateDelegate<Func<T, bool>>();
                return Function(a);
            }

            // This can be optimized
            Function = A =>
            {
                if (IsInteger(A) && !LessThan(A, Constant<T>.Two))
                {
                    if (Equate(A, Constant<T>.Two))
                    {
                        return true;
                    }
                    if (IsEven(A))
                    {
                        return false;
                    }
                    T squareRoot = SquareRoot(A);
                    for (T divisor = Constant<T>.Three; LessThanOrEqual(divisor, squareRoot); divisor = Addition(divisor, Constant<T>.Two))
                    {
                        if (Equate(Remainder<T>(A, divisor), Constant<T>.Zero))
                        {
                            return false;
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            };
            return Function(a);
        };
    }

    #endregion IsPrime

    #region AbsoluteValue

    /// <summary>Gets the absolute value of a value.</summary>
    /// <typeparam name="T">The numeric type of the operation.</typeparam>
    /// <param name="a">The value to get the absolute value of.</param>
    /// <returns>The absolute value of the provided value.</returns>
    public static T AbsoluteValue<T>(T a) =>
        AbsoluteValueImplementation<T>.Function(a);

    internal static class AbsoluteValueImplementation<T>
    {
        internal static Func<T, T> Function = a =>
        {
            ParameterExpression A = Expression.Parameter(typeof(T));
            LabelTarget RETURN = Expression.Label(typeof(T));
            Expression BODY = Expression.Block(
                Expression.IfThenElse(
                    Expression.LessThan(A, Expression.Constant(Constant<T>.Zero)),
                    Expression.Return(RETURN, Expression.Negate(A)),
                    Expression.Return(RETURN, A)),
                Expression.Label(RETURN, Expression.Constant(default(T), typeof(T))));
            Function = Expression.Lambda<Func<T, T>>(BODY, A).Compile();
            return Function(a);
        };
    }

    #endregion AbsoluteValue

    #region Clamp

    /// <summary>Gets a value restricted to a minimum and maximum range.</summary>
    /// <typeparam name="T">The numeric type of the operation.</typeparam>
    /// <param name="value">The value to clamp.</param>
    /// <param name="minimum">The minimum of the range to clamp the value by.</param>
    /// <param name="maximum">The maximum of the range to clamp the value by.</param>
    /// <returns>The value restricted to the provided range.</returns>
    public static T Clamp<T>(T value, T minimum, T maximum) =>
        LessThan(value, minimum)
        ? minimum
        : GreaterThan(value, maximum)
            ? maximum
            : value;

    #endregion Clamp

    #region EqualToLeniency

    /// <summary>Checks for equality between two numeric values with a range of possibly leniency.</summary>
    /// <typeparam name="T">The numeric type of the operation.</typeparam>
    /// <param name="a">The first operand of the equality check.</param>
    /// <param name="b">The second operand of the equality check.</param>
    /// <param name="leniency">The allowed distance between the values to still be considered equal.</param>
    /// <returns>True if the values are within the allowed leniency of each other. False if not.</returns>
    public static bool EqualToLeniency<T>(T a, T b, T leniency) =>
#warning TODO: add an ArgumentOutOfBounds check on leniency
        EqualToLeniencyImplementation<T>.Function(a, b, leniency);

    internal static class EqualToLeniencyImplementation<T>
    {
        internal static Func<T, T, T, bool> Function = (T a, T b, T c) =>
        {
            var A = Expression.Parameter(typeof(T));
            var B = Expression.Parameter(typeof(T));
            var C = Expression.Parameter(typeof(T));
            var D = Expression.Variable(typeof(T));
            var RETURN = Expression.Label(typeof(bool));
            var BODY = Expression.Block(Ɐ(D),
                Expression.Assign(D, Expression.Subtract(A, B)),
                Expression.IfThenElse(
                    Expression.LessThan(D, Expression.Constant(Constant<T>.Zero)),
                    Expression.Assign(D, Expression.Negate(D)),
                    Expression.Assign(D, D)),
                Expression.Return(RETURN, Expression.LessThanOrEqual(D, C), typeof(bool)),
                Expression.Label(RETURN, Expression.Constant(default(bool))));
            Function = Expression.Lambda<Func<T, T, T, bool>>(BODY, A, B, C).Compile();
            return Function(a, b, c);
        };
    }

    #endregion EqualToLeniency

    #region GreatestCommonFactor

    /// <summary>Computes the greatest common factor of a set of numbers.</summary>
    /// <typeparam name="T">The numeric type of the computation.</typeparam>
    /// <param name="a">The first operand of the greatest common factor computation.</param>
    /// <param name="b">The second operand of the greatest common factor computation.</param>
    /// <param name="c">The remaining operands of the greatest common factor computation.</param>
    /// <returns>The computed greatest common factor of the set of numbers.</returns>
    public static T GreatestCommonFactor<T>(T a, T b, params T[] c) =>
        GreatestCommonFactor<T>(step => { step(a); step(b); c.ToStepper()(step); });

    /// <summary>Computes the greatest common factor of a set of numbers.</summary>
    /// <typeparam name="T">The numeric type of the computation.</typeparam>
    /// <param name="stepper">The set of numbers to compute the greatest common factor of.</param>
    /// <returns>The computed greatest common factor of the set of numbers.</returns>
    public static T GreatestCommonFactor<T>(Action<Action<T>> stepper)
    {
        if (stepper is null) throw new ArgumentNullException(nameof(stepper));
        bool assigned = false;
        T answer = Constant<T>.Zero;
        stepper((T n) =>
        {
            if (n is null)
            {
                throw new ArgumentNullException(null, nameof(stepper) + " contians null values");
            }
            if (Equate(n, Constant<T>.Zero))
            {
                throw new ArgumentException("Encountered Zero (0) while computing the " + nameof(GreatestCommonFactor));
            }
            else if (!IsInteger(n))
            {
                throw new ArgumentException(nameof(stepper) + " contains non-integer value(s).");
            }
            if (!assigned)
            {
                answer = AbsoluteValue(n);
                assigned = true;
            }
            else
            {
                if (GreaterThan(answer, Constant<T>.One))
                {
                    T a = answer;
                    T b = n;
                    while (Inequate(b, Constant<T>.Zero))
                    {
                        T remainder = Remainder(a, b);
                        a = b;
                        b = remainder;
                    }
                    answer = AbsoluteValue(a);
                }
            }
        });
        if (!assigned)
        {
            throw new ArgumentException(nameof(stepper) + " is empty.", nameof(stepper));
        }
        return answer;
    }

    #endregion GreatestCommonFactor

    #region LeastCommonMultiple

    /// <summary>Computes the least common multiple of a set of numbers.</summary>
    /// <typeparam name="T">The numeric type of the operation.</typeparam>
    /// <param name="a">The first operand of the least common muiltiple computation.</param>
    /// <param name="b">The second operand of the least common muiltiple computation.</param>
    /// <param name="c">The remaining operands of the least common muiltiple computation.</param>
    /// <returns>The computed least common least common multiple of the set of numbers.</returns>
    public static T LeastCommonMultiple<T>(T a, T b, params T[] c) =>
        LeastCommonMultiple<T>(step => { step(a); step(b); c.ToStepper()(step); });

    /// <summary>Computes the least common multiple of a set of numbers.</summary>
    /// <typeparam name="T">The numeric type of the operation.</typeparam>
    /// <param name="stepper">The set of numbers to compute the least common multiple of.</param>
    /// <returns>The computed least common least common multiple of the set of numbers.</returns>
    public static T LeastCommonMultiple<T>(Action<Action<T>> stepper)
    {
        if (stepper is null) throw new ArgumentNullException(nameof(stepper));
        bool assigned = false;
        T? answer = default;
        stepper(parameter =>
        {
            if (Equate(parameter, Constant<T>.Zero))
            {
                throw new ArgumentException(nameof(stepper) + " contains 0 value(s).");
            }
            if (!IsInteger(parameter))
            {
                throw new ArgumentException(nameof(stepper) + " contains non-integer value(s).");
            }
            parameter = AbsoluteValue(parameter);
            if (!assigned)
            {
                answer = parameter;
                assigned = true;
            }
            else
            {
                answer = Division(Multiplication(answer, parameter), GreatestCommonFactor(answer, parameter));
            }
        });
        if (!assigned)
        {
            throw new ArgumentException(nameof(stepper) + " is empty.", nameof(stepper));
        }
        return answer!;
    }

    #endregion LeastCommonMultiple

    #region LinearInterpolation

    /// <summary>Linearly interpolations a value.</summary>
    /// <typeparam name="T">The numeric type of the operation.</typeparam>
    /// <param name="x">The value along the first dimension to compute the linear interpolation for.</param>
    /// <param name="x0">A known starting point along the first dimension.</param>
    /// <param name="x1">A known ending point along the first dimension.</param>
    /// <param name="y0">A known starting point along the second dimension.</param>
    /// <param name="y1">A known ending point along the second dimension.</param>
    /// <returns>The linearly interpolated value.</returns>
    public static T LinearInterpolation<T>(T x, T x0, T x1, T y0, T y1)
    {
        if (GreaterThan(x0, x1) ||
            GreaterThan(x, x1) ||
            LessThan(x, x0))
        {
            throw new ArgumentException($"!({nameof(x0)}[{x0}] <= {nameof(x)}[{x}] <= {nameof(x1)}[{x1}])");
        }
        if (Equate(x0, x1))
        {
            if (Inequate(y0, y1))
            {
                throw new ArgumentException($"{nameof(x0)}[{x0}] == {nameof(x1)}[{x1}] && {nameof(y0)}[{y0}] != {nameof(y1)}[{y1}]");
            }
            else
            {
                return y0;
            }
        }
        return Addition(y0, Division(Multiplication(Subtraction(x, x0), Subtraction(y1, y0)), Subtraction(x1, x0)));
    }

    #endregion LinearInterpolation

    #region Factorial

    /// <summary>Computes the factorial of a numeric value [<paramref name="a"/>!] == [<paramref name="a"/> * (<paramref name="a"/> - 1) * (<paramref name="a"/> - 2) * ... * 1].</summary>
    /// <typeparam name="T">The numeric type of the operation.</typeparam>
    /// <param name="a">The integer value to compute the factorial of.</param>
    /// <returns>The computed factorial value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the parameter is not an integer value.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the parameter is less than zero.</exception>
    public static T Factorial<T>(T a) =>
        FactorialImplementation<T>.Function(a);

    internal static class FactorialImplementation<T>
    {
        internal static Func<T, T> Function = a =>
        {
            var methodInfo = Meta.GetFactorialMethod<T>();
            if (methodInfo is not null)
            {
                Function = methodInfo.CreateDelegate<Func<T, T>>();
                return Function(a);
            }

            Function = A =>
            {
                if (!IsInteger(A))
                {
                    throw new ArgumentOutOfRangeException(nameof(A), A, $"!{nameof(A)}[{A}].{nameof(IsInteger)}()");
                }
                if (LessThan(A, Constant<T>.Zero))
                {
                    throw new ArgumentOutOfRangeException(nameof(A), A, $"!({nameof(A)}[{A}] >= 0)");
                }
                T result = Constant<T>.One;
                for (; GreaterThan(A, Constant<T>.One); A = Subtraction(A, Constant<T>.One))
                    result = Multiplication(A, result);
                return result;
            };
            return Function(a);
        };
    }

    #endregion Factorial

    #region Combinations

    /// <summary>Computes the combinations of <paramref name="N"/> values using the <paramref name="n"/> grouping definitions.</summary>
    /// <typeparam name="T">The numeric type of the operation.</typeparam>
    /// <param name="N">The number of values to compute the combinations of.</param>
    /// <param name="n">The groups and how many values fall into each group.</param>
    /// <returns>The computed number of combinations.</returns>
    public static T CombinationsCount<T>(T N, T[] n)
    {
        if (!IsInteger(N))
        {
            throw new ArgumentOutOfRangeException(nameof(N), N, $"!({nameof(N)}.{nameof(IsInteger)})");
        }
        T result = Factorial(N);
        T sum = Constant<T>.Zero;
        for (int i = 0; i < n.Length; i++)
        {
            if (!IsInteger(n[i]))
            {
                throw new ArgumentOutOfRangeException(nameof(n) + "[" + i + "]", n[i], "!(" + nameof(n) + "[" + i + "]." + nameof(IsInteger) + ")");
            }
            result = Division(result, Factorial(n[i]));
            sum = Addition(sum, n[i]);
        }
        if (GreaterThan(sum, N))
        {
            throw new ArgumentException("Aurguments out of range !(" + nameof(N) + " < Add(" + nameof(n) + ") [" + N + " < " + sum + "].");
        }
        return result;
    }

    #endregion Combinations

    #region BinomialCoefficient

    /// <summary>Computes the Binomial coefficient (N choose n).</summary>
    /// <typeparam name="T">The numeric type of the computation.</typeparam>
    /// <param name="N">The size of the entire set (N choose n).</param>
    /// <param name="n">The size of the subset (N choose n).</param>
    /// <returns>The computed binomial coefficient (N choose n).</returns>
    public static T BinomialCoefficient<T>(T N, T n)
    {
        if (LessThan(N, Constant<T>.Zero))
        {
            throw new ArgumentOutOfRangeException(nameof(N), N, "!(" + nameof(N) + " >= 0)");
        }
        if (!IsInteger(N))
        {
            throw new ArgumentOutOfRangeException(nameof(N), N, "!(" + nameof(N) + "." + nameof(IsInteger) + ")");
        }
        if (!IsInteger(n))
        {
            throw new ArgumentOutOfRangeException(nameof(n), n, "!(" + nameof(n) + "." + nameof(IsInteger) + ")");
        }
        if (LessThan(N, n))
        {
            throw new ArgumentException("Arguments out of range !(" + nameof(N) + " <= " + nameof(n) + ") [" + N + " <= " + n + "].");
        }
        return Division(Factorial(N), Multiplication(Factorial(n), Factorial(Subtraction(N, n))));
    }

    #endregion BinomialCoefficient

    #region Exponential

    /// <summary>Computes the exponentional of a value [e ^ <paramref name="a"/>].</summary>
    /// <typeparam name="T">The generic type of the operation.</typeparam>
    /// <param name="a">The value to compute the exponentional of.</param>
    /// <returns>The exponential of the value [e ^ <paramref name="a"/>].</returns>
    public static T Exponential<T>(T a)
    {
#warning TODO
        throw new NotImplementedException();
    }

    #endregion Exponential

    #region NaturalLogarithm

    /// <summary>Computes the natural logarithm of a value [ln(<paramref name="a"/>)].</summary>
    /// <typeparam name="T">The generic type of the operation.</typeparam>
    /// <param name="a">The value to compute the natural log of.</param>
    /// <returns>The natural log of the provided value [ln(<paramref name="a"/>)].</returns>
    public static T NaturalLogarithm<T>(T a) =>
        NaturalLogarithmImplementation<T>.Function(a);

    internal static class NaturalLogarithmImplementation<T>
    {
        internal static Func<T, T> Function = a =>
        {
            // optimization for specific known types
            if (TypeDescriptor.GetConverter(typeof(T)).CanConvertTo(typeof(double)))
            {
                ParameterExpression A = Expression.Parameter(typeof(T));
                MethodInfo? Math_Log = typeof(Math).GetMethod("Log");
                if (Math_Log is not null)
                {
                    Expression BODY = Expression.Call(Math_Log, A);
                    Function = Expression.Lambda<Func<T, T>>(BODY, A).Compile();
                    return Function(a);
                }
            }
#warning TODO
            throw new NotImplementedException();
        };
    }

    #endregion NaturalLogarithm

    #region LinearRegression2D

    /// <summary>Computes the best fit line from a set of points in 2D space [y = slope * x + y_intercept].</summary>
    /// <typeparam name="T">The numeric type of the operation.</typeparam>
    /// <param name="points">The points to compute the best fit line of.</param>
    /// <param name="slope">The slope of the computed best fit line [y = slope * x + y_intercept].</param>
    /// <param name="y_intercept">The y intercept of the computed best fit line [y = slope * x + y_intercept].</param>
    public static void LinearRegression2D<T>(Action<Action<T, T>> points, out T slope, out T y_intercept)
    {
        if (points is null) throw new ArgumentNullException(nameof(points));
        int count = 0;
        T sumx = Constant<T>.Zero;
        T sumy = Constant<T>.Zero;
        points((T x, T y) =>
        {
            sumx = Addition(sumx, x);
            sumy = Addition(sumy, y);
            count++;
        });
        if (count < 2)
        {
            throw new ArgumentException("At least 3 points must be provided for linear regressions");
        }
        T tcount = Convert<int, T>(count);
        T meanx = Division(sumx, tcount);
        T meany = Division(sumy, tcount);
        T variancex = Constant<T>.Zero;
        T variancey = Constant<T>.Zero;
        points((T x, T y) =>
        {
            T offset = Subtraction(x, meanx);
            variancey = Addition(variancey, Multiplication(offset, Subtraction(y, meany)));
            variancex = Addition(variancex, Multiplication(offset, offset));
        });
        slope = Division(variancey, variancex);
        y_intercept = Subtraction(meany, Multiplication(slope, meanx));
    }

    #endregion LinearRegression2D

    #region FactorPrimes

    /// <summary>Factors the primes numbers of a numeric integer value.</summary>
    /// <typeparam name="T">The numeric type of the operation.</typeparam>
    /// <param name="a">The value to factor the prime numbers of.</param>
    /// <param name="step">The action to perform on all found prime factors.</param>
    public static void FactorPrimes<T>(T a, Action<T> step) =>
        FactorPrimesImplementation<T>.Function(a, step);

    internal static class FactorPrimesImplementation<T>
    {
        internal static Action<T, Action<T>> Function = (a, x) =>
        {
            Function = (A, step) =>
            {
                if (!IsInteger(A))
                {
                    throw new ArgumentOutOfRangeException(nameof(A), A, "!(" + nameof(A) + "." + nameof(IsInteger) + ")");
                }
                if (IsNegative(A))
                {
                    A = AbsoluteValue(A);
                    step(Convert<int, T>(-1));
                }
                while (IsEven(A))
                {
                    step(Constant<T>.Two);
                    A = Division(A, Constant<T>.Two);
                }
                for (T i = Constant<T>.Three; LessThanOrEqual(i, SquareRoot(A)); i = Addition(i, Constant<T>.Two))
                {
                    while (Equate(Remainder(A, i), Constant<T>.Zero))
                    {
                        step(i);
                        A = Division(A, i);
                    }
                }
                if (GreaterThan(A, Constant<T>.Two))
                {
                    step(A);
                }
            };
            Function(a, x);
        };
    }

    #endregion FactorPrimes
}