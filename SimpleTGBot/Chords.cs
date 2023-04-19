using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.Payments;
using Telegram.Bot.Types.ReplyMarkups;
using File = System.IO.File;

namespace SimpleTGBot;

public static class Chords
{

    public static string chord = randomPath();

    public static void changePath()
    {
        chord = randomPath();
    }
    public static async Task mainGame(TelegramBotClient bot, long chatId, CancellationTokenSource cts)
    {
        var replyMarkup = new ReplyKeyboardMarkup(new[]
        {
            new[]
            {
                new KeyboardButton("Am"),
                new KeyboardButton("C")
            },
            new[]
            {
                new KeyboardButton("Dm"),
                new KeyboardButton("Em")
            },
            new[]
            {
                new KeyboardButton("F"),
                new KeyboardButton("G")
            }
        });
        var getAnswer = chord;
        Console.WriteLine(getAnswer);
        var filePath = $"input-files/victorinaChords/{getAnswer}/{getAnswer}{randomNumberOfMelody()}.wav";
        await using Stream stream = File.OpenRead(filePath);
        await bot.SendAudioAsync(
            chatId: chatId,
            audio: new InputOnlineFile(content: stream, fileName: chord), // "аккорд"
            caption: getRandomPhrase(),
            replyMarkup: replyMarkup,
            cancellationToken: cts.Token);
        
        
    }
    

    private static string getRandomPhrase()
    {
        Random r = new Random();
        String[] phrases =
        {
            "Этот аккорд может быть сложным для угадывания. Ты справишься?", "Думаешь, ты знаешь этот аккорд?",
            "Какой аккорд играю я? Попробуй угадать.", "Давай узнаем, какой это аккорд.",
            "Давай узнаем, какой это аккорд.",
            "Попробуй определить этот аккорд с первого раза.", "А этот?", "Попробуй ещё раз",
            "Возьмёшь вызов и угадаешь, какой аккорд играю?", "Попробуй определить этот аккорд - готов к вызову?"
        };
        return phrases[r.Next(0, 10)];
    }

    private static string randomNumberOfMelody()
    {
        Random r = new Random();
        return r.Next(1, 21).ToString();
    }

    public static string randomPath()
    {
        Random r = new();
        var number = r.Next(0, 6);
        String[] path = { "Am", "C", "Dm", "Em", "F", "G" };
        return path[number];
    }
}