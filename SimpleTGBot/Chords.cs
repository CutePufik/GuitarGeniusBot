using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using File = System.IO.File;

namespace SimpleTGBot;

public class Chords
{
    
        public static async Task mainGame(TelegramBotClient bot,long chatId,CancellationTokenSource cts)
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
        var getAnswer = randomPath();
        
        var filePath = $"input-files/victorinaChords/{getAnswer}/{getAnswer}{randomNumberOfMelody()}.wav";
        await using Stream stream = File.OpenRead(filePath);
        await bot.SendAudioAsync(
            chatId: chatId,
            audio: new InputOnlineFile(content: stream, fileName: randomPath()), // "аккорд"
            caption: getRandomPhrase(),
            replyMarkup: replyMarkup,
            cancellationToken:cts.Token);
        int offset = 0;
        int timeout = 0;
        Console.WriteLine("wait answer");
        try
        {
            var updates = await bot.GetUpdatesAsync(offset, timeout);
            foreach (var update in updates)
            {
                var message = update.Message.Text;
                Console.WriteLine(getAnswer);
                if (message == getAnswer)
                {
                    await bot.SendTextMessageAsync(chatId, "Правильно,молодец!",
                        cancellationToken: cts.Token);
                }
                else if (message == "да")
                {
                    await mainGame(bot, chatId, cts);
                }
                else if (message == "Аккорды")
                {
                   
                }
                else
                {
                    await bot.SendTextMessageAsync(chatId, "Попробуй ещё раз :(",
                        cancellationToken: cts.Token);
                }
                
                
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        


    }

    // private static async Task waitAnswer(TelegramBotClient bot,long chatId,CancellationTokenSource cts,String rightAnswer)
    // {
    //     int offset = 0;
    //     int timeout = 0;
    //     Console.WriteLine("wait answer");
    //     try
    //     {
    //         var updates = await bot.GetUpdatesAsync(offset, timeout);
    //         foreach (var update in updates)
    //         {
    //             var message = update.Message.Text;
    //             Console.WriteLine(message);
    //             if (message == rightAnswer)
    //             {
    //                 await bot.SendTextMessageAsync(chatId, "Правильно,молодец!",
    //                     cancellationToken: cts.Token);
    //             }
    //             else
    //             {
    //                 await bot.SendTextMessageAsync(chatId, "Попробуй ещё раз :(",
    //                     cancellationToken: cts.Token);
    //             }
    //         }
    //     }
    //     catch (Exception e)
    //     {
    //         Console.WriteLine(e.Message);
    //     }
    //     
    // }

    private static string getRandomPhrase()
    {
        Random r = new Random();
        String[] phrases = { "Этот аккорд может быть сложным для угадывания. Ты справишься?","Думаешь, ты знаешь этот аккорд?",
            "Какой аккорд играю я? Попробуй угадать.","Давай узнаем, какой это аккорд.","Давай узнаем, какой это аккорд.",
            "Попробуй определить этот аккорд с первого раза.","А этот?","Попробуй ещё раз",
            "Возьмёшь вызов и угадаешь, какой аккорд играю?","Попробуй определить этот аккорд - готов к вызову?"};
        return phrases[r.Next(0, 10)];
    }

    private static string randomNumberOfMelody()
    {
        Random r = new Random();
        return r.Next(1, 21).ToString();
    }

    private static string randomPath()
    {
        Random r = new();
        var number = r.Next(0, 6);
        String[] path = { "Am", "C", "Dm", "Em", "F", "G"};
        return path[number];
    }

   
}