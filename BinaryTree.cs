using System.Collections;
using ClassLibrary2;

namespace thirdtask
{
    public class BinaryTree<T> : ICollection<T> where T : IInit, ICloneable, IComparable<T>, new()
    {
        public TreePoint<T> root; // Корень дерева (owner - владелец дерева)
        public int count;

        public BinaryTree()
        {
            root = null;
            count = 0;
        }

        public BinaryTree(int length) : this()
        {
            if (length <= 0)
                throw new ArgumentOutOfRangeException(nameof(length));

            for (int i = 0; i < length; i++)
            {
                T item = new T();
                item.RandomInit();
                AddToSearchTree(item);
            }
        }

        public BinaryTree(BinaryTree<T> other) : this()
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            foreach (var item in other)
            {
                Add((T)item.Clone());
            }
        }

        public int Count => count;

        // 1. Создание идеально сбалансированного дерева
        public void CreateBalancedTree(T[] data)
        {
            root = CreateBalanced(data, 0, data.Length - 1);
            count = data.Length;
        }

        private TreePoint<T> CreateBalanced(T[] data, int start, int end)
        {
            if (start > end) return null;

            int mid = (start + end) / 2;
            var node = new TreePoint<T>(data[mid]);

            node.Left = CreateBalanced(data, start, mid - 1);
            node.Right = CreateBalanced(data, mid + 1, end);

            return node;
        }

        public BinaryTree<T> ToSearchTree()
        {
            List<T> elements = new List<T>();
            InOrderTraversal(root, elements);

            BinaryTree<T> searchTree = new BinaryTree<T>();
            foreach (var element in elements)
            {
                searchTree.AddToSearchTree(element);
            }
            return searchTree;
        }

        private void AddToSearchTree(T data)
        {
            root = AddToSearchTree(root, data);
            count++;
        }

        private TreePoint<T> AddToSearchTree(TreePoint<T> node, T data)
        {
            if (node == null)
            {
                return new TreePoint<T>(data);
            }

            int compareResult = data.CompareTo(node.Data);
            if (compareResult <= 0) // Изменено для размещения одинаковых элементов друг за другом
            {
                node.Left = AddToSearchTree(node.Left, data);
            }
            else
            {
                node.Right = AddToSearchTree(node.Right, data);
            }

            return node;
        }

        public bool Remove(T key)
        {
            int beforeCount = count;
            root = Remove(root, key);
            return count < beforeCount;
        }

        private TreePoint<T> Remove(TreePoint<T> node, T key)
        {
            if (node == null) return null;

            int compareResult = key.CompareTo(node.Data);

            if (compareResult < 0)
            {
                node.Left = Remove(node.Left, key);
            }
            else if (compareResult > 0)
            {
                node.Right = Remove(node.Right, key);
            }
            else
            {
                count--;
                if (node.Left == null)
                    return node.Right;
                if (node.Right == null)
                    return node.Left;

                node.Data = MinValue(node.Right);
                node.Right = Remove(node.Right, node.Data);
            }
            return node;
        }

        private T MinValue(TreePoint<T> node)
        {
            T minValue = node.Data;
            while (node.Left != null)
            {
                minValue = node.Left.Data;
                node = node.Left;
            }
            return minValue;
        }

        public void PrintTree(string treeType = "Дерево")
        {
            Console.WriteLine($"\n{treeType} (по уровням):");
            if (root == null)
            {
                Console.WriteLine("Дерево пустое");
                return;
            }

            Queue<TreePoint<T>> queue = new Queue<TreePoint<T>>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                int levelSize = queue.Count;
                for (int i = 0; i < levelSize; i++)
                {
                    var node = queue.Dequeue();
                    Console.Write(node.Data + " ");

                    if (node.Left != null)
                        queue.Enqueue(node.Left);
                    if (node.Right != null)
                        queue.Enqueue(node.Right);
                }
                Console.WriteLine();
            }
        }

        private void InOrderTraversal(TreePoint<T> node, List<T> elements)
        {
            if (node == null) return;

            InOrderTraversal(node.Left, elements);
            elements.Add(node.Data);
            InOrderTraversal(node.Right, elements);
        }

        public int CountNodes(TreePoint<T> node)
        {
            if (node == null) return 0;
            return 1 + CountNodes(node.Left) + CountNodes(node.Right);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return InOrderTraversal().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private IEnumerable<T> InOrderTraversal()
        {
            if (root == null) yield break;

            Stack<TreePoint<T>> stack = new Stack<TreePoint<T>>();
            var current = root;

            while (current != null || stack.Count > 0)
            {
                while (current != null)
                {
                    stack.Push(current);
                    current = current.Left;
                }

                current = stack.Pop();
                yield return current.Data;
                current = current.Right;
            }
        }

        public void Clear()
        {
            root = null;
            count = 0;
        }

        public BinaryTree<T> DeepClone()
        {
            var newTree = new BinaryTree<T>();
            newTree.root = CloneTree(root);
            newTree.count = count;
            return newTree;
        }

        private TreePoint<T> CloneTree(TreePoint<T> node)
        {
            if (node == null) return null;

            var newNode = (TreePoint<T>)node.Clone();
            newNode.Left = CloneTree(node.Left);
            newNode.Right = CloneTree(node.Right);

            return newNode;
        }

        public void PrintCardsTree()
        {
            if (root == null)
            {
                Console.WriteLine("Дерево карт пустое");
                return;
            }

            Console.WriteLine("Структура дерева:");
            PrintCardsTree(root, "", true);
        }

        private void PrintCardsTree(TreePoint<T> node, string indent, bool isLast)
        {
            if (node == null) return;

            Console.Write(indent);
            if (isLast)
            {
                Console.Write("└── ");
                indent += "    ";
            }
            else
            {
                Console.Write("├── ");
                indent += "│   ";
            }

            Console.WriteLine(node.Data);

            bool hasRight = node.Right != null;
            bool hasLeft = node.Left != null;

            if (hasRight && hasLeft)
            {
                PrintCardsTree(node.Right, indent, false);
                PrintCardsTree(node.Left, indent, true);
            }
            else if (hasRight)
            {
                PrintCardsTree(node.Right, indent, true);
            }
            else if (hasLeft)
            {
                PrintCardsTree(node.Left, indent, true);
            }
        }

        public bool IsReadOnly => false;

        public void Add(T item) => AddToSearchTree(item);

        public bool Contains(T item)
        {
            return FindItem(root, item) != null;
        }

        public TreePoint<T>? FindItem(TreePoint<T>? node, T item)
        {
            if (node == null) return null;

            int compareResult = item.CompareTo(node.Data);
            if (compareResult == 0) return node;

            return compareResult < 0 ? FindItem(node.Left, item) : FindItem(node.Right, item);
        }

        public TreePoint<T>? Find(T item)
        {
            return FindItem(root, item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0 || arrayIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            if (array.Length - arrayIndex < Count)
                throw new ArgumentException("Недостаточно места в массиве");

            foreach (var item in this)
            {
                array[arrayIndex++] = item;
            }
        }
    }
}