// See https://aka.ms/new-console-template for more information
using System.Globalization;

class Program
{
    // Структура для хранения информации о трате
    struct Expense
    {
        public string Name;      // Название товара/услуги
        public decimal Amount;   // Сумма траты в рублях

        // Конструктор для создания новой траты
        public Expense(string name, decimal amount)
        {
            Name = name;
            Amount = amount;
        }
    }
    static void Main(string[] args)
    {
        // Устанавливаем кодировку для корректного отображения символов
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        // Создаем список для хранения всех трат
        List<Expense> expenses = new List<Expense>();

        // Запрашиваем у пользователя количество операций (от 2 до 40)
        int operationsCount = 0;
        while (operationsCount < 2 || operationsCount > 40)
        {
            Console.Write("Введите количество операций (2-40): ");

            // Проверяем корректность ввода
            if (!int.TryParse(Console.ReadLine(), out operationsCount) || operationsCount < 2 || operationsCount > 40)
            {
                Console.WriteLine("Ошибка! Введите число от 2 до 40.");
                operationsCount = 0; // Сбрасываем значение для повторного ввода
            }
        }

        // Выводим инструкцию по формату ввода данных
        Console.WriteLine("\nВведите траты в формате: Название; Сумма");
        Console.WriteLine("Пример: Влажные салфетки \"Лента\"; 235");

        // Цикл для ввода всех операций
        for (int i = 0; i < operationsCount; i++)
        {
            bool validInput = false; // Флаг корректности ввода

            // Повторяем ввод пока данные не будут корректными
            while (!validInput)
            {
                Console.Write($"Операция {i + 1}: ");
                string input = Console.ReadLine();

                // Проверяем что строка не пустая
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Ошибка! Введите данные.");
                    continue;
                }

                // Разделяем ввод на части по точке с запятой
                string[] parts = input.Split(';');

                // Проверяем что есть две части: название и сумма
                if (parts.Length != 2)
                {
                    Console.WriteLine("Ошибка формата! Используйте: Название; Сумма");
                    continue;
                }

                // Извлекаем и очищаем название от пробелов
                string name = parts[0].Trim();

                // Проверяем что название не пустое
                if (string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine("Ошибка! Название не может быть пустым.");
                    continue;
                }

                // Пытаемся преобразовать вторую часть в число (сумму)
                // Используем InvariantCulture чтобы избежать проблем с разделителями
                if (decimal.TryParse(parts[1].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal amount) && amount > 0)
                {
                    // Добавляем корректную трату в список
                    expenses.Add(new Expense(name, amount));
                    validInput = true; // Ввод корректен, выходим из цикла
                }
                else
                {
                    Console.WriteLine("Ошибка! Введите корректную сумму.");
                }
            }
        }
        // Основной цикл работы с меню
        bool exit = false;
        while (!exit)
        {
            // Вывод меню
            Console.WriteLine("\n=== МЕНЮ ===");
            Console.WriteLine("1. Вывод данных");
            Console.WriteLine("2. Статистика");
            Console.WriteLine("3. Сортировка по цене");
            Console.WriteLine("4. Конвертация валюты");
            Console.WriteLine("5. Поиск по названию");
            Console.WriteLine("0. Выход");

            Console.Write("Выберите пункт меню: ");
            string choice = Console.ReadLine();

            // Обработка выбора пользователя
            switch (choice)
            {
                case "1":
                    DisplayData(expenses); // Вывод всех данных
                    break;
                case "2":
                    ShowStatistics(expenses); // Показать статистику
                    break;
                case "3":
                    BubbleSortByPrice(ref expenses); // Сортировать по цене
                    Console.WriteLine("Данные отсортированы по цене!");
                    break;
                case "4":
                    ConvertCurrency(expenses); // Конвертировать валюту
                    break;
                case "5":
                    SearchByName(expenses); // Поиск по названию
                    break;
                case "0":
                    exit = true; // Выход из программы
                    Console.WriteLine("Выход из программы...");
                    break;
                default:
                    Console.WriteLine("Неверный выбор! Попробуйте снова.");
                    break;
            }
        }
    }

    // Метод для вывода всех трат в табличном формате
    static void DisplayData(List<Expense> expenses)
    {
        // Проверяем есть ли данные для отображения
        if (expenses.Count == 0)
        {
            Console.WriteLine("Нет данных для отображения.");
            return;
        }

        Console.WriteLine("\n=== ВАШИ ТРАТЫ ===");
        Console.WriteLine("№  Название\t\tСумма (руб)");
        Console.WriteLine("--------------------------------");

        // Выводим каждую трату с номером, названием и суммой
        for (int i = 0; i < expenses.Count; i++)
        {
            // {i + 1} - номер, {expenses[i].Name,-20} - название с выравниванием слева на 20 символов
            // {expenses[i].Amount,10:F2} - сумма с выравниванием справа на 10 символов и 2 знаками после запятой
            Console.WriteLine($"{i + 1}. {expenses[i].Name,-20} {expenses[i].Amount,10:F2}");
        }
    }

}
