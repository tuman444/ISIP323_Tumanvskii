using System.Xml.Linq;

namespace UniversityManagementSystem
{
    public abstract class Person //класс для всех людей в университете
    {

        public person() //конструктор класса
        {

        }

        // Абстрактный метод для полиморфизма 
        public abstract string GetInfo();

        // Виртуальный метод 
        public virtual string GetBasicInfo()
        {
            
        }

    }
}