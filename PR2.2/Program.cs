public enum Category // Перечисление категорий товаров
{
    Electronics, // Категория: Электроника
    Clothing,    // Категория: Одежда
    Food,        // Категория: Еда
    Books,       // Категория: Книги
    Sports       // Категория: Спорт
}

public class Product // Класс, представляющий товар
{
    // Свойства товара с модификаторами доступа
    public string Code { get; private set; } // Уникальный код (только для чтения извне)
    public string Name { get; set; }         // Название товара
    public decimal Price { get; set; }       // Цена товара
    public int Quantity { get; set; }        // Количество товара
    public bool InStock { get; private set; } // Наличие на складе (только для чтения)
    public Category Category { get; set; }   // Категория товара

    // Конструктор класса Product
    public Product(string name, decimal price, int quantity, Category category)
    {
        Code = GenerateCode();      // Генерация уникального кода
        Name = name;                // Установка названия
        Price = price;              // Установка цены
        Quantity = quantity;        // Установка количества
        Category = category;        // Установка категории
        UpdateStockStatus();        // Обновление статуса наличия
    }

    // Приватный метод для генерации уникального кода
    private string GenerateCode()
    {
        // Генерация кода на основе текущего времени (тиков)
        return "1" + DateTime.Now.Ticks.ToString().Substring(10, 6);
    }

    // Метод для обновления статуса наличия товара
    public void UpdateStockStatus()
    {
        InStock = Quantity > 0; // Товар в наличии, если количество > 0
    }

    // Метод для получения строкового представления товара
    public string GetProductInfo()
    {
        return $"Код: {Code}, Название: {Name}, Цена: {Price:C}, " +
               $"Количество: {Quantity}, В наличии: {(InStock ? "Да" : "Нет")}, " +
               $"Категория: {Category}";
    }
}
