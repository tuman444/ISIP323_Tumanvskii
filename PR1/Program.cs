// See https://aka.ms/new-console-template for more information
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
}
