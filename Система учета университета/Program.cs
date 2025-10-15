using System.Linq;
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
            if(string.IsNullOrEmpty(name)) throw new ArgumentException("Имя не должно быть пустым");
            if(age < 0 || age > 100) throw new ArgumentException("Взраст должен быть не меньше 0 и не больше 100");
            if (string.IsNullOrEmpty(email)) throw new ArgumentException("email не должен быть пустым");
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
            if(course == null) throw new ArgumentNullException(nameof(course));
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
            if (enrolledCourses.Count == 0) return "Студент не записан на курсы";
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
            if (string.IsNullOrEmpty(departament)) throw new ArgumentNullException("Кафедра не может быть пустой");
            teacherId = nextId++;
            this.departament = departament;
            teachingCourses = new List<Course>();
        }
        // Свойства только для чтения
        public int TeacherId => teacherId;
        public string Departament => departament;
        public void AssignToCourse(Course course) // Метод для назначения на курс
        {
            if (course == null) throw new ArgumentNullException(nameof(course));
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
            return $"{GetBasicInfo()}, ID: {TeacherId}, Кафедра: {Departament}, Количество курсов: {teachingCourses.Count}";
        }
    }

    public class Course // класс курсов
    {
        private string courseName;
        private string courseCode;
        private int maxStudents;
        private List<Student> enrolledStudents;
        private Teacher assignedTeacher;
        private static int nextCode = 100;
        public Course(string courseName, int maxStudents)
        {
            if (string.IsNullOrEmpty(courseName)) throw new ArgumentNullException("Название курса не может быть пустым");
            if (maxStudents < 0) throw new ArgumentException("Максимальное количество студентов должно быть положительным");
            this.courseName = courseName;
            this.maxStudents = maxStudents;
            this.courseCode = $"CS{nextCode++}";
            enrolledStudents = new List<Student>();
            assignedTeacher = null;
        }
        // Свойства только для чтения
        public string CourseName => courseName;
        public string CourseCode => courseCode;
        public int MaxStudents => maxStudents;
        public int CurrentStudents => enrolledStudents.Count;
        public bool IsFull => CurrentStudents >= MaxStudents;
        public Teacher AssignedTeacher => assignedTeacher;
        public IReadOnlyList<Student> EnrolledStudents => enrolledStudents.AsReadOnly();
        public void AddStudent(Student student) //Метод для добавления студента
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student));

            if (IsFull)
                throw new InvalidOperationException("Курс заполнен");

            if (!enrolledStudents.Contains(student))
            {
                enrolledStudents.Add(student);
            }
        }
        public void RemoveStudent(Student student) // Метод для удаления студента
        {
            if (student != null && enrolledStudents.Contains(student))
            {
                enrolledStudents.Remove(student);
            }
        }
        public void AssignTeacher(Teacher teacher) // Метод для назначения преподавателя
        {
            if (teacher == null) throw new ArgumentNullException(nameof(teacher));

            assignedTeacher = teacher;
        }
        public void RemoveTeacher() //Метод для удаления преподавателя
        {
            assignedTeacher = null;
        }
        public string GetCourseInfo() //Метод для получения информации о курсе
        {
            string teacherInfo = assignedTeacher != null ? assignedTeacher.Name : "Не назначен";
            return $"Курс: {CourseName} ({CourseCode}), Преподаватель: {teacherInfo}, Студентов: {CurrentStudents}/{MaxStudents}";
        }
    }

    public class UniversManager //основной класс для управления системы университета
    {
        private List<Student> students;
        private List<Teacher> teachers;
        private List<Course> courses;

        public UniversManager()
        {
            students = new List<Student>();
            teachers = new List<Teacher>();
            courses = new List<Course>();
        }

        public void AddStudent(Student student) // Методы для добавления сущностей
        {
            if (student == null) throw new ArgumentNullException(nameof(student));
            students.Add(student);
        }
        public void AddTeacher(Teacher teacher)
        {
            if (teacher == null) throw new ArgumentNullException(nameof(teacher));
            teachers.Add(teacher);
        }
        public void AddCourse(Course course)
        {
            if (course == null) throw new ArgumentNullException(nameof(course));
            courses.Add(course);
        }
        // Методы для получения списков
        public IReadOnlyList<Student> GetAllStudents() => students.AsReadOnly();
        public IReadOnlyList<Teacher> GetAllTeachers() => teachers.AsReadOnly();
        public IReadOnlyList<Course> GetAllCourses() => courses.AsReadOnly();

        public Student FindStudentById(int id) //Метод для поиска студента по ID
        {
            return students.FirstOrDefault(s => s.StudentId == id);
        }
        public Teacher FindTeacherById(int id) // Метод для поиска преподавателя по ID
        {
            return teachers.FirstOrDefault(t => t.TeacherId == id);
        }
        public Course FindCourseByCode(string code) // Метод для поиска курса по коду
        {
            return courses.FirstOrDefault(c => c.CourseCode == code);
        }

        public void EnrollStudentInCourse(int studentId, string courseCode) // Метод для записи студента на курс
        {
            var student = FindStudentById(studentId);
            var course = FindCourseByCode(courseCode);

            if (student == null || course == null)
                throw new ArgumentException("Студент или курс не найден");

            student.EnrollInCourse(course);
        }
        private void UnenrollStudentFromCourse(int studentId, string courseCode) // метод для снятия студента с курса
        {
            var student = FindStudentById(studentId);
            var course = FindCourseByCode(courseCode);
            if (student == null || course == null)
                throw new ArgumentException("Студент или курс не найден");
            student.UnenrollFromCourse(course);
        }
        public void AssignTeacherToCourse(int teacherId, string courseCode) // Метод для назначения преподавателя на курс
        {
            var teacher = FindTeacherById(teacherId);
            var course = FindCourseByCode(courseCode);

            if (teacher == null || course == null)
                throw new ArgumentException("Преподаватель или курс не найден");

            teacher.AssignToCourse(course);
        }
        public void RemoveTeacherFromCourse(int teacherId, string courseCode) // Метод для снятия преподавателя с курса
        {
            var teacher = FindTeacherById(teacherId);
            var course = FindCourseByCode(courseCode);

            if (teacher == null || course == null)
                throw new ArgumentException("Преподаватель или курс не найден");

            teacher.RemoveFromCourse(course);
        }
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