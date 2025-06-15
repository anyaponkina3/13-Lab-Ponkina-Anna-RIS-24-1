using ClassLibrary2;
using thirdtask;
using Delegat;

namespace ObservableCollectionDemo
{
    class Program
    {
        static MyObservableCollection<BankCard> collection1;
        static MyObservableCollection<BankCard> collection2;
        static Journal journal1;
        static Journal journal2;

        static void Main(string[] args)
        {
            InitializeCollections();
            InitializeJournals();
            SubscribeJournals();

            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Меню управления наблюдаемыми коллекциями");
                Console.WriteLine("1. Добавить карту в коллекцию 1");
                Console.WriteLine("2. Добавить карту в коллекцию 2");
                Console.WriteLine("3. Удалить карту из коллекции 1");
                Console.WriteLine("4. Удалить карту из коллекции 2");
                Console.WriteLine("5. Изменить карту в коллекции 1");
                Console.WriteLine("6. Изменить карту в коллекции 2");
                Console.WriteLine("7. Показать коллекцию 1");
                Console.WriteLine("8. Показать коллекцию 2");
                Console.WriteLine("9. Показать журнал 1 (подписан на coll1)");
                Console.WriteLine("10. Показать журнал 2 (подписан на coll1 и coll2)");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите действие: ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            AddCardToCollection(collection1);
                            break;
                        case 2:
                            AddCardToCollection(collection2);
                            break;
                        case 3:
                            RemoveCardFromCollection(collection1);
                            break;
                        case 4:
                            RemoveCardFromCollection(collection2);
                            break;
                        case 5:
                            ModifyCardInCollection(collection1);
                            break;
                        case 6:
                            ModifyCardInCollection(collection2);
                            break;
                        case 7:
                            ShowCollection(collection1);
                            break;
                        case 8:
                            ShowCollection(collection2);
                            break;
                        case 9:
                            ShowJournal(journal1, "Журнал 1");
                            break;
                        case 10:
                            ShowJournal(journal2, "Журнал 2");
                            break;
                        case 0:
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Неверный выбор!");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Некорректный ввод!");
                }

                if (!exit)
                {
                    Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                }
            }
        }

        static void InitializeCollections()
        {
            collection1 = new MyObservableCollection<BankCard>("Коллекция 1");
            collection2 = new MyObservableCollection<BankCard>("Коллекция 2");

            // Добавим несколько начальных элементов
            for (int i = 0; i < 3; i++)
            {
                var card = new BankCard();
                card.RandomInit();
                collection1.Add(card);

                card = new BankCard();
                card.RandomInit();
                collection2.Add(card);
            }
        }

        static void InitializeJournals()
        {
            journal1 = new Journal();
            journal2 = new Journal();
        }

        static void SubscribeJournals()
        {
            collection1.CollectionCountChanged += (sender, e) => journal1.AddEntry(new JournalEntry(collection1.Name, e.ChangeType, e.ChangedItem?.ToString() ?? "null"));

            collection1.CollectionReferenceChanged += (sender, e) => journal1.AddEntry(new JournalEntry(collection1.Name, e.ChangeType, e.ChangedItem?.ToString() ?? "null"));

            collection1.CollectionReferenceChanged += (sender, e) => journal2.AddEntry(new JournalEntry(collection1.Name, e.ChangeType, e.ChangedItem?.ToString() ?? "null"));

            collection2.CollectionReferenceChanged += (sender, e) => journal2.AddEntry(new JournalEntry(collection2.Name, e.ChangeType, e.ChangedItem?.ToString() ?? "null"));
        }

        static void AddCardToCollection(MyObservableCollection<BankCard> collection)
        {
            var card = new BankCard();
            card.RandomInit();
            collection.Add(card);
            Console.WriteLine($"Добавлена карта: {card}");
        }

        static void RemoveCardFromCollection(MyObservableCollection<BankCard> collection)
        {
            Console.WriteLine("Содержимое коллекции:");
            int index = 1;
            foreach (var card in collection)
            {
                Console.WriteLine($"{index++}. {card}");
            }

            Console.Write("Введите номер карты для удаления: ");
            if (int.TryParse(Console.ReadLine(), out int cardNum) && cardNum > 0 && cardNum <= collection.Count)
            {
                int current = 1;
                foreach (var card in collection)
                {
                    if (current++ == cardNum)
                    {
                        collection.Remove(card);
                        Console.WriteLine("Карта удалена");
                        return;
                    }
                }
            }
            Console.WriteLine("Неверный номер карты!");
        }

        static void ModifyCardInCollection(MyObservableCollection<BankCard> collection)
        {
            Console.WriteLine("Содержимое коллекции:");
            int index = 1;
            foreach (var card in collection)
            {
                Console.WriteLine($"{index++}. {card}");
            }

            Console.Write("Введите номер карты для изменения: ");
            if (int.TryParse(Console.ReadLine(), out int cardNum) && cardNum > 0 && cardNum <= collection.Count)
            {
                int current = 1;
                foreach (var card in collection)
                {
                    if (current++ == cardNum)
                    {
                        var newCard = new BankCard();
                        newCard.RandomInit();
                        collection[card] = newCard;
                        Console.WriteLine($"Карта изменена на: {newCard}");
                        return;
                    }
                }
            }
            Console.WriteLine("Неверный номер карты!");
        }

        static void ShowCollection(MyObservableCollection<BankCard> collection)
        {
            Console.WriteLine($"{collection.Name}");
            if (collection.Count == 0)
            {
                Console.WriteLine("Коллекция пуста");
                return;
            }

            int index = 1;
            foreach (var card in collection)
            {
                Console.WriteLine($"{index++}. {card}");
            }
        }

        static void ShowJournal(Journal journal, string title)
        {
            Console.WriteLine($"{title}");
            if (journal.ToString() == "")
            {
                Console.WriteLine("Журнал пуст");
                return;
            }
            Console.WriteLine(journal);
        }
    }
}