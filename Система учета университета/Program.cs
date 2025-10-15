using System.Xml.Linq;

namespace UniversityManagementSystem
{
    public abstract class Person //класс для всех людей в университете
    {
        public Person() //конструктор класса
        {

        }
        // Абстрактный метод для полиморфизма 
        public abstract string GetInfo();
        // Виртуальный метод 
        public virtual string GetBasicInfo()
        {

        }

    }

    public class Student : Person // Класс студентоа, наследуется от Person
    {
        public Student() //конструктор класса
        {

        }
        public void EnrollInCourse() //Метод для записи на курс
        {

        }
        public void UnenrollFromCourse() // Метод для отписки от курса
        {

        }
        public override string GetInfo() //переопределение GetInfo
        {

        }
        public string GetCoursesInfo()
        {

        }
    }

    
    public class Teacher : Person // Класс преподавателя, наследуется от Person
    {
        public Teacher()
        {

        }

        public void AssignToCourse() // Метод для назначения на курс
        {

        }
        public void RemoveFromCourse() // Метод для снятия с курса
        {

        }
        public override string GetInfo() //Переопределение абстрактного метода 
        {

        }
    }

    public class Course // класс курсов
    {

        public Course()
        {

        }
    }

    public class UniversManager //основной класс для управления системы университета
    {

        public UniversManager()
        {

        }
    }

    public class ConsoleInterface // Класс для работы с консольным интерфейсом
    {
        public ConsoleInterface()
        {

        }   
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Создание и запуск консольного интерфейса
            var consoleInterface = new ConsoleInterface();

        }
    }
}