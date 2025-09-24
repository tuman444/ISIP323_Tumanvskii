using System.Text;

class TextAnalyzer
{
    class TextStatistics
    {
        public string Text { get; set; }              // Исходный текст (сокращенный)
        public int WordCount { get; set; }            // Количество слов
        public string ShortestWord { get; set; }      // Самое короткое слово
        public int SentenceCount { get; set; }        // Количество предложений
        public int VowelCount { get; set; }           // Количество гласных букв
        public int ConsonantCount { get; set; }       // Количество согласных букв
        public string LongestWord { get; set; }       // Самое длинное слово
        public Dictionary<char, int> LetterFrequency { get; set; } // Частота каждой буквы
        public DateTime AnalysisTime { get; set; }    // Время анализа
    }

    private static List<TextStatistics> allStatistics = new List<TextStatistics>();

    private static readonly HashSet<char> vowels = new HashSet<char>
    {
        'а', 'е', 'ё', 'и', 'о', 'и', 'у', 'ы', 'ю', 'э', 'я', 
        'a', 'e', 'y', 'u', 'i', 'o'
    };

    private static readonly HashSet<char> consanats = new HashSet<char>
    {
        'й', 'ц', 'к', 'н', 'г', 'ш', 'щ', 'з', 'х', 'ф', 'в', 'п', 'р', 'л', 'д', 'ж', 'ч', 'с', 'м', 'т', 'б',
        'q', 'w', 'r', 't', 'p', 's', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'z', 'x', 'c', 'v', 'b', 'n', 'm'
    };

    static void Main(string[] args)
    {
        Console.WriteLine("=== Анализатор текста ===");

        //bool continueWorking = true;

        while (true)
        {
            ShowMainMenu();
            string choice = Console.ReadLine(); 

            switch(choice)
            {
                case "1":
                    AnalyzeNewText();
                    break;
                 case "2":
                    ShowPreviosStatistics();
                    break;
                 case "3":
                    //continueWorking = false;
                    Console.WriteLine("До свидания");
                    break;
                 default:
                    Console.WriteLine("Ошибка выбора");
                    break;
            }
        }
    }

    static void ShowMainMenu()
    {
        Console.WriteLine("\n=== Главное меню ===");
        Console.WriteLine("1. Анализировать текст");
        Console.WriteLine("2. Показать статистику по прошлым текстам");
        Console.WriteLine("3. Выйти");
        Console.WriteLine("Выберите действие");
    }

    // Получение текста от пользователя с проверкой длины
    static string GetTextUser()
    {
        string text;

        while (true)
        {
            Console.WriteLine("Введите текст (минимум 100 символов):");
            text = Console.ReadLine(); // получение текста

            if (text == null) // проверка на пустую строку
            {
                Console.WriteLine("Ошибка ввода. Попробуйте снова.");
                continue;
            }

            // Удаляем лишние пробелы и проверяем длину
            string cleanedText = CleanText(text);

            if (cleanedText.Length < 100)
            {
                Console.WriteLine($"Текст слишком короткий! Введено {cleanedText.Length} символов. Минимум 100 символов.");
                Console.Write("Хотите попробовать снова? (д/н): ");
                string answer = Console.ReadLine();
                if (answer?.ToLower() != "д")
                {
                    return null;
                }
            }
            else
            {
                return cleanedText;
            }
        }
    }

    // Очистка текста от лишних пробелов
    static string CleanText(string text)
    {
        // Заменяем множественные пробелы на одинарные и обрезаем края
        StringBuilder cleaned = new StringBuilder();  // Накопитель для результата
        bool previousWasSpace = false;      

        foreach (char c in text) //Начало цикла по символам
        {
            if (char.IsWhiteSpace(c)) // если символ пробельный 
            {
                if (!previousWasSpace) // если предыдущий символ не был пробельным
                {
                    cleaned.Append(' ');  //добавляем один пробел
                    previousWasSpace = true; // теперь пробел был
                }
            }
            else // символ не пробел 
            {
                cleaned.Append(c);  //добавляем сивол в результат
                previousWasSpace = false;       //текущий символ не пробел
            }
        }

        return cleaned.ToString().Trim();  //убираем пробел по краям 
    }

}