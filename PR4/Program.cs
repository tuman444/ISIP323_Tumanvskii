namespace library
{
    public enum Genre // список жанров
    { 
        Fantasy,
        ScienceFiction,
        Mystery,
        Romance,
        Horror,
        Biography
    }
    public class Book // Класс, представляющий книгу в библиотеке
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public string Author { get; private set; }
        public Genre BookGenre { get; private set; }
        public int Year { get; private set; }
        public decimal Price { get; private set; }

        // Конструктор книги
        public Book(int id, string title, string author, Genre genre, int year, decimal price)
        {
            Id = id;
            Title = title;
            Author = author;
            BookGenre = genre;
            Year = year;
            Price = price;
        }


        // Вывод полной информации о книге в консоль
        public void DisplayInfo()
        {
            Console.WriteLine($"ID: {Id}");
            Console.WriteLine($"Название: {Title}");
            Console.WriteLine($"Автор: {Author}");
            Console.WriteLine($"Жанр: {BookGenre}");
            Console.WriteLine($"Год издания: {Year}");
            Console.WriteLine($"Цена: {Price:C}");
            Console.WriteLine(new string('-', 40));
        }

        // Краткое строковое представление книги
        public override string ToString()
        {
            return $"{Title} - {Author} ({Year})";
        }
    }
}