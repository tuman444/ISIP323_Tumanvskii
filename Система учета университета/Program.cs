using System.Xml.Linq;

namespace UniversityManagementSystem
{
    public abstract class Person //класс для всех людей в университете
    {
        private string name;
        private int age;
        private string email;
        public Person(string name, int age, string email) //конструктор класса
        {
            if(string.IsNullOrEmpty(name))
                throw new ArgumentException("Имя не должно быть пустым");
            if(age < 0 || age > 100)
                throw new ArgumentException("Взраст должен быть не меньше 0 и не больше 100");
            if (string.IsNullOrEmpty(email))
                throw new ArgumentException("email не должен быть пустым");
            this.name = name;
            this.age = age;
            this.email = email;
        }
        public string Name // Свойства с защищенным set для инкапсуляции
        {
            get => name;
            protected set => name = value;
        }
        public int Age
        {
            get => age;
            protected set => age = value;
        }
        public string Email
        {
            get => email;
            protected set => email = value;
        }
        // Абстрактный метод для полиморфизма 
        public abstract string GetInfo();
        // Виртуальный метод 
        public virtual string GetBasicInfo()
        {
            return $"Имя: {Name}, Возраст: {Age}, Email: {Email}";
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
        private void UnenrollStudentFromCourse() // метод для снятия студента с курса
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

        public void Run() //Основной метод запуска интерфейса
        {

        }
        private void ShowMainMenu() // Метод для отображения главного меню
        {

        }
        private int GetUserChoice() // Метод для получения выбора пользователя
        {

        }
        private void AddStudent() //Метод для добавления студента
        {

        }
        private void AddTeacher() // Метод для добавления преподавателя
        {

        }
        private void AddCourse() // Метод для добавления курса
        {

        }
        private void ShowAllStudents() 
        { 

        }//Метод для отображения всех студентов
        private void ShowAllTeachers() // Метод для отображения всех преподавателей
        {

        }
        private void ShowAllCourse() // Метод для отображения всех курсов
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