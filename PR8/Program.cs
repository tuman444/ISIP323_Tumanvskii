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
        private static void CreateOrder() // оформление заказа
        {

        }
        private static void ViewOrderHistory() // проосмотр истории заказов 
        {

        }
        private static void ShowOrderDetails() // Метод для показа деталей конкретного заказа
        {

        }
        #endregion

        #region Главный метод вывода меню
        private static void MainLoop() // вывод меню
        {

        }
        private static void ShowGuestMenu() // меню для не зарегистрированого пользователя
        {

        }
        private static void ShowUserMenu() // меню для зарегистрированого пользователя
        {

        }
        #endregion

        static void Main(string[] args)
        {
        }
    }
}
