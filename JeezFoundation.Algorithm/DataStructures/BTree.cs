namespace JeezFoundation.Algorithm.DataStructures;

/// <summary>Static helpers for <see cref="BTreeLinked{T, TCompare}"/>.</summary>
public static class BTreeLinked
{
    #region Extension Methods

    /// <inheritdoc cref="BTreeLinked{T, TCompare}.BTreeLinked(byte, TCompare)" />
    /// <inheritdoc cref="BTreeLinked{T, TCompare}" />
    /// <returns>The new constructed <see cref="BTreeLinked{T, TCompare}"/>.</returns>
    public static BTreeLinked<T, SFunc<T, T, CompareResult>> New<T>(
        byte maxDegree,
        Func<T, T, CompareResult>? compare = null) =>
        new(maxDegree, compare ?? Compare);

    #endregion Extension Methods
}

/// <summary>B-Tree Data structure.</summary>
/// <typeparam name="T">The type to store</typeparam>
/// <typeparam name="TCompare">The type that is comparing <typeparamref name="T"/> values.</typeparam>
public class BTreeLinked<T, TCompare> : IDataStructure<T>,
    DataStructure.IAddable<T>,
    DataStructure.IRemovable<T>,
    DataStructure.ICountable,
    DataStructure.IClearable,
    DataStructure.IAuditable<T>,
    DataStructure.IComparing<T, TCompare>,
    ICloneable<BTreeLinked<T, TCompare>>
    where TCompare : struct, IFunc<T, T, CompareResult>
{
    internal Node _root;
    internal int _count;
    internal TCompare _compare;
    internal byte _maxDegree;

    #region Nested Types

    internal class Node
    {
        internal T[] _values;
        internal Node?[] _children;
        internal Node? _parent;
        internal byte _parentIndex;
        internal byte _count;

        public Node(byte maxdegree)
        {
            _values = new T[maxdegree - 1];
            _children = new Node?[maxdegree];
        }

        public bool IsLeaf => _children[0] is null;

        public bool IsFull => _count + 1 == _children.Length;
    }

    #endregion Nested Types

    #region Constructors

    /// <summary>Creates a B-Tree having nodes of given maximum size. Maximum size must be even</summary>
    /// <param name="maxdegree">The maximum degree/children a node can have. Must be even</param>
    /// <param name="compare">The function for comparing <typeparamref name="T"/> values.</param>
    public BTreeLinked(byte maxdegree, TCompare compare = default)
    {
        if (maxdegree < 4) throw new ArgumentException("Maximum degree should be at least 4", nameof(maxdegree));
        if (maxdegree % 2 is not 0) throw new ArgumentException("Maximum degree should be an even number", nameof(maxdegree));
        _root = new(maxdegree);
        _count = 0;
        _maxDegree = maxdegree;
        _compare = compare;
    }

    #endregion Constructors

    #region Properties

    /// <inheritdoc />
    public int Count => _count;

    /// <inheritdoc />
    public TCompare Compare => _compare;

    /// <summary>The fixed size of a node within this tree</summary>
    public byte MaxDegree => _maxDegree;

    #endregion Properties

    #region Methods

    /// <inheritdoc />
    public void Clear()
    {
        _count = 0;
        _root._count = 0;
        for (int i = 0; i < MaxDegree; i++)
        {
            _root._children[i] = null;
        }
    }

    /// <summary>
    /// Searches for an value in the tree and provides the
    /// node it is contained in and its index in the array (as 'out' variables)
    /// if it exists. Otherwise provides the leaf node where the value shold be inserted
    /// </summary>
    /// <param name="value">The value to search</param>
    /// <param name="node">The node found</param>
    /// <param name="index">The index at which it is found</param>
    /// <returns>Returns true if value exists in the tree, otherwise false</returns>
    internal bool Search(T value, out Node node, out int index)
    {
        index = -1;
        Node? next = _root;
        do
        {
            node = next;
            next = node._children[node._count];
            for (int i = 0; i < node._count; i++)
            {
                CompareResult c = Compare.Invoke(node._values[i], value);
                if (c is Equal)
                {
                    index = i;
                    return true;
                }
                else if (c is Greater)
                {
                    next = node._children[i];
                    break;
                }
            }
        } while (next is not null);
        return false;
    }

    /// <inheritdoc />
    public bool Contains(T value)
    {
        return Search(value, out _, out _);
    }

    /// <summary>
    /// Finds the best index at which the insertion can be done and
    /// inserts the value at the given index, displacing the values as necessary <br/> <br/>
    /// Before using this method, ensure that the node is <br/>
    /// 1. A leaf node <br/>
    /// 2. Can support adding a value in it, i.e Count is at most MaxDegree-1 after insertion
    /// </summary>
    /// <param name="value">value to add</param>
    /// <param name="node">The node in which the value is to be added</param>
    /// <returns>returns true if addition was successful. If a duplicate is found, addition fails and the method returns false</returns>
    internal (bool Success, Exception? Exception) TryAdd(T value, Node node)
    {
        int index = 0;
        if (node._count > 0)
        {
            for (; index < node._count; index++)
            {
                CompareResult c = Compare.Invoke(node._values[index], value);
                if (c is Equal)
                {
                    return (false, new ArgumentException($"Adding to add a duplicate value to an {nameof(BTreeLinked<T, TCompare>)}: {value}.", nameof(value)));
                }
                else if (c is Greater)
                {
                    break;
                }
            }
            for (int i = node._count - 1; i >= index; i--)
            {
                node._values[i + 1] = node._values[i];
            }
        }
        node._values[index] = value;
        node._count++;
        _count++;
        return (true, default);
    }

    /// <summary>
    /// Splits the full node into a node having two child nodes<br/><br/>
    /// Before calling this method, ensure that <br/>
    /// 1. the node is full <br/>
    /// 2. the parent node is Not full (Not applicable for the Root node)
    /// </summary>
    /// <param name="fullNode">The node to split</param>
    internal void Split(Node fullNode)
    {
        Node right = new(MaxDegree);
        Node? parent = fullNode._parent, x;
        int l = MaxDegree - 1, MedianIndex = l >> 1;
        int i;
        int j;
        for (i = MedianIndex + 1, j = 0; i < l; i++, j++)
        {
            right._values[j] = fullNode._values[i];
            fullNode._values[i] = default!;
        }
        right._count = (byte)j;
        if (!fullNode.IsLeaf)
        {
            for (i = MedianIndex + 1, j = 0; i <= l; i++, j++)
            {
                right._children[j] = x = fullNode._children[i];
                if (x is not null)
                {
                    x._parent = right;
                    x._parentIndex = (byte)j;
                }
                fullNode._children[i] = null;
            }
        }
        fullNode._count = (byte)MedianIndex;
        if (parent is null) // 'fullnode' is Root
        {
            _root = new(MaxDegree);
            _root._values[0] = fullNode._values[MedianIndex];
            fullNode._values[MedianIndex] = default!;
            _root._count = 1;
            _root._children[0] = fullNode;
            fullNode._parentIndex = 0;
            _root._children[1] = right;
            right._parentIndex = 1;
            fullNode._parent = right._parent = _root;
        }
        else
        {
            for (i = parent._count + 1, j = i--; i > fullNode._parentIndex; j--, i--)
            {
                parent._children[j] = x = parent._children[i];
                if (x is not null)
                {
                    x._parentIndex = (byte)j;
                }
            }
            for (i = parent._count, j = i--; i >= fullNode._parentIndex; j--, i--)
            {
                parent._values[j] = parent._values[i];
            }
            parent._values[j] = fullNode._values[MedianIndex];
            fullNode._values[MedianIndex] = default!;
            parent._children[j] = fullNode;
            fullNode._parentIndex = (byte)j;
            parent._children[j + 1] = right;
            right._parentIndex = (byte)(j + 1);
            right._parent = parent;
            parent._count++;
        }
    }

    /// <inheritdoc />
    public (bool Success, Exception? Exception) TryAdd(T value)
    {
        if (_root.IsFull)
        {
            Split(_root);
        }
        Node node = _root;
        Node? child;
        while (!node.IsLeaf)
        {
            int i = 0;
            CompareResult c;
            while (i < node._count && (c = Compare.Invoke(node._values[i], value)) is not Greater)
            {
                if (c is Equal)
                {
                    return (false, new ArgumentException($"Adding to add a duplicate value to an {nameof(BTreeLinked<T, TCompare>)}: {value}.", nameof(value)));
                }
                i++;
            }
            child = node._children[i];
            if (child is not null && child.IsFull)
            {
                Split(child);
                c = Compare.Invoke(node._values[i], value);
                switch (c)
                {
                    case Greater: child = node._children[i]; break;
                    case Less: child = node._children[i + 1]; break;
                    case Equal: return (false, new ArgumentException($"Adding to add a duplicate value to an {nameof(BTreeLinked<T, TCompare>)}: {value}.", nameof(value)));
                }
            }
            if (child is null)
            {
                return (false, new CorruptedDataStructureException("Expected a non-null internal node, but found a null node"));
            }
            node = child;
        }
        return TryAdd(value, node);
    }

    /// <inheritdoc />
    public (bool Success, Exception? Exception) TryRemove(T value)
    {
        if (_count is 0)
        {
            return (false, new ArgumentException($"Attempting to remove a non-existing entry: {value}.", nameof(value)));
        }
        Node? node = _root;
        Node? child;
        int t = MaxDegree >> 1;
        // All nodes (except root) must contain at least this many values
        // [value of (t) in Cormen's "Introduction to Algorithms"]
        do
        {
            int i = 0;
            CompareResult c;
            while (i < node._count && (c = Compare.Invoke(node._values[i], value)) is not Greater)
            {
                if (c is Equal)
                {
                    if (node.IsLeaf) // CASE 1
                    {
                        for (int j = i + 1; j < node._count; j++)
                        {
                            node._values[j - 1] = node._values[j];
                        }
                        node._values[--node._count] = default!;
                        node = null;
                    }
                    else
                    {
                        Node? lc = node._children[i];
                        Node? rc = node._children[i + 1];
                        if (lc is null || rc is null)
                        {
                            return (false, new CorruptedDataStructureException("Found null children of an internal node!"));
                        }
                        if (lc._count >= t) // CASE 2a
                        {
                            do
                            {
                                child = lc;
                                lc = lc._children[lc._count];
                            } while (lc is not null);
                            // At this point lc is null, child is the rightmost node of subtree preceeding 'value'
                            value = node._values[i] = child._values[child._count - 1];
                        }
                        else if (rc._count >= t) // Case 2b
                        {
                            do
                            {
                                child = rc;
                                rc = rc._children[0];
                            } while (rc is not null);
                            // At this point rc is null, child is the leftmost node of subtree following 'value'
                            value = node._values[i++] = child._values[0];
                        }
                        else // Case 2c
                        {
                            int p;
                            int q;
                            int r = lc._count;
                            lc._values[lc._count++] = node._values[i];
                            for (p = i, q = i + 1; q < node._count; p++, q++)
                            {
                                node._values[p] = node._values[q];
                            }
                            node._values[p] = default!;
                            for (p = i + 1, q = p + 1; q <= node._count; p++, q++)
                            {
                                child = node._children[p] = node._children[q];
                                if (child is not null)
                                {
                                    child._parentIndex = (byte)p;
                                }
                                else
                                {
                                    return (false, new CorruptedDataStructureException("Found null children of an internal node!"));
                                }
                            }
                            node._children[p] = null;
                            node._count--;
                            for (p = lc._count, q = 0; q < rc._count; p++, q++, lc._count++)
                            {
                                lc._values[p] = rc._values[q];
                                rc._values[q] = default!;
                            }
                            for (p = r + 1, q = 0; q <= rc._count; p++, q++)
                            {
                                child = lc._children[p] = rc._children[q];
                                if (child is not null)
                                {
                                    child._parentIndex = (byte)p;
                                    child._parent = lc;
                                }
                                rc._children[q] = null;
                            }
                            rc._parent = null; // delete rc from memory ?
                            if (node == _root && node._count is 0)
                            {
                                _root = lc;
                                _root._parent = null;
                                node._values = default!;
                                node._children = default!; // delete node from memory ?
                                node = lc;
                                i = 0;
                                continue;
                            }
                        }
                    }
                    break;
                }
                i++;
            }
            if (node is null)
            {
                break; // 'node' would be null after Case 1
            }
            else
            {
                child = node._children[i]; // 'child' will be null only if 'node' is a Leaf node
            }
            if (child is null)
            {
                return (false, new ArgumentException($"Attempting to remove a non-existing entry: {value}.", nameof(value)));
            }
            else if (child._count < t)
            {
                Node? lc = i > 0 ? node._children[i - 1] : null;
                Node? rc = i < node._count ? node._children[i + 1] : null;
                int j;
                if (lc is not null && lc._count >= t) // Case 3a-Left
                {
                    for (j = child._count; j > 0; j--)
                    {
                        child._values[j] = child._values[j - 1];
                    }
                    child._count++;
                    for (j = child._count; j > 0; j--)
                    {
                        rc = child._children[j] = child._children[j - 1];
                        if (rc is not null)
                        {
                            rc._parentIndex = (byte)j;
                        }
                    }
                    rc = lc._children[lc._count];
                    if (rc is not null)
                    {
                        child._children[0] = rc;
                        rc._parent = child;
                        rc._parentIndex = 0;
                    }
                    child._values[0] = node._values[i - 1];
                    node._values[i - 1] = lc._values[--lc._count];
                    lc._values[lc._count] = default!;
                }
                else if (rc is not null && rc._count >= t) // Case 3a-Right
                {
                    child._values[child._count++] = node._values[i];
                    node._values[i] = rc._values[0];
                    lc = rc._children[0];
                    if (lc is not null)
                    {
                        child._children[child._count] = lc;
                        lc._parentIndex = child._count;
                        lc._parent = child;
                    }
                    for (j = 1; j < rc._count; j++)
                    {
                        rc._values[j - 1] = rc._values[j];
                    }
                    for (j = 1; j <= rc._count; j++)
                    {
                        lc = rc._children[j - 1] = rc._children[j];
                        if (lc is not null)
                        {
                            lc._parentIndex = (byte)(j - 1);
                        }
                    }
                    rc._children[rc._count--] = null;
                    rc._values[rc._count] = default!;
                }
                else
                {
                    int k;
                    if (lc is not null) // Case 3b-Left
                    {
                        lc._values[lc._count++] = node._values[i - 1];
                        for (j = lc._count, k = 0; k <= child._count; j++, k++)
                        {
                            rc = lc._children[j] = child._children[k];
                            if (rc is not null)
                            {
                                rc._parentIndex = (byte)j;
                                rc._parent = lc;
                            }
                        }
                        for (k = 0; k < child._count; k++)
                        {
                            lc._values[lc._count++] = child._values[k];
                        }
                        child._values = default!;
                        child._children = default!;
                        child._parent = null;
                        child = lc;
                    }
                    else if (rc is not null) // Case 3b-Right || PS: can leave it at else {...} but this avoids Null reference warning
                    {
                        child._values[child._count++] = node._values[i];
                        for (j = child._count, k = 0; k <= rc._count; j++, k++)
                        {
                            lc = child._children[j] = rc._children[k];
                            if (lc is not null)
                            {
                                lc._parentIndex = (byte)j;
                                lc._parent = child;
                            }
                        }
                        for (k = 0; k < rc._count; k++)
                        {
                            child._values[child._count++] = rc._values[k];
                        }
                        rc._values = default!;
                        rc._children = default!;
                        rc._parent = null;
                    }
                    else
                    {
                        return (false, new CorruptedDataStructureException("Found null children of an internal node!"));
                    }
                    for (k = child._parentIndex + 1; k < node._count; k++)
                    {
                        node._values[k - 1] = node._values[k];
                        lc = node._children[k] = node._children[k + 1];
                        if (lc is not null)
                        {
                            lc._parentIndex = (byte)k;
                        }
                    }
                    node._children[node._count--] = null;
                    node._values[node._count] = default!;
                    if (node == _root && node._count is 0)
                    {
                        node._children = null!;
                        _root = child;
                        _root._parent = null;
                        _root._parentIndex = 0;
                    }
                }
            }
            node = child;
        } while (node is not null);
        _count--;
        return (true, default);
    }

    /// <inheritdoc />
    public T[] ToArray()
    {
        if (_count is 0)
        {
            return Array.Empty<T>();
        }
        T[] array = new T[_count];
        this.Stepper<T, FillArray<T>>(array);
        return array;
    }

    internal static Node LeftmostNode(Node node)
    {
        Node? leftChild = node._children[0];
        while (leftChild is not null)
        {
            node = leftChild;
            leftChild = node._children[0];
        }
        return node;
    }

    /// <summary>Given a value's index in a node, returns the node and index of the value which is next to the given value.</summary>
    /// <param name="node">The node in which the current value is contained</param>
    /// <param name="index">The index of the value within the node</param>
    /// <returns>The node and index of the next value</returns>
    internal static (Node? Node, int Index) NextNode(Node node, int index)
    {
        index++;
        Node? p = node._children[index];
        if (index >= node._count && p is null)
        {
            index = node._parentIndex;
            p = node._parent;
            while (p is not null && index == p._count)
            {
                node = p;
                p = p._parent;
                index = node._parentIndex;
            }
            return (p, index);
        }
        else if (p is not null)
        {
            return (LeftmostNode(p), 0);
        }
        else
        {
            return (node, index);
        }
    }

    /// <inheritdoc />
    public System.Collections.Generic.IEnumerator<T> GetEnumerator()
    {
        if (_count > 0)
        {
            int index = 0;
            Node node;
            Node? nextnode = LeftmostNode(_root);
            do
            {
                node = nextnode;
                yield return node._values[index];
                (nextnode, index) = NextNode(node, index);
            } while (nextnode is not null);
        }
        yield break;
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

    /// <inheritdoc />
    public StepStatus StepperBreak<TStep>(TStep step = default)
        where TStep : struct, IFunc<T, StepStatus>
    {
        if (_count is 0)
        {
            return Continue;
        }
        return StepperBreak(_root, step);
    }

    internal StepStatus StepperBreak<TStep>(Node start, TStep step = default)
        where TStep : struct, IFunc<T, StepStatus>
    {
        int index = 0;
        Node? node = LeftmostNode(start);
        do
        {
            if (step.Invoke(node._values[index]) is Break)
            {
                return Break;
            }
            (node, index) = NextNode(node!, index);
        } while (node is not null);
        return Continue;
    }

    /// <inheritdoc />
    public BTreeLinked<T, TCompare> Clone()
    {
        BTreeLinked<T, TCompare> clone = new(_maxDegree, _compare);
        StackLinked<(Node original, Node copy)> stack = new();
        stack.Push((_root, clone._root));
        while (stack._count > 0)
        {
            (Node original, Node copy) = stack.Pop();
            copy._parentIndex = original._parentIndex;
            copy._count = original._count;
            for (int i = 0; i < original._count; i++)
            {
                copy._values[i] = original._values[i];
            }
            if (!original.IsLeaf)
            {
                for (int i = 0; i <= original._count; i++)
                {
                    Node c = copy._children[i] = new(MaxDegree);
                    c._parent = copy;
                    Node? o = original._children[i];
                    if (o is null) throw new CorruptedDataStructureException("Found null children of an internal node!");
                    stack.Push((o, c));
                }
            }
        }
        clone._count = _count;
        return clone;
    }

    #endregion Methods
}