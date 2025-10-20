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
            return $"{Name} (Урон: {Damage}";
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
        public int Attack { get; protected set; }
        public int Defense { get; protected set; }
        public Enemy(string name, int hp, int attack, int defense)
        {
            Name = name;
            HP = hp;
            Attack = attack;
            Defense = defense;
        }

        public abstract void PerformAttack(Player player);
        public abstract void SpecialAbility(Player player);
        public void TakeDamage(int damage) { }
        public bool IsAlive() { return HP > 0; }
    }
    public class Goblin : Enemy // класс врагов гоблин, скелет и маг с наледием от главного класса врагов
    {
        public double CritChance { get; private set; }
        public Goblin(string name, int hp, int attack, int defense, double critChance)
            : base(name, hp, attack, defense)
        {
            CritChance = critChance;
        }

        public override void PerformAttack(Player player) { }
        public override void SpecialAbility(Player player) { }
    }
    public class Skeleton : Enemy
    {
        public Skeleton(string name, int hp, int attack, int defense)
                    : base(name, hp, attack, defense) { }

        public override void PerformAttack(Player player)
        {
        
            player.TakeDamage(Attack, ignoreArmor: true);
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

        public override void PerformAttack(Player player) { }
        public override void SpecialAbility(Player player) { }
    }
    public abstract class Boss : Enemy //босс
    {
        public Boss(string name, int hp, int attack, int defense) : base(name, hp, attack, defense)
        {

        }
    }
    public class VVG : Goblin // конкретные боссы
    {
            public VVG() : base("ВВГ", 100, 15, 12, 0.3)
            {

            }
        public override void PerformAttack(Player player)
        {
            // Ковальский сохраняет способность скелета игнорировать защиту
            player.TakeDamage(Attack, ignoreArmor: true);
        }
    }
    public class Kovalsky : Skeleton
        {
            public Kovalsky() : base("Ковальский", 125, 13, 14)
            {

            }

    }
    public class ArchmageCPP : Mage
    {
            public ArchmageCPP() : base("Архимаг C++", 90, 16, 11, 0.25) { }
    }

    public class PestovC : Skeleton
    {
        public double FreezeChance { get; private set; }

        public PestovC() : base("Пестов С", 65, 18, 6)
        {
            FreezeChance = 0.2;
        }

        public override void PerformAttack(Player player)
        {
            player.TakeDamage(Attack, ignoreArmor: true);
        }

        public override void SpecialAbility(Player player)
        {

        }
    }
    public class Chest // класс сундука
    {
        public Item Open()
        {

        }
    }
    public class Game //главный класс игры
    {
        private Player player;
        private Random random;
        private int turnCount;

        public Game()
        {
            player = new Player(100);
            random = new Random();
            turnCount = 0;
        }

        public void StartGame() { }
        public void ProcessTurn() { }
        public void StartBattle(Enemy enemy) { }
        public void OpenChest() { }
        public Enemy GenerateRandomEnemy() { return null; }
        public Enemy GenerateRandomBoss() { return null; }
        public void ShowPlayerStatus() { }
    }
    class Program
    {
        static void Main(string[] args)
        {

        }
    }
}