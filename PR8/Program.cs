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

        }
        private static void AddToCart() // метод добавления товара в корзину
        {

        }
        private static void ViewCart() // метод просмотры корзины
        {

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
