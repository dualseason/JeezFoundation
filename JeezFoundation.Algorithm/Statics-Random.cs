﻿namespace JeezFoundation.Algorithm;

/// <summary>Root type of the static functional methods in JeezFoundation.Algorithm.</summary>
public static partial class Statics
{
    #region Next (with exclusions)

    /// <summary>
    /// Generates <paramref name="count"/> random <see cref="int"/> values in the
    /// [<paramref name="minValue"/>..<paramref name="maxValue"/>] range where <paramref name="minValue"/> is
    /// inclusive and <paramref name="maxValue"/> is exclusive.
    /// </summary>
    /// <typeparam name="TStep">The type of function to perform on each generated <see cref="int"/> value.</typeparam>
    /// <typeparam name="TRandom">The type of random to generation algorithm.</typeparam>
    /// <param name="count">The number of <see cref="int"/> values to generate.</param>
    /// <param name="minValue">Inclusive endpoint of the random generation range.</param>
    /// <param name="maxValue">Exclusive endpoint of the random generation range.</param>
    /// <param name="excluded">Values that should be excluded during generation.</param>
    /// <param name="random">The random to generation algorithm.</param>
    /// <param name="step">The function to perform on each generated <see cref="int"/> value.</param>
    public static void Next<TStep, TRandom>(int count, int minValue, int maxValue, ReadOnlySpan<int> excluded, TRandom random = default, TStep step = default)
        where TStep : struct, IAction<int>
        where TRandom : struct, IFunc<int, int, int>
    {
        if (count * excluded.Length + .5 * Math.Pow(excluded.Length, 2) < (maxValue - minValue) + count + 2 * excluded.Length)
        {
            NextRollTracking(count, minValue, maxValue, excluded, random, step);
        }
        else
        {
            NextPoolTracking(count, minValue, maxValue, excluded, random, step);
        }
    }

    /// <inheritdoc cref="Next{TStep, TRandom}(int, int, int, ReadOnlySpan{int}, TRandom, TStep)"></inheritdoc>
    public static void NextRollTracking<TStep, TRandom>(int count, int minValue, int maxValue, ReadOnlySpan<int> excluded, TRandom random = default, TStep step = default)
        where TStep : struct, IAction<int>
        where TRandom : struct, IFunc<int, int, int>
    {
        if (sourceof(maxValue < minValue, out string c1)) throw new ArgumentOutOfRangeException(nameof(maxValue), c1);
        if (sourceof(count < 0, out string c2)) throw new ArgumentOutOfRangeException(nameof(count), c2);
        // Algorithm B: O(count * excluded.Length + .5*excluded.Length^2)
        Node<int>? head = null;
        int excludeCount = 0;
        foreach (int i in excluded) // Θ(excluded.Length)
        {
            if (i < minValue || i >= maxValue)
            {
                continue;
            }
            Node<int>? node = head;
            Node<int>? previous = null;
            while (node is not null && node.Value <= i) // O(.5*excluded.Length), Ω(0), ε(.5*excluded.Length)
            {
                if (node.Value == i)
                {
                    goto Continue;
                }
                previous = node;
                node = node.Next;
            }
            excludeCount++;
            if (previous is null)
            {
                head = new Node<int>() { Value = i, Next = head, };
            }
            else
            {
                previous.Next = new Node<int>() { Value = i, Next = previous.Next };
            }
        Continue:
            continue;
        }
        if (excludeCount >= maxValue - minValue)
        {
            throw new ArgumentException($"{nameof(excluded)}.{nameof(excluded.Length)} >= {nameof(count)}");
        }
        for (int i = 0; i < count; i++) // Θ(count)
        {
            int roll = random.Invoke(minValue, maxValue - excludeCount);
            if (roll < minValue || roll >= maxValue - excludeCount)
            {
                throw new ArgumentException("The Random provided returned a value outside the requested range.");
            }
            Node<int>? node = head;
            while (node is not null && node.Value <= roll) // O(excluded.Length), Ω(0)
            {
                roll++;
                node = node.Next;
            }
            step.Invoke(roll);
        }
    }

    /// <inheritdoc cref="Next{TStep, TRandom}(int, int, int, ReadOnlySpan{int}, TRandom, TStep)"></inheritdoc>
    public static void NextPoolTracking<TStep, TRandom>(int count, int minValue, int maxValue, ReadOnlySpan<int> excluded, TRandom random = default, TStep step = default)
        where TStep : struct, IAction<int>
        where TRandom : struct, IFunc<int, int, int>
    {
        if (sourceof(maxValue < minValue, out string c1)) throw new ArgumentOutOfRangeException(nameof(maxValue), c1);
        if (sourceof(count < 0, out string c2)) throw new ArgumentOutOfRangeException(nameof(count), c2);
        // Algorithm: Θ(range + count + 2*excluded.Length)
        SetHashLinked<int, Int32Equate, Int32Hash> set = new(expectedCount: excluded.Length); // Θ(excluded)
        foreach (int value in excluded)
        {
            if (minValue <= value && value < maxValue)
            {
                set.TryAdd(value);
            }
        }
        if (set.Count >= maxValue - minValue)
        {
            throw new ArgumentException($"{nameof(excluded)}.{nameof(excluded.Length)} >= {nameof(maxValue)} - {nameof(minValue)}");
        }
        int pool = maxValue - minValue - set.Count;
        Span<int> span = new int[pool];
        for (int i = 0, j = minValue; i < pool; j++) // Θ(range + excluded.Length)
        {
            if (!set.Contains(j))
            {
                span[i++] = j;
            }
        }
        for (int i = 0; i < count; i++) // Θ(count)
        {
            int rollIndex = random.Invoke(0, pool);
            if (rollIndex < 0 || rollIndex >= pool)
            {
                throw new ArgumentException("The Random provided returned a value outside the requested range.");
            }
            int roll = span[rollIndex];
            step.Invoke(roll);
        }
    }

    #region Overloads

    /// <inheritdoc cref="Next{TStep, TRandom}(int, int, int, ReadOnlySpan{int}, TRandom, TStep)"></inheritdoc>
    /// <param name="step">The function to perform on each randomly generated value.</param>
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)

    public static void Next<TRandom>(int count, int minValue, int maxValue, ReadOnlySpan<int> excluded, Action<int> step, TRandom random = default)
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
        where TRandom : struct, IFunc<int, int, int>
    {
        if (step is null) throw new ArgumentNullException(nameof(step));
        if (sourceof(maxValue < minValue, out string c1)) throw new ArgumentOutOfRangeException(nameof(maxValue), c1);
        if (sourceof(count < 0, out string c2)) throw new ArgumentOutOfRangeException(nameof(count), c2);
        Next<SAction<int>, TRandom>(count, minValue, maxValue, excluded, random, step);
    }

    /// <inheritdoc cref="Next{TRandom}(int, int, int, ReadOnlySpan{int}, Action{int}, TRandom)"></inheritdoc>
    public static void NextRollTracking<TRandom>(int count, int minValue, int maxValue, ReadOnlySpan<int> excluded, Action<int> step, TRandom random = default)
        where TRandom : struct, IFunc<int, int, int>
    {
        if (step is null) throw new ArgumentNullException(nameof(step));
        if (sourceof(maxValue < minValue, out string c1)) throw new ArgumentOutOfRangeException(nameof(maxValue), c1);
        if (sourceof(count < 0, out string c2)) throw new ArgumentOutOfRangeException(nameof(count), c2);
        NextRollTracking<SAction<int>, TRandom>(count, minValue, maxValue, excluded, random, step);
    }

    /// <inheritdoc cref="Next{TRandom}(int, int, int, ReadOnlySpan{int}, Action{int}, TRandom)"></inheritdoc>
    public static void NextPoolTracking<TRandom>(int count, int minValue, int maxValue, ReadOnlySpan<int> excluded, Action<int> step, TRandom random = default)
        where TRandom : struct, IFunc<int, int, int>
    {
        if (step is null) throw new ArgumentNullException(nameof(step));
        if (sourceof(maxValue < minValue, out string c1)) throw new ArgumentOutOfRangeException(nameof(maxValue), c1);
        if (sourceof(count < 0, out string c2)) throw new ArgumentOutOfRangeException(nameof(count), c2);
        NextPoolTracking<SAction<int>, TRandom>(count, minValue, maxValue, excluded, random, step);
    }

    /// <inheritdoc cref="Next{TStep, TRandom}(int, int, int, ReadOnlySpan{int}, TRandom, TStep)"></inheritdoc>
    /// <returns>The randomly generated values.</returns>
    public static int[] Next<TRandom>(int count, int minValue, int maxValue, ReadOnlySpan<int> excluded, TRandom random = default)
        where TRandom : struct, IFunc<int, int, int>
    {
        if (sourceof(maxValue < minValue, out string c1)) throw new ArgumentOutOfRangeException(nameof(maxValue), c1);
        if (sourceof(count < 0, out string c2)) throw new ArgumentOutOfRangeException(nameof(count), c2);
        int[] values = new int[count];
        Next<FillArray<int>, TRandom>(count, minValue, maxValue, excluded, random, values);
        return values;
    }

    /// <inheritdoc cref="Next{TRandom}(int, int, int, ReadOnlySpan{int}, TRandom)"/>
    public static int[] NextRollTracking<TRandom>(int count, int minValue, int maxValue, ReadOnlySpan<int> excluded, TRandom random = default)
        where TRandom : struct, IFunc<int, int, int>
    {
        if (sourceof(maxValue < minValue, out string c1)) throw new ArgumentOutOfRangeException(nameof(maxValue), c1);
        if (sourceof(count < 0, out string c2)) throw new ArgumentOutOfRangeException(nameof(count), c2);
        int[] values = new int[count];
        NextRollTracking<FillArray<int>, TRandom>(count, minValue, maxValue, excluded, random, values);
        return values;
    }

    /// <inheritdoc cref="Next{TRandom}(int, int, int, ReadOnlySpan{int}, TRandom)"/>
    public static int[] NextPoolTracking<TRandom>(int count, int minValue, int maxValue, ReadOnlySpan<int> excluded, TRandom random = default)
        where TRandom : struct, IFunc<int, int, int>
    {
        if (sourceof(maxValue < minValue, out string c1)) throw new ArgumentOutOfRangeException(nameof(maxValue), c1);
        if (sourceof(count < 0, out string c2)) throw new ArgumentOutOfRangeException(nameof(count), c2);
        int[] values = new int[count];
        NextPoolTracking<FillArray<int>, TRandom>(count, minValue, maxValue, excluded, random, values);
        return values;
    }

    #endregion Overloads

    #endregion Next (with exclusions)

    #region NextUnique

    /// <summary>
    /// Generates <paramref name="count"/> unique random <see cref="int"/> values in the
    /// [<paramref name="minValue"/>..<paramref name="maxValue"/>] range where <paramref name="minValue"/> is
    /// inclusive and <paramref name="maxValue"/> is exclusive.
    /// </summary>
    /// <typeparam name="TStep">The type of function to perform on each generated <see cref="int"/> value.</typeparam>
    /// <typeparam name="TRandom">The type of random to generation algorithm.</typeparam>
    /// <param name="count">The number of <see cref="int"/> values to generate.</param>
    /// <param name="minValue">Inclusive endpoint of the random generation range.</param>
    /// <param name="maxValue">Exclusive endpoint of the random generation range.</param>
    /// <param name="random">The random to generation algorithm.</param>
    /// <param name="step">The function to perform on each generated <see cref="int"/> value.</param>
    public static void NextUnique<TStep, TRandom>(int count, int minValue, int maxValue, TRandom random = default, TStep step = default)
        where TStep : struct, IAction<int>
        where TRandom : struct, IFunc<int, int, int>
    {
        if (count < Math.Sqrt(maxValue - minValue))
        {
            NextUniqueRollTracking(count, minValue, maxValue, random, step);
        }
        else
        {
            NextUniquePoolTracking(count, minValue, maxValue, random, step);
        }
    }

    /// <inheritdoc cref="NextUnique{TStep, TRandom}(int, int, int, TRandom, TStep)"/>
    public static void NextUniqueRollTracking<TStep, TRandom>(int count, int minValue, int maxValue, TRandom random = default, TStep step = default)
        where TStep : struct, IAction<int>
        where TRandom : struct, IFunc<int, int, int>
    {
        if (sourceof(maxValue < minValue, out string c1)) throw new ArgumentOutOfRangeException(nameof(maxValue), c1);
        if (sourceof(count < 0, out string c2)) throw new ArgumentOutOfRangeException(nameof(count), c2);
        if (sourceof(maxValue - minValue < count, out string c3)) throw new ArgumentOutOfRangeException(nameof(count), c3);
#if stackalloc
			if (count <= 128)
			{
				// Algorithm B: Θ(.5*count^2)
				Span<int> span = stackalloc int[count];
				for (int i = 0; i < count; i++) // Θ(count)
				{
					int roll = random.Do(minValue, maxValue - i);
					if (roll < minValue || roll >= maxValue - i)
					{
						throw new ArgumentException("The Random provided returned a value outside the requested range.");
					}
					int index = 0;
					for (; index < i && span[index] <= roll; index++)
					{
						roll++;
					}
					step.Do(roll);
					for (int j = i; j > index; j--)
					{
						span[j] = span[j - 1];
					}
					span[index] = roll;
				}
			}
			else
			{
#endif
        // Algorithm: O(.5*count^2), Ω(count), ε(.5*count^2)
        Node<int>? head = null;
        for (int i = 0; i < count; i++) // Θ(count)
        {
            int roll = random.Invoke(minValue, maxValue - i);
            if (roll < minValue || roll >= maxValue - i)
            {
                throw new ArgumentException("The Random provided returned a value outside the requested range.");
            }
            Node<int>? node = head;
            Node<int>? previous = null;
            while (node is not null && node.Value <= roll) // O(.5*count), Ω(0), ε(.5*count)
            {
                roll++;
                previous = node;
                node = node.Next;
            }
            step.Invoke(roll);
            if (previous is null)
            {
                head = new Node<int>() { Value = roll, Next = head, };
            }
            else
            {
                previous.Next = new Node<int>() { Value = roll, Next = previous.Next };
            }
        }
#if stackalloc
			}
#endif
    }

    /// <inheritdoc cref="NextUnique{TStep, TRandom}(int, int, int, TRandom, TStep)"/>
    public static void NextUniquePoolTracking<TStep, TRandom>(int count, int minValue, int maxValue, TRandom random = default, TStep step = default)
        where TStep : struct, IAction<int>
        where TRandom : struct, IFunc<int, int, int>
    {
        if (sourceof(maxValue < minValue, out string c1)) throw new ArgumentOutOfRangeException(nameof(maxValue), c1);
        if (sourceof(count < 0, out string c2)) throw new ArgumentOutOfRangeException(nameof(count), c2);
        if (sourceof(maxValue - minValue < count, out string c3)) throw new ArgumentOutOfRangeException(nameof(count), c3);
        // Algorithm: Θ(range + count)
        int pool = maxValue - minValue;
        Span<int> span = sizeof(int) * pool <= Stackalloc ? stackalloc int[pool] : new int[pool];
        for (int i = 0, j = minValue; j < maxValue; i++, j++) // Θ(range)
        {
            span[i] = j;
        }
        for (int i = 0; i < count; i++) // Θ(count)
        {
            int rollIndex = random.Invoke(0, pool);
            if (rollIndex < 0 || rollIndex >= pool)
            {
                throw new ArgumentException("The Random provided returned a value outside the requested range.");
            }
            int roll = span[rollIndex];
            span[rollIndex] = span[--pool];
            step.Invoke(roll);
        }
    }

    #region Overloads

    /// <inheritdoc cref="NextUnique{TStep, TRandom}(int, int, int, TRandom, TStep)"></inheritdoc>
    /// <param name="step">The function to perform on each randomly generated value.</param>
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)

    public static void NextUnique<TRandom>(int count, int minValue, int maxValue, Action<int> step, TRandom random = default)
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
        where TRandom : struct, IFunc<int, int, int>
    {
        if (step is null) throw new ArgumentNullException(nameof(step));
        if (sourceof(maxValue < minValue, out string c1)) throw new ArgumentOutOfRangeException(nameof(maxValue), c1);
        if (sourceof(count < 0, out string c2)) throw new ArgumentOutOfRangeException(nameof(count), c2);
        NextUnique<SAction<int>, TRandom>(count, minValue, maxValue, random, step);
    }

    /// <inheritdoc cref="NextUnique{TRandom}(int, int, int, Action{int}, TRandom)"></inheritdoc>
    public static void NextUniqueRollTracking<TRandom>(int count, int minValue, int maxValue, Action<int> step, TRandom random = default)
        where TRandom : struct, IFunc<int, int, int>
    {
        if (step is null) throw new ArgumentNullException(nameof(step));
        if (sourceof(maxValue < minValue, out string c1)) throw new ArgumentOutOfRangeException(nameof(maxValue), c1);
        if (sourceof(count < 0, out string c2)) throw new ArgumentOutOfRangeException(nameof(count), c2);
        NextUniqueRollTracking<SAction<int>, TRandom>(count, minValue, maxValue, random, step);
    }

    /// <inheritdoc cref="NextUnique{TRandom}(int, int, int, Action{int}, TRandom)"></inheritdoc>
    public static void NextUniquePoolTracking<TRandom>(int count, int minValue, int maxValue, Action<int> step, TRandom random = default)
        where TRandom : struct, IFunc<int, int, int>
    {
        if (step is null) throw new ArgumentNullException(nameof(step));
        if (sourceof(maxValue < minValue, out string c1)) throw new ArgumentOutOfRangeException(nameof(maxValue), c1);
        if (sourceof(count < 0, out string c2)) throw new ArgumentOutOfRangeException(nameof(count), c2);
        NextUniquePoolTracking<SAction<int>, TRandom>(count, minValue, maxValue, random, step);
    }

    /// <inheritdoc cref="NextUnique{TStep, TRandom}(int, int, int, TRandom, TStep)"></inheritdoc>
    /// <returns>The randomly generated values.</returns>
    public static int[] NextUnique<TRandom>(int count, int minValue, int maxValue, TRandom random = default)
        where TRandom : struct, IFunc<int, int, int>
    {
        if (sourceof(maxValue < minValue, out string c1)) throw new ArgumentOutOfRangeException(nameof(maxValue), c1);
        if (sourceof(count < 0, out string c2)) throw new ArgumentOutOfRangeException(nameof(count), c2);
        if (sourceof(maxValue - minValue < count, out string c3)) throw new ArgumentOutOfRangeException(nameof(count), c3);
        int[] values = new int[count];
        NextUnique<FillArray<int>, TRandom>(count, minValue, maxValue, random, values);
        return values;
    }

    /// <inheritdoc cref="NextUnique{TRandom}(int, int, int, TRandom)"></inheritdoc>
    public static int[] NextUniqueRollTracking<TRandom>(int count, int minValue, int maxValue, TRandom random = default)
        where TRandom : struct, IFunc<int, int, int>
    {
        if (sourceof(maxValue < minValue, out string c1)) throw new ArgumentOutOfRangeException(nameof(maxValue), c1);
        if (sourceof(count < 0, out string c2)) throw new ArgumentOutOfRangeException(nameof(count), c2);
        if (sourceof(maxValue - minValue < count, out string c3)) throw new ArgumentOutOfRangeException(nameof(count), c3);
        int[] values = new int[count];
        NextUniqueRollTracking<FillArray<int>, TRandom>(count, minValue, maxValue, random, values);
        return values;
    }

    /// <inheritdoc cref="NextUnique{TRandom}(int, int, int, TRandom)"></inheritdoc>
    public static int[] NextUniquePoolTracking<TRandom>(int count, int minValue, int maxValue, TRandom random = default)
        where TRandom : struct, IFunc<int, int, int>
    {
        if (sourceof(maxValue < minValue, out string c1)) throw new ArgumentOutOfRangeException(nameof(maxValue), c1);
        if (sourceof(count < 0, out string c2)) throw new ArgumentOutOfRangeException(nameof(count), c2);
        if (sourceof(maxValue - minValue < count, out string c3)) throw new ArgumentOutOfRangeException(nameof(count), c3);
        int[] values = new int[count];
        NextUniquePoolTracking<FillArray<int>, TRandom>(count, minValue, maxValue, random, values);
        return values;
    }

    #endregion Overloads

    #endregion NextUnique

    #region NextUnique (with exclusions)

    /// <summary>
    /// Generates <paramref name="count"/> unique random <see cref="int"/> values in the
    /// [<paramref name="minValue"/>..<paramref name="maxValue"/>] range where <paramref name="minValue"/> is
    /// inclusive and <paramref name="maxValue"/> is exclusive.
    /// </summary>
    /// <typeparam name="TStep">The type of function to perform on each generated <see cref="int"/> value.</typeparam>
    /// <typeparam name="TRandom">The type of random to generation algorithm.</typeparam>
    /// <param name="count">The number of <see cref="int"/> values to generate.</param>
    /// <param name="minValue">Inclusive endpoint of the random generation range.</param>
    /// <param name="maxValue">Exclusive endpoint of the random generation range.</param>
    /// <param name="excluded">Values that should be excluded during generation.</param>
    /// <param name="random">The random to generation algorithm.</param>
    /// <param name="step">The function to perform on each generated <see cref="int"/> value.</param>
    public static void NextUnique<TStep, TRandom>(int count, int minValue, int maxValue, ReadOnlySpan<int> excluded, TRandom random = default, TStep step = default)
        where TStep : struct, IAction<int>
        where TRandom : struct, IFunc<int, int, int>
    {
        if (count + excluded.Length < Math.Sqrt(maxValue - minValue))
        {
            NextUniqueRollTracking(count, minValue, maxValue, excluded, random, step);
        }
        else
        {
            NextUniquePoolTracking(count, minValue, maxValue, excluded, random, step);
        }
    }

    /// <inheritdoc cref="NextUnique{TStep, TRandom}(int, int, int, ReadOnlySpan{int}, TRandom, TStep)"/>
    public static void NextUniqueRollTracking<TStep, TRandom>(int count, int minValue, int maxValue, ReadOnlySpan<int> excluded, TRandom random = default, TStep step = default)
        where TStep : struct, IAction<int>
        where TRandom : struct, IFunc<int, int, int>
    {
        if (sourceof(maxValue < minValue, out string c1)) throw new ArgumentOutOfRangeException(nameof(maxValue), c1);
        if (sourceof(count < 0, out string c2)) throw new ArgumentOutOfRangeException(nameof(count), c2);
        if (sourceof(maxValue - minValue < count, out string c3)) throw new ArgumentOutOfRangeException(nameof(count), c3);
#if stackalloc
			if (count + excluded.Length <= 128)
			{
				// Algorithm B: Θ(.5*count^2)
				int excludeCount = 0;
				Span<int> span = stackalloc int[count + excluded.Length];
				{
					foreach (int exclude in excluded)
					{
						if (minValue <= exclude && exclude < maxValue)
						{
							int index = 0;
							for (; index < excludeCount && exclude > span[index]; index++)
							{
								if (span[index] == exclude)
								{
									goto Continue;
								}
							}
							for (int j = excludeCount + 1; j > index; j--)
							{
								span[j] = span[j - 1];
							}
							span[excludeCount++] = exclude;
						}
					Continue:
						continue;
					}
				}
				if (maxValue - minValue - excludeCount < count)
				{
					throw new ArgumentException($"{nameof(maxValue)} - {nameof(minValue)} - {nameof(excluded)}.Length [{maxValue - minValue - excludeCount < count}] < {nameof(count)} [{count}]");
				}
				for (int i = 0; i < count; i++) // Θ(count)
				{
					int roll = random.Do(minValue, maxValue - i + excludeCount);
					if (roll < minValue || roll >= maxValue - i + excludeCount)
					{
						throw new ArgumentException("The Random provided returned a value outside the requested range.");
					}
					int index = 0;
					for (; index < i + excludeCount && span[index] <= roll; index++)
					{
						roll++;
					}
					step.Do(roll);
					for (int j = i + excludeCount; j > index; j--)
					{
						span[j] = span[j - 1];
					}
					span[index] = roll;
				}
			}
			else
			{
#endif
        // Algorithm B: O(.5*(count + excluded.Length)^2 + .5*excluded.Length^2), Ω(count + excluded.Length), ε(.5*(count + excluded.Length)^2 + .5*excluded.Length^2)
        Node<int>? head = null;
        int excludeCount = 0;
        foreach (int i in excluded) // Θ(excluded.Length)
        {
            if (i < minValue || i >= maxValue)
            {
                continue;
            }
            Node<int>? node = head;
            Node<int>? previous = null;
            while (node is not null && node.Value <= i) // O(.5*excluded.Length), Ω(0), ε(.5*excluded.Length)
            {
                if (node.Value == i)
                {
                    goto Continue;
                }
                previous = node;
                node = node.Next;
            }
            excludeCount++;
            if (previous is null)
            {
                head = new Node<int>() { Value = i, Next = head, };
            }
            else
            {
                previous.Next = new Node<int>() { Value = i, Next = previous.Next };
            }
        Continue:
            continue;
        }
        if (maxValue - minValue - excludeCount < count)
        {
            throw new ArgumentException($"{nameof(maxValue)} - {nameof(minValue)} - {nameof(excluded)}.Length [{maxValue - minValue - excludeCount < count}] < {nameof(count)} [{count}]");
        }
        for (int i = 0; i < count; i++) // Θ(count)
        {
            int roll = random.Invoke(minValue, maxValue - i - excludeCount);
            if (roll < minValue || roll >= maxValue - i - excludeCount)
            {
                throw new ArgumentException("The Random provided returned a value outside the requested range.");
            }
            Node<int>? node = head;
            Node<int>? previous = null;
            while (node is not null && node.Value <= roll) // O(.5*(count + excluded.Length)), Ω(0), ε(.5*(count + excluded.Length))
            {
                roll++;
                previous = node;
                node = node.Next;
            }
            step.Invoke(roll);
            if (previous is null)
            {
                head = new Node<int>() { Value = roll, Next = head, };
            }
            else
            {
                previous.Next = new Node<int>() { Value = roll, Next = previous.Next };
            }
        }
#if stackalloc
			}
#endif
    }

    /// <inheritdoc cref="NextUnique{TStep, TRandom}(int, int, int, ReadOnlySpan{int}, TRandom, TStep)"/>
    public static void NextUniquePoolTracking<TStep, TRandom>(int count, int minValue, int maxValue, ReadOnlySpan<int> excluded, TRandom random = default, TStep step = default)
        where TStep : struct, IAction<int>
        where TRandom : struct, IFunc<int, int, int>
    {
        // Algorithm: Θ(range + count + 2*excluded.Length)
        SetHashLinked<int, Int32Equate, Int32Hash> set = new(expectedCount: excluded.Length); // Θ(excluded)
        foreach (int value in excluded)
        {
            if (minValue <= value && value < maxValue)
            {
                set.TryAdd(value);
            }
        }
        if (maxValue - minValue - set.Count < count)
        {
            throw new ArgumentException($"{nameof(maxValue)} - {nameof(minValue)} - {nameof(excluded)}.Length [{maxValue - minValue - set.Count < count}] < {nameof(count)} [{count}]");
        }
        if (sourceof(maxValue < minValue, out string c2)) throw new ArgumentOutOfRangeException(nameof(maxValue), c2);
        if (sourceof(count < 0, out string c3)) throw new ArgumentOutOfRangeException(nameof(count), c3);
        if (sourceof(maxValue - minValue < count, out string c4)) throw new ArgumentOutOfRangeException(nameof(count), c4);
        int pool = maxValue - minValue - set.Count;
        Span<int> span =
#if stackalloc
				pool <= 128
				?
				stackalloc int[pool]
				:
#endif
                new int[pool];
        for (int i = 0, j = minValue; i < pool; j++) // Θ(range + excluded.Length)
        {
            if (!set.Contains(j))
            {
                span[i++] = j;
            }
        }
        for (int i = 0; i < count; i++) // Θ(count)
        {
            int rollIndex = random.Invoke(0, pool);
            if (rollIndex < 0 || rollIndex >= pool)
            {
                throw new ArgumentException("The Random provided returned a value outside the requested range.");
            }
            int roll = span[rollIndex];
            span[rollIndex] = span[--pool];
            step.Invoke(roll);
        }
    }

    #region Overloads

    /// <inheritdoc cref="NextUnique{TStep, TRandom}(int, int, int, ReadOnlySpan{int}, TRandom, TStep)"></inheritdoc>
    /// <param name="step">The function to perform on each randomly generated value.</param>
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)

    public static void NextUnique<TRandom>(int count, int minValue, int maxValue, ReadOnlySpan<int> excluded, Action<int> step, TRandom random = default)
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
            where TRandom : struct, IFunc<int, int, int>
    {
        if (step is null) throw new ArgumentNullException(nameof(step));
        if (sourceof(maxValue < minValue, out string c1)) throw new ArgumentOutOfRangeException(nameof(maxValue), c1);
        if (sourceof(count < 0, out string c2)) throw new ArgumentOutOfRangeException(nameof(count), c2);
        NextUnique<SAction<int>, TRandom>(count, minValue, maxValue, excluded, random, step);
    }

    /// <inheritdoc cref="NextUnique{Random}(int, int, int, ReadOnlySpan{int}, Action{int}, Random)"></inheritdoc>
    public static void NextUniqueRollTracking<TRandom>(int count, int minValue, int maxValue, ReadOnlySpan<int> excluded, Action<int> step, TRandom random = default)
        where TRandom : struct, IFunc<int, int, int>
    {
        if (step is null) throw new ArgumentNullException(nameof(step));
        if (sourceof(maxValue < minValue, out string c1)) throw new ArgumentOutOfRangeException(nameof(maxValue), c1);
        if (sourceof(count < 0, out string c2)) throw new ArgumentOutOfRangeException(nameof(count), c2);
        NextUniqueRollTracking<SAction<int>, TRandom>(count, minValue, maxValue, excluded, random, step);
    }

    /// <inheritdoc cref="NextUnique{Random}(int, int, int, ReadOnlySpan{int}, Action{int}, Random)"></inheritdoc>
    public static void NextUniquePoolTracking<TRandom>(int count, int minValue, int maxValue, ReadOnlySpan<int> excluded, Action<int> step, TRandom random = default)
        where TRandom : struct, IFunc<int, int, int>
    {
        if (step is null) throw new ArgumentNullException(nameof(step));
        if (sourceof(maxValue < minValue, out string c1)) throw new ArgumentOutOfRangeException(nameof(maxValue), c1);
        if (sourceof(count < 0, out string c2)) throw new ArgumentOutOfRangeException(nameof(count), c2);
        NextUniquePoolTracking<SAction<int>, TRandom>(count, minValue, maxValue, excluded, random, step);
    }

    /// <inheritdoc cref="NextUnique{Step, Random}(int, int, int, ReadOnlySpan{int}, Random, Step)"/>
    /// <returns>The randomly generated values.</returns>
    public static int[] NextUnique<TRandom>(int count, int minValue, int maxValue, ReadOnlySpan<int> excluded, TRandom random = default)
        where TRandom : struct, IFunc<int, int, int>
    {
        if (sourceof(maxValue < minValue, out string c1)) throw new ArgumentOutOfRangeException(nameof(maxValue), c1);
        if (sourceof(count < 0, out string c2)) throw new ArgumentOutOfRangeException(nameof(count), c2);
        if (sourceof(maxValue - minValue < count, out string c3)) throw new ArgumentOutOfRangeException(nameof(count), c3);
        int[] values = new int[count];
        NextUnique<FillArray<int>, TRandom>(count, minValue, maxValue, excluded, random, values);
        return values;
    }

    /// <inheritdoc cref="NextUnique{Random}(int, int, int, ReadOnlySpan{int}, Random)"/>
    public static int[] NextUniqueRollTracking<TRandom>(int count, int minValue, int maxValue, ReadOnlySpan<int> excluded, TRandom random = default)
        where TRandom : struct, IFunc<int, int, int>
    {
        if (sourceof(maxValue < minValue, out string c1)) throw new ArgumentOutOfRangeException(nameof(maxValue), c1);
        if (sourceof(count < 0, out string c2)) throw new ArgumentOutOfRangeException(nameof(count), c2);
        if (sourceof(maxValue - minValue < count, out string c3)) throw new ArgumentOutOfRangeException(nameof(count), c3);
        int[] values = new int[count];
        NextUniqueRollTracking<FillArray<int>, TRandom>(count, minValue, maxValue, excluded, random, values);
        return values;
    }

    /// <inheritdoc cref="NextUnique{Random}(int, int, int, ReadOnlySpan{int}, Random)"/>
    public static int[] NextUniquePoolTracking<TRandom>(int count, int minValue, int maxValue, ReadOnlySpan<int> excluded, TRandom random = default)
        where TRandom : struct, IFunc<int, int, int>
    {
        if (sourceof(maxValue < minValue, out string c1)) throw new ArgumentOutOfRangeException(nameof(maxValue), c1);
        if (sourceof(count < 0, out string c2)) throw new ArgumentOutOfRangeException(nameof(count), c2);
        if (sourceof(maxValue - minValue < count, out string c3)) throw new ArgumentOutOfRangeException(nameof(count), c3);
        int[] values = new int[count];
        NextUniquePoolTracking<FillArray<int>, TRandom>(count, minValue, maxValue, excluded, random, values);
        return values;
    }

    #endregion Overloads

    #endregion NextUnique (with exclusions)

    #region Extensions

    /// <summary>Generates a random <see cref="bool"/> value.</summary>
    /// <param name="random">The random generation algorithm.</param>
    /// <returns>A randomly generated <see cref="bool"/> value.</returns>
    public static bool NextBool(this Random random) => random.Next(2) is 0;

    /// <summary>Generates a random <see cref="byte"/> value.</summary>
    /// <param name="random">The random generation algorithm.</param>
    /// <returns>A randomly generated <see cref="byte"/> value.</returns>
    public static byte NextByte(this Random random) => (byte)random.Next(byte.MinValue, byte.MaxValue);

    /// <summary>Generates a random <see cref="string"/> of a given length using the System.Random generator.</summary>
    /// <param name="random">The random generation algorithm.</param>
    /// <param name="length">The length of the randomized <see cref="string"/> to generate.</param>
    /// <returns>The generated randomized <see cref="string"/>.</returns>
    public static string NextString(this Random random, int length)
    {
        if (random is null) throw new ArgumentNullException(nameof(random));
        if (sourceof(length < 0, out string c)) throw new ArgumentOutOfRangeException(nameof(length), c);
        char[] randomstring = new char[length];
        for (int i = 0; i < randomstring.Length; i++)
        {
            randomstring[i] = (char)random.Next(char.MinValue, char.MaxValue);
        }
        return new string(randomstring);
    }

    /// <summary>Generates a random <see cref="string"/> of a given length using the System.Random generator with a specific set of characters.</summary>
    /// <param name="random">The random generation algorithm.</param>
    /// <param name="length">The length of the randomized <see cref="string"/> to generate.</param>
    /// <param name="characterPool">The set of allowable characters.</param>
    /// <returns>The generated randomized <see cref="string"/>.</returns>
    public static string NextString(this Random random, int length, char[] characterPool)
    {
        if (random is null) throw new ArgumentNullException(nameof(random));
        if (characterPool is null) throw new ArgumentNullException(nameof(characterPool));
        if (length < 1)
        {
            throw new ArgumentException("(" + nameof(length) + " < 1)");
        }
        if (characterPool.Length < 1)
        {
            throw new ArgumentException("(" + nameof(characterPool) + "." + nameof(characterPool.Length) + " < 1)");
        }
        char[] randomstring = new char[length];
        for (int i = 0; i < randomstring.Length; i++)
        {
            randomstring[i] = random.From(characterPool);
        }
        return new string(randomstring);
    }

    internal const string UpperCaseEnglishCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    internal const string LowerCaseEnglishCharacters = "abcdefghijklmnopqrstuvwxyz";
    internal const string EnglishDigits = "0123456789";

    /// <summary>Generates a random English alphanumeric <see cref="string"/> of a given length (includes upper and lower case characters).</summary>
    /// <param name="random">The random generation algorithm.</param>
    /// <param name="length">The length of the randomized alphanumeric <see cref="string"/> to generate.</param>
    /// <returns>The generated randomized alphanumeric <see cref="string"/>.</returns>
    public static string NextEnglishAlphaNumericString(this Random random, int length) =>
        NextString(random, length, (UpperCaseEnglishCharacters + LowerCaseEnglishCharacters + EnglishDigits).ToCharArray());

    /// <summary>Generates a random English alphanumeric <see cref="string"/> of a given length (upper case characters only).</summary>
    /// <param name="random">The random generation algorithm.</param>
    /// <param name="length">The length of the randomized alphanumeric <see cref="string"/> to generate.</param>
    /// <returns>The generated randomized alphanumeric <see cref="string"/>.</returns>
    public static string NextUpperCaseEnglishAlphaNumericString(this Random random, int length) =>
        NextString(random, length, (UpperCaseEnglishCharacters + EnglishDigits).ToCharArray());

    /// <summary>Generates a random English alphanumeric <see cref="string"/> of a given length (lower case characters only).</summary>
    /// <param name="random">The random generation algorithm.</param>
    /// <param name="length">The length of the randomized alphanumeric <see cref="string"/> to generate.</param>
    /// <returns>The generated randomized alphanumeric <see cref="string"/>.</returns>
    public static string NextLowerCaseEnglishAlphaNumericString(this Random random, int length) =>
        NextString(random, length, (LowerCaseEnglishCharacters + EnglishDigits).ToCharArray());

    /// <summary>Generates a random English numeric <see cref="string"/> of a given length.</summary>
    /// <param name="random">The random generation algorithm.</param>
    /// <param name="length">The length of the randomized numeric <see cref="string"/> to generate.</param>
    /// <returns>The generated randomized numeric <see cref="string"/>.</returns>
    public static string NumericEnglishString(this Random random, int length) =>
        NextString(random, length, EnglishDigits.ToCharArray());

    /// <summary>Generates a random English alhpabetical <see cref="string"/> of a given length (includes upper and lower case characters).</summary>
    /// <param name="random">The random generation algorithm.</param>
    /// <param name="length">The length of the randomized alphabetical <see cref="string"/> to generate.</param>
    /// <returns>The generated randomized alphabetical <see cref="string"/>.</returns>
    public static string NextEnglishAlphabeticString(this Random random, int length) =>
        NextString(random, length, (UpperCaseEnglishCharacters + LowerCaseEnglishCharacters).ToCharArray());

    /// <summary>Generates a random English alhpabetical <see cref="string"/> of a given length (upper case characters only).</summary>
    /// <param name="random">The random generation algorithm.</param>
    /// <param name="length">The length of the randomized alphabetical <see cref="string"/> to generate.</param>
    /// <returns>The generated randomized alphabetical <see cref="string"/>.</returns>
    public static string NextUpperCaseEnglishAlphabeticString(this Random random, int length) =>
        NextString(random, length, UpperCaseEnglishCharacters.ToCharArray());

    /// <summary>Generates a random English alhpabetical <see cref="string"/> of a given length (lower case characters only).</summary>
    /// <param name="random">The random generation algorithm.</param>
    /// <param name="length">The length of the randomized alphabetical <see cref="string"/> to generate.</param>
    /// <returns>The generated randomized alphabetical <see cref="string"/>.</returns>
    public static string NextLowerCaseEnglishAlphabeticString(this Random random, int length) =>
        NextString(random, length, LowerCaseEnglishCharacters.ToCharArray());

    /// <summary>Generates a random char value.</summary>
    /// <param name="random">The random generation algorithm.</param>
    /// <returns>A randomly generated char value.</returns>
    public static char NextChar(this Random random) =>
        NextChar(random, char.MinValue, char.MaxValue);

    /// <summary>Generates a random char value.</summary>
    /// <param name="random">The random generation algorithm.</param>
    /// <param name="min">Minimum allowed value of the random generation.</param>
    /// <param name="max">Maximum allowed value of the random generation.</param>
    /// <returns>A randomly generated char value.</returns>
    public static char NextChar(this Random random, char min, char max) =>
        (char)random.Next(min, max);

    /// <summary>Generates a random <see cref="decimal"/> value.</summary>
    /// <param name="random">The random generation algorithm.</param>
    /// <returns>A randomly generated <see cref="decimal"/> value.</returns>
    public static decimal NextDecimal(this Random random) => new(
        random.Next(int.MinValue, int.MaxValue),
        random.Next(int.MinValue, int.MaxValue),
        random.Next(int.MinValue, int.MaxValue),
        random.NextBool(),
        (byte)random.Next(29));

    /// <summary>Generates a random <see cref="decimal"/> value.</summary>
    /// <param name="random">The random generation algorithm.</param>
    /// <param name="min">The minimum allowed value of the random generation.</param>
    /// <param name="max">The maximum allowed value of the random generation.</param>
    /// <returns>A randomly generated <see cref="decimal"/> value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Called if the range of random generation is too large and therefore invalid.</exception>
    public static decimal NextDecimal(this Random random, decimal min, decimal max)
    {
        try
        {
            decimal next = NextDecimal(random);
            return checked((next < 0 ? -next : next) % (max - min) + min);
        }
        catch (OverflowException exception)
        {
            throw new ArgumentOutOfRangeException("The bounds of random generation were to large (min...max).", exception);
        }
    }

    /// <summary>Generates a random DateTime value.</summary>
    /// <param name="random">The random generation algorithm.</param>
    /// <returns>A randomly generated DateTime value.</returns>
    public static DateTime NextDateTime(this Random random) =>
        NextDateTime(random, DateTime.MaxValue);

    /// <summary>Generates a random DateTime value.</summary>
    /// <param name="random">The random generation algorithm.</param>
    /// <param name="max">The maximum allowed value of the random generation.</param>
    /// <returns>A randomly generated DateTime value.</returns>
    public static DateTime NextDateTime(this Random random, DateTime max) =>
        NextDateTime(random, DateTime.MinValue, max);

    /// <summary>Generates a random DateTime value.</summary>
    /// <param name="random">The random generation algorithm.</param>
    /// <param name="min">The minimum allowed value of the random generation.</param>
    /// <param name="max">The maximum allowed value of the random generation.</param>
    /// <returns>A randomly generated DateTime value.</returns>
    public static DateTime NextDateTime(this Random random, DateTime min, DateTime max)
    {
        if (random is null) throw new ArgumentNullException(nameof(random));
        if (sourceof(min > max, out string c)) throw new ArgumentException(c);
        TimeSpan randomTimeSpan = NextTimeSpan(random, TimeSpan.Zero, max - min);
        return min.Add(randomTimeSpan);
    }

    /// <summary>Generates a random TimeSpan value.</summary>
    /// <param name="random">The random generation algorithm.</param>
    /// <returns>A randomly generated TimeSpan value.</returns>
    public static TimeSpan NextTimeSpan(this Random random) =>
        NextTimeSpan(random, TimeSpan.MaxValue);

    /// <summary>Generates a random TimeSpan value.</summary>
    /// <param name="random">The random generation algorithm.</param>
    /// <param name="max">The maximum allowed value of the random generation.</param>
    /// <returns>A randomly generated TimeSpan value.</returns>
    public static TimeSpan NextTimeSpan(this Random random, TimeSpan max) =>
        NextTimeSpan(random, TimeSpan.Zero, max);

    /// <summary>Generates a random TimeSpan value.</summary>
    /// <param name="random">The random generation algorithm.</param>
    /// <param name="min">The minimum allowed value of the random generation.</param>
    /// <param name="max">The maximum allowed value of the random generation.</param>
    /// <returns>A randomly generated TimeSpan value.</returns>
    public static TimeSpan NextTimeSpan(this Random random, TimeSpan min, TimeSpan max)
    {
        if (random is null) throw new ArgumentNullException(nameof(random));
        if (sourceof(min > max, out string c)) throw new ArgumentException(c);
        long tickRange = max.Ticks - min.Ticks;
        long randomLong = random.NextInt64(0, tickRange);
        return TimeSpan.FromTicks(min.Ticks + randomLong);
    }

    #region Next (with exclusions)

    /// <inheritdoc cref="Next{Random}(int, int, int, ReadOnlySpan{int}, Action{int}, Random)"/>
    public static void Next(this Random random, int count, int minValue, int maxValue, ReadOnlySpan<int> excluded, Action<int> step) =>
        Next<RandomNextIntMinValueIntMaxValue>(count, minValue, maxValue, excluded, step, random ?? new Random());

    /// <inheritdoc cref="Next(Random, int, int, int, ReadOnlySpan{int}, Action{int})"/>
    public static void NextRollTracking(this Random random, int count, int minValue, int maxValue, ReadOnlySpan<int> excluded, Action<int> step) =>
        NextRollTracking<RandomNextIntMinValueIntMaxValue>(count, minValue, maxValue, excluded, step, random ?? new Random());

    /// <inheritdoc cref="Next(Random, int, int, int, ReadOnlySpan{int}, Action{int})"/>
    public static void NextPoolTracking(this Random random, int count, int minValue, int maxValue, ReadOnlySpan<int> excluded, Action<int> step) =>
        NextPoolTracking<RandomNextIntMinValueIntMaxValue>(count, minValue, maxValue, excluded, step, random ?? new Random());

    /// <inheritdoc cref="Next{Random}(int, int, int, ReadOnlySpan{int}, Random)"/>
    public static int[] Next(this Random random, int count, int minValue, int maxValue, ReadOnlySpan<int> excluded) =>
        Next<RandomNextIntMinValueIntMaxValue>(count, minValue, maxValue, excluded, random ?? new Random());

    /// <inheritdoc cref="Next(Random, int, int, int, ReadOnlySpan{int})"/>
    public static int[] NextRollTracking(this Random random, int count, int minValue, int maxValue, ReadOnlySpan<int> excluded) =>
        NextRollTracking<RandomNextIntMinValueIntMaxValue>(count, minValue, maxValue, excluded, random ?? new Random());

    /// <inheritdoc cref="Next(Random, int, int, int, ReadOnlySpan{int})"/>
    public static int[] NextPoolTracking(this Random random, int count, int minValue, int maxValue, ReadOnlySpan<int> excluded) =>
        NextPoolTracking<RandomNextIntMinValueIntMaxValue>(count, minValue, maxValue, excluded, random ?? new Random());

    /// <inheritdoc cref="Next{Step, Random}(int, int, int, ReadOnlySpan{int}, Random, Step)"/>
    public static void Next<TStep>(this Random random, int count, int minValue, int maxValue, ReadOnlySpan<int> excluded, TStep step = default)
        where TStep : struct, IAction<int> =>
        Next<TStep, RandomNextIntMinValueIntMaxValue>(count, minValue, maxValue, excluded, random ?? new Random(), step);

    /// <inheritdoc cref="Next{Step}(Random, int, int, int, ReadOnlySpan{int}, Step)"/>
    public static void NextRollTracking<TStep>(this Random random, int count, int minValue, int maxValue, ReadOnlySpan<int> excluded, TStep step = default)
        where TStep : struct, IAction<int> =>
        NextRollTracking<TStep, RandomNextIntMinValueIntMaxValue>(count, minValue, maxValue, excluded, random ?? new Random(), step);

    /// <inheritdoc cref="Next{Step}(Random, int, int, int, ReadOnlySpan{int}, Step)"/>
    public static void NextPoolTracking<TStep>(this Random random, int count, int minValue, int maxValue, ReadOnlySpan<int> excluded, TStep step = default)
        where TStep : struct, IAction<int> =>
        NextPoolTracking<TStep, RandomNextIntMinValueIntMaxValue>(count, minValue, maxValue, excluded, random ?? new Random(), step);

    #endregion Next (with exclusions)

    #region NextUnique

    /// <inheritdoc cref="NextUnique{Random}(int, int, int, Action{int}, Random)"/>
    public static void NextUnique(this Random random, int count, int minValue, int maxValue, Action<int> step) =>
        NextUnique<RandomNextIntMinValueIntMaxValue>(count, minValue, maxValue, step, random ?? new Random());

    /// <inheritdoc cref="NextUnique(Random, int, int, int, Action{int})"/>
    public static void NextUniqueRollTracking(this Random random, int count, int minValue, int maxValue, Action<int> step) =>
        NextUniqueRollTracking<RandomNextIntMinValueIntMaxValue>(count, minValue, maxValue, step, random ?? new Random());

    /// <inheritdoc cref="NextUnique(Random, int, int, int, Action{int})"/>
    public static void NextUniquePoolTracking(this Random random, int count, int minValue, int maxValue, Action<int> step) =>
        NextUniquePoolTracking<RandomNextIntMinValueIntMaxValue>(count, minValue, maxValue, step, random ?? new Random());

    /// <inheritdoc cref="NextUnique{Random}(int, int, int, Random)"/>
    public static int[] NextUnique(this Random random, int count, int minValue, int maxValue) =>
        NextUnique<RandomNextIntMinValueIntMaxValue>(count, minValue, maxValue, random ?? new Random());

    /// <inheritdoc cref="NextUnique(Random, int, int, int)"/>
    public static int[] NextUniqueRollTracking(this Random random, int count, int minValue, int maxValue) =>
        NextUniqueRollTracking<RandomNextIntMinValueIntMaxValue>(count, minValue, maxValue, random ?? new Random());

    /// <inheritdoc cref="NextUnique(Random, int, int, int)"/>
    public static int[] NextUniquePoolTracking(this Random random, int count, int minValue, int maxValue) =>
        NextUniquePoolTracking<RandomNextIntMinValueIntMaxValue>(count, minValue, maxValue, random ?? new Random());

    /// <inheritdoc cref="NextUnique{Step, Random}(int, int, int, Random, Step)"/>
    public static void NextUnique<TStep>(this Random random, int count, int minValue, int maxValue, TStep step = default)
        where TStep : struct, IAction<int> =>
        NextUnique<TStep, RandomNextIntMinValueIntMaxValue>(count, minValue, maxValue, random ?? new Random(), step);

    /// <inheritdoc cref="NextUnique{Step}(Random, int, int, int, Step)"/>
    public static void NextUniqueRollTracking<TStep>(this Random random, int count, int minValue, int maxValue, TStep step = default)
        where TStep : struct, IAction<int> =>
        NextUniqueRollTracking<TStep, RandomNextIntMinValueIntMaxValue>(count, minValue, maxValue, random ?? new Random(), step);

    /// <inheritdoc cref="NextUnique{Step}(Random, int, int, int, Step)"/>
    public static void NextUniquePoolTracking<TStep>(this Random random, int count, int minValue, int maxValue, TStep step = default)
        where TStep : struct, IAction<int> =>
        NextUniquePoolTracking<TStep, RandomNextIntMinValueIntMaxValue>(count, minValue, maxValue, random ?? new Random(), step);

    #endregion NextUnique

    #region NextUnique (with exclusions)

    /// <inheritdoc cref="NextUnique{Random}(int, int, int, ReadOnlySpan{int}, Action{int}, Random)"/>
    public static void NextUnique(this Random random, int count, int minValue, int maxValue, ReadOnlySpan<int> excluded, Action<int> step) =>
        NextUnique<RandomNextIntMinValueIntMaxValue>(count, minValue, maxValue, excluded, step, random ?? new Random());

    /// <inheritdoc cref="NextUnique(Random, int, int, int, ReadOnlySpan{int}, Action{int})"/>
    public static void NextUniqueRollTracking(this Random random, int count, int minValue, int maxValue, ReadOnlySpan<int> excluded, Action<int> step) =>
        NextUniqueRollTracking<RandomNextIntMinValueIntMaxValue>(count, minValue, maxValue, excluded, step, random ?? new Random());

    /// <inheritdoc cref="NextUnique(Random, int, int, int, ReadOnlySpan{int}, Action{int})"/>
    public static void NextUniquePoolTracking(this Random random, int count, int minValue, int maxValue, ReadOnlySpan<int> excluded, Action<int> step) =>
        NextUniquePoolTracking<RandomNextIntMinValueIntMaxValue>(count, minValue, maxValue, excluded, step, random ?? new Random());

    /// <inheritdoc cref="NextUnique{Random}(int, int, int, ReadOnlySpan{int}, Random)"/>
    public static int[] NextUnique(this Random random, int count, int minValue, int maxValue, ReadOnlySpan<int> excluded) =>
        NextUnique<RandomNextIntMinValueIntMaxValue>(count, minValue, maxValue, excluded, random ?? new Random());

    /// <inheritdoc cref="NextUnique(Random, int, int, int, ReadOnlySpan{int})"/>
    public static int[] NextUniqueRollTracking(this Random random, int count, int minValue, int maxValue, ReadOnlySpan<int> excluded) =>
        NextUniqueRollTracking<RandomNextIntMinValueIntMaxValue>(count, minValue, maxValue, excluded, random ?? new Random());

    /// <inheritdoc cref="NextUnique(Random, int, int, int, ReadOnlySpan{int})"/>
    public static int[] NextUniquePoolTracking(this Random random, int count, int minValue, int maxValue, ReadOnlySpan<int> excluded) =>
        NextUniquePoolTracking<RandomNextIntMinValueIntMaxValue>(count, minValue, maxValue, excluded, random ?? new Random());

    /// <inheritdoc cref="NextUnique{Step, Random}(int, int, int, ReadOnlySpan{int}, Random, Step)"/>
    public static void NextUnique<TStep>(this Random random, int count, int minValue, int maxValue, ReadOnlySpan<int> excluded, TStep step = default)
        where TStep : struct, IAction<int> =>
        NextUnique<TStep, RandomNextIntMinValueIntMaxValue>(count, minValue, maxValue, excluded, random ?? new Random(), step);

    /// <inheritdoc cref="NextUnique{Step}(Random, int, int, int, ReadOnlySpan{int}, Step)"/>
    public static void NextUniqueRollTracking<TStep>(this Random random, int count, int minValue, int maxValue, ReadOnlySpan<int> excluded, TStep step = default)
        where TStep : struct, IAction<int> =>
        NextUniqueRollTracking<TStep, RandomNextIntMinValueIntMaxValue>(count, minValue, maxValue, excluded, random ?? new Random(), step);

    /// <inheritdoc cref="NextUnique{Step}(Random, int, int, int, ReadOnlySpan{int}, Step)"/>
    public static void NextUniquePoolTracking<TStep>(this Random random, int count, int minValue, int maxValue, ReadOnlySpan<int> excluded, TStep step = default)
        where TStep : struct, IAction<int> =>
        NextUniquePoolTracking<TStep, RandomNextIntMinValueIntMaxValue>(count, minValue, maxValue, excluded, random ?? new Random(), step);

    #endregion NextUnique (with exclusions)

    /// <summary>Chooses a value at random from <paramref name="values"/>.</summary>
    /// <typeparam name="T">The generic type of the items to choose from.</typeparam>
    /// <param name="random">The random algorithm for index generation.</param>
    /// <param name="values">The values to choose from.</param>
    /// <returns>A randomly selected value from the supplied options.</returns>
    public static T From<T>(this Random random, params T[] values)
    {
        if (values is null) throw new ArgumentNullException(nameof(values));
        if (sourceof(values.Length < 1, out string c)) throw new ArgumentException(c, nameof(values));
        return values[random.Next(values.Length)];
    }

    /// <inheritdoc cref="From{T}(Random, T[])"/>
    public static T From<T>(this Random random, ReadOnlySpan<T> values)
    {
        if (sourceof(values.IsEmpty, out string c)) throw new ArgumentException(c, nameof(values));
        return values[random.Next(values.Length)];
    }

    /// <inheritdoc cref="From{T}(Random, T[])"/>
    public static T From<T>(this Random random, System.Collections.Generic.IList<T> values)
    {
        if (values is null) throw new ArgumentNullException(nameof(values));
        if (sourceof(values.Count < 1, out string c)) throw new ArgumentException(c);
        return values[random.Next(values.Count)];
    }

    /// <summary>Selects a random value from a collection of weighted options.</summary>
    /// <typeparam name="T">The generic type to select a random instance of.</typeparam>
    /// <param name="random">The random algorithm.</param>
    /// <param name="pool">The pool of weighted values to choose from.</param>
    /// <returns>A randomly selected value from the weighted pool.</returns>
    public static T Next<T>(this Random random, params (T Value, double Weight)[] pool) =>
        random.Next((System.Collections.Generic.IEnumerable<(T Value, double Weight)>)pool);

    /// <summary>Selects a random value from a collection of weighted options.</summary>
    /// <typeparam name="T">The generic type to select a random instance of.</typeparam>
    /// <param name="random">The random algorithm.</param>
    /// <param name="totalWeight">The total weight of all the values in the pool.</param>
    /// <param name="pool">The pool of weighted values to choose from.</param>
    /// <returns>A randomly selected value from the weighted pool.</returns>
    public static T Next<T>(this Random random, double totalWeight, params (T Value, double Weight)[] pool) =>
        random.Next(pool, totalWeight);

    /// <summary>Selects a random value from a collection of weighted options.</summary>
    /// <typeparam name="T">The generic type to select a random instance of.</typeparam>
    /// <param name="random">The random algorithm.</param>
    /// <param name="pool">The pool of weighted values to choose from.</param>
    /// <param name="totalWeight">The total weight of all the values in the pool.</param>
    /// <returns>A randomly selected value from the weighted pool.</returns>
    public static T Next<T>(this Random random, System.Collections.Generic.IEnumerable<(T Value, double Weight)> pool, double? totalWeight = null)
    {
        if (!totalWeight.HasValue)
        {
            totalWeight = 0;
            foreach (var (Value, Weight) in pool)
            {
                if (Weight < 0)
                    throw new ArgumentOutOfRangeException(nameof(pool), $"A value in {nameof(pool)} had a weight less than zero.");
                totalWeight += Weight;
            }
        }
        else
        {
            if (totalWeight < 0)
                throw new ArgumentOutOfRangeException(nameof(totalWeight), $"The provided {nameof(totalWeight)} of the {nameof(pool)} was less than zero.");
        }
        if (totalWeight is 0)
            throw new ArgumentOutOfRangeException(nameof(pool), $"The total weight of all values in the {nameof(pool)} was zero.");
        if (double.IsInfinity(totalWeight.Value))
            throw new ArgumentOutOfRangeException(nameof(pool), $"The total weight of all values in the {nameof(pool)} was an infinite double.");
        double randomDouble = random.NextDouble();
        double range = 0;
        foreach (var (Value, Weight) in pool)
        {
            range += Weight;
            if (range > totalWeight)
                throw new ArgumentOutOfRangeException(nameof(totalWeight), $"The provided {nameof(totalWeight)} of the {nameof(pool)} was less than the actual total weight.");
            if (randomDouble < range / totalWeight)
                return Value;
        }
        throw new TowelBugException("There is a bug in the code.");
    }

#if false

		//public static T Next<T>(this Random random, StepperBreak<(T Value, double Weight)> pool, double? totalWeight = null)
		//{
		//	if (!totalWeight.HasValue)
		//	{
		//		totalWeight = 0;
		//		pool(x =>
		//		{
		//			if (x.Weight < 0)
		//				throw new ArgumentOutOfRangeException("A value in the pool had a weight less than zero.");
		//			totalWeight += x.Weight;
		//			return Continue;
		//		});
		//	}
		//	else
		//	{
		//		if (totalWeight < 0)
		//			throw new ArgumentOutOfRangeException("The provided total weight of the pool was less than zero.");
		//	}
		//	if (totalWeight is 0)
		//		throw new ArgumentOutOfRangeException("The total weight of all values in the pool was zero.");
		//	if (double.IsInfinity(totalWeight.Value))
		//		throw new ArgumentOutOfRangeException("The total weight of all values in the pool was an infinite double.");
		//	double randomDouble = random.NextDouble();
		//	double range = 0;
		//	T @return = default;
		//	StepStatus status = pool(x =>
		//	{
		//		range += x.Weight;
		//		if (range > totalWeight)
		//			throw new ArgumentOutOfRangeException("The provided total weight of the pool was less than the actual total weight.");
		//		if (randomDouble < range / totalWeight)
		//		{
		//			@return = x.Value;
		//			return Break;
		//		}
		//		return Continue;
		//	});
		//	return status is Break
		//		? @return
		//		: throw new TowelBugException("There is a bug in the code.");
		//}

#endif

    #endregion Extensions
}