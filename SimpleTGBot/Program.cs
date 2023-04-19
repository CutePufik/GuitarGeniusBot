using System.Net;
using File = Telegram.Bot.Types.File;

namespace SimpleTGBot;

public static class Program
{
    // Метод main немного видоизменился для асинхронной работы
    // https://www.kaggle.com/datasets/fabianavinci/guitar-chords-v3  откуда брал аккорды
    public static async Task Main(string[] args)
    {
        TelegramBot telegramBot = new TelegramBot();
        await telegramBot.Run();
        
    }
    
}