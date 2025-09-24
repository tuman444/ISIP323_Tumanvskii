using Microsoft.VisualBasic;
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

    private static readonly HashSet<char> consonants = new HashSet<char>
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

    // Анализ нового текста
    static void AnalyzeNewText()
    {
        Console.WriteLine("\n--- Анализ нового текста ---");

        string text = GetTextUser();    // Получение текста от пользователя
        if (string.IsNullOrEmpty(text)) return;

        TextStatistics stats = new TextStatistics   // Создание объекта для хранения статистики
        {
            Text = text.Length > 50 ? text.Substring(0, 47) + "..." : text,
            AnalysisTime = DateTime.Now
        };

        // Выполнение всех анализов
        AnalyzeWordCount(text, stats);                  //подсчитываем кол-во слов
        FindShortestAndLongestWords(text, stats);       //находим самое короткое и самое длинное слово
        AnalyzeSentenceCount(text, stats);              //подсчитываем кол-во предложений
        AnalyzeLetters(text, stats);                    //подсчитываем глассные и согласные буквы
        AnalyzeLetterFrequency(text, stats);            //создаем статистику частоты букв

        allStatistics.Add(stats);

        DisplayCurrentStatistics(stats);                //вывод статистики
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

    // Подсчет количества слов в тексте
    static void AnalyzeWordCount(string text, TextStatistics stats)
    {
        if (string.IsNullOrEmpty(text)) // проверка на пустую строку
        {
            stats.WordCount = 0;
            return;
        }

        int wordCount = 0;
        bool inWord = false;

        foreach (char c in text)  // Проходим по каждому символу текста
        {
            if (char.IsLetterOrDigit(c) || c == '\'') // Буквы, цифры и апострофы считаются частью слова
            {
                if (!inWord)  // если мы не в слове
                {
                    wordCount++;  //начинаем новое слово
                    inWord = true;
                }
            }
            else // любые другие символы
            {
                inWord = false;
            }
        }

        stats.WordCount = wordCount;
    }


    // Поиск самого короткого и самого длинного слова
    static void FindShortestAndLongestWords(string text, TextStatistics stats)
    {
        if (string.IsNullOrEmpty(text))  //проверка на пустую текст
        {
            stats.ShortestWord = "";
            stats.LongestWord = "";
            return;
        }

        string shortestWord = null; //корткое слово 
        string longestWord = null;  //длинное слово
        StringBuilder currentWord = new StringBuilder(); //накопление текущего слова

        // Проходим по каждому символу текста для выделения слов
        foreach (char c in text)
        {
            if (char.IsLetterOrDigit(c) || c == '\'')
            {
                currentWord.Append(c);  // добавляем символ к текущему слову
            }
            else  //если символ разделитель
            {
                if (currentWord.Length > 0)    //если накопилось слово
                {
                    string word = currentWord.ToString();

                    // Проверяем самое короткое слово
                    if (shortestWord == null || word.Length < shortestWord.Length)
                    {
                        shortestWord = word;
                    }

                    // Проверяем самое длинное слово
                    if (longestWord == null || word.Length > longestWord.Length)
                    {
                        longestWord = word;
                    }

                    currentWord.Clear();
                }
            }
        }

        // Проверяем последнее слово
        if (currentWord.Length > 0)
        {
            string word = currentWord.ToString();
            if (shortestWord == null || word.Length < shortestWord.Length)
            {
                shortestWord = word;
            }
            if (longestWord == null || word.Length > longestWord.Length)
            {
                longestWord = word;
            }
        }

        stats.ShortestWord = shortestWord ?? "";  //Защита от null значений
        stats.LongestWord = longestWord ?? "";
    }


    // Подсчет количества предложений
    static void AnalyzeSentenceCount(string text, TextStatistics stats)
    {
        if (string.IsNullOrEmpty(text))
        {
            stats.SentenceCount = 0;
            return;
        }

        int sentenceCount = 0;
        bool sentenceEnded = true;

        // Проходим по каждому символу текста
        foreach (char c in text)
        {
            if (c == '.' || c == '!' || c == '?' || c == ';')
            {
                if (sentenceEnded == false)
                {
                    sentenceCount++;
                    sentenceEnded = true;
                }
            }
            else if (char.IsLetter(c))  //Если символ - буква
            {
                sentenceEnded = false;
            }
        }

        // Если текст заканчивается без знака препинания
        if (!sentenceEnded)
        {
            sentenceCount++;
        }

        stats.SentenceCount = sentenceCount;
    }

    // Подсчет количества гласных и согласных букв
    static void AnalyzeLetters(string text, TextStatistics stats)
    {
        int vowelCount = 0;
        int consonantCount = 0;

        // Проходим по каждому символу текста
        foreach (char c in text.ToLower()) // Приводим к нижнему регистру для сравнения
        {
            if (vowels.Contains(c))
            {
                vowelCount++;
            }
            else if (consonants.Contains(c))
            {
                consonantCount++;
            }
        }

        stats.VowelCount = vowelCount;
        stats.ConsonantCount = consonantCount;
    }

    // Анализ частоты встречаемости букв
    static void AnalyzeLetterFrequency(string text, TextStatistics stats)
    {
        Dictionary<char, int> frequency = new Dictionary<char, int>(); //инициализация словаря

        // Проходим по каждому символу текста
        foreach (char c in text.ToLower()) // Приводим к нижнему регистру
        {
            if (char.IsLetter(c)) // Учитываем только буквы
            {
                if (frequency.ContainsKey(c)) //если буква есть в словаре
                {
                    frequency[c]++;
                }
                else   //если буква встречается впервые
                {
                    frequency[c] = 1;  //добавляем букву в словарь со счетчиком 1
                }
            }
        }

        stats.LetterFrequency = frequency;  //сохранение результатов
    }





}