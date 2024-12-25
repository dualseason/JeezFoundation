﻿namespace Towel_Benchmarking;

[Tag(Program.Name, "Random With Exclusions")]
[Tag(Program.OutputFile, nameof(RandomWithExclusionsBenchmarks))]
public class RandomWithExclusionsBenchmarks
{
    [Params(0)]
    public int MinValue;

    [Params(10, 100, 1000, 10000, 100000, 1000000)]
    public int MaxValue;

    [Params("1: sqrt(sqrt(range))", "2: .5*sqrt(range)", "3: sqrt(range)", "4: 2*sqrt(range)")]
    public string? Count;

    [Params("1: sqrt(sqrt(range))", "2: .5*sqrt(range)", "3: sqrt(range)", "4: 2*sqrt(range)")]
    public string? Exclued;

    internal Random? Random;
    internal int[]? Excludes;
    internal int CountInt;
    internal int[]? temp;

    [IterationSetup]
    public void IterationSetup()
    {
        Random = new Random(7);
        int excludeCount = Exclued switch
        {
            "1: sqrt(sqrt(range))" => (int)Math.Sqrt(Math.Sqrt(MaxValue - MinValue)),
            "2: .5*sqrt(range)" => (int)(.5 * Math.Sqrt(MaxValue - MinValue)),
            "3: sqrt(range)" => (int)Math.Sqrt(MaxValue - MinValue),
            "4: 2*sqrt(range)" => (int)(2 * Math.Sqrt(MaxValue - MinValue)),
            _ => throw new NotImplementedException(),
        };
        Excludes = Random.NextUnique(excludeCount, MinValue, MaxValue);
        CountInt = Count switch
        {
            "1: sqrt(sqrt(range))" => (int)Math.Sqrt(Math.Sqrt(MaxValue - MinValue)),
            "2: .5*sqrt(range)" => (int)(.5 * Math.Sqrt(MaxValue - MinValue)),
            "3: sqrt(range)" => (int)Math.Sqrt(MaxValue - MinValue),
            "4: 2*sqrt(range)" => (int)(2 * Math.Sqrt(MaxValue - MinValue)),
            _ => throw new NotImplementedException(),
        };
    }

    [IterationCleanup]
    public void IterationCleanUp()
    {
        // use "temp" to prevent some optimizations... still need to validate IL though
        Console.WriteLine(temp!.Sum());
    }

    [Benchmark]
    public void Towel()
    {
        int[] result = Random!.Next(CountInt, MinValue, MaxValue, Excludes);
        temp = result;
    }

    [Benchmark]
    public void HashSetAndArray()
    {
        System.Collections.Generic.HashSet<int> excluded = new(Excludes!.Length);
        foreach (int exclude in Excludes)
        {
            if (MinValue <= exclude && exclude < MaxValue)
            {
                excluded.Add(exclude);
            }
        }
        int[] pool = new int[MaxValue - MinValue - excluded.Count];
        for (int value = MinValue, i = 0; value < MaxValue; value++)
        {
            if (!excluded.Contains(value))
            {
                pool[i++] = value;
            }
        }
        int[] result = new int[CountInt];
        for (int i = 0; i < CountInt; i++)
        {
            int index = Random!.Next(0, pool.Length);
            result[i] = pool[index];
        }
        temp = result;
    }

    [Benchmark]
    public void SetHashLinkedAndArray()
    {
        JeezFoundation.Algorithm.DataStructures.SetHashLinked<int, Int32Equate, Int32Hash> excluded = new(default, default, Excludes!.Length);
        foreach (int exclude in Excludes)
        {
            if (MinValue <= exclude && exclude < MaxValue)
            {
                excluded.TryAdd(exclude);
            }
        }
        int[] pool = new int[MaxValue - MinValue - excluded.Count];
        for (int value = MinValue, i = 0; value < MaxValue; value++)
        {
            if (!excluded.Contains(value))
            {
                pool[i++] = value;
            }
        }
        int[] result = new int[CountInt];
        for (int i = 0; i < CountInt; i++)
        {
            int index = Random!.Next(0, pool.Length);
            result[i] = pool[index];
        }
        temp = result;
    }

#if false

		[Benchmark]
		public void SetHashArrayAndArray()
		{
			JeezFoundation.Algorithm.DataStructures.SetHashArray<int, IntEquate, IntHash> excluded = new(default, default, Excludes.Length);
			foreach (int exclude in Excludes)
			{
				if (MinValue <= exclude && exclude < MaxValue)
				{
					excluded.Add(exclude);
				}
			}
			int[] pool = new int[MaxValue - MinValue - excluded.Count];
			for (int value = MinValue, i = 0; value < MaxValue; value++)
			{
				if (!excluded.Contains(value))
				{
					pool[i++] = value;
				}
			}
			int[] result = new int[Count];
			for (int i = 0; i < Count; i++)
			{
				int index = Random.Next(0, pool.Length);
				result[i] = pool[index];
			}
			temp = result;
		}

#endif

    [Benchmark]
    public void RelativelySimpleCode()
    {
        if (MaxValue > 1000)
        {
            throw new Exception("Slow as balls...");
        }
        System.Collections.Generic.HashSet<int> exclude = new(Excludes!);
        System.Collections.Generic.IEnumerable<int> range = Enumerable.Range(MinValue, MaxValue).Where(i => !exclude.Contains(i));
        int[] result = Enumerable.Range(0, CountInt).Select(i => range.ElementAt(Random!.Next(range.Count()))).ToArray();
        temp = result;
    }
}