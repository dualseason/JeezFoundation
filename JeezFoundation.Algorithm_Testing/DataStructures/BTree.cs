namespace JeezFoundation.Algorithm_Testing.DataStructures;

[TestClass]
public class BTree_Testing
{
	[TestMethod]
	public void StepperTest()
	{
		BTreeLinked<int, SFunc<int, int, CompareResult>>? tree = BTreeLinked.New<int>(8);
		tree.Stepper(i => Assert.Fail());
	}

	[TestMethod]
	public void EnumeratorTest()
	{
		BTreeLinked<int, SFunc<int, int, CompareResult>>? tree = BTreeLinked.New<int>(8);
		foreach (int _ in tree)
		{
			Assert.Fail();
		}
	}

	[TestMethod]
	public void ClearTest()
	{
		BTreeLinked<int, SFunc<int, int, CompareResult>>? tree = BTreeLinked.New<int>(8);
		for (int i = 0; i < 10000; i++)
		{
			tree.TryAdd(i);
		}
		tree.Clear();
		Assert.IsTrue(tree.Count is 0);
		int[] arr = new int[100];
		for (int i = 0; i < 100; i++)
		{
			arr[i] = 300 - i;
			tree.TryAdd(arr[i]);
		}
		Array.Sort(arr);
		CollectionAssert.AreEqual(arr, tree.ToArray());
	}

	[TestMethod]
	public void AddTest()
	{
		BTreeLinked<int, SFunc<int, int, CompareResult>>? tree = BTreeLinked.New<int>(8);
		Assert.IsTrue(tree._root.IsLeaf);
		Assert.IsTrue(tree.Count is 0);
		for (int i = 100; i > 0; i--)
		{
			tree.TryAdd(i);
		}
		int[] exp = new int[100];
		for (int i = 1; i <= 100; i++)
		{
			exp[i - 1] = i;
		}
		int[] act = tree.ToArray();
		CollectionAssert.AreEqual(exp, act);
	}

	[TestMethod]
	public void FalseAddTest_1()
	{
		BTreeLinked<int, SFunc<int, int, CompareResult>>? tree = BTreeLinked.New<int>(6);
		Assert.IsTrue(tree.TryAdd(1).Success);
		Assert.IsTrue(tree.TryAdd(2).Success);
		Assert.IsTrue(tree.TryAdd(3).Success);
		Assert.IsFalse(tree.TryAdd(3).Success);
		Assert.IsFalse(tree.TryAdd(2).Success);
	}

	[TestMethod]
	public void FalseAddTest_2()
	{
		BTreeLinked<int, SFunc<int, int, CompareResult>>? tree = BTreeLinked.New<int>(4);
		for (int i = 10; i < 1000; i++)
		{
			Assert.IsTrue(tree.TryAdd(i).Success);
		}
		Assert.IsTrue(tree.TryAdd(1).Success);
		Assert.IsTrue(tree.TryAdd(2).Success);
		Assert.IsTrue(tree.TryAdd(3).Success);
		Assert.IsFalse(tree.TryAdd(3).Success);
		Assert.IsFalse(tree.TryAdd(2).Success);
	}

	[TestMethod]
	public void FalseRemoveTest_1()
	{
		BTreeLinked<int, SFunc<int, int, CompareResult>>? tree = BTreeLinked.New<int>(6);
		Assert.IsTrue(tree.TryAdd(1).Success);
		Assert.IsTrue(tree.TryAdd(2).Success);
		Assert.IsTrue(tree.TryAdd(3).Success);
		Assert.IsFalse(tree.TryAdd(3).Success);
		Assert.IsFalse(tree.TryAdd(2).Success);
		Assert.IsTrue(tree.TryRemove(3).Success);
	}

	[TestMethod]
	public void FalseRemoveTest_2()
	{
		BTreeLinked<int, SFunc<int, int, CompareResult>>? tree = BTreeLinked.New<int>(6);
		for (int i = 10; i < 2000; i++)
		{
			Assert.IsTrue(tree.TryAdd(i).Success);
		}
		for (int i = 0; i < 10; i++)
		{
			Assert.IsFalse(tree.TryRemove(i).Success);
		}
	}

	[TestMethod]
	public void Add_SearchTest()
	{
		BTreeLinked<int, SFunc<int, int, CompareResult>>? tree = BTreeLinked.New<int>(10);
		for (int i = 0; i < 1000; i++)
		{
			Assert.IsTrue(tree.TryAdd(i * 3).Success);
		}
		for (int i = 0; i < 3000; i += 3)
		{
			Assert.IsTrue(tree.Contains(i));
			Assert.IsFalse(tree.Contains(i + 1));
			Assert.IsFalse(tree.Contains(i + 2));
		}
	}

	[TestMethod]
	public void Add_Remove_SearchTest()
	{
		BTreeLinked<int, SFunc<int, int, CompareResult>>? tree = BTreeLinked.New<int>(10);
		Random r = new();
		const int size = 1000 * 3;
		int[] arr = new int[size];
		for (int i = 0, j; i < size; i++)
		{
			do
			{
				j = r.Next(size * size);
			} while (Array.IndexOf(arr, j, 0, i) >= 0);
			arr[i] = j;
		}
		for (int i = 0; i < size; i++)
		{
			Assert.IsTrue(tree.TryAdd(arr[i]).Success);
		}
		for (int i = 0; i < size; i += 3)
		{
			Assert.IsTrue(tree.TryRemove(arr[i + 1]).Success);
			Assert.IsTrue(tree.TryRemove(arr[i + 2]).Success);
		}
		for (int i = 0; i < size; i += 3)
		{
			Assert.IsTrue(tree.Contains(arr[i]));
			Assert.IsFalse(tree.Contains(arr[i + 1]));
			Assert.IsFalse(tree.Contains(arr[i + 2]));
		}
	}

	[TestMethod]
	public void RemoveAll_1()
	{
		Random r = new();
		const int size = 14;
		const int max = size * size;
		BTreeLinked<int, SFunc<int, int, CompareResult>>? tree = BTreeLinked.New<int>(4);
		int[] arr = new int[size];
		for (int i = 0, j; i < size; i++)
		{
			do
			{
				j = max + r.Next(max);
			} while (Array.IndexOf(arr, j, 0, i) >= 0);
			arr[i] = j;
		}
		for (int i = 0; i < size; i++)
		{
			Assert.IsTrue(tree.TryAdd(arr[i]).Success);
		}
		for (int i = 0, j; i < size; i++)
		{
			Assert.IsTrue(tree.TryRemove(arr[i]).Success);
			do
			{
				j = r.Next(max);
			} while (Array.IndexOf(arr, j, 0, i) >= 0);
		}
		Assert.IsTrue(tree.Count is 0);
		for (int i = 0; i < size; i++)
		{
			Assert.IsTrue(tree.TryAdd(arr[i]).Success);
		}
		Array.Sort(arr);
		CollectionAssert.AreEqual(arr, tree.ToArray());
	}

	[TestMethod]
	public void RemoveAll_2()
	{
		Random r = new();
		const int size = 400;
		const int max = size * size;
		BTreeLinked<int, SFunc<int, int, CompareResult>>? tree = BTreeLinked.New<int>(8);
		int[] arr = new int[size];
		for (int i = 0, j; i < size; i++)
		{
			do
			{
				j = max + r.Next(max);
			} while (Array.IndexOf(arr, j, 0, i) >= 0);
			arr[i] = j;
		}
		for (int i = 0; i < size; i++)
		{
			Assert.IsTrue(tree.TryAdd(arr[i]).Success);
		}
		for (int i = 0, j; i < size; i++)
		{
			Assert.IsTrue(tree.TryRemove(arr[i]).Success);
			do
			{
				j = r.Next(max);
			} while (Array.IndexOf(arr, j, 0, i) >= 0);
		}
		Assert.IsTrue(tree.Count is 0);
		for (int i = 0; i < size; i++)
		{
			Assert.IsTrue(tree.TryAdd(arr[i]).Success);
		}
		Array.Sort(arr);
		CollectionAssert.AreEqual(arr, tree.ToArray());
	}
	[TestMethod]
	public void CloneTest_1()
	{
		Random r = new();
		const int size = 400;
		const int max = size * size;
		BTreeLinked<int, SFunc<int, int, CompareResult>>? tree = BTreeLinked.New<int>(8);
		int[] arr = new int[size];
		for (int i = 0, j; i < size; i++)
		{
			do
			{
				j = max + r.Next(max);
			} while (Array.IndexOf(arr, j, 0, i) >= 0);
			arr[i] = j;
		}
		foreach (var x in arr)
			Assert.IsTrue(tree.TryAdd(x).Success);
		var clonedTree = tree.Clone();
		tree.Clear();
		Array.Sort(arr);
		CollectionAssert.AreEqual(arr, clonedTree.ToArray());
	}
	[TestMethod]
	public void CloneTest_2()
	{
		BTreeLinked<int, SFunc<int, int, CompareResult>>? tree = BTreeLinked.New<int>(8);
		BTreeLinked<int, SFunc<int, int, CompareResult>>? clonedTree = tree.Clone();
		CollectionAssert.AreEqual(Array.Empty<int>(), clonedTree.ToArray());
	}
}