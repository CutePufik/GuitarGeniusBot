
using System.Linq.Expressions;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using File = System.IO.File;

namespace SimpleTGBot;

public class TelegramBot
{
    
    // Токен TG-бота. Можно получить у @BotFather
    private const string BotToken = "6224558457:AAEHlTeLGhA6zZe4mS2qChytdFTt5gvMtZs";
    private TelegramBotClient botClient = new(BotToken);
    private CancellationTokenSource cts = new CancellationTokenSource();
    String pathToMyFavourite = "./input-files/tabs/годнота";
    String pathToZveri = "./input-files/tabs/звери";
    String pathToNerves = "./input-files/tabs/нервы";
    String pathToSplin = "./input-files/tabs/сплин";
    public static String rigthAnswer = Chords.chord;


    /// <summary>
    /// Инициализирует и обеспечивает работу бота до нажатия клавиши Esc
    /// </summary>
    public async Task Run()
    {
        
        // Служебные вещи для организации правильной работы с потоками
        

        // Разрешённые события, которые будет получать и обрабатывать наш бот.
        // Будем получать только сообщения. При желании можно поработать с другими событиями.
        ReceiverOptions receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = new[] {UpdateType.Message, UpdateType.CallbackQuery },
            ThrowPendingUpdates = true
        };
       

        // Привязываем все обработчики и начинаем принимать сообщения для бота
        botClient.StartReceiving(
            updateHandler: OnMessageReceived,
            pollingErrorHandler: OnErrorOccured,
            receiverOptions: receiverOptions,
            cancellationToken: cts.Token
        );

        // Проверяем что токен верный и получаем информацию о боте
        var me = await botClient.GetMeAsync(cancellationToken: cts.Token);
        Console.WriteLine($"Бот @{me.Username} запущен.\nДля остановки нажмите клавишу Esc...");

        // Ждём, пока будет нажата клавиша Esc, тогда завершаем работу бота
        while (Console.ReadKey().Key != ConsoleKey.Escape)
        {
        }

        // Отправляем запрос для остановки работы клиента.
        cts.Cancel();
    }

    /// <summary>
    /// Обработчик события получения сообщения.
    /// </summary>
    /// <param name="botClient">Клиент, который получил сообщение</param>
    /// <param name="update">Событие, произошедшее в чате. Новое сообщение, голос в опросе, исключение из чата и т. д.</param>
    /// <param name="cancellationToken">Служебный токен для работы с многопоточностью</param>
    async Task OnMessageReceived(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        // Работаем только с сообщениями. Остальные события игнорируем
        
        var message = update.Message;
        if (!(message is null || update.CallbackQuery is null))
        {
            return;
        }

        
        
        
        // Будем обрабатывать только текстовые сообщения.
        // При желании можно обрабатывать стикеры, фото, голосовые и т. д.
        //
        // Обратите внимание на использованную конструкцию. Она эквивалентна проверке на null, приведённой выше.
        // Подробнее об этом синтаксисе: https://medium.com/@mattkenefick/snippets-in-c-more-ways-to-check-for-null-4eb735594c09
        // if (message.Text is not { } messageText )
        // {
        //     return;
        // }

        
        // Получаем ID чата, в которое пришло сообщение. Полезно, чтобы отличать пользователей друг от друга.
        long chatId = 0;

        if (update.Message != null)
        {
            chatId = update.Message.Chat.Id;
        }
        else if (update.CallbackQuery != null)
        {
            chatId = update.CallbackQuery.Message.Chat.Id;
        }



        if (message is not null)
        {
            if (message.Text == "Аккорды")
            {
                
                await Chords.mainGame((TelegramBotClient)botClient, chatId, cts);
                
            }
            else
            {
                await GetMessages(chatId);
            }
            
        }
        
        
        
        
        
        
        if (update.Type == UpdateType.CallbackQuery)
        {
            Console.WriteLine(update.Type);
            if (update.CallbackQuery?.Data is null)
            {
                return;
            }

            var query = update.CallbackQuery.Data;
            Stream stream;
            switch (query)
            {
                case "Время назад":
                {
                    stream = File.OpenRead(pathToSplin+"\\время назад.txt");
                    await botClient.SendDocumentAsync(
                        chatId: chatId,
                        document: new InputOnlineFile(content: stream, fileName: "tabs"),
                        caption: "Время назад",cancellationToken: cts.Token);
                    break;
                }
                case "Мороз по коже":
                    stream = File.OpenRead(pathToSplin+"\\мороз по коже.txt");
                    await botClient.SendDocumentAsync(
                        chatId: chatId,
                        document: new InputOnlineFile(content: stream, fileName: "tabs"),
                        caption: "мороз по коже",cancellationToken: cts.Token);
                    break;
                    
                case "Романс":
                    stream = File.OpenRead(pathToSplin+"\\романс.txt");
                    await botClient.SendDocumentAsync(
                        chatId: chatId,
                        document: new InputOnlineFile(content: stream, fileName: "tabs"),
                        caption: "Романс",cancellationToken: cts.Token);
                    break;
                case "Выхода нет":
                    stream = File.OpenRead(pathToSplin+"\\выхода нет.txt");
                    await botClient.SendDocumentAsync(
                        chatId: chatId,
                        document: new InputOnlineFile(content: stream, fileName: "tabs"),
                        caption: "выхода нет",cancellationToken: cts.Token);
                    break;   
                case "Мое сердце":
                   stream = File.OpenRead(pathToSplin+"\\сердце.txt");
                    await botClient.SendDocumentAsync(
                        chatId: chatId,
                        document: new InputOnlineFile(content: stream, fileName: "tabs"),
                        caption: "Мое сердце",cancellationToken: cts.Token);
                    break;
                case "Весь этот бред":
                    stream = File.OpenRead(pathToSplin+"\\весь этот бред.txt");
                    await botClient.SendDocumentAsync(
                        chatId: chatId,
                        document: new InputOnlineFile(content: stream, fileName: "tabs"),
                        caption: "Весь этот бред",cancellationToken: cts.Token);
                    break;
                case "Топай":
                    stream = File.OpenRead(pathToSplin+"\\топай.txt");
                    await botClient.SendDocumentAsync(
                        chatId: chatId,
                        document: new InputOnlineFile(content: stream, fileName: "tabs"),
                        caption: "Топай",cancellationToken: cts.Token);
                    break;
                case "Передайте Гарри Поттеру":
                    stream = File.OpenRead(pathToSplin+"\\передайте гарри поттеру.txt");
                    await botClient.SendDocumentAsync(
                        chatId: chatId,
                        document: new InputOnlineFile(content: stream, fileName: "tabs"),
                        caption: "Передайте это Гарри Поттеру",cancellationToken: cts.Token);
                    break;
                case "батареи":
                    stream = File.OpenRead(pathToNerves+"\\батареи.txt");
                    await botClient.SendDocumentAsync(
                        chatId: chatId,
                        document: new InputOnlineFile(content: stream, fileName: "tabs"),
                        caption: "батареи",cancellationToken: cts.Token);
                    break;
                
                case "кофе мой друг":
                    stream = File.OpenRead(pathToNerves+"\\кофе мой друг.txt");
                    await botClient.SendDocumentAsync(
                        chatId: chatId,
                        document: new InputOnlineFile(content: stream, fileName: "tabs"),
                        caption: "кофе мой друг",cancellationToken: cts.Token);
                    break;
                case "май вай":
                    stream = File.OpenRead(pathToNerves+"\\май вай.txt");
                    await botClient.SendDocumentAsync(
                        chatId: chatId,
                        document: new InputOnlineFile(content: stream, fileName: "tabs"),
                        caption: "май вай",cancellationToken: cts.Token);
                    break;
                case "Всё,что тебя касается":
                    stream = File.OpenRead(pathToZveri+"\\всё,что тебя касается.txt");
                    await botClient.SendDocumentAsync(
                        chatId: chatId,
                        document: new InputOnlineFile(content: stream, fileName: "tabs"),
                        caption: "всё,что тебя касается",cancellationToken: cts.Token);
                    break;
                case "До скорой встречи":
                    stream = File.OpenRead(pathToZveri+"\\до скорой встречи.txt");
                    await botClient.SendDocumentAsync(
                        chatId: chatId,
                        document: new InputOnlineFile(content: stream, fileName: "tabs"),
                        caption: "До скорой встречи",cancellationToken: cts.Token);
                    break;
                case "Для тебя":
                    stream = File.OpenRead(pathToZveri+"\\Звери - Для тебя.txt");
                    await botClient.SendDocumentAsync(
                        chatId: chatId,
                        document: new InputOnlineFile(content: stream, fileName: "tabs"),
                        caption: "Для тебя",cancellationToken: cts.Token);
                    break;
                case "Дожди-пистолеты":
                    stream = File.OpenRead(pathToZveri+"\\звери дожди пистолеты.txt");
                    await botClient.SendDocumentAsync(
                        chatId: chatId,
                        document: new InputOnlineFile(content: stream, fileName: "tabs"),
                        caption: "Дожди-пистолеты",cancellationToken: cts.Token);
                    break;
                case "Напитки покрепче":
                    stream = File.OpenRead(pathToZveri+"\\напитки покрепче.txt");
                    await botClient.SendDocumentAsync(
                        chatId: chatId,
                        document: new InputOnlineFile(content: stream, fileName: "tabs"),
                        caption: "напитки покрепче",cancellationToken: cts.Token);
                    break;
                case "Не беда":
                    stream = File.OpenRead(pathToZveri+"\\не беда.txt");
                    await botClient.SendDocumentAsync(
                        chatId: chatId,
                        document: new InputOnlineFile(content: stream, fileName: "tabs"),
                        caption: "не беда",cancellationToken: cts.Token);
                    break;
                case "Районы":
                    stream = File.OpenRead(pathToZveri+"\\районы кварталы.txt");
                    await botClient.SendDocumentAsync(
                        chatId: chatId,
                        document: new InputOnlineFile(content: stream, fileName: "tabs"),
                        caption: "Районы",cancellationToken: cts.Token);
                    break;
                case "Танцуй":
                    stream = File.OpenRead(pathToZveri+"\\танцуй.txt");
                    await botClient.SendDocumentAsync(
                        chatId: chatId,
                        document: new InputOnlineFile(content: stream, fileName: "tabs"),
                        caption: "Танцуй",cancellationToken: cts.Token);
                    break;
                case "Ты так прекрасна":
                    stream = File.OpenRead(pathToZveri+"\\ты так прекрасна.txt");
                    await botClient.SendDocumentAsync(
                        chatId: chatId,
                        document: new InputOnlineFile(content: stream, fileName: "tabs"),
                        caption: "Ты так прекрасна",cancellationToken: cts.Token);
                    break;
                case "Просто такая сильная любовь":
                    stream = File.OpenRead(pathToZveri+"\\просто такая сильная любоваь.txt");
                    await botClient.SendDocumentAsync(
                        chatId: chatId,
                        document: new InputOnlineFile(content: stream, fileName: "tabs"),
                        caption: "просто такая сильная любовь",cancellationToken: cts.Token);
                    break;
                case "Тебе":
                    stream = File.OpenRead(pathToZveri+"\\звери тебе.txt");
                    await botClient.SendDocumentAsync(
                        chatId: chatId,
                        document: new InputOnlineFile(content: stream, fileName: "tabs"),
                        caption: "Тебе",cancellationToken: cts.Token);
                    break;
                case "Я с тобой":
                    stream = File.OpenRead(pathToZveri+"\\я с тобой.txt");
                    await botClient.SendDocumentAsync(
                        chatId: chatId,
                        document: new InputOnlineFile(content: stream, fileName: "tabs"),
                        caption: "Я с тобой",cancellationToken: cts.Token);
                    break;
                case "Березы":
                    stream = File.OpenRead(pathToMyFavourite+"\\березы любэ.txt");
                    await botClient.SendDocumentAsync(
                        chatId: chatId,
                        document: new InputOnlineFile(content: stream, fileName: "tabs"),
                        caption: "березы любэ",cancellationToken: cts.Token);
                    break;
                case "Девочка-Пай":
                    stream = File.OpenRead(pathToMyFavourite+"\\девочка пай.txt");
                    await botClient.SendDocumentAsync(
                        chatId: chatId,
                        document: new InputOnlineFile(content: stream, fileName: "tabs"),
                        caption: "девочка пай",cancellationToken: cts.Token);
                    break;
                case "Супермаркет":
                    stream = File.OpenRead(pathToMyFavourite+"\\супермаркет.txt");
                    await botClient.SendDocumentAsync(
                        chatId: chatId,
                        document: new InputOnlineFile(content: stream, fileName: "tabs"),
                        caption: "Супермаркет",cancellationToken: cts.Token);
                    break;
                case "Кто такая Элиз?":
                    stream = File.OpenRead(pathToMyFavourite+"\\кто такая элиз.txt");
                    await botClient.SendDocumentAsync(
                        chatId: chatId,
                        document: new InputOnlineFile(content: stream, fileName: "tabs"),
                        caption: "Кто такая Элиз?",cancellationToken: cts.Token);
                    break;
                case "Лирическая":
                    stream = File.OpenRead(pathToMyFavourite+"\\лирическая.txt");
                    await botClient.SendDocumentAsync(
                        chatId: chatId,
                        document: new InputOnlineFile(content: stream, fileName: "tabs"),
                        caption: "лирическая",cancellationToken: cts.Token);
                    break;
                case "Самый лучший день":
                    stream = File.OpenRead(pathToMyFavourite+"\\самый лучший ден.txt");
                    await botClient.SendDocumentAsync(
                        chatId: chatId,
                        document: new InputOnlineFile(content: stream, fileName: "tabs"),
                        caption: "Самый лучший день",cancellationToken: cts.Token);
                    break;
                case "Седьмой лепесток":
                    stream = File.OpenRead(pathToMyFavourite+"\\седьмой лепесток.txt");
                    await botClient.SendDocumentAsync(
                        chatId: chatId,
                        document: new InputOnlineFile(content: stream, fileName: "tabs"),
                        caption: "самый дорогой человек",cancellationToken: cts.Token);
                    break;
                case "Этот город":
                    stream = File.OpenRead(pathToMyFavourite+"\\этот город браво кап 3л.txt");
                    await botClient.SendDocumentAsync(
                        chatId: chatId,
                        document: new InputOnlineFile(content: stream, fileName: "tabs"),
                        caption: "этот город",cancellationToken: cts.Token);
                    break;
                
                
                
                
            }
                Console.WriteLine(query);
            Console.WriteLine(update.CallbackQuery.Data);
        }
        
    }
    


    /// <summary>
    /// Обработчик исключений, возникших при работе бота
    /// </summary>
    /// <param name="botClient">Клиент, для которого возникло исключение</param>
    /// <param name="exception">Возникшее исключение</param>
    /// <param name="cancellationToken">Служебный токен для работы с многопоточностью</param>
    /// <returns></returns>
    Task OnErrorOccured(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        // В зависимости от типа исключения печатаем различные сообщения об ошибке
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",

            _ => exception.ToString()
        };

        Console.WriteLine(errorMessage);

        // Завершаем работу
        return Task.CompletedTask;
    }


    /// <summary>
    /// обработка меню кнопок
    /// </summary>
    /// 
    public async Task GetMessages(long chatId)
    {
        int offset = 0;
        int timeout = 0;
        
        try
        {
            var updates = await botClient.GetUpdatesAsync(offset, timeout,allowedUpdates:new [] { UpdateType.Message ,UpdateType.CallbackQuery},cancellationToken:cts.Token);
                foreach (var update in updates)
                {
                    
                    var message = update.Message.Text;
                    
                    if (message == "/start" || message == "Назад" || message == "Круто!!!" || message == "Все понятно!")
                    {
                        var replyKeyboardMarkup = new ReplyKeyboardMarkup(new[]   //возможно,не стоило все так хардкодить..
                        {
                            new[]
                            {
                                new KeyboardButton("Инфо"),
                                new KeyboardButton("Викторина"),
                                new KeyboardButton("Полезные материалы"),
                            },
                            new[]
                            {
                                new KeyboardButton("Интересные факты"),
                                new KeyboardButton("Табы"),
                                new KeyboardButton("лучшая песня"),
                            }
                           
                        });

                        await botClient.SendTextMessageAsync(chatId, "Выберите пункт меню:",
                            replyMarkup: replyKeyboardMarkup,
                            cancellationToken: cts.Token);
                    }
                    else if (message == "лучшая песня")
                    {
                        var animation = new InputMedia("https://media.tenor.com/CWgfFh7ozHkAAAAM/rick-astly-rick-rolled.gif");
                        await botClient.SendAnimationAsync(chatId, animation,caption:"были сомнения?",cancellationToken:cts.Token);
                        
                        
                    }
                    else if (message == "Инфо")
                    {
                        var replyMarkup = new ReplyKeyboardMarkup(new[]
                        {
                            new[]
                            {
                                new KeyboardButton("Что я умею?"),
                                new KeyboardButton("Назад")
                            }
                        });
                        await botClient.SendTextMessageAsync(chatId, "Приветствую! Я рад, что ты обратился ко мне." +
                                                                     " Я могу предложить тебе рассказать кучу интересных фактов о гитаре и группах," +
                                                                     " а также предложить готовые табулатуры и устроить викторину для проверки твоего музыкального слуха!",
                            replyMarkup: replyMarkup,
                            cancellationToken: cts.Token);
                    }
                    else if (message == "Полезные материалы")
                    {
                        // TODO
                        var replyMarkup = new ReplyKeyboardMarkup(new[]
                        {
                            new[]
                            {
                                new KeyboardButton("ютуб-видео"),
                                new KeyboardButton("картинки аккордов")
                            },
                            new[]
                            {
                                new KeyboardButton("Разные строи"),
                                new KeyboardButton("звучание аккордов")
                            },
                            new[]
                            {
                                new KeyboardButton("Назад")
                            }
                        });
                        await botClient.SendTextMessageAsync(chatId, "Чем могу помочь?",
                            replyMarkup: replyMarkup,
                            cancellationToken: cts.Token);
                    }
                    else if (message == "Викторина")
                    {
                        var replyMarkup = new ReplyKeyboardMarkup(new[]
                        {
                            new[]
                            {
                                new KeyboardButton("Аккорды")
                            },
                            new[]
                            {
                                new KeyboardButton("Назад")
                            }
                        });
                        await botClient.SendTextMessageAsync(chatId, "Выберите тип викторины",
                            replyMarkup: replyMarkup,
                            cancellationToken: cts.Token);
                    }
                    else if (message == "Аккорды")
                    {
                        rigthAnswer = Chords.randomPath();
                    }
                    else if (new[] { "Am", "C", "Dm", "Em", "F", "G" }.Contains(message))
                    { 
                        var replyMarkup = new ReplyKeyboardMarkup(new[]
                        {
                            new[]
                            {
                                new KeyboardButton("Ещё хочу сыграть!"),
                            },
                            new[]
                            {
                                new KeyboardButton("Назад")
                            }
                        });
                        if (message == rigthAnswer)
                        {
                            await botClient.SendTextMessageAsync(
                                chatId: chatId,
                                text: "молодец правильно!",replyMarkup: replyMarkup,
                                cancellationToken: cts.Token);
                        }
                        else
                        {

                            await botClient.SendTextMessageAsync(
                                chatId: chatId,
                                text: "NEправильно!",replyMarkup:replyMarkup,
                                cancellationToken: cts.Token);
                        }
                        
                    }
                    else if (message == "Ещё хочу сыграть!")
                    {
                        Chords.changePath();
                        rigthAnswer = Chords.chord;
                        await Chords.mainGame(botClient, chatId, cts);

                    }
                    else if (message == "Интересные факты")
                    {
                        var replyMarkup = new ReplyKeyboardMarkup(new[]
                        {
                            new[]
                            {
                                new KeyboardButton("Ещё хочу!"),
                            },
                            new[]
                            {
                                new KeyboardButton("Назад")
                            }
                        });
                        await botClient.SendTextMessageAsync(chatId, "Вы выбрали интересные факты",
                            replyMarkup: replyMarkup,cancellationToken: cts.Token);
                        await botClient.SendTextMessageAsync(chatId, Facts.getRandomFact(),cancellationToken: cts.Token);

                    }
                    else if (message == "Ещё хочу!")
                    {
                        await botClient.SendTextMessageAsync(chatId, Facts.getRandomFact(),cancellationToken: cts.Token);
                    }
                    else if (message == "Табы")
                    {
                        var replyKeyboardMarkup = new ReplyKeyboardMarkup(new[]
                        {
                            new[]
                            {
                                new KeyboardButton("Сплин"),
                                new KeyboardButton("Звери"),
                            },
                            new[]
                            {
                                new KeyboardButton("Мои любимые"),
                                new KeyboardButton("Нервы"),
                            },
                            new[]
                            {
                                new KeyboardButton("Назад"),
                            }
                        });

                        await botClient.SendTextMessageAsync(chatId,
                            "Выбирайте группу, я с радостью поделюсь табулатурой популярной песни",
                            replyMarkup: replyKeyboardMarkup,cancellationToken: cts.Token);
                    }
                    else if (message == "Что я умею?")
                    {
                        var replyMarkup = new ReplyKeyboardMarkup(new[]
                        {
                            new[]
                            {
                                new KeyboardButton("Круто!!!"),
                            },
                            new[]
                            {
                                new KeyboardButton("Все понятно!")
                            }
                        });
                        await botClient.SendTextMessageAsync(chatId, "Я тебе всё сейчас объясню.",
                            replyMarkup: replyMarkup,cancellationToken: cts.Token);
                        await botClient.SendTextMessageAsync(chatId,
                            "В разделе \"интересные факты\" ты сможешь узнать что-то новое в мире" +
                            " гитары.",cancellationToken: cts.Token);
                        await botClient.SendTextMessageAsync(chatId,
                            "В разделе \"Табы\" ты сможешь выбрать табулатуры из моих" +
                            " любимых групп и скачать себе её на телефон.",cancellationToken: cts.Token);
                        await botClient.SendTextMessageAsync(chatId,
                            "В разделе \"Викторина\" ты сможешь сыграть в игру." +
                            " Выбрав категорию, тебе надо будет отгадать  парочку(а может и нет) аккордов ",
                            cancellationToken: cts.Token);
                    }
                    else if (message == "Сплин")
                    {
                        InlineKeyboardMarkup inlineKeyboard = new(new[]
                        {
                            // first row
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData(text: "Время назад", callbackData: "Время назад"),
                                InlineKeyboardButton.WithCallbackData(text: "Мороз по коже",
                                    callbackData: "Мороз по коже"),
                            },
                            // second row
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData(text: "Романс", callbackData: "Романс"),
                                InlineKeyboardButton.WithCallbackData(text: "Мое сердце", callbackData: "Мое сердце"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData(text: "Весь этот бред",
                                    callbackData: "Весь этот бред"),
                                InlineKeyboardButton.WithCallbackData(text: "Топай", callbackData: "Топай"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData(text: "Передайте Гарри Поттеру",
                                    callbackData: "Передайте Гарри Поттеру"),
                                InlineKeyboardButton.WithCallbackData(text: "Выхода нет", callbackData: "Выхода нет"),
                            },
                        });

                        Message sentMessage = await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: "A message with an inline keyboard markup",
                            replyMarkup: inlineKeyboard,
                            cancellationToken: cts.Token
                        );
                    }
                    else if (message == "Нервы")
                    {
                        InlineKeyboardMarkup inlineKeyboard = new(new[]
                        {
                            // first row
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData(text: "батареи", callbackData: "батареи"),
                                InlineKeyboardButton.WithCallbackData(text: "кофе мой друг",
                                    callbackData: "кофе мой друг"),
                            },
                            // second row
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData(text: "май вай", callbackData: "май вай"),
                                InlineKeyboardButton.WithCallbackData(text: "самый дорогой человек",
                                    callbackData: "самый дорогой человек"),
                            }
                        });

                        Message sentMessage = await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: "A message with an inline keyboard markup",
                            replyMarkup: inlineKeyboard,
                            cancellationToken: cts.Token
                        );
                    }
                    else if (message == "Звери")
                    {
                        InlineKeyboardMarkup inlineKeyboard = new(new[]
                        {
                            // first row
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData(text: "Всё,что тебя касается",
                                    callbackData: "Всё,что тебя касается"),
                                InlineKeyboardButton.WithCallbackData(text: "До скорой встречи",
                                    callbackData: "До скорой встречи"),
                            },
                            // second row
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData(text: "Для тебя", callbackData: "Для тебя"),
                                InlineKeyboardButton.WithCallbackData(text: "Дожди-пистолеты",
                                    callbackData: "Дожди-пистолеты"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData(text: "Напитки покрепче",
                                    callbackData: "Напитки покрепче"),
                                InlineKeyboardButton.WithCallbackData(text: "Не беда", callbackData: "Не беда"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData(text: "Районы", callbackData: "Районы"),
                                InlineKeyboardButton.WithCallbackData(text: "Танцуй", callbackData: "Танцуй"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData(text: "Я с тобой", callbackData: "Я с тобой"),
                                InlineKeyboardButton.WithCallbackData(text: "Ты так прекрасна",
                                    callbackData: "Ты так прекрасна"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData(text: "Просто такая сильная любовь",
                                    callbackData: "Просто такая сильная любовь"),
                                InlineKeyboardButton.WithCallbackData(text: "Тебе", callbackData: "Тебе"),
                            }
                        });

                        Message sentMessage = await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: "Легендарные звери!",
                            replyMarkup: inlineKeyboard,
                            cancellationToken: cts.Token
                        );
                    }
                    else if (message == "Мои любимые")
                    {
                        InlineKeyboardMarkup inlineKeyboard = new(new[]
                        {
                            // first row
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData(text: "Березы", callbackData: "Березы"),
                                InlineKeyboardButton.WithCallbackData(text: "Девочка-Пай", callbackData: "Девочка-Пай"),
                            },
                            // second row
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData(text: "Кто такая Элиз?",
                                    callbackData: "Кто такая Элиз?"),
                                InlineKeyboardButton.WithCallbackData(text: "Лирическая", callbackData: "Лирическая"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData(text: "Самый лучший день",
                                    callbackData: "Самый лучший день"),
                                InlineKeyboardButton.WithCallbackData(text: "Седьмой лепесток",
                                    callbackData: "Седьмой лепесток"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData(text: "Супермаркет", callbackData: "Супермаркет"),
                                InlineKeyboardButton.WithCallbackData(text: "Этот город", callbackData: "Этот город"),
                            }
                        });

                        Message sentMessage = await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: "Мои лучшие песни,вот они!",
                            replyMarkup: inlineKeyboard,
                            cancellationToken: cts.Token
                        );
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(chatId, "Неверный выбор. Попробуйте еще раз.",
                            cancellationToken: cts.Token);
                    }

                    
                }

            
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
    
    
    

    
}