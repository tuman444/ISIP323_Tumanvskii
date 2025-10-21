namespace RogueLike
{
    public abstract class Item // класс для всех предметов
    {
        public string Name { get; protected set; }
        public Item(string name)
        {
            Name = name;
        }
        public abstract void Use(Player player);
        public virtual string GetInfo()
        {
            return Name;
        }
    }
    public class Weapon : Item //класс оружия
    {
        public int Damage { get; private set; }
        public Weapon(string name, int damage) : base(name)
        {
            Damage = damage;
        }
        public override void Use(Player player)
        {
            player.EquipWeapon(this);
        }
        public override string GetInfo()
        {
            return $"{Name} (Урон: {Damage})";
        }
    }
    public class Armor : Item // класс брони
    {
        public int Defense { get; private set; }

        public Armor(string name, int defense) : base(name)
        {
            Defense = defense;
        }
        public override void Use(Player player)
        {
            player.EquipArmor(this);
        }
        public override string GetInfo()
        {
            return $"{Name} (Защита: {Defense})";
        }
    }
    public class Potion : Item //класс лечебного зелья
    {
        public int HealAmount { get; private set; }

        public Potion(string name, int healAmount) : base(name)
        {
            HealAmount = healAmount;
        }

        public override void Use(Player player)
        {
            player.Heal(HealAmount);
        }
        public override string GetInfo()
        {
            return $"{Name} (Восстановление: {HealAmount} HP)";
        }
    }
    public class Player // класс игрока
    {
        public int HP { get; private set; }
        public int MaxHP { get; private set; }
        public Weapon CurrentWeapon { get; private set; }
        public Armor CurrentArmor { get; private set; }
        public bool IsFrozen { get; set; }
        public bool IsDefending { get; set; }
        private Random random;

        public Player(int maxHP)
        {
            MaxHP = maxHP;
            HP = maxHP;
            random = new Random();
            // Стартовое снаряжение
            CurrentWeapon = new Weapon("Кулаки", 5);
            CurrentArmor = new Armor("Одежда", 2);
        }
        public void Attack(Enemy enemy)
        {
            if (IsFrozen)
            {
                Console.WriteLine("Игрок заморожен и пропускает ход!");
                IsFrozen = false;
                return;
            }

            int damage = CurrentWeapon.Damage;
            enemy.TakeDamage(damage);
            Console.WriteLine($"Вы атаковали {enemy.Name} и нанесли {damage} урона!");
        }
        public void Defend()
        {
            if (IsFrozen)
            {
                Console.WriteLine("Игрок заморожен и пропускает ход!");
                IsFrozen = false;
                return;
            }
            IsDefending = true;
            Console.WriteLine("Вы приготовились к защите!");
        }
        public void TakeDamage(int damage, bool ignoreArmor = false)
        {
            if (IsDefending)
            {
                // 40% шанс уклониться
                if (random.NextDouble() < 0.4)
                {
                    Console.WriteLine("Вы увернулись от атаки!");
                    IsDefending = false;
                    return;
                }

                // Блокирование урона
                if (!ignoreArmor)
                {
                    double blockPercent = 0.7 + (random.NextDouble() * 0.3); // 70-100%
                    int blockedDamage = (int)(CurrentArmor.Defense * blockPercent);
                    damage = Math.Max(0, damage - blockedDamage);
                    Console.WriteLine($"Вы заблокировали {blockedDamage} урона!");
                }

                IsDefending = false;
            }

            HP -= damage;
            HP = Math.Max(0, HP);
            Console.WriteLine($"Вы получили {damage} урона. Осталось HP: {HP}");
        }
        public void Heal(int amount)
        {
            HP = Math.Min(MaxHP, HP + amount);
            Console.WriteLine($"Вы восстановили {amount} HP. Теперь HP: {HP}");
        }
        public void EquipWeapon(Weapon weapon)
        {
            CurrentWeapon = weapon;
            Console.WriteLine($"Экипировано оружие: {weapon.GetInfo()}");
        }
        public void EquipArmor(Armor armor)
        {
            CurrentArmor = armor;
            Console.WriteLine($"Экипированы доспехи: {armor.GetInfo()}");
        }
        public bool IsAlive() => HP > 0;
        public void ShowStatus()
        {
            Console.WriteLine($"=== Статус игрока ===");
            Console.WriteLine($"HP: {HP}/{MaxHP}");
            Console.WriteLine($"Оружие: {CurrentWeapon.GetInfo()}");
            Console.WriteLine($"Доспехи: {CurrentArmor.GetInfo()}");
            Console.WriteLine($"=====================");
        }
    }
    public abstract class Enemy // класс врагов
    {
        public string Name { get; protected set; }
        public int HP { get; protected set; }
        public int MaxHP { get; protected set; }
        public int Attack { get; protected set; }
        public int Defense { get; protected set; }
        protected Random random;
        public Enemy(string name, int hp, int attack, int defense)
        {
            Name = name;
            HP = hp;
            MaxHP = hp;
            Attack = attack;
            Defense = defense;
            random = new Random();  
        }

        public abstract void PerformAttack(Player player);
        public abstract void SpecialAbility(Player player);
        public void TakeDamage(int damage)
        {
            HP -= damage;
            HP = Math.Max(0, HP);
            Console.WriteLine($"{Name} получил {damage} урона. Осталось HP: {HP}");
        }
        public bool IsAlive() { return HP > 0; }
        public virtual string GetInfo()
        {
            return $"{Name} (HP: {HP}/{MaxHP}, Атака: {Attack}, Защита: {Defense})";
        }
    }
    public class Goblin : Enemy // класс врагов гоблин, скелет и маг с наледием от главного класса врагов
    {
        public double CritChance { get; private set; }
        public Goblin(string name, int hp, int attack, int defense, double critChance)
            : base(name, hp, attack, defense)
        {
            CritChance = critChance;
        }
        public Goblin() : this("Гоблин", 30, 8, 5, 0.2) { }

        public override void PerformAttack(Player player)
        {
            int damage = Attack;

            if (random.NextDouble() < CritChance)
            {
                damage *= 2;
                Console.WriteLine($"{Name} наносит критический удар!");
            }
            player.TakeDamage(damage);
        }
        public override void SpecialAbility(Player player) { }
        public override string GetInfo()
        {
            return base.GetInfo() + $", Крит: {CritChance * 100}%";
        }
    }
    public class Skeleton : Enemy
    {
        public Skeleton(string name, int hp, int attack, int defense)
                    : base(name, hp, attack, defense) { }
        public Skeleton() : this("Скелет", 50, 10, 3) { }
        public override void PerformAttack(Player player)
        {

            player.TakeDamage(Attack, ignoreArmor: true);
            Console.WriteLine($"{Name} игнорирует вашу защиту!");
        }

        public override void SpecialAbility(Player player)
        {

        }
    }
    public class Mage : Enemy
    {
        public double FreezeChance { get; private set; }

        public Mage(string name, int hp, int attack, int defense, double freezeChance) : base(name, hp, attack, defense)
        {
            FreezeChance = freezeChance;
        }
        public Mage() : this("Маг", 20, 12, 2, 0.15) { }
        public override void PerformAttack(Player player)
        {
            player.TakeDamage(Attack);
        }
        public override void SpecialAbility(Player player)
        {
            if (random.NextDouble() < FreezeChance)
            {
                player.IsFrozen = true;
                Console.WriteLine($"{Name} замораживает вас! Вы пропустите следующий ход.");
            }
        }
        public override string GetInfo()
        {
            return base.GetInfo() + $", Заморозка: {FreezeChance * 100}%";
        }
    }
    public class VVG : Goblin // конкретные боссы
    {
        public VVG() : base("ВВГ", 100, 15, 12, 0.3) { }
    }
    public class Kovalsky : Skeleton
    {
        public Kovalsky() : base("Ковальский", 125, 13, 14) { }

    }
    public class ArchmageCPP : Mage
    {
        public ArchmageCPP() : base("Архимаг C++", 90, 16, 11, 0.25) { }
    }

    public class PestovC : Skeleton
    {
        public double FreezeChance { get; private set; }

        public PestovC() : base("Пестов С--", 65, 18, 6)
        {
            FreezeChance = 0.3;
        }

        public override void PerformAttack(Player player)
        {
            player.TakeDamage(Attack, ignoreArmor: true);
            Console.WriteLine($"{Name} игнорирует вашу защиту!");
        }

        public override void SpecialAbility(Player player)
        {
            if (random.NextDouble() < FreezeChance)
            {
                player.IsFrozen = true;
                Console.WriteLine($"{Name} замораживает вас своей ледяной магией! Вы пропустите следующий ход.");
            }
        }
        public override string GetInfo()
        {
            return base.GetInfo() + $", Заморозка: {FreezeChance * 100}%";
        }
    }
    public class Chest // класс сундука
    {
        private Random random;
        private List<Item> possibleItems;
        public Chest()
        {
            random = new Random();
            possibleItems = new List<Item>
            {
                new Potion("Лечебное зелье", 30),
                new Potion("Большое зелье", 50),
                new Weapon("Меч", 10),
                new Weapon("Топор", 12),
                new Weapon("Магический посох", 8),
                new Armor("Кожаные доспехи", 5),
                new Armor("Кольчуга", 8),
                new Armor("Латные доспехи", 12)
            };
        }
        public Item Open()
        {
            int index = random.Next(possibleItems.Count);
            return possibleItems[index];
        }
    }
    public class Game //главный класс игры
    {
        private Player player;
        private Random random;
        private int turnCount;
        private bool gameRunning;

        public Game()
        {
            player = new Player(1000);
            random = new Random();
            turnCount = 0;
            gameRunning = true;
        }

        public void StartGame()
        {
            Console.WriteLine("Добро пожаловать в текстовый рогалик!");
            Console.WriteLine("Каждый ход вы будете встречать либо сундук, либо врага.");
            Console.WriteLine("Каждые 10 ходов вас ждет встреча с боссом!");
            Console.WriteLine();

            while (gameRunning && player.IsAlive())
            {
                ProcessTurn();
                turnCount++;  
            }

            if (!player.IsAlive())
            {
                Console.WriteLine("Игра окончена! Вы погибли...");
            }
            else
            {
                Console.WriteLine("Игра завершена. Спасибо за игру!");
            }
        }

        public void ProcessTurn()
        {
            int currentTurn = turnCount + 1;
            Console.WriteLine($"\n=== Ход {currentTurn} ===");
            player.ShowStatus();

            // Босс каждые 10 ходов 
            if (currentTurn % 10 == 0)
            {
                Enemy boss = GenerateRandomBoss();
                Console.WriteLine($"!!! ВАМ ПОПАЛСЯ БОСС: {boss.GetInfo()} !!!");
                StartBattle(boss);
            }
            else
            {
                // Случайное событие: 50% сундук, 50% враг
                if (random.NextDouble() < 0.5)
                {
                    OpenChest();
                }
                else
                {
                    Enemy enemy = GenerateRandomEnemy();
                    Console.WriteLine($"Перед вами: {enemy.GetInfo()}");
                    StartBattle(enemy);
                }
            }

            if (player.IsAlive())
            {
                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }
        public void StartBattle(Enemy enemy) 
        {
            Console.WriteLine($"\n*** Начался бой с {enemy.Name}! ***");
            while (player.IsAlive() && enemy.IsAlive())
            {
                PlayerTurn(enemy);
                if (!enemy.IsAlive()) break;
                EnemyTurn(enemy);
            }
            if (!enemy.IsAlive())
            {
                Console.WriteLine($"\n*** {enemy.Name} побежден! ***");
            }
        }
        private void PlayerTurn(Enemy enemy)
        {
            Console.WriteLine("\n--- Ваш ход ---");
            Console.WriteLine("1. Атаковать");
            Console.WriteLine("2. Защищаться");
            Console.Write("Выберите действие: ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    player.Attack(enemy); break;
                case "2":
                    player.Defend(); break;
                default:
                    Console.WriteLine("Неверный выбор! Вы пропускаете ход.");
                    break;
            }
        } 
        private void EnemyTurn (Enemy enemy)
        {
            Console.WriteLine($"\n--- Ход {enemy.Name} ---");
            enemy.PerformAttack(player);
            enemy.SpecialAbility(player);
        }
        public void OpenChest() 
        {
            Console.WriteLine("\n*** Вы нашли сундук! ***");
            Chest chest = new Chest();
            Item item = chest.Open();
            Console.WriteLine($"В сундуке: {item.GetInfo()}");
            if (item is Potion potion)
            {
                Console.WriteLine("Использовать зелье? (y/n)");
                if (Console.ReadLine().ToLower() == "y")
                {
                    potion.Use(player);
                }
            }
            else if (item is Weapon weapon)
            {

                Console.WriteLine($"Текущее оружие: {player.CurrentWeapon.GetInfo()}");
                Console.WriteLine("Заменить оружие? (y/n)");
                if(Console.ReadLine().ToLower() == "y")
                {
                    weapon.Use(player);
                }
            }
            else if(item is Armor armor) 
            {
                Console.WriteLine($"Текущие доспехи: {player.CurrentArmor.GetInfo()}");
                Console.WriteLine("Заменить доспехи? (y/n)");
                if(Console.ReadLine().ToLower() == "y")
                {
                    armor.Use(player);  
                }
            }
        }
        public Enemy GenerateRandomEnemy()
        {
            int enemyType = random.Next(3);
            return enemyType switch
            {
                0 => new Goblin(),
                1 => new Skeleton(),
                2 => new Mage(),
                _ => new Goblin()
            };
        }
        public Enemy GenerateRandomBoss() 
        { 
            int bossType = random.Next(4);
            return bossType switch
            {
                0 => new VVG(),
                1 => new Kovalsky(),
                2 => new ArchmageCPP(),
                3 => new PestovC(),
                _ => new VVG()
            };
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.StartGame();
        }
    }
}