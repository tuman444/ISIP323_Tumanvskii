namespace RogueLike
{
    public abstract class Item // класс для всех предметов
    {

    }
    public class Weapon : Item //класс оружия
    {

    }
    public class Armor : Item // класс оружия
    {

    }
    public class Potion : Item //класс лечебного зелья
    {

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
}