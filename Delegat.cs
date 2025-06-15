namespace Delegat
{
    public class CollectionHandlerEventArgs : EventArgs
    {
        public string ChangeType { get; set; }
        public object ChangedItem { get; set; }

        public CollectionHandlerEventArgs(string changeType, object changedItem)
        {
            ChangeType = changeType;
            ChangedItem = changedItem;
        }
    }

    public delegate void CollectionHandler(object source, CollectionHandlerEventArgs args);

    public class JournalEntry
    {
        public string CollectionName { get; set; }
        public string ChangeType { get; set; }
        public string ItemInfo { get; set; }

        public JournalEntry(string collectionName, string changeType, string itemInfo)
        {
            CollectionName = collectionName;
            ChangeType = changeType;
            ItemInfo = itemInfo;
        }

        public override string ToString()
        {
            return $"Коллекция: {CollectionName}, Изменение: {ChangeType}, Элемент: {ItemInfo}";
        }
    }

    public class Journal
    {
        private List<JournalEntry> entries = new List<JournalEntry>();

        public void AddEntry(JournalEntry entry)
        {
            entries.Add(entry);
        }

        public override string ToString()
        {
            return string.Join("\n", entries);
        }
    }
}