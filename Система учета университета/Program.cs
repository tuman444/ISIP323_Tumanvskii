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
        private List<Course> enrolledCourses;
        private static int nextId = 1;
        private int studentId;
        public Student(string name, int age, string email) : base(name, age, email)//конструктор класса
        {
            studentId = nextId++;
            enrolledCourses = new List<Course>();
        }

        public int StudentId => studentId;

        public void EnrollInCourse(Course course) //Метод для записи на курс
        {
            if(course == null)
                throw new ArgumentNullException(nameof(course));
            if (!enrolledCourses.Contains(course))
            {
                enrolledCourses.Add(course);
                course.AddStudent(this);
            }

        }
        public void UnenrollFromCourse(Course course) // Метод для отписки от курса
        {
            if (course != null && enrolledCourses.Contains(course))
            {
                enrolledCourses.Remove(course);
                course.RemoveStudent(this);
            }
        }
        public override string GetInfo() //переопределение GetInfo
        {
            return $"{GetBasicInfo()}, ID: {StudentId}, Количество курсов: {enrolledCourses.Count}";
        }
        public string GetCoursesInfo() // Метод для получения информации о курсах студента
        {
            if (enrolledCourses.Count == 0)
                return "Студент не записан на курсы";
            return string.Join(", ", enrolledCourses.Select(c => c.CourseName));
        }
    }

    
    public class Teacher : Person // Класс преподавателя, наследуется от Person
    {
        private List<Course> teachingCourses;
        private static int nextId = 1;
        private int teacherId;
        private string departament;
        public Teacher(string name, int age, string email, string departament) : base(name,age,email)
        {
            if (string.IsNullOrEmpty(departament)) 
                throw new ArgumentNullException("Кафедра не может быть пустой");
            teacherId = nextId++;
            this.departament = departament;
            teachingCourses = new List<Course>();
        }
        // Свойства только для чтения
        public int TeacherId => teacherId;
        public string Departament => departament;
        public void AssignToCourse(Course course) // Метод для назначения на курс
        {
            if (course == null)
                throw new ArgumentNullException(nameof(course));
            if (!teachingCourses.Contains(course))
            {
                teachingCourses.Add(course);
                course.AssignTeacher(this);
            }

        }
        public void RemoveFromCourse(Course course) // Метод для снятия с курса
        {
            if (course != null && teachingCourses.Contains(course))
            {
                teachingCourses.Remove(course);
                course.RemoveTeacher();
            }
        }
        public override string GetInfo() //Переопределение абстрактного метода 
        {
            return $"{GetBasicInfo()}, ID: {TeacherId}, Кафедра: {Department}, Количество курсов: {teachingCourses.Count}";
        }
    }

    public class Course // класс курсов
    {
        
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