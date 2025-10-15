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


        public void AddStudent() //Метод для добавления студента
        {

        }
        public void RemoveStudent() // Метод для удаления студента
        {

        }
        public void AssignTeacher() // Метод для назначения преподавателя
        {

        }
        public void RemoveTeacher() //Метод для удаления преподавателя
        {

        }
        public string GetCourseInfo() //Метод для получения информации о курсе
        {

        }
    }

    public class UniversManager //основной класс для управления системы университета
    {

        public UniversManager()
        {

        }

        public void AddStudent() // Методы для добавления сущностей
        {

        }
        public void AddTeacher()
        {

        }
        public void AddCourse()
        {

        }
        public Student FindStudentById() //Метод для поиска студента по ID
        {

        }
        public Teacher FindTeacherById() // Метод для поиска преподавателя по ID
        {

        }
        public Course FindCourseByCode() // Метод для поиска курса по коду
        {

        }

        public void EnrollStudentInCourse() // Метод для записи студента на курс
        {

        }
        public void AssignTeacherToCourse() // Метод для назначения преподавателя на курс
        {

        }
        public void RemoveTeacherFromCourse() // Метод для снятия преподавателя с курса
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