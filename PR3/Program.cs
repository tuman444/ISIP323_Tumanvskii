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

}