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
    }


}