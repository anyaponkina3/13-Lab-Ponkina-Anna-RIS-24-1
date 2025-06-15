using ClassLibrary2;
namespace thirdtask
{
    public class TreePoint<T> : ICloneable where T : IComparable<T>, ICloneable, IInit, new()
    {
        public T Data { get; set; }
        public TreePoint<T> Left { get; set; }
        public TreePoint<T> Right { get; set; }

        public TreePoint()
        {
            Data = default(T);
            Left = null;
            Right = null;
        }

        public TreePoint(T data)
        {
            Data = data;
            Left = null;
            Right = null;
        }

        public override string ToString()
        {
            return Data?.ToString() ?? "null";
        }

        public object Clone()
        {
            T clonedData = (T)Data?.Clone();
            TreePoint<T> clonedPoint = new TreePoint<T>(clonedData);

            // Не клонируем Left и Right, чтобы избежать рекурсивного клонирования всего дерева
            clonedPoint.Left = null;
            clonedPoint.Right = null;

            return clonedPoint;
        }
    }
}