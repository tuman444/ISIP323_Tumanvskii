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

public class ProductManager // Класс для управления товарами
{
    private List<Product> products; // Список всех товаров

    // Конструктор менеджера товаров
    public ProductManager()
    {
        products = new List<Product>(); // Инициализация списка
        InitializeTestData();           // Загрузка тестовых данных
    }

    // Метод для инициализации тестовых данных
    private void InitializeTestData()
    {
        // Добавление 5 тестовых товаров
        AddProduct("Смартфон Samsung", 29999.99m, 15, Category.Electronics);
        AddProduct("Футболка хлопковая", 1499.50m, 30, Category.Clothing);
        AddProduct("Шоколад молочный", 89.90m, 100, Category.Food);
        AddProduct("Роман '1984'", 450.00m, 25, Category.Books);
        AddProduct("Футбольный мяч", 2499.00m, 10, Category.Sports);
    }

    // Метод для добавления нового товара
    public void AddProduct(string name, decimal price, int quantity, Category category)
    {
        try // Обработка возможных исключений
        {
            ValidateProductData(name, price, quantity); // Валидация данных

            // Создание нового товара
            var product = new Product(name, price, quantity, category);
            products.Add(product); // Добавление в список

            // Вывод сообщения об успешном добавлении
            Console.WriteLine($"Товар '{name}' успешно добавлен с кодом {product.Code}");
        }
        catch (ArgumentException ex) // Обработка ошибок валидации
        {
            Console.WriteLine($"Ошибка при добавлении товара: {ex.Message}");
        }
    }

    // Приватный метод для валидации данных товара
    private void ValidateProductData(string name, decimal price, int quantity)
    {
        if (string.IsNullOrWhiteSpace(name)) // Проверка названия
            throw new ArgumentException("Название товара не может быть пустым");

        if (price <= 0) // Проверка цены
            throw new ArgumentException("Цена должна быть положительной");

        if (quantity < 0) // Проверка количества
            throw new ArgumentException("Количество не может быть отрицательным");
    }

    // Метод для удаления товара по коду
    public bool RemoveProduct(string code)
    {
        // Поиск товара по коду
        var product = products.FirstOrDefault(p => p.Code == code);
        if (product != null) // Если товар найден
        {
            products.Remove(product); // Удаление из списка
            return true; // Возврат успешного статуса
        }
        return false; // Товар не найден
    }

    // Метод для поиска товара по коду
    public Product FindProductByCode(string code)
    {
        // Возврат товара или null если не найден
        return products.FirstOrDefault(p => p.Code == code);
    }

    // Метод для добавления поставки товара
    public bool SupplyProduct(string code, int quantity)
    {
        if (quantity <= 0) // Проверка количества поставки
        {
            Console.WriteLine("Количество поставки должно быть положительным");
            return false; // Неверное количество
        }

        var product = FindProductByCode(code); // Поиск товара
        if (product != null) // Если товар найден
        {
            product.Quantity += quantity; // Увеличение количества
            product.UpdateStockStatus(); // Обновление статуса
            Console.WriteLine($"Поставка успешно добавлена. Новое количество: {product.Quantity}");
            return true; // Успешное выполнение
        }

        Console.WriteLine("Товар с указанным кодом не найден");
        return false; // Товар не найден
    }

    // Метод для продажи товара
    public bool SellProduct(string code, int quantity)
    {
        if (quantity <= 0) // Проверка количества продажи
        {
            Console.WriteLine("Количество продажи должно быть положительным");
            return false; // Неверное количество
        }

        var product = FindProductByCode(code); // Поиск товара
        if (product != null) // Если товар найден
        {
            if (product.Quantity < quantity) // Проверка достаточности товара
            {
                Console.WriteLine($"Недостаточно товара на складе. Доступно: {product.Quantity}");
                return false; // Недостаточно товара
            }

            product.Quantity -= quantity; // Уменьшение количества
            product.UpdateStockStatus(); // Обновление статуса
            Console.WriteLine($"Продажа успешно завершена. Остаток: {product.Quantity}");
            return true; // Успешное выполнение
        }

        Console.WriteLine("Товар с указанным кодом не найден");
        return false; // Товар не найден
    }

    // Метод для поиска товаров по различным критериям
    public List<Product> SearchProducts(string searchTerm)
    {
        // LINQ запрос для поиска по коду, названию или категории
        return products.Where(p =>
            p.Code.Contains(searchTerm) || // Поиск в коде
            p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) || // Поиск в названии (без учета регистра)
            p.Category.ToString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase) // Поиск в категории
        ).ToList(); // Преобразование результата в список
    }

    // Метод для получения всех товаров
    public List<Product> GetAllProducts()
    {
        return products; // Возврат всего списка товаров
    }
}
