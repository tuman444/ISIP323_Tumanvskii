using CarServiceDB;
using PR7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace CarServiceGame
{
    public static class GameManager 
    {
        private static Random _random = new Random();

        public static void InitializeGame() //Метод для первоначальной проверки и настройки игры
        {
            Console.WriteLine("Добро пожаловать в Ваш Автосервис!");
            
            if (!Core.Context.GameStatus.Any())
            {
                Console.WriteLine("ОШИБКА: База данных не заполнена! Заполните GameStatus.");
                Environment.Exit(1);
            }
            if (!Core.Context.PartTypes.Any())
            {
                Console.WriteLine("ОШИБКА: В базе нет ни одной запчасти (PartTypes)!");
                Environment.Exit(1);
            }
        }
        public static void UpdateDeliveries() // Обновление отложенных поставок
        {

        }
        public static void ProcessClientTurn() // обработка одного хода
        {

        }
        public static void ShowShop() // Показ меню магазина для закупки деталей
        {

        }
        public static bool CheckGameOver() // Проверка условия проигрыша
        {

        }
    }
}
