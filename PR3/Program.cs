class TextStatistics
{
    public string Text { get; set; }               // Исходный текст (сокращенный)
    public int WordCount { get; set; }            // Количество слов
    public string ShortestWord { get; set; }      // Самое короткое слово
    public int SentenceCount { get; set; }        // Количество предложений
    public int VowelCount { get; set; }           // Количество гласных букв
    public int ConsonantCount { get; set; }       // Количество согласных букв
    public string LongestWord { get; set; }       // Самое длинное слово
    public Dictionary<char, int> LetterFrequency { get; set; } // Частота каждой буквы
    public DateTime AnalysisTime { get; set; }    // Время анализа
}