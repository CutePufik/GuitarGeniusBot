using System.Text.RegularExpressions;

namespace SimpleTGBot;

public static class Facts
{
    public static String getRandomFact()
    {
        var file = File.ReadAllLines("./input-files/interestingFacts.txt");

        Random r = new Random();
        Match match = Regex.Match(file[r.Next(0, file.Length)], @"^\d+\.\s+(.*)$");
        string sentence = match.Groups[1].Value;
        Console.WriteLine(sentence);
        return sentence;
    }
}