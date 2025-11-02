using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using PR8; 

namespace MarketPlace
{
    internal class Program
    {
        // переменная для хранения данных о вошедшем пользователе.
        // null == "Гость"
        private static Users currentUser = null;

        #region Методы Регистрации, Входа и Выхода
        private static void Register() // метод регистрации
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("== Регистрация нового пользователя ==");

            Console.Write("Введите Username: ");
            string username = Console.ReadLine();

            Console.Write("Введите Email: ");
            string email = Console.ReadLine();

            Console.Write("Введите пароль: ");
            string pass1 = Console.ReadLine();

            Console.Write("Подтвердите пароль: ");
            string pass2 = Console.ReadLine();
            
            if (pass1 == pass2) // проверка на совпадение паролей
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ошибка: Пароли не совпадают!");
                Console.ReadLine();
            }
            if (Core.Context.Users.Any(u => u.Username == username || u.Email == email)) //Проверка, занят ли Email или Username
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ошибка: Пользователь с таким Email или Username уже существует!");
                Console.ReadLine();
            }
            string passHash = SimpleHash(pass1); // хеширование пароля
            
            Users newUser = new Users // Добавление пользователя в "Users"
            {
                Username = username,
                Email = email,
                PasswordHash = passHash
            };
            Core.Context.Users.Add(newUser);

            try // сохраняем пользователя
            {
                Core.Context.SaveChanges();

                // 6d. Создание корзины для пользователя
                Carts newCart = new Carts
                {
                    UserID = newUser.UserID // <-- UserID стал доступен после SaveChanges()
                };
                Core.Context.Carts.Add(newCart);
                Core.Context.SaveChanges();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Регистрация прошла успешно! Теперь вы можете войти.");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Ошибка при сохранении в БД: {ex.Message}");
            }

            Console.ReadLine();
        }
        private static void Login() // метод входа в аккаунт
        {
            Console.Clear();
            Console.ForegroundColor= ConsoleColor.Blue;
            Console.WriteLine("== Вход в аккаунт ==");

            Console.Write("Введите Email: ");
            string email = Console.ReadLine();

            Console.Write("Введите пароль: ");
            string password = Console.ReadLine();

            string passHash = SimpleHash(password); // Хэшируем введенный пароль

            Users user = Core.Context.Users.FirstOrDefault(u => u.Email == email && u.PasswordHash == passHash); // Ищем пользователя в БД

            if (user == null) // Если не найден
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ошибка: Неверный Email или пароль!");
                Console.ReadLine();
            }
            else // Если найден - сохраняем его в currentUser
            {
                currentUser = user;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Добро пожаловать, {currentUser.Username}!");
                Console.ReadLine();
            }
        }
        private static void Logout() // метод выхода из аккаунта
        {
            currentUser = null;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Вы вышли из аккаунта.");
            Console.ReadLine();
        }
        #endregion

        #region Методы просмотра товаров и корзины
        private static void ViewProducts() //метод просмотра товаров
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("== Список доступных товаров ==");

            var products = Core.Context.Products // Получаем товары из БД
                .Where(p => p.StockQuantity > 0) // Показываем только то, что в наличии
                .ToList();

            if (!products.Any())
            {
                Console.WriteLine("Товаров в наличии нет.");
                Console.ReadLine();
            }

            foreach (var p in products) // выводим список
            {
                Console.WriteLine($"[ID: {p.ProductID}] {p.Name} - {p.Price:C}");
                Console.WriteLine($"    (Остаток: {p.StockQuantity} шт.) Описание: {p.Description}");
                Console.WriteLine();
            }

            if (currentUser != null) //Если пользователь авторизован, предлагаем добавить в корзину
            {
                Console.WriteLine("-----------------------------------");
                Console.Write("Введите ID товара для добавления в корзину (или 0 для возврата): ");
                string choice = Console.ReadLine();

                if (int.TryParse(choice, out int productID) && productID != 0)
                {
                    AddToCart(productID); // Вызываем метод добавления в корзину
                }
            }
            else
            {
                Console.WriteLine("Войдите в аккаунт, чтобы добавлять товары в корзину.");
                Console.ReadLine();
            }
        }
        private static void AddToCart(int productID) // метод добавления товара в корзину
        {
            var product = Core.Context.Products.Find(productID); // Находим продукт в БД

            if (product == null || product.StockQuantity <= 0) // Проверка, что такой товар есть и он в наличии
            {
                Console.ForegroundColor= ConsoleColor.Red;
                Console.WriteLine("Такого товара нет или он закончился.");
                Console.ReadLine();
            }

            Console.Write($"Введите количество (доступно: {product.StockQuantity}): "); // Запрос количества
            if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0) 
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Неверное количество.");
                Console.ReadLine();
            }

            if (quantity > product.StockQuantity) // Проверка остатка
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Ошибка: Недостаточно товара на складе (Остаток: {product.StockQuantity})");
                Console.ReadLine();
            }

            var cart = Core.Context.Carts.FirstOrDefault(c => c.UserID == currentUser.UserID); // Находим CartID пользователя
            if (cart == null)
            {
                // Этого не должно случиться, если мы создаем корзину при регистрации
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Критическая ошибка: Корзина не найдена!");
                Console.ReadLine();
                return;
            }

            // Проверяем, есть ли уже этот товар в корзине
            var cartItem = Core.Context.CartItems
                .FirstOrDefault(ci => ci.CartID == cart.CartID && ci.ProductID == productID);

            if (cartItem != null)
            {
                // Если да - обновить количество
                cartItem.Quantity += quantity;
            }
            else
            {
                // Если нет - добавить новую запись
                cartItem = new CartItems
                {
                    CartID = cart.CartID,
                    ProductID = productID,
                    Quantity = quantity
                };
                Core.Context.CartItems.Add(cartItem);
            }

            try
            {
                Core.Context.SaveChanges();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Товар '{product.Name}' (x{quantity}) добавлен в корзину.");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Ошибка сохранения: {ex.Message}");
            }

            Console.ReadLine();
        }
        private static void ViewCart() // метод просмотры корзины
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("== Моя корзина ==");

            // Находим CartID
            var cart = Core.Context.Carts.FirstOrDefault(c => c.UserID == currentUser.UserID);

            // Получаем содержимое корзины
            var items = Core.Context.CartItems
                .Where(ci => ci.CartID == cart.CartID)
                .Include(ci => ci.Products) 
                .ToList();

            if (!items.Any())
            {

                Console.WriteLine("Ваша корзина пуста.");
                Console.ReadLine();
                return;
            }

            decimal totalPrice = 0;

            // Выводим список
            foreach (var item in items)
            {
                if (item.Products != null)
                {
                    decimal itemTotalPrice = item.Products.Price * item.Quantity;
                    Console.WriteLine($"Товар: {item.Products.Name}");
                    Console.WriteLine($"   Кол-во: {item.Quantity} x {item.Products.Price:C} = {itemTotalPrice:C}");
                    totalPrice += itemTotalPrice;
                }
            }

            // Выводим итог
            Console.WriteLine("-----------------------------------");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Итоговая сумма: {totalPrice:C}");
            Console.WriteLine();

            // Предлогаем оформить заказ
            Console.Write("Хотите оформить заказ? (Да/Нет): ");
            string choice = Console.ReadLine();

            if (choice.Equals("Да", StringComparison.OrdinalIgnoreCase))
            {
                // Переход к оформлению заказа
                CreateOrder(items, totalPrice);
            }
        }
        #endregion

        #region Методы для Заказа
        private static void CreateOrder(System.Collections.Generic.List<CartItems> items, decimal totalPrice) // оформление заказа
        {
            // Начинаем транзакцию. Либо все операции пройдут, либо ни одной.
            using (var transaction = Core.Context.Database.BeginTransaction())
            {
                try
                {
                    // (Блокировка/Проверка)
                    // Мы должны "перечитать" данные о товарах из БД внутри транзакции
                    // чтобы убедиться, что их не купил кто-то другой, пока мы смотрели корзину.
                    foreach (var item in items)
                    {
                        var productInDb = Core.Context.Products.Find(item.ProductID);
                        if (productInDb.StockQuantity < item.Quantity)
                        {
                            // Откат
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Ошибка: Товара '{productInDb.Name}' не осталось на складе (Остаток: {productInDb.StockQuantity}).");
                            Console.WriteLine("Заказ отменен.");
                            transaction.Rollback();
                            Console.ReadLine();
                            return;
                        }
                    }

                    // Выбор ПВЗ
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("== Выбор пункта выдачи заказов (ПВЗ) ==");
                    var pickupPoints = Core.Context.PickupPoints.ToList();
                    if (!pickupPoints.Any())
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Ошибка: Нет доступных ПВЗ. Заказ невозможен.");
                        transaction.Rollback();
                        Console.ReadLine();
                        return;
                    }

                    foreach (var p in pickupPoints)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"[ID: {p.PickupPointID}] {p.Address} (Часы работы: {p.OperatingHours})");
                    }

                    // Выбор
                    int pickupID = 0;
                    while (true)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("Введите ID ПВЗ: ");
                        if (int.TryParse(Console.ReadLine(), out pickupID) && pickupPoints.Any(p => p.PickupPointID == pickupID))
                        {
                            break; // Выбор верный
                        }
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Неверный ID. Попробуйте снова.");
                    }

                    // Создание заказа (Orders)
                    Orders newOrder = new Orders
                    {
                        UserID = currentUser.UserID,
                        PickupPointID = pickupID,
                        OrderDate = DateTime.Now,
                        Status = "В обработке",
                        TotalPrice = totalPrice
                    };
                    Core.Context.Orders.Add(newOrder);

                    // Сохраняем, чтобы получить OrderID
                    Core.Context.SaveChanges();

                    // Перенос товаров из корзины (items) в OrderItems
                    foreach (var item in items)
                    {
                        // Создаем OrderItem
                        OrderItems orderItem = new OrderItems
                        {
                            OrderID = newOrder.OrderID, // <-- ID из созданного заказа
                            ProductID = item.ProductID,
                            Quantity = item.Quantity,
                            PriceAtPurchase = item.Products.Price // <-- Цена на момент покупки
                        };
                        Core.Context.OrderItems.Add(orderItem);

                        // Уменьшаем остаток на складе
                        var productToUpdate = Core.Context.Products.Find(item.ProductID);
                        productToUpdate.StockQuantity -= item.Quantity;
                    }

                    // Очистить корзину пользователя
                    Core.Context.CartItems.RemoveRange(items);

                    // Сохраняем все изменения (OrderItems, Склад) и Подтверждаем транзакцию
                    Core.Context.SaveChanges();
                    transaction.Commit();

                    // Успех
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Заказ №{newOrder.OrderID} успешно создан!");
                    Console.ReadLine();
                }
                catch (Exception ex)
                {
                    // Если что-то пошло не так на любом этапе - откат
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Критическая ошибка при оформлении заказа: {ex.Message}");
                    transaction.Rollback();
                    Console.ReadLine();
                }
            }
        }
        private static void ViewOrderHistory() // проосмотр истории заказов 
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("== Моя история заказов ==");

            // Получить список заказов пользователя
            var orders = Core.Context.Orders
                .Where(o => o.UserID == currentUser.UserID)
                .OrderByDescending(o => o.OrderDate) // 1. Сортировка по дате (сначала новые)
                .ToList();

            // Если заказов нет
            if (!orders.Any())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("У вас пока нет заказов.");
                Console.ReadLine();
                return;
            }

            // Вывести список
            foreach (var order in orders)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Заказ: №{order.OrderID} от {order.OrderDate.ToShortDateString()}");
                Console.WriteLine($"   Статус: {order.Status}, Сумма: {order.TotalPrice:C}");
            }
            Console.WriteLine("-----------------------------------");

            // Детализация заказа
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("Введите ID заказа для просмотра деталей (или 0 для возврата): ");
            if (int.TryParse(Console.ReadLine(), out int orderID) && orderID != 0)
            {
                ShowOrderDetails(orderID);
            }
        }
        private static void ShowOrderDetails(int orderID) // Метод для показа деталей конкретного заказа
        {
            // Находим заказ, но только если он принадлежит текущему пользователю
            var order = Core.Context.Orders
                .Include(o => o.PickupPoints) // Загружаем связанный ПВЗ
                .FirstOrDefault(o => o.OrderID == orderID && o.UserID == currentUser.UserID);

            if (order == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Заказ не найден или он вам не принадлежит.");
                Console.ReadLine();
                return;
            }

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"== Детали заказа №{order.OrderID} ==");
            Console.WriteLine($"Дата: {order.OrderDate}, Статус: {order.Status}, Сумма: {order.TotalPrice:C}");
            Console.WriteLine($"Пункт выдачи: {order.PickupPoints.Address} ({order.PickupPoints.OperatingHours})");
            Console.WriteLine();
            Console.WriteLine("Состав заказа:");

            // Получаем детали (товары)
            var orderItems = Core.Context.OrderItems
                .Where(oi => oi.OrderID == orderID)
                .Include(oi => oi.Products) // Загружаем связанные Товары
                .ToList();

            // Выводим детали
            foreach (var item in orderItems)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($" - {item.Products.Name} (x{item.Quantity} шт. по {item.PriceAtPurchase:C})");
            }

            Console.ReadLine();
        }
        #endregion

        #region Главный метод вывода меню
        private static void MainLoop() // вывод меню
        {
            while (true)
            {
                // Очищаем консоль для красоты
                Console.Clear();

                if (currentUser == null)
                {
                    // Если пользователь - Гость
                    ShowGuestMenu();
                }
                else
                {
                    // Если пользователь авторизован
                    ShowUserMenu();
                }
            }
        }
        private static void ShowGuestMenu() // меню для не зарегистрированого пользователя
        {
            Console.ForegroundColor= ConsoleColor.Green;
            Console.WriteLine("Добро пожаловать в маркетплейс WONGG!");
            Console.WriteLine("1. Войти в аккаунт");
            Console.WriteLine("2. Зарегистрироваться");
            Console.WriteLine("3. Просмотреть товары");
            Console.WriteLine("0. Выйти из программы");
            Console.Write("Выберите действие: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Login(); // Переход к методу входа
                    break;
                case "2":
                    Register(); // Переход к методу регистрации
                    break;
                case "3":
                    ViewProducts(); // Переход к просмотру товаров
                    break;
                case "0":
                    Environment.Exit(0); // Выход
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Неверный выбор. Нажмите Enter для продолжения.");
                    Console.ReadLine();
                    break;
            }
        }
        private static void ShowUserMenu() // меню для зарегистрированого пользователя
        {
            Console.ForegroundColor= ConsoleColor.Green;
            Console.WriteLine($"Вы вошли как: {currentUser.Username}");
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("1. Просмотреть товары (и добавить в корзину)");
            Console.WriteLine("2. Просмотреть мою корзину");
            Console.WriteLine("3. Просмотреть историю моих заказов");
            Console.WriteLine("9. Выйти из аккаунта");
            Console.WriteLine("0. Выйти из программы");
            Console.Write("Выберите действие: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ViewProducts(); // Просмотр товаров
                    break;
                case "2":
                    ViewCart(); // Просмотр корзины
                    break;
                case "3":
                    ViewOrderHistory(); // Просмотр заказов
                    break;
                case "9":
                    Logout(); // Выход из аккаунта
                    break;
                case "0":
                    Environment.Exit(0); // Выход
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Неверный выбор. Нажмите Enter для продолжения.");
                    Console.ReadLine();
                    break;
                }
            }
        #endregion

        private static string SimpleHash(string password)
        {
            char[] charArray = password.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        static void Main(string[] args)
        {
            MainLoop();
        }
    }
}
