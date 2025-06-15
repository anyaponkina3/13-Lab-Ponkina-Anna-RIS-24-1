using ClassLibrary2;
using Delegat;
using thirdtask;

public class MyObservableCollection<T> : BinaryTree<T> where T : IInit, ICloneable, IComparable<T>, new()
{
    public string Name { get; set; }

    public event CollectionHandler CollectionCountChanged;
    public event CollectionHandler CollectionReferenceChanged;

    public MyObservableCollection(string name) : base()
    {
        Name = name;
    }

    public MyObservableCollection(string name, int length) : base(length)
    {
        Name = name;
    }

    public new void Add(T item)
    {
        base.Add(item);
        OnCollectionCountChanged("Добавлен элемент", item);
    }

    public new bool Remove(T item)
    {
        bool result = base.Remove(item);
        if (result)
        {
            OnCollectionCountChanged("Удален элемент", item);
        }
        return result;
    }

    public new void Clear()
    {
        base.Clear();
        OnCollectionCountChanged("Коллекция очищена", default(T));
    }

    protected virtual void OnCollectionCountChanged(string changeType, object changedItem)
    {
        CollectionCountChanged?.Invoke(this, new CollectionHandlerEventArgs(changeType, changedItem));
    }

    protected virtual void OnCollectionReferenceChanged(string changeType, object changedItem)
    {
        CollectionReferenceChanged?.Invoke(this, new CollectionHandlerEventArgs(changeType, changedItem));
    }

    public T this[T key]
    {
        get
        {
            var node = Find(key);
            if (node != null) return node.Data;
            throw new KeyNotFoundException();
        }
        set
        {
            if (Remove(key))
            {
                Add(value);
                OnCollectionReferenceChanged("Элемент изменен", value);
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }
    }
}