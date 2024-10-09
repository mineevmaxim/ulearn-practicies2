using System.Collections;

namespace Generics.BinaryTrees;

public class Node<T>
    where T : IComparable<T>
{
    public Node<T>? Left { get; private set; }
    public Node<T>? Right { get; private set; }
    public readonly T Value;

    public Node(T value)
    {
        Left = null;
        Right = null;
        Value = value;
    }

    public void Add(T value)
    {
        if (value.CompareTo(Value) < 1)
            if (Left == null)
                Left = new Node<T>(value);
            else
                Left.Add(value);
        else
            if (Right == null)
                Right = new Node<T>(value);
            else
                Right.Add(value);
    }
}

public class BinaryTree
{
    public static BinaryTree<TItem> Create<TItem>(params TItem[] items)
        where TItem : IComparable<TItem>
    {
        var tree = new BinaryTree<TItem>();
        foreach (var item in items)
            tree.Add(item);

        return tree;
    }
}

public class BinaryTree<T> : IEnumerable<T>
    where T : IComparable<T>
{
    public T? Value => Root == null ? default(T) : Root.Value;

    public Node<T>? Left => Root?.Left;
    public Node<T>? Right => Root?.Right;
    private Node<T>? Root { get; set; }

    public void Add(T value)
    {
        if (Root is null)
            Root = new Node<T>(value);
        else
            Root.Add(value);
    }

    public IEnumerator<T> GetEnumerator()
        => InOrderTraversal(Root).GetEnumerator();

    private static IEnumerable<T> InOrderTraversal(Node<T>? node)
    {
        if (node == null)
            yield break;
        foreach (var value in InOrderTraversal(node.Left))
            yield return value;
        
        yield return node.Value;
        
        foreach (var value in InOrderTraversal(node.Right))
            yield return value;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}