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
    }
    public class Weapon : Item //класс доспех
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
    }
    public class Armor : Item // класс оружия
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
    }
    public class Player // класс игрока
    {
        public int HP { get; private set; }
        public int MaxHP { get; private set; }
        public Weapon CurrentWeapon { get; private set; }
        public Armor CurrentArmor { get; private set; }
        public bool IsFrozen { get; set; }
        public bool IsDefending { get; set; }

        public Player(int maxHP)
        {
            MaxHP = maxHP;
            HP = maxHP;
            // Стартовое снаряжение
            CurrentWeapon = new Weapon("Кулаки", 5);
            CurrentArmor = new Armor("Одежда", 2);
        }

        public void Attack(Enemy enemy) { }
        public void Defend() { }
        public void TakeDamage(int damage, bool ignoreArmor = false) { }
        public void Heal(int amount) { }
        public void EquipWeapon(Weapon weapon) { }
        public void EquipArmor(Armor armor) { }
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