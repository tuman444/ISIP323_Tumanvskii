namespace RogueLike
{
    public abstract class Item // класс для всех предметов
    {
        public string Name { get; protected set; }
        public Item (string name)
        {
            Name = name;
        }
        public abstract void Use(Player player);
    }
    public class Weapon : Item //класс оружия
    {
        public int Damage {  get; private set; }
        public  Weapon(string name, int damage) : base (name)
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

    }
    public abstract class Enemy // класс врагов
    {

    }
    public class Goblin : Enemy // класс врагов гоблин, скелет и маг с наледием от главного класса врагов
    { 

    }
    public class Skeleton : Enemy 
    {
        
    }
    public class Mage : Enemy
    {

    }
    public abstract class Boss : Enemy //босс
    {

    } 
    public class VVG : Goblin // конкретные боссы
    {

    } 
    public class Kovalsky : Skeleton
    {

    }
    public class ArchmageCPP : Mage
    {

    }
    public class PestovC : Skeleton
    {

    }
    public class Chest // класс сундука
    {

    }
    public class Game //главный класс игры
    {

    }
    class Program
    {
        static void Main(string[] args)
        {

        }
    }
}