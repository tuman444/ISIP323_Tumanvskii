using CarServiceGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServiceDB
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                GameManager.InitializeGame();

                // Основной игровой цикл
                while (true)
                {
                    // 1. Сначала обновляем доставки (задание "спустя 2 машины")
                    GameManager.UpdateDeliveries();

                    // 2. Обрабатываем нового клиента
                    GameManager.ProcessClientTurn();

                    // 3. Проверяем, не проиграли ли мы
                    if (GameManager.CheckGameOver())
                    {
                        break; // Выход из цикла, если игра окончена
                    }

                    // 4. Предлагаем зайти в магазин
                    GameManager.ShowShop();

                    Console.WriteLine("\nНажмите Enter, чтобы принять следующего клиента...");
                    Console.ReadLine();
                }
            }
            
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\n!!! КРИТИЧЕСКАЯ ОШИБКА !!!");
                Console.WriteLine(ex.Message);
                Console.WriteLine("Программа будет завершена. Обратитесь к разработчику.");
                Console.ResetColor();
            }
        

            Console.WriteLine("Нажмите Enter для выхода...");
            Console.ReadLine();
        }
    }
}
