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
                    // Поставка прибыла
                    var warehouseItem = Core.Context.WarehouseItems.FirstOrDefault(w => w.PartTypeID == delivery.PartTypeID);

                    var partName = Core.Context.PartTypes.First(p => p.PartTypeID == delivery.PartTypeID).Name;

                    if (warehouseItem != null)
                    {
                        warehouseItem.Quantity += delivery.Quantity;
                    }
                    else
                    {
                        //  добавляем новую запись
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
        
        public static void ProcessClientTurn() // обработка одного хода
        {
            var game = Core.Context.GameStatus.First();
            Console.WriteLine($"\n--- Новый клиент! --- Ваш баланс: {game.Balance:C}");

            int maxPartId = Core.Context.PartTypes.Max(p => p.PartTypeID);
            int randomPartId = _random.Next(1, maxPartId + 1);

            var neededPart = Core.Context.PartTypes.First(p => p.PartTypeID == randomPartId);

            decimal repairCost = neededPart.ShopPrice + neededPart.LaborCost;
            decimal penaltyRefuse = neededPart.ShopPrice * 0.5m; // Штраф за отказ (пример)
            decimal penaltyFail = repairCost * 1.5m; // Штраф за "критическую ошибку" (пример)

            Console.WriteLine($"Клиент приехал с поломкой: '{neededPart.Name}'");
            Console.WriteLine($"Стоимость ремонта для клиента: {repairCost:C} (деталь {neededPart.ShopPrice:C} + работа {neededPart.LaborCost:C})");

            var partInStock = Core.Context.WarehouseItems.FirstOrDefault(w => w.PartTypeID == neededPart.PartTypeID && w.Quantity > 0); // Проверяем наличие на складе

            if (partInStock != null)
            {
                Console.WriteLine($"У вас на складе: {partInStock.Quantity} шт.");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("У вас на складе НЕТ такой детали!");
                Console.ResetColor();
            }

            Console.WriteLine("\nВаши действия:");
            Console.WriteLine("1. Принять заказ");
            Console.WriteLine("2. Отказаться от заказа (штраф {0:C})", penaltyRefuse);

            string choice = Console.ReadLine();

            if (choice == "1")
            {
                if (partInStock != null)
                {
                    partInStock.Quantity--; // УСПЕХ: Деталь есть
                    game.Balance += repairCost;
                    Core.Context.SaveChanges();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\nУспешный ремонт! +{repairCost:C}.");
                    Console.WriteLine($"Деталей '{neededPart.Name}' осталось: {partInStock.Quantity} шт.");
                    Console.ResetColor();
                }
                else
                {
                    game.Balance -= penaltyFail;  // КРИТИЧЕСКАЯ ОШИБКА: Детали нет, но заказ приняли
                    Core.Context.SaveChanges();
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine($"\nКРИТИЧЕСКИЙ ПРОВАЛ! Вы взяли заказ без детали!");
                    Console.WriteLine($"Вы заплатили неустойку клиенту: -{penaltyFail:C}");
                    Console.ResetColor();
                }
            }
            else 
            {
                game.Balance -= penaltyRefuse; //отказ от заказа
                Core.Context.SaveChanges();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\nВы отказались от заказа. Штраф: -{penaltyRefuse:C}");
                Console.ResetColor();
            }
        }
        public static void ShowShop() // Показ меню магазина для закупки деталей
        {
            Console.WriteLine("\nХотите зайти в магазин запчастей? (y/n)");
            if (Console.ReadLine().ToLower() != "y")
            {
                return;
            }
            var game = Core.Context.GameStatus.First();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n[--- МАГАЗИН ЗАПЧАСТЕЙ ---]");
            Console.WriteLine($"Ваш баланс: {game.Balance:C}");

            var allParts = Core.Context.PartTypes.ToList();
            foreach (var part in allParts)
            {
                Console.WriteLine($"{part.PartTypeID}. {part.Name} - {part.ShopPrice:C} / шт.");
            }
            Console.WriteLine("0. Выйти из магазина");

            while (true)
            {
                Console.Write("Введите ID детали для покупки (или 0): ");
                if (!int.TryParse(Console.ReadLine(), out int partId) || partId == 0)
                {
                    break;
                }

                var partToBuy = allParts.FirstOrDefault(p => p.PartTypeID == partId);
                if (partToBuy == null)
                {
                    Console.WriteLine("Такой детали нет!");
                    continue;
                }

                Console.Write($"Сколько '{partToBuy.Name}' хотите купить? ");
                if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
                {
                    Console.WriteLine("Неверное количество.");
                    continue;
                }

                decimal totalCost = partToBuy.ShopPrice * quantity;
                if (game.Balance < totalCost)
                {
                    Console.WriteLine($"Недостаточно денег! Нужно {totalCost:C}, у вас {game.Balance:C}");
                    continue;
                }

                // Покупка
                game.Balance -= totalCost;

                // Добавляем в отложенную поставку
                Core.Context.PendingDeliveries.Add(new PendingDeliveries
                {
                    PartTypeID = partToBuy.PartTypeID,
                    Quantity = quantity,
                    ClientsToWait = 2 // Поставка прибудет через 2 клиента
                });

                Core.Context.SaveChanges();
                Console.WriteLine($"\nУспешно куплено: {partToBuy.Name} (x{quantity}) за {totalCost:C}");
                Console.WriteLine("Поставка прибудет через 2-х клиентов.");
                Console.WriteLine($"Остаток баланса: {game.Balance:C}");
            }

            Console.WriteLine("[--- Выход из магазина ---]");
            Console.ResetColor();
        }
        
        public static bool CheckGameOver() // Проверка условия проигрыша
        {
            var game = Core.Context.GameStatus.First();
            var totalParts = Core.Context.WarehouseItems.Sum(w => (int?)w.Quantity) ?? 0;

            // Если нет деталей, проверяем, можем ли мы купить самую дешевую
            if (totalParts == 0)
            {
                decimal cheapestPartPrice = Core.Context.PartTypes.Min(p => p.ShopPrice);
                if (game.Balance < cheapestPartPrice)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n================== ИГРА ОКОНЧЕНА ==================");
                    Console.WriteLine("У вас не осталось запчастей на складе и не хватает денег,");
                    Console.WriteLine($"чтобы купить даже самую дешевую деталь ({cheapestPartPrice:C}).");
                    Console.WriteLine($"Ваш финальный баланс: {game.Balance:C}");
                    Console.WriteLine("====================================================");
                    Console.ResetColor();
                    return true;
                }
            }
            return false;
        }
    }
}
