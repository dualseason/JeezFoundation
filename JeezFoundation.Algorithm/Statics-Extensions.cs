﻿using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace JeezFoundation.Algorithm;

/// <summary>Root type of the static functional methods in JeezFoundation.Algorithm.</summary>
public static partial class Statics
{
    #region System.String

    #region Replace (multiple replace)

    /// <summary>Returns a new <see cref="string"/> in which all occurrences of Unicode <see cref="string"/> patterns in this instance are replaced with a relative Unicode <see cref="string"/> replacements.</summary>
    /// <remarks>Uses Regex without a timeout.</remarks>
    /// <param name="original">The <see cref="string"/> to perform the replacements on.</param>
    /// <param name="rules">The patterns and relative replacements to apply to this <see cref="string"/>.</param>
    /// <returns>A new <see cref="string"/> in which all occurrences of Unicode <see cref="string"/> patterns in this instance are replaced with a relative Unicode <see cref="string"/> replacements.</returns>
    /// <exception cref="ArgumentNullException">Thrown if any of the parameters are null or contain null values.</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="rules"/> is empty, <paramref name="rules"/> contains empty patterns, or <paramref name="rules"/> contains duplicate patterns.</exception>
    public static string Replace(this string original, params (string A, string B)[] rules)
    {
        if (original is null) throw new ArgumentNullException(nameof(original));
        if (rules is null) throw new ArgumentNullException(nameof(rules));
        if (rules.Length is 0) throw new ArgumentException(paramName: nameof(rules), message: $"{nameof(rules)}.{nameof(rules.Length)} is 0");
        MapHashLinked<string, string, StringEquate, StringHash> map = new(expectedCount: rules.Length);
        foreach (var (a, b) in rules)
        {
            if (a is null) throw new ArgumentNullException(paramName: nameof(rules), message: $"{nameof(rules)} contains null {nameof(a)}s");
            if (b is null) throw new ArgumentNullException(paramName: nameof(rules), message: $"{nameof(rules)} contains null {nameof(b)}s");
            if (a.Length is 0) throw new ArgumentException(paramName: nameof(rules), message: $"{nameof(rules)} contains 0 length {nameof(a)}s");
            var (success, exception) = map.TryAdd(a, b);
            if (!success)
            {
                throw new ArgumentException($"{nameof(rules)} contains duplicate {nameof(a)}s", exception);
            }
        }
        string regexPattern = string.Join("|", map.GetKeys());
        return Regex.Replace(original, regexPattern, match => map[match.Value], RegexOptions.None, Regex.InfiniteMatchTimeout);
    }

    #endregion Replace (multiple replace)

    /// <summary>Checks if a string contains any of a collections on characters.</summary>
    /// <param name="string">The string to see if it contains any of the specified characters.</param>
    /// <param name="chars">The characters to check if the string contains any of them.</param>
    /// <returns>True if the string contains any of the provided characters. False if not.</returns>
    public static bool ContainsAny(this string @string, params char[] chars)
    {
        if (chars is null) throw new ArgumentNullException(nameof(chars));
        if (@string is null) throw new ArgumentNullException(nameof(@string));
        if (chars.Length < 1)
        {
            throw new InvalidOperationException("Attempting a contains check with an empty set.");
        }

        SetHashLinked<char, CharEquate, CharHash> set = new();
        foreach (char c in chars)
        {
            set.Add(c);
        }
        foreach (char c in @string)
        {
            if (set.Contains(c))
            {
                return true;
            }
        }
        return false;
    }

    internal static string RemoveCarriageReturns(this string @string) =>
        @string.Replace("\r", string.Empty);

    /// <summary>Removes carriage returns and then replaces all new line characters with System.Environment.NewLine.</summary>
    /// <param name="string">The string to standardize the new lines of.</param>
    /// <returns>The new line standardized string.</returns>
    internal static string StandardizeNewLines(this string @string) =>
        @string.RemoveCarriageReturns().Replace("\n", Environment.NewLine);

    /// <summary>Creates a string of a repreated string a provided number of times.</summary>
    /// <param name="string">The string to repeat.</param>
    /// <param name="count">The number of repetitions of the string to repeat.</param>
    /// <returns>The string of the repeated string to repeat.</returns>
    public static string Repeat(this string @string, int count)
    {
        if (@string is null) throw new ArgumentNullException(nameof(@string));
        if (sourceof(count < 0, out string c1)) throw new ArgumentOutOfRangeException(nameof(count), count, c1);
        string[] sets = new string[count];
        for (int i = 0; i < count; i++)
        {
            sets[i] = @string;
        }
        return string.Concat(sets);
    }

    /// <summary>Splits the string into the individual lines.</summary>
    /// <param name="string">The string to get the lines of.</param>
    /// <returns>an array of the individual lines of the string.</returns>
    public static string[] SplitLines(this string @string)
    {
        if (@string is null) throw new ArgumentNullException(nameof(@string));
        return @string.RemoveCarriageReturns().Split('\n');
    }

    /// <summary>Indents every line in a string with a single tab character.</summary>
    /// <param name="string">The string to indent the lines of.</param>
    /// <returns>The indented string.</returns>
    public static string IndentLines(this string @string) =>
        PadLinesLeft(@string, "\t");

    /// <summary>Indents every line in a string with a given number of tab characters.</summary>
    /// <param name="string">The string to indent the lines of.</param>
    /// <param name="count">The number of tabs of the indention.</param>
    /// <returns>The indented string.</returns>
    public static string IndentLines(this string @string, int count) =>
        PadLinesLeft(@string, new string('\t', count));

    /// <summary>Indents after every new line sequence found between two string indeces.</summary>
    /// <param name="string">The string to be indented.</param>
    /// <param name="start">The starting index to look for new line sequences to indent.</param>
    /// <param name="end">The ending index to look for new line sequences to indent.</param>
    /// <returns>The indented string.</returns>
    public static string IndentNewLinesBetweenIndeces(this string @string, int start, int end) =>
        PadLinesLeftBetweenIndeces(@string, "\t", start, end);

    /// <summary>Indents after every new line sequence found between two string indeces.</summary>
    /// <param name="string">The string to be indented.</param>
    /// <param name="count">The number of tabs of this indention.</param>
    /// <param name="start">The starting index to look for new line sequences to indent.</param>
    /// <param name="end">The ending index to look for new line sequences to indent.</param>
    /// <returns>The indented string.</returns>
    public static string IndentNewLinesBetweenIndeces(this string @string, int count, int start, int end) =>
        PadLinesLeftBetweenIndeces(@string, new string('\t', count), start, end);

    /// <summary>Indents a range of line numbers in a string.</summary>
    /// <param name="string">The string to indent specified lines of.</param>
    /// <param name="startingLineNumber">The line number to start line indention on.</param>
    /// <param name="endingLineNumber">The line number to stop line indention on.</param>
    /// <returns>The string with the specified lines indented.</returns>
    public static string IndentLineNumbers(this string @string, int startingLineNumber, int endingLineNumber) =>
        PadLinesLeft(@string, "\t", startingLineNumber, endingLineNumber);

    /// <summary>Indents a range of line numbers in a string.</summary>
    /// <param name="string">The string to indent specified lines of.</param>
    /// <param name="count">The number of tabs for the indention.</param>
    /// <param name="startingLineNumber">The line number to start line indention on.</param>
    /// <param name="endingLineNumber">The line number to stop line indention on.</param>
    /// <returns>The string with the specified lines indented.</returns>
    public static string IndentLineNumbers(this string @string, int count, int startingLineNumber, int endingLineNumber) =>
        PadLinesLeft(@string, new string('\t', count), startingLineNumber, endingLineNumber);

    /// <summary>Adds a string onto the beginning of every line in a string.</summary>
    /// <param name="string">The string to pad.</param>
    /// <param name="padding">The padding to add to the front of every line.</param>
    /// <returns>The padded string.</returns>
    public static string PadLinesLeft(this string @string, string padding)
    {
        if (@string is null) throw new ArgumentNullException(nameof(@string));
        if (padding is null) throw new ArgumentNullException(nameof(padding));
        if (padding.CompareTo(string.Empty) is 0)
        {
            return @string;
        }
        return string.Concat(padding, @string.RemoveCarriageReturns().Replace("\n", Environment.NewLine + padding));
    }

    /// <summary>Adds a string onto the end of every line in a string.</summary>
    /// <param name="string">The string to pad.</param>
    /// <param name="padding">The padding to add to the front of every line.</param>
    /// <returns>The padded string.</returns>
    public static string PadSubstringLinesRight(this string @string, string padding)
    {
        if (@string is null) throw new ArgumentNullException(nameof(@string));
        if (padding is null) throw new ArgumentNullException(nameof(padding));
        if (padding.CompareTo(string.Empty) is 0)
        {
            return @string;
        }
        return string.Concat(@string.RemoveCarriageReturns().Replace("\n", padding + Environment.NewLine), padding);
    }

    /// <summary>Adds a string after every new line squence found between two indeces of a string.</summary>
    /// <param name="string">The string to be padded.</param>
    /// <param name="padding">The padding to apply after every newline sequence found.</param>
    /// <param name="start">The starting index of the string to search for new line sequences.</param>
    /// <param name="end">The ending index of the string to search for new line sequences.</param>
    /// <returns>The padded string.</returns>
    public static string PadLinesLeftBetweenIndeces(this string @string, string padding, int start, int end)
    {
        if (@string is null) throw new ArgumentNullException(nameof(@string));
        if (padding is null) throw new ArgumentNullException(nameof(padding));
        if (start < 0 || start >= @string.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(start), start, "!(0 <= " + nameof(start) + " <= " + nameof(@string) + "." + nameof(@string.Length) + ")");
        }
        if (end >= @string.Length || end < start)
        {
            throw new ArgumentOutOfRangeException(nameof(end), end, "!(" + nameof(start) + " <= " + nameof(end) + " <= " + nameof(@string) + "." + nameof(@string.Length) + ")");
        }
        string header = @string[..start].StandardizeNewLines();
        string body = string.Concat(@string[start..end].RemoveCarriageReturns().Replace("\n", Environment.NewLine + padding));
        string footer = @string[end..].StandardizeNewLines();
        return string.Concat(header, body, footer);
    }

    /// <summary>Adds a string before every new line squence found between two indeces of a string.</summary>
    /// <param name="string">The string to be padded.</param>
    /// <param name="padding">The padding to apply before every newline sequence found.</param>
    /// <param name="start">The starting index of the string to search for new line sequences.</param>
    /// <param name="end">The ending index of the string to search for new line sequences.</param>
    /// <returns>The padded string.</returns>
    public static string PadLinesRightBetweenIndeces(this string @string, string padding, int start, int end)
    {
        if (@string is null) throw new ArgumentNullException(nameof(@string));
        if (padding is null) throw new ArgumentNullException(nameof(padding));
        if (start < 0 || start >= @string.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(start), start, "!(0 <= " + nameof(start) + " <= " + nameof(@string) + "." + nameof(@string.Length) + ")");
        }
        if (end >= @string.Length || end < start)
        {
            throw new ArgumentOutOfRangeException(nameof(end), end, "!(" + nameof(start) + " <= " + nameof(end) + " <= " + nameof(@string) + "." + nameof(@string.Length) + ")");
        }
        string header = @string[..start].StandardizeNewLines();
        string body = string.Concat(@string[start..end].RemoveCarriageReturns().Replace("\n", padding + Environment.NewLine));
        string footer = @string[end..].StandardizeNewLines();
        return string.Concat(header, body, footer);
    }

    /// <summary>Adds a string after every new line squence found between two indeces of a string.</summary>
    /// <param name="string">The string to be padded.</param>
    /// <param name="padding">The padding to apply after every newline sequence found.</param>
    /// <param name="startingLineNumber">The starting index of the line in the string to pad.</param>
    /// <param name="endingLineNumber">The ending index of the line in the string to pad.</param>
    /// <returns>The padded string.</returns>
    public static string PadLinesLeft(this string @string, string padding, int startingLineNumber, int endingLineNumber)
    {
        if (@string is null) throw new ArgumentNullException(nameof(@string));
        if (padding is null) throw new ArgumentNullException(nameof(padding));
        string[] lines = @string.SplitLines();
        if (startingLineNumber < 0 || startingLineNumber >= lines.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(startingLineNumber));
        }
        if (endingLineNumber >= lines.Length || endingLineNumber < startingLineNumber)
        {
            throw new ArgumentOutOfRangeException(nameof(endingLineNumber));
        }
        for (int i = startingLineNumber; i <= endingLineNumber; i++)
        {
            lines[i] = padding + lines[i];
        }
        return string.Concat(lines);
    }

    /// <summary>Adds a string before every new line squence found between two indeces of a string.</summary>
    /// <param name="string">The string to be padded.</param>
    /// <param name="padding">The padding to apply before every newline sequence found.</param>
    /// <param name="startingLineNumber">The starting index of the line in the string to pad.</param>
    /// <param name="endingLineNumber">The ending index of the line in the string to pad.</param>
    /// <returns>The padded string.</returns>
    public static string PadLinesRight(this string @string, string padding, int startingLineNumber, int endingLineNumber)
    {
        if (@string is null) throw new ArgumentNullException(nameof(@string));
        if (padding is null) throw new ArgumentNullException(nameof(padding));
        string[] lines = @string.SplitLines();
        if (startingLineNumber < 0 || startingLineNumber >= lines.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(startingLineNumber), startingLineNumber, "!(0 <= " + nameof(startingLineNumber) + " < [Line Count])");
        }
        if (endingLineNumber >= lines.Length || endingLineNumber < startingLineNumber)
        {
            throw new ArgumentOutOfRangeException(nameof(endingLineNumber), endingLineNumber, "!(" + nameof(startingLineNumber) + " < " + nameof(endingLineNumber) + " < [Line Count])");
        }
        for (int i = startingLineNumber; i <= endingLineNumber; i++)
        {
            lines[i] += padding;
        }
        return string.Concat(lines);
    }

    /// <summary>Reverses the characters in a string.</summary>
    /// <param name="string">The string to reverse the characters of.</param>
    /// <returns>The reversed character string.</returns>
    public static string Reverse(this string @string)
    {
        char[] characters = @string.ToCharArray();
        Array.Reverse(characters);
        return new string(characters);
    }

    /// <summary>Removes all the characters from a string based on a predicate.</summary>
    /// <param name="string">The string to remove characters from.</param>
    /// <param name="where">The predicate determining removal of each character.</param>
    /// <returns>The string after removing any predicated characters.</returns>
    public static string Remove(this string @string, Predicate<char> where)
    {
        StringBuilder stringBuilder = new();
        foreach (char c in @string)
        {
            if (!where(c))
            {
                stringBuilder.Append(c);
            }
        }
        return stringBuilder.ToString();
    }

    /// <summary>Counts the number of lines in the string.</summary>
    /// <param name="str">The string to get the line count of.</param>
    /// <returns>The number of lines in the string.</returns>
    public static int CountLines(this string str) =>
        Regex.Matches(str.StandardizeNewLines(), Environment.NewLine).Count + 1;

    #endregion System.String

    #region XML_Stepper

#pragma warning disable CS1735, CS1572, CS1711, SA1625, SA1617

    /// <summary>Performs a method on every value in a sequence.</summary>
    /// <typeparam name="T">The type of values in the sequence.</typeparam>
    /// <typeparam name="TStep">The type of method to perform on every <typeparamref name="T"/> value.</typeparam>
    /// <param name="span">The sequence of <typeparamref name="T"/> values.</param>
    /// <param name="array">The sequence of <typeparamref name="T"/> values.</param>
    /// <param name="step">The method to perform on every <typeparamref name="T"/> value.</param>
    /// <param name="start">The inclusive starting index.</param>
    /// <param name="end">The exclusive ending index.</param>
    [Obsolete(NotIntended, true)]
    public static void XML_Stepper() => throw new DocumentationMethodException();

    /// <inheritdoc cref="XML_Stepper"/>
    /// <returns><see cref="StepStatus"/></returns>
    [Obsolete(NotIntended, true)]
    public static void XML_StepperBreak() => throw new DocumentationMethodException();

#pragma warning restore CS1735, CS1572, CS1711, SA1625, SA1617

    #endregion XML_Stepper

    #region Span

    #region Stepper

    /// <inheritdoc cref="XML_Stepper"/>
    public static void Stepper<T>(this ReadOnlySpan<T> span, Action<T> step) =>
        Stepper<T, SAction<T>>(span, step);

    /// <inheritdoc cref="XML_Stepper"/>
    public static void Stepper<T, TStep>(this ReadOnlySpan<T> span, TStep step = default)
        where TStep : struct, IAction<T> =>
        StepperBreak<T, StepBreakFromAction<T, TStep>>(span, step);

    /// <inheritdoc cref="XML_StepperBreak"/>
    public static StepStatus StepperBreak<T>(this ReadOnlySpan<T> span, Func<T, StepStatus> step)
    {
        if (step is null) throw new ArgumentNullException(nameof(step));
        return StepperBreak<T, SFunc<T, StepStatus>>(span, step);
    }

    /// <inheritdoc cref="XML_StepperBreak"/>
    public static StepStatus StepperBreak<T, TStep>(this ReadOnlySpan<T> span, TStep step = default)
        where TStep : struct, IFunc<T, StepStatus> =>
        StepperBreak<T, TStep>(span, 0, span.Length, step);

    /// <inheritdoc cref="XML_Stepper"/>
    public static void Stepper<T>(this ReadOnlySpan<T> span, int start, int end, Action<T> step) =>
        Stepper<T, SAction<T>>(span, start, end, step);

    /// <inheritdoc cref="XML_Stepper"/>
    public static void Stepper<T, TStep>(this ReadOnlySpan<T> span, int start, int end, TStep step = default)
        where TStep : struct, IAction<T> =>
        StepperBreak<T, StepBreakFromAction<T, TStep>>(span, start, end, step);

    /// <inheritdoc cref="XML_StepperBreak"/>
    public static StepStatus Stepper<T>(this ReadOnlySpan<T> span, int start, int end, Func<T, StepStatus> step) =>
        StepperBreak<T, SFunc<T, StepStatus>>(span, start, end, step);

    /// <inheritdoc cref="XML_StepperBreak"/>
    public static StepStatus StepperBreak<T, TStep>(this ReadOnlySpan<T> span, int start, int end, TStep step = default)
        where TStep : struct, IFunc<T, StepStatus>
    {
        for (int i = start; i < end; i++)
        {
            if (step.Invoke(span[i]) is Break)
            {
                return Break;
            }
        }
        return Continue;
    }

    #endregion Stepper

    #endregion Span

    #region Array

    #region Stepper

    /// <inheritdoc cref="XML_Stepper"/>
    public static void Stepper<T>(this T[] array, Action<T> step)
    {
        if (step is null) throw new ArgumentNullException(nameof(step));
        Stepper<T, SAction<T>>(array, step);
    }

    /// <inheritdoc cref="XML_Stepper"/>
    public static void Stepper<T, TStep>(this T[] array, TStep step = default)
        where TStep : struct, IAction<T> =>
        StepperBreak<T, StepBreakFromAction<T, TStep>>(array, step);

    /// <inheritdoc cref="XML_StepperBreak"/>
    public static StepStatus StepperBreak<T>(this T[] array, Func<T, StepStatus> step)
    {
        if (step is null) throw new ArgumentNullException(nameof(step));
        return StepperBreak<T, SFunc<T, StepStatus>>(array, step);
    }

    /// <inheritdoc cref="XML_StepperBreak"/>
    public static StepStatus StepperBreak<T, TStep>(this T[] array, TStep step = default)
        where TStep : struct, IFunc<T, StepStatus> =>
        StepperBreak<T, TStep>(array, 0, array.Length, step);

    /// <inheritdoc cref="XML_Stepper"/>
    public static void Stepper<T>(this T[] array, int start, int end, Action<T> step) =>
        Stepper<T, SAction<T>>(array, start, end, step);

    /// <inheritdoc cref="XML_Stepper"/>
    public static void Stepper<T, TStep>(this T[] array, int start, int end, TStep step = default)
        where TStep : struct, IAction<T> =>
        StepperBreak<T, StepBreakFromAction<T, TStep>>(array, start, end, step);

    /// <inheritdoc cref="XML_StepperBreak"/>
    public static StepStatus Stepper<T>(this T[] array, int start, int end, Func<T, StepStatus> step) =>
        StepperBreak<T, SFunc<T, StepStatus>>(array, start, end, step);

    /// <inheritdoc cref="XML_StepperBreak"/>
    public static StepStatus StepperBreak<T, TStep>(this T[] array, int start, int end, TStep step = default)
        where TStep : struct, IFunc<T, StepStatus>
    {
        for (int i = start; i < end; i++)
        {
            if (step.Invoke(array[i]) is Break)
            {
                return Break;
            }
        }
        return Continue;
    }

    #endregion Stepper

    /// <summary>Builds an array from a size and initialization delegate.</summary>
    /// <typeparam name="T">The generic type of the array.</typeparam>
    /// <param name="size">The size of the array to build.</param>
    /// <param name="func">The initialization pattern.</param>
    /// <returns>The built array.</returns>
    public static T[] BuildArray<T>(int size, Func<int, T> func)
    {
        T[] array = new T[size];
        for (int i = 0; i < size; i++)
        {
            array[i] = func(i);
        }
        return array;
    }

    /// <summary>Formats an array so that all values are the same.</summary>
    /// <typeparam name="T">The generic type of the array to format.</typeparam>
    /// <param name="array">The array to format.</param>
    /// <param name="value">The value to format all entries in the array with.</param>
    public static void Format<T>(this T[] array, T value)
    {
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = value;
        }
    }

    /// <summary>Formats an array so that all values are the same.</summary>
    /// <typeparam name="T">The generic type of the array to format.</typeparam>
    /// <param name="array">The array to format.</param>
    /// <param name="func">The per-index format function.</param>
    public static void Format<T>(this T[] array, Func<int, T> func)
    {
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = func(i);
        }
    }

    /// <summary>Constructs a square jagged array of the desired dimensions.</summary>
    /// <typeparam name="T">The generic type to store in the jagged array.</typeparam>
    /// <param name="length1">The length of the first dimension.</param>
    /// <param name="length2">The length of the second dimension.</param>
    /// <returns>The constructed jagged array.</returns>
    public static T[][] ConstructRectangularJaggedArray<T>(int length1, int length2)
    {
        T[][] jaggedArray = new T[length1][];
        for (int i = 0; i < length1; i++)
        {
            jaggedArray[i] = new T[length2];
        }
        return jaggedArray;
    }

    /// <summary>Constructs a square jagged array of the desired dimensions.</summary>
    /// <typeparam name="T">The generic type to store in the jagged array.</typeparam>
    /// <param name="length1">The length of the first dimension.</param>
    /// <param name="length2">The length of the second dimension.</param>
    /// <param name="func">The function to initialize the values with.</param>
    /// <returns>The constructed jagged array.</returns>
    public static T[][] ConstructRectangularJaggedArray<T>(int length1, int length2, Func<int, int, T> func)
    {
        T[][] jaggedArray = new T[length1][];
        for (int i = 0; i < length1; i++)
        {
            jaggedArray[i] = new T[length2];
        }
        for (int i = 0; i < length1; i++)
        {
            for (int j = 0; j < length2; j++)
            {
                jaggedArray[i][j] = func(i, j);
            }
        }
        return jaggedArray;
    }

    /// <summary>Constructs a square jagged array of the desired dimensions.</summary>
    /// <typeparam name="T">The generic type to store in the jagged array.</typeparam>
    /// <param name="sideLength">The length of each dimension.</param>
    /// <returns>The constructed jagged array.</returns>
    public static T[][] ConstructSquareJaggedArray<T>(int sideLength) =>
        ConstructRectangularJaggedArray<T>(sideLength, sideLength);

    /// <summary>Constructs a square jagged array of the desired dimensions.</summary>
    /// <typeparam name="T">The generic type to store in the jagged array.</typeparam>
    /// <param name="sideLength">The length of each dimension.</param>
    /// <param name="func">The function to initialize the values with.</param>
    /// <returns>The constructed jagged array.</returns>
    public static T[][] ConstructSquareJaggedArray<T>(int sideLength, Func<int, int, T> func) =>
        ConstructRectangularJaggedArray<T>(sideLength, sideLength, func);

    #endregion Array

    #region System.Action

    /// <summary>Times an action using System.DateTime.</summary>
    /// <param name="action">The action to time.</param>
    /// <returns>The TimeSpan the action took to complete.</returns>
    public static TimeSpan Time_DateTime(this Action action)
    {
        DateTime a = DateTime.Now;
        action();
        DateTime b = DateTime.Now;
        return b - a;
    }

    /// <summary>Times an action using System.Diagnostics.Stopwatch.</summary>
    /// <param name="action">The action to time.</param>
    /// <returns>The TimeSpan the action took to complete.</returns>
    public static TimeSpan Time_StopWatch(this Action action)
    {
        Stopwatch watch = new();
        watch.Restart();
        action();
        watch.Stop();
        return watch.Elapsed;
    }

    #endregion System.Action

    #region System.Collections.Generic.IEnumerable<T>

    /// <summary>Tries to get the first value in an <see cref="System.Collections.Generic.IEnumerable{T}"/>.</summary>
    /// <typeparam name="T">The generic type of <see cref="System.Collections.Generic.IEnumerable{T}"/>.</typeparam>
    /// <param name="iEnumerable">The IEnumerable to try to get the first value of.</param>
    /// <param name="first">The first value of the <see cref="System.Collections.Generic.IEnumerable{T}"/> or default if empty.</param>
    /// <returns>True if the <see cref="System.Collections.Generic.IEnumerable{T}"/> has a first value or false if it is empty.</returns>
    public static bool TryFirst<T>(this System.Collections.Generic.IEnumerable<T> iEnumerable, out T? first)
    {
        foreach (T value in iEnumerable)
        {
            first = value;
            return true;
        }
        first = default;
        return false;
    }

    #endregion System.Collections.Generic.IEnumerable<T>

    #region System.Reflection.MethodInfo

    /// <summary>Creates a delegate of the specified type from this <see cref="MethodInfo"/>.</summary>
    /// <typeparam name="TDelegate">The type of the delegate to create.</typeparam>
    /// <param name="methodInfo">The <see cref="MethodInfo"/> to create the delegate from.</param>
    /// <returns>The delegate for this <see cref="MethodInfo"/>.</returns>
    /// <remarks>This extension is syntax sugar so you don't have to cast the return.</remarks>
    public static TDelegate CreateDelegate<TDelegate>(this MethodInfo methodInfo)
        where TDelegate : Delegate =>
        (TDelegate)methodInfo.CreateDelegate(typeof(TDelegate));

    #endregion System.Reflection.MethodInfo

    #region Enum (Generic)

    /// <inheritdoc cref="Enum.IsDefined{TEnum}"/>
    public static bool IsDefined<TEnum>(this TEnum value) where TEnum : struct, Enum => Enum.IsDefined<TEnum>(value);

    #endregion Enum (Generic)

    #region System.Range

    /// <summary>Converts a <see cref="System.Range"/> to an <see cref="System.Collections.Generic.IEnumerable{T}"/>.</summary>
    /// <param name="range">The <see cref="System.Range"/> to convert int a <see cref="System.Collections.Generic.IEnumerable{T}"/>.</param>
    /// <returns>The resulting <see cref="System.Collections.Generic.IEnumerable{T}"/> of the conversion.</returns>
    /// <exception cref="ArgumentException">range.Start.IsFromEnd</exception>
    /// <exception cref="ArgumentException">range.End.IsFromEnd</exception>
    public static System.Collections.Generic.IEnumerable<int> ToIEnumerable(this Range range)
    {
        if (range.Start.IsFromEnd)
        {
            throw new ArgumentException($"{nameof(range)}.{nameof(range.Start)}.{nameof(range.Start.IsFromEnd)}", nameof(range));
        }
        if (range.End.IsFromEnd)
        {
            throw new ArgumentException($"{nameof(range)}.{nameof(range.End)}.{nameof(range.End.IsFromEnd)}", nameof(range));
        }
        if (range.End.Value < range.Start.Value)
        {
            for (int i = range.Start.Value; i > range.End.Value; i--)
            {
                yield return i;
            }
        }
        else
        {
            for (int i = range.Start.Value; i < range.End.Value; i++)
            {
                yield return i;
            }
        }
    }

    /// <summary>Returns an <see cref="System.Collections.Generic.IEnumerator{T}"/> that iterates through the <paramref name="range"/>.</summary>
    /// <param name="range">The range to get the <see cref="System.Collections.Generic.IEnumerator{T}"/> of.</param>
    /// <returns>An <see cref="System.Collections.Generic.IEnumerator{T}"/> that iterates through the <paramref name="range"/>.</returns>
    public static System.Collections.Generic.IEnumerator<int> GetEnumerator(this Range range) => ToIEnumerable(range).GetEnumerator();

    /// <inheritdoc cref="ToSpan{T, TSelect}(Range, TSelect)" />
    public static Span<int> ToSpan(this Range range) =>
        ToArray<int, Identity<int>>(range);

    /// <inheritdoc cref="ToSpan{T, TSelect}(Range, TSelect)" />
    public static Span<T> ToSpan<T>(this Range range, Func<int, T> select)
    {
        if (select is null) throw new ArgumentNullException(nameof(select));
        return ToArray<T, SFunc<int, T>>(range, select);
    }

    /// <summary>Converts a <paramref name="range"/> to a to an span of values.</summary>
    /// <typeparam name="T">The resulting element type of the span.</typeparam>
    /// <typeparam name="TSelect">The type of method for selecting a <typeparamref name="T"/> based on an <see cref="int"/>.</typeparam>
    /// <param name="range">The range of values to convert into an span.</param>
    /// <param name="select">The method for selecting a <typeparamref name="T"/> based on an <see cref="int"/>.</param>
    /// <returns>An span of the values of the <paramref name="range"/>.</returns>
    public static Span<T> ToSpan<T, TSelect>(this Range range, TSelect select = default)
        where TSelect : struct, IFunc<int, T> =>
        ToArray<T, TSelect>(range, select);

    /// <inheritdoc cref="ToArray{T, TSelect}(Range, TSelect)"/>
    public static int[] ToArray(this Range range) =>
        ToArray<int, Identity<int>>(range);

    /// <inheritdoc cref="ToArray{T, TSelect}(Range, TSelect)"/>
    public static T[] ToArray<T>(this Range range, Func<int, T> select)
    {
        if (select is null) throw new ArgumentNullException(nameof(select));
        return ToArray<T, SFunc<int, T>>(range, select);
    }

    /// <summary>Converts a <paramref name="range"/> to a to an array of values.</summary>
    /// <typeparam name="T">The resulting element type of the array.</typeparam>
    /// <typeparam name="TSelect">The type of method for selecting a <typeparamref name="T"/> based on an <see cref="int"/>.</typeparam>
    /// <param name="range">The range of values to convert into an array.</param>
    /// <param name="select">The method for selecting a <typeparamref name="T"/> based on an <see cref="int"/>.</param>
    /// <returns>An array of the values of the <paramref name="range"/>.</returns>
    public static T[] ToArray<T, TSelect>(this Range range, TSelect select = default)
        where TSelect : struct, IFunc<int, T>
    {
        if (range.Start.IsFromEnd)
        {
            throw new ArgumentException($"{nameof(range)}.{nameof(range.Start)}.{nameof(range.Start.IsFromEnd)}", nameof(range));
        }
        if (range.End.IsFromEnd)
        {
            throw new ArgumentException($"{nameof(range)}.{nameof(range.End)}.{nameof(range.End.IsFromEnd)}", nameof(range));
        }
        T[] array = new T[Math.Abs(range.Start.Value - range.End.Value)];
        int index = 0;
        if (range.End.Value < range.Start.Value)
        {
            for (int i = range.Start.Value; i > range.End.Value; i--)
            {
                array[index++] = select.Invoke(i);
            }
        }
        else
        {
            for (int i = range.Start.Value; i < range.End.Value; i++)
            {
                array[index++] = select.Invoke(i);
            }
        }
        return array;
    }

    /// <inheritdoc cref="Select{T, TSelect}(Range, TSelect)"/>
    public static System.Collections.Generic.IEnumerable<T> Select<T>(this Range range, Func<int, T> select)
    {
        if (select is null) throw new ArgumentNullException(nameof(select));
        return Select<T, SFunc<int, T>>(range, select);
    }

    /// <summary>Projects each element of a sequence into a new form.</summary>
    /// <typeparam name="T">The type of the value returned by selector.</typeparam>
    /// <typeparam name="TSelect">The type of function for selecting a <typeparamref name="T"/> based on an <see cref="int"/>.</typeparam>
    /// <param name="range">A sequence of values to invoke a transform function on.</param>
    /// <param name="select">The function for selecting a <typeparamref name="T"/> based on an <see cref="int"/>.</param>
    /// <returns>An <see cref="System.Collections.Generic.IEnumerable{T}"/> whose elements are the result of invoking the transform function on each element of source.</returns>
    public static System.Collections.Generic.IEnumerable<T> Select<T, TSelect>(this Range range, TSelect select = default)
        where TSelect : struct, IFunc<int, T>
    {
        if (range.Start.IsFromEnd)
        {
            throw new ArgumentException($"{nameof(range)}.{nameof(range.Start)}.{nameof(range.Start.IsFromEnd)}", nameof(range));
        }
        if (range.End.IsFromEnd)
        {
            throw new ArgumentException($"{nameof(range)}.{nameof(range.End)}.{nameof(range.End.IsFromEnd)}", nameof(range));
        }
        if (range.End.Value < range.Start.Value)
        {
            for (int i = range.Start.Value; i > range.End.Value; i--)
            {
                yield return select.Invoke(i);
            }
        }
        else
        {
            for (int i = range.Start.Value; i < range.End.Value; i++)
            {
                yield return select.Invoke(i);
            }
        }
    }

    #endregion System.Range

    #region Step

    /// <summary>Adds a step to the gaps (in-betweens) of another step funtion.</summary>
    /// <typeparam name="T">The generic type of the step function.</typeparam>
    /// <param name="step">The step to add a gap step to.</param>
    /// <param name="gapStep">The step to perform in the gaps.</param>
    /// <returns>The combined step + gapStep function.</returns>
    public static Action<T> Gaps<T>(this Action<T> step, Action<T> gapStep)
    {
        bool first = true;
        return
            x =>
            {
                if (!first)
                {
                    gapStep(x);
                }
                step(x);
                first = false;
            };
    }

    #endregion Step

    #region Stepper

    /// <summary>Converts the values in this stepper to another type.</summary>
    /// <typeparam name="TA">The generic type of the values of the original stepper.</typeparam>
    /// <typeparam name="TB">The generic type of the values to convert the stepper into.</typeparam>
    /// <param name="stepper">The stepper to convert.</param>
    /// <param name="func">The conversion function.</param>
    /// <returns>The converted stepper.</returns>
    public static Action<Action<TB>> Convert<TA, TB>(this Action<Action<TA>> stepper, Func<TA, TB> func) =>
        b => stepper(a => b(func(a)));

    /// <summary>Appends values to the stepper.</summary>
    /// <typeparam name="T">The generic type of the stepper.</typeparam>
    /// <param name="stepper">The stepper to append to.</param>
    /// <param name="values">The values to append to the stepper.</param>
    /// <returns>The resulting stepper with the appended values.</returns>
    public static Action<Action<T>> Append<T>(this Action<Action<T>> stepper, params T[] values) =>
        stepper.Concat(values.ToStepper());

    /// <summary>Builds a stepper from values.</summary>
    /// <typeparam name="T">The generic type of the stepper to build.</typeparam>
    /// <param name="values">The values to build the stepper from.</param>
    /// <returns>The resulting stepper function for the provided values.</returns>
    public static Action<Action<T>> Build<T>(params T[] values) =>
        values.ToStepper();

    /// <summary>Concatenates steppers.</summary>
    /// <typeparam name="T">The generic type of the stepper.</typeparam>
    /// <param name="stepper">The first stepper of the contactenation.</param>
    /// <param name="otherSteppers">The other steppers of the concatenation.</param>
    /// <returns>The concatenated steppers as a single stepper.</returns>
    public static Action<Action<T>> Concat<T>(this Action<Action<T>> stepper, params Action<Action<T>>[] otherSteppers) =>
        step =>
        {
            stepper(step);
            foreach (Action<Action<T>> otherStepper in otherSteppers)
            {
                otherStepper(step);
            }
        };

    /// <summary>Filters a stepper using a where predicate.</summary>
    /// <typeparam name="T">The generic type of the stepper.</typeparam>
    /// <param name="stepper">The stepper to filter.</param>
    /// <param name="predicate">The predicate of the where filter.</param>
    /// <returns>The filtered stepper.</returns>
    public static Action<Action<T>> Where<T>(this Action<Action<T>> stepper, Func<T, bool> predicate) =>
        step => stepper(x =>
        {
            if (predicate(x))
            {
                step(x);
            }
        });

    /// <summary>Steps through a set number of integers.</summary>
    /// <param name="iterations">The number of times to iterate.</param>
    /// <param name="step">The step function.</param>
    public static void Iterate(int iterations, Action<int> step)
    {
        for (int i = 0; i < iterations; i++)
        {
            step(i);
        }
    }

    /// <summary>Converts an IEnumerable into a stepper delegate./></summary>
    /// <typeparam name="T">The generic type being iterated.</typeparam>
    /// <param name="iEnumerable">The IEnumerable to convert.</param>
    /// <returns>The stepper delegate comparable to the IEnumerable provided.</returns>
    public static Action<Action<T>> ToStepper<T>(this System.Collections.Generic.IEnumerable<T> iEnumerable) =>
        step =>
        {
            foreach (T value in iEnumerable)
            {
                step(value);
            }
        };

    /// <summary>Converts an IEnumerable into a stepper delegate./></summary>
    /// <typeparam name="T">The generic type being iterated.</typeparam>
    /// <param name="iEnumerable">The IEnumerable to convert.</param>
    /// <returns>The stepper delegate comparable to the IEnumerable provided.</returns>
    public static Func<Func<T, StepStatus>, StepStatus> ToStepperBreak<T>(this System.Collections.Generic.IEnumerable<T> iEnumerable) =>
        step =>
        {
            foreach (T value in iEnumerable)
            {
                if (step(value) is Break)
                {
                    return Break;
                }
            }
            return Continue;
        };

    /// <summary>Converts the stepper into an array.</summary>
    /// <typeparam name="T">The generic type of the stepper.</typeparam>
    /// <param name="stepper">The stepper to convert.</param>
    /// <returns>The array created from the stepper.</returns>
    public static T[] ToArray<T>(this Action<Action<T>> stepper)
    {
        int count = stepper.Count();
        T[] array = new T[count];
        int i = 0;
        stepper(x => array[i++] = x);
        return array;
    }

    /// <summary>Counts the number of items in the stepper.</summary>
    /// <typeparam name="T">The generic type of the stepper.</typeparam>
    /// <param name="stepper">The stepper to count the items of.</param>
    /// <returns>The number of items in the stepper.</returns>
    public static int Count<T>(this Action<Action<T>> stepper)
    {
        int count = 0;
        stepper(step => count++);
        return count;
    }

    /// <summary>Reduces the stepper to be every nth value.</summary>
    /// <typeparam name="T">The generic type of the stepper.</typeparam>
    /// <param name="stepper">The stepper to reduce.</param>
    /// <param name="nth">Represents the values to reduce the stepper to; "5" means every 5th value.</param>
    /// <returns>The reduced stepper function.</returns>
    public static Action<Action<T>> EveryNth<T>(this Action<Action<T>> stepper, int nth)
    {
        if (stepper is null) throw new ArgumentNullException(nameof(stepper));
        if (nth <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(nth), nth, "!(" + nameof(nth) + " > 0)");
        }
        int i = 1;
        return step => stepper(x =>
        {
            if (i == nth)
            {
                step(x);
                i = 1;
            }
            else
            {
                i++;
            }
        });
    }

    /// <summary>Determines if the data contains any duplicates.</summary>
    /// <typeparam name="T">The generic type of the data.</typeparam>
    /// <param name="stepper">The stepper function for the data.</param>
    /// <param name="equate">An equality function for the data</param>
    /// <param name="hash">A hashing function for the data.</param>
    /// <returns>True if the data contains duplicates. False if not.</returns>
    public static bool ContainsDuplicates<T>(this Func<Func<T, StepStatus>, StepStatus> stepper, Func<T, T, bool>? equate = null, Func<T, int>? hash = null)
    {
        bool duplicateFound = false;
        var set = SetHashLinked.New(equate, hash);
        stepper(x =>
        {
            if (set.Contains(x))
            {
                duplicateFound = true;
                return Break;
            }
            else
            {
                set.Add(x);
                return Continue;
            }
        });
        return duplicateFound;
    }

    /// <summary>Determines if the data contains any duplicates.</summary>
    /// <typeparam name="T">The generic type of the data.</typeparam>
    /// <param name="stepper">The stepper function for the data.</param>
    /// <param name="equate">An equality function for the data</param>
    /// <param name="hash">A hashing function for the data.</param>
    /// <returns>True if the data contains duplicates. False if not.</returns>
    /// <remarks>Use the StepperBreak overload if possible. It is more effiecient.</remarks>
    public static bool ContainsDuplicates<T>(this Action<Action<T>> stepper, Func<T, T, bool>? equate = null, Func<T, int>? hash = null)
    {
        bool duplicateFound = false;
        var set = SetHashLinked.New(equate, hash);
        stepper(x =>
        {
            if (set.Contains(x))
            {
                duplicateFound = true;
            }
            else
            {
                set.Add(x);
            }
        });
        return duplicateFound;
    }

    /// <summary>Determines if the data contains any duplicates.</summary>
    /// <typeparam name="T">The generic type of the data.</typeparam>
    /// <param name="stepper">The stepper function for the data.</param>
    /// <returns>True if the data contains duplicates. False if not.</returns>
    public static bool ContainsDuplicates<T>(this Func<Func<T, StepStatus>, StepStatus> stepper) =>
        ContainsDuplicates(stepper, Equate, Hash);

    /// <summary>Determines if the data contains any duplicates.</summary>
    /// <typeparam name="T">The generic type of the data.</typeparam>
    /// <param name="stepper">The stepper function for the data.</param>
    /// <returns>True if the data contains duplicates. False if not.</returns>
    /// <remarks>Use the StepperBreak overload if possible. It is more effiecient.</remarks>
    public static bool ContainsDuplicates<T>(this Action<Action<T>> stepper) =>
        ContainsDuplicates(stepper, Equate, Hash);

    /// <summary>Determines if the stepper contains any of the predicated values.</summary>
    /// <typeparam name="T">The generic type of the stepper.</typeparam>
    /// <param name="stepper">The stepper to determine if any predicated values exist.</param>
    /// <param name="where">The predicate.</param>
    /// <returns>True if any of the predicated values exist or </returns>
    public static bool Any<T>(this Action<Action<T>> stepper, Predicate<T> where)
    {
        bool any = false;
        stepper(x => any = any || where(x));
        return any;
    }

    /// <summary>Determines if the stepper contains any of the predicated values.</summary>
    /// <typeparam name="T">The generic type of the stepper.</typeparam>
    /// <param name="stepper">The stepper to determine if any predicated values exist.</param>
    /// <param name="where">The predicate.</param>
    /// <returns>True if any of the predicated values exist or </returns>
    public static bool Any<T>(this Func<Func<T, StepStatus>, StepStatus> stepper, Predicate<T> where)
    {
        bool any = false;
        stepper(x => (any = where(x))
            ? Break
            : Continue);
        return any;
    }

    /// <summary>Converts a stepper into a string of the concatenated chars.</summary>
    /// <param name="stepper">The stepper to concatenate the values into a string.</param>
    /// <returns>The string of the concatenated chars.</returns>
    public static string ConcatToString(this Action<Action<char>> stepper)
    {
        StringBuilder stringBuilder = new();
        stepper(c => stringBuilder.Append(c));
        return stringBuilder.ToString();
    }

    #endregion Stepper

    #region int

    /// <summary>Converts an <see cref="int"/> to the relative <see cref="CompareResult"/>.</summary>
    /// <param name="compareResult">The <see cref="int"/> to convert to the relative <see cref="CompareResult"/>.</param>
    /// <returns>
    /// <paramref name="compareResult"/> &lt; 0 =&gt; <see cref="CompareResult.Less"/><br/>
    /// <paramref name="compareResult"/> &gt; 0 =&gt; <see cref="CompareResult.Greater"/><br/>
    /// <paramref name="compareResult"/> is 0 =&gt; <see cref="CompareResult.Equal"/>
    /// </returns>
    public static CompareResult ToCompareResult(this int compareResult) =>
        compareResult switch
        {
            < 0 => Less,
            > 0 => Greater,
            0 => Equal,
        };

    #endregion int
}