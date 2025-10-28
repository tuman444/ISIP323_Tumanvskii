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
            Console.ForegroundColor = ConsoleColor.Green;
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
        public static void UpdateDeliveries() // Обновление поставок
        {
            var deliveries = Core.Context.PendingDeliveries.ToList();
            if (!deliveries.Any()) return;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n[--- Обновление поставок ---]");

            foreach (var delivery in deliveries)
            {
                delivery.ClientsToWait--;
                if (delivery.ClientsToWait <= 0)
                {
                    // Поставка прибыла!
                    var warehouseItem = Core.Context.WarehouseItems
                        .FirstOrDefault(w => w.PartTypeID == delivery.PartTypeID);

                    var partName = Core.Context.PartTypes
                        .First(p => p.PartTypeID == delivery.PartTypeID).Name;

                    if (warehouseItem != null)
                    {
                        warehouseItem.Quantity += delivery.Quantity;
                    }
                    else
                    {
                        // Этой запчасти у нас не было, добавляем новую запись
                        Core.Context.WarehouseItems.Add(new WarehouseItems
                        {
                            PartTypeID = delivery.PartTypeID,
                            Quantity = delivery.Quantity
                        });
                    }
                    Console.WriteLine($"Прибыла поставка: {partName} (x{delivery.Quantity})!");
                    Core.Context.PendingDeliveries.Remove(delivery);
                }
            }
            Core.Context.SaveChanges(); // Сохраняем все изменения (и уменьшение счетчика, и удаление)
            Console.WriteLine("[-----------------------------]");
            Console.ResetColor();
        }
        }
        public static void ProcessClientTurn() // обработка одного хода
        {

        }
        public static void ShowShop() // Показ меню магазина для закупки деталей
        {

        }
        public static bool CheckGameOver() // Проверка условия проигрыша
        {
            return true;
        }
    }
}
