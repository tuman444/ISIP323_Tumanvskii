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
    }
