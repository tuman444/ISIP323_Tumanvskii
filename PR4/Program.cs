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

    // Класс для управления коллекцией книг в библиотеке
    public class Library
    {
        private List<Book> books;
        private int nextId;

        public Library()
        {
            books = new List<Book>();
            nextId = 1;
        }

        // Добавление новой книги с генерацией ID
        public bool AddBook(string title, string author, Genre genre, int year, decimal price)
        {
            if (!ValidateBookData(title, author, year, price))
                return false;

            Book newBook = new Book(nextId, title, author, genre, year, price);
            books.Add(newBook);
            nextId++;

            Console.WriteLine($"Книга успешно добавлена! ID: {newBook.Id}");
            return true;
        }

        // Удаление книги по идентификатору 
        public bool RemoveBook(int id)
        {
            Book bookToRemove = books.FirstOrDefault(b => b.Id == id);
            if (bookToRemove != null)
            {
                books.Remove(bookToRemove);
                Console.WriteLine($"Книга с ID {id} успешно удалена.");
                return true;
            }
            else
            {
                Console.WriteLine($"Книга с ID {id} не найдена.");
                return false;
            }
        }

        // Валидация данных книги перед добавлением
        private bool ValidateBookData(string title, string author, int year, decimal price)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                Console.WriteLine("Ошибка: Название книги не может быть пустым.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(author))
            {
                Console.WriteLine("Ошибка: Автор не может быть пустым.");
                return false;
            }

            if (year < 1000 || year > DateTime.Now.Year)
            {
                Console.WriteLine($"Ошибка: Год издания должен быть между 1000 и {DateTime.Now.Year}.");
                return false;
            }

            if (price < 0)
            {
                Console.WriteLine("Ошибка: Цена не может быть отрицательной.");
                return false;
            }

            return true;
        }

        // МЕТОДЫ ПОИСКА

        // Поиск книг по названию
        public List<Book> FindBooksByTitle(string title)
        {
            return books.Where(b => b.Title.Contains(title, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        // Поиск книг по автору
        public List<Book> FindBooksByAuthor(string author)
        {
            return books.Where(b => b.Author.Contains(author, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        // Поиск книг по жанру 
        public List<Book> FindBooksByGenre(Genre genre)
        {
            return books.Where(b => b.BookGenre == genre).ToList();
        }


        //МЕТОДЫ СОРТИРОВКИ

        // Сортировка книг по названию 
        public List<Book> SortByTitle()
        {
            // LINQ: OrderBy для сортировки по названию
            return books.OrderBy(b => b.Title).ToList();
        }

        /// Сортировка книг по году издания 
        public List<Book> SortByYear()
        {
            // LINQ: OrderBy для сортировки по году
            return books.OrderBy(b => b.Year).ToList();
        }


        // МЕТОДЫ АНАЛИЗА 

        // Получение самой дорогой книги 
        public Book GetMostExpensiveBook()
        {
            return books.OrderByDescending(b => b.Price).FirstOrDefault();
        }

        // Получение самой дешёвой книги
        public Book GetCheapestBook()
        {
            return books.OrderBy(b => b.Price).FirstOrDefault();
        }

        // Группировка книг по авторам 
        public Dictionary<string, int> GroupBooksByAuthor()
        {
            return books.GroupBy(b => b.Author).ToDictionary(g => g.Key, g => g.Count());
        }

        // Получение статистики по библиотеке
        public void DisplayLibraryStats()
        {
            Console.WriteLine($"\n=== СТАТИСТИКА БИБЛИОТЕКИ ===");

            Console.WriteLine($"Всего книг: {books.Count}");

            if (books.Any())
            {
                var mostExpensive = GetMostExpensiveBook();
                var cheapest = GetCheapestBook();

                Console.WriteLine($"Самая дорогая книга: {mostExpensive.Title} - {mostExpensive.Price:C}");
                Console.WriteLine($"Самая дешёвая книга: {cheapest.Title} - {cheapest.Price:C}");

                Console.WriteLine($"Средняя цена: {books.Average(b => b.Price):C}");

                var genreStats = books.GroupBy(b => b.BookGenre)
                                     .Select(g => new { Genre = g.Key, Count = g.Count() });

                Console.WriteLine("\nКниги по жанрам:");
                foreach (var stat in genreStats)
                {
                    Console.WriteLine($"  {stat.Genre}: {stat.Count} книг");
                }
            }
        }



    }
}