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
        foreach (var product in products)
        {
            if (product.Code == code)
            {
                return product;
            }
        }
        return null;
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

class Program // Главный класс программы
{
    private static ProductManager productManager; // Статическое поле менеджера товаров

    // Главный метод программы
    static void Main(string[] args)
    {
        productManager = new ProductManager(); // Создание менеджера товаров

        Console.WriteLine("Добро пожаловать в систему учета товаров!");
        Console.WriteLine("Тестовые данные загружены успешно!");
        Console.WriteLine("Нажмите любую клавишу для продолжения...");
        Console.ReadKey(); // Ожидание нажатия клавиши

            DisplayMainMenu(); // Запуск главного меню
    }

    // Метод для отображения главного меню
    static void DisplayMainMenu()
    {
        while (true) // Бесконечный цикл меню
        {
            Console.Clear(); // Очистка консоли
            Console.WriteLine("=== ГЛАВНОЕ МЕНЮ ===");
            Console.WriteLine("1. Просмотреть все товары");
            Console.WriteLine("2. Добавить товар");
            Console.WriteLine("3. Удалить товар");
            Console.WriteLine("4. Заказать поставку");
            Console.WriteLine("5. Продать товар");
            Console.WriteLine("6. Поиск товаров");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите опцию: ");

            var choice = Console.ReadLine(); // Чтение выбора пользователя

            switch (choice) // Обработка выбора
            {
                case "1":
                    DisplayAllProducts(); // Показать все товары
                    break;
                case "2":
                    AddProduct(); // Добавить товар
                    break;
                case "3":
                    RemoveProduct(); // Удалить товар
                    break;
                case "4":
                    SupplyProduct(); // Заказать поставку
                    break;
                case "5":
                    SellProduct(); // Продать товар
                    break;
                case "6":
                    SearchProducts(); // Поиск товаров
                    break;
                case "0":
                    Console.WriteLine("До свидания!"); // Выход
                    return;
                default:
                    Console.WriteLine("Неверный выбор. Нажмите любую клавишу...");
                    Console.ReadKey(); // Ожидание нажатия
                    break;
            }
        }
    }

    // Метод для отображения всех товаров
    static void DisplayAllProducts()
    {
        Console.Clear();
        Console.WriteLine("=== ВСЕ ТОВАРЫ ===");

        var products = productManager.GetAllProducts(); // Получение всех товаров
        if (products.Count == 0) // Проверка на пустой список
        {
            Console.WriteLine("Товары не найдены.");
        }
        else
        {
            foreach (var product in products) // Перебор всех товаров
            {
                Console.WriteLine(product.GetProductInfo()); // Вывод информации о товаре
            }
        }

        Console.WriteLine("\nНажмите любую клавишу для возврата...");
        Console.ReadKey(); // Ожидание нажатия
    }

    // Метод для добавления товара (интерактивный)
    static void AddProduct()
    {
        Console.Clear();
        Console.WriteLine("=== ДОБАВЛЕНИЕ ТОВАРА ===");

        try // Обработка исключений
        {
            Console.Write("Введите название товара: ");
            var name = Console.ReadLine(); // Чтение названия

            Console.Write("Введите цену товара: ");
            var priceInput = Console.ReadLine(); // Чтение цены
            if (!decimal.TryParse(priceInput, out decimal price) || price <= 0) // Валидация цены
            {
                Console.WriteLine("Ошибка: цена должна быть положительным числом");
                return; // Выход при ошибке
            }

            Console.Write("Введите количество: ");
            var quantityInput = Console.ReadLine(); // Чтение количества
            if (!int.TryParse(quantityInput, out int quantity) || quantity < 0) // Валидация количества
            {
                Console.WriteLine("Ошибка: количество должно быть неотрицательным числом");
                return; // Выход при ошибке
            }

            Console.WriteLine("Доступные категории:");
            foreach (Category category1 in Enum.GetValues(typeof(Category))) // Перебор всех категорий
            {
                Console.WriteLine($"{(int)category1}. {category1}"); // Вывод номера и названия категории
            }

            Console.Write("Выберите категорию (номер): ");
            var categoryInput = Console.ReadLine(); // Чтение категории
            if (!Enum.TryParse(categoryInput, out Category category) || !Enum.IsDefined(typeof(Category), category)) // Валидация категории
            {
                Console.WriteLine("Ошибка: неверная категория");
                return; // Выход при ошибке
            }

            productManager.AddProduct(name, price, quantity, category); // Добавление товара
        }
        catch (Exception ex) // Обработка общих исключений
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }

        Console.WriteLine("\nНажмите любую клавишу для возврата...");
        Console.ReadKey(); // Ожидание нажатия
    }

    // Метод для удаления товара
    static void RemoveProduct()
    {
        Console.Clear();
        Console.WriteLine("=== УДАЛЕНИЕ ТОВАРА ===");

        Console.Write("Введите код товара для удаления: ");
        var code = Console.ReadLine(); // Чтение кода товара

        if (productManager.RemoveProduct(code)) // Попытка удаления
        {
            Console.WriteLine("Товар успешно удален.");
        }
        else
        {
            Console.WriteLine("Товар с указанным кодом не найден.");
        }

        Console.WriteLine("\nНажмите любую клавишу для возврата...");
        Console.ReadKey(); // Ожидание нажатия
    }

    // Метод для заказа поставки
    static void SupplyProduct()
    {
        Console.Clear();
        Console.WriteLine("=== ЗАКАЗ ПОСТАВКИ ===");

        Console.Write("Введите код товара: ");
        var code = Console.ReadLine(); // Чтение кода товара

        Console.Write("Введите количество для поставки: ");
        if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0) // Валидация количества
        {
            Console.WriteLine("Ошибка: количество должно быть положительным числом");
            return; // Выход при ошибке
        }

        productManager.SupplyProduct(code, quantity); // Вызов метода поставки

        Console.WriteLine("\nНажмите любую клавишу для возврата...");
        Console.ReadKey(); // Ожидание нажатия
    }

    // Метод для продажи товара
    static void SellProduct()
    {
        Console.Clear();
        Console.WriteLine("=== ПРОДАЖА ТОВАРА ===");

        Console.Write("Введите код товара: ");
        var code = Console.ReadLine(); // Чтение кода товара

        Console.Write("Введите количество для продажи: ");
        if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0) // Валидация количества
        {
            Console.WriteLine("Ошибка: количество должно быть положительным числом");
            return; // Выход при ошибке
        }

        productManager.SellProduct(code, quantity); // Вызов метода продажи

        Console.WriteLine("\nНажмите любую клавишу для возврата...");
        Console.ReadKey(); // Ожидание нажатия
    }

    // Метод для поиска товаров
    static void SearchProducts()
    {
        Console.Clear();
        Console.WriteLine("=== ПОИСК ТОВАРОВ ===");

        Console.Write("Введите код, название или категорию для поиска: ");
        var searchTerm = Console.ReadLine(); // Чтение поискового запроса

        if (string.IsNullOrWhiteSpace(searchTerm)) // Проверка пустого запроса
        {
            Console.WriteLine("Поисковый запрос не может быть пустым");
            return; // Выход при ошибке
        }

        var results = productManager.SearchProducts(searchTerm); // Выполнение поиска

        Console.WriteLine("\n=== РЕЗУЛЬТАТЫ ПОИСКА ===");
        if (results.Count == 0) // Проверка результатов
        {
            Console.WriteLine("Товары не найдены.");
        }
        else
        {
            foreach (var product in results) // Перебор результатов
            {
                Console.WriteLine(product.GetProductInfo()); // Вывод информации о товаре
            }
            Console.WriteLine($"\nНайдено товаров: {results.Count}"); // Вывод количества найденных
        }

        Console.WriteLine("\nНажмите любую клавишу для возврата...");
        Console.ReadKey(); // Ожидание нажатия
    }
}

