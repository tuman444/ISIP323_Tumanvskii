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


        // Добавление тестовых книг
        public void AddTestData()
        {
            books.Add(new Book(nextId++, "Властелин Колец", "Дж. Р. Р. Толкин", Genre.Fantasy, 1954, 1200m));
            books.Add(new Book(nextId++, "1984", "Джордж Оруэлл", Genre.ScienceFiction, 1949, 850m));
            books.Add(new Book(nextId++, "Убийство в Восточном экспрессе", "Агата Кристи", Genre.Mystery, 1934, 650m));
            books.Add(new Book(nextId++, "Гордость и предубеждение", "Джейн Остин", Genre.Romance, 1813, 720m));
            books.Add(new Book(nextId++, "Дракула", "Брэм Стокер", Genre.Horror, 1897, 780m));

            Console.WriteLine("Тестовые данные успешно добавлены!");
        }

        // Отображение списка всех книг в консоли
        public void DisplayAllBooks()
        {
            if (!books.Any())
            {
                Console.WriteLine("В библиотеке нет книг.");
                return;
            }

            Console.WriteLine("\n=== ВСЕ КНИГИ В БИБЛИОТЕКЕ ===");
            foreach (var book in books)
            {
                book.DisplayInfo();
            }
        }

        // Отображение результатов поиска
        public void DisplaySearchResults(List<Book> results, string searchType)
        {
            if (!results.Any())
            {
                Console.WriteLine($"По запросу '{searchType}' ничего не найдено.");
                return;
            }

            Console.WriteLine($"\n=== РЕЗУЛЬТАТЫ ПОИСКА ({results.Count} книг) ===");
            foreach (var book in results)
            {
                book.DisplayInfo();
            }
        }

        //отображение книг по авторам 
        public void DisplayBooksByAuthor() 
        {
            var authorGroups = GroupBooksByAuthor();

            if (!authorGroups.Any())
            {
                Console.WriteLine("В библиотеке нет книг.");
                return;
            }

            Console.WriteLine("\n=== КНИГИ ПО АВТОРАМ ===");

            // для сортировки по количеству книг
            foreach (var authorGroup in authorGroups.OrderByDescending(a => a.Value))
            {
                Console.WriteLine($"{authorGroup.Key}: {authorGroup.Value} книг(и)");

                //для фильтрации книг по автору
                var authorBooks = books.Where(b => b.Author == authorGroup.Key);
                foreach (var book in authorBooks)
                {
                    Console.WriteLine($"  - {book.Title} ({book.Year})");
                }
                Console.WriteLine();
            }
        }
    }


    // Главный класс приложения 
    public class Program
    {
        private Library library;

        public Program()
        {
            library = new Library();
        }

        // Главный метод запуска приложения
        public void Run()
        {
            Console.WriteLine("Добро пожаловать в систему учета библиотеки!");

            library.AddTestData();

            bool exit = false;
            while (!exit)
            {
                DisplayMenu();
                exit = UserInput();
            }

            Console.WriteLine("Спасибо за использование системы! До свидания!");
        }

        // Отображение главного меню
        private void DisplayMenu()
        {
            Console.WriteLine("\n=== ГЛАВНОЕ МЕНЮ ===");
            Console.WriteLine("1. Показать все книги");
            Console.WriteLine("2. Добавить книгу");
            Console.WriteLine("3. Удалить книгу");
            Console.WriteLine("4. Найти книги");
            Console.WriteLine("5. Сортировать книги");
            Console.WriteLine("6. Статистика библиотеки");
            Console.WriteLine("7. Книги по авторам");
            Console.WriteLine("8. Выход");
            Console.Write("Выберите действие: ");
        }

        // Обработка пользовательского ввода
        private bool UserInput()
        {
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    library.DisplayAllBooks();
                    break;
                case "2":
                    AddBookInterface();
                    break;
                case "3":
                    RemoveBookInterface();
                    break;
                case "4":
                    SearchBooksInterface();
                    break;
                case "5":
                    SortBooksInterface();
                    break;
                case "6":
                    library.DisplayLibraryStats();
                    break;
                case "7":
                    library.DisplayBooksByAuthor();
                    break;
                case "8":
                    return true;
                default:
                    Console.WriteLine("Неверный ввод. Пожалуйста, выберите действие от 1 до 8.");
                    break;
            }

            return false;
        }

        // Интерфейс добавления книги с вводом данных от пользователя
        private void AddBookInterface()
        {
            Console.WriteLine("\n=== ДОБАВЛЕНИЕ НОВОЙ КНИГИ ===");

            try
            {
                Console.Write("Введите название книги: ");
                string title = Console.ReadLine();

                Console.Write("Введите автора: ");
                string author = Console.ReadLine();

                Console.WriteLine("Доступные жанры:");
                foreach (var Genre in Enum.GetValues(typeof(Genre)))
                {
                    Console.WriteLine($"  {(int)Genre}. {Genre}");
                }
                Console.Write("Выберите жанр (номер): ");
                Genre genre = (Genre)int.Parse(Console.ReadLine());

                Console.Write("Введите год издания: ");
                int year = int.Parse(Console.ReadLine());

                Console.Write("Введите цену: ");
                decimal price = decimal.Parse(Console.ReadLine());

                bool success = library.AddBook(title, author, genre, year, price);
                if (!success)
                {
                    Console.WriteLine("Не удалось добавить книгу. Проверьте введенные данные.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка ввода: {ex.Message}");
            }
        }

        // Интерфейс удаления книги
        private void RemoveBookInterface()
        {
            Console.WriteLine("\n=== УДАЛЕНИЕ КНИГИ ===");

            try
            {
                Console.Write("Введите ID книги для удаления: ");
                int id = int.Parse(Console.ReadLine());

                library.RemoveBook(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка ввода: {ex.Message}");
            }
        }

        // Интерфейс поиска книг
        private void SearchBooksInterface()
        {
            Console.WriteLine("\n=== ПОИСК КНИГ ===");
            Console.WriteLine("1. По названию");
            Console.WriteLine("2. По автору");
            Console.WriteLine("3. По жанру");
            Console.Write("Выберите тип поиска: ");

            string choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        Console.Write("Введите название для поиска: ");
                        string title = Console.ReadLine();
                        var titleResults = library.FindBooksByTitle(title);
                        library.DisplaySearchResults(titleResults, $"по названию '{title}'");
                        break;

                    case "2":
                        Console.Write("Введите автора для поиска: ");
                        string author = Console.ReadLine();
                        var authorResults = library.FindBooksByAuthor(author);
                        library.DisplaySearchResults(authorResults, $"по автору '{author}'");
                        break;

                    case "3":
                        Console.WriteLine("Доступные жанры:");
                        foreach (var genre in Enum.GetValues(typeof(Genre)))
                        {
                            Console.WriteLine($"  {(int)genre}. {genre}");
                        }
                        Console.Write("Выберите жанр (номер): ");
                        Genre genreSearch = (Genre)int.Parse(Console.ReadLine());
                        var genreResults = library.FindBooksByGenre(genreSearch);
                        library.DisplaySearchResults(genreResults, $"по жанру '{genreSearch}'");
                        break;

                    default:
                        Console.WriteLine("Неверный выбор.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка поиска: {ex.Message}");
            }
        }

        // Интерфейс сортировки книг
        private void SortBooksInterface()
        {
            Console.WriteLine("\n=== СОРТИРОВКА КНИГ ===");
            Console.WriteLine("1. По названию");
            Console.WriteLine("2. По году издания");
            Console.Write("Выберите тип сортировки: ");

            string choice = Console.ReadLine();
            List<Book> sortedBooks = new List<Book>();

            switch (choice)
            {
                case "1":
                    sortedBooks = library.SortByTitle();
                    Console.WriteLine("\nКниги отсортированы по названию:");
                    break;

                case "2":
                    sortedBooks = library.SortByYear();
                    Console.WriteLine("\nКниги отсортированы по году издания:");
                    break;


                default:
                    Console.WriteLine("Неверный выбор.");
                    return;
            }

            if (sortedBooks.Any())
            {
                foreach (var book in sortedBooks)
                {
                    book.DisplayInfo();
                }
            }
            else
            {
                Console.WriteLine("Нет книг для отображения.");
            }
        }
        static void Main(string[] args) //задаем точку входу
        {
            Program app = new Program();
            app.Run();
        }
    }
}