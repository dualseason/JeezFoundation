﻿namespace JeezFoundation.Algorithm.DataStructures;

/// <summary>An indexed fixed-sized data structure.</summary>
/// <typeparam name="T">The generic type within the structure.</typeparam>
/// <typeparam name="TIndex">The generic type of the indexing.</typeparam>
public interface IArray<T, TIndex> : IDataStructure<T>
{
    /// <summary>Allows indexed access of the array.</summary>
    /// <param name="index">The index of the array to get/set.</param>
    /// <returns>The value at the desired index.</returns>
    T this[TIndex index] { get; set; }

    /// <summary>
    /// Gets the length of the array.
    /// </summary>
    TIndex Length { get; }
}

/// <summary>An indexed fixed-sized data structure.</summary>
/// <typeparam name="T">The generic type within the structure.</typeparam>
public interface IArray<T> : IArray<T, int>
{
}

/// <summary>Contiguous fixed-sized data structure.</summary>
/// <typeparam name="T">The generic type within the structure.</typeparam>
public class Array<T> : IArray<T>, ICloneable<Array<T>>
{
    internal T[] _array;

    #region Constructors

    /// <summary>Constructs an array that implements a traversal delegate function
    /// which is an optimized "foreach" implementation.</summary>
    /// <param name="size">The length of the array in memory.</param>
    public Array(int size)
    {
        if (size < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(size), "size of the array must be at least 1.");
        }
        _array = new T[size];
    }

    /// <summary>Constructs by wrapping an existing array.</summary>
    /// <param name="array">The array to be wrapped.</param>
    public Array(params T[] array) => _array = array;

    #endregion Constructors

    #region Properties

    /// <inheritdoc/>
    public T this[int index]
    {
        get
        {
            if (!(0 <= index || index < _array.Length))
            {
                throw new ArgumentOutOfRangeException(nameof(index), $"!(0 <= {nameof(index)} < this.{nameof(_array.Length)})");
            }
            return _array[index];
        }
        set
        {
            if (!(0 <= index || index < _array.Length))
            {
                throw new ArgumentOutOfRangeException(nameof(index), $"!(0 <= {nameof(index)} < this.{nameof(_array.Length)})");
            }
            _array[index] = value;
        }
    }

    /// <summary>The length of the array.</summary>
    public int Length => _array.Length;

    #endregion Properties

    #region Operators

    /// <summary>Implicitly converts a C# System array into a JeezFoundation.Algorithm array.</summary>
    /// <param name="array">The array to be represented as a JeezFoundation.Algorithm array.</param>
    public static implicit operator Array<T>(T[] array) => new(array);

    /// <summary>Implicitly converts a JeezFoundation.Algorithm array into a C# System array.</summary>
    /// <param name="array">The array to be represented as a C# System array.</param>
    public static implicit operator T[](Array<T> array) => array.ToArray();

    #endregion Operators

    #region Methods

    /// <inheritdoc/>
    public Array<T> Clone() => (T[])_array.Clone();

    /// <inheritdoc/>
    public StepStatus StepperBreak<TStep>(TStep step)
        where TStep : struct, IFunc<T, StepStatus> =>
        _array.StepperBreak(step);

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

    /// <inheritdoc/>
    public System.Collections.Generic.IEnumerator<T> GetEnumerator() => ((System.Collections.Generic.IEnumerable<T>)_array).GetEnumerator();

    /// <inheritdoc/>
    public T[] ToArray() => Length is 0 ? Array.Empty<T>() : _array[..];

    #endregion Methods
}