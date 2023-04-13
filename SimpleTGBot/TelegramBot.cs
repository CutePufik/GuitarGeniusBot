
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using File = System.IO.File;
using Telegram.Bot;




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

    /// <summary>
    /// Инициализирует и обеспечивает работу бота до нажатия клавиши Esc
    /// </summary>
    public async Task Run()
    {
        
        

        // Служебные вещи для организации правильной работы с потоками
        

        // Разрешённые события, которые будет получать и обрабатывать наш бот.
        // Будем получать только сообщения. При желании можно поработать с другими событиями.
        ReceiverOptions receiverOptions = new ReceiverOptions()
        {
            AllowedUpdates = new[] { UpdateType.Message }
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
        if (message is null)
        {
            return;
        }

        
        // Будем обрабатывать только текстовые сообщения.
        // При желании можно обрабатывать стикеры, фото, голосовые и т. д.
        //
        // Обратите внимание на использованную конструкцию. Она эквивалентна проверке на null, приведённой выше.
        // Подробнее об этом синтаксисе: https://medium.com/@mattkenefick/snippets-in-c-more-ways-to-check-for-null-4eb735594c09
        if (message.Text is not { } messageText)
        {
            return;
        }

        // Получаем ID чата, в которое пришло сообщение. Полезно, чтобы отличать пользователей друг от друга.
        var chatId = message.Chat.Id;
        GetMessages(chatId);


        // Печатаем на консоль факт получения сообщения
        Console.WriteLine($"Получено сообщение в чате {chatId}: '{messageText}'");

        // TODO: Обработка пришедших сообщений

        // Отправляем обратно то же сообщение, что и получили
        Message sentMessage = await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Ты написал:\n" + messageText,
            cancellationToken: cancellationToken);
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
    async Task GetMessages(long chatId)
    {
        int offset = 0;
        int timeout = 0;
        try
        {
            var updates = await botClient.GetUpdatesAsync(offset, timeout);
            foreach (var update in updates)
            {
                var message = update.Message.Text;
                if (message == "/start" || message == "Назад" || message == "Круто!!!" || message == "Все понятно!")
                {
                    var replyKeyboardMarkup = new ReplyKeyboardMarkup(new[]
                    {
                        new[]
                        {
                            new KeyboardButton("Инфо"),
                            new KeyboardButton("Викторина"),
                        },
                        new[]
                        {
                            new KeyboardButton("Интересные факты"),
                            new KeyboardButton("Табы"),
                        }
                    });

                    await botClient.SendTextMessageAsync(chatId, "Выберите пункт меню:",
                        replyMarkup: replyKeyboardMarkup);
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
                                                                 " а также предложить готовые табулатуры и устроить викторину для проверки твоего музыкального слуха!", replyMarkup: replyMarkup);
                }
                else if (message == "Викторина")
                {
                    var replyMarkup = new ReplyKeyboardMarkup(new[]
                    {
                        new[]
                        {
                            new KeyboardButton("Аккорды"),
                            new KeyboardButton("Одиночные звуки")
                        },
                        new[]
                        {
                            new KeyboardButton("Назад")
                        }
                    });
                    await botClient.SendTextMessageAsync(chatId, "Выберите тип викторины", replyMarkup: replyMarkup);
                }
                else if (message == "Аккорды")
                {
                    Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "Начинаем игру!",
                        replyMarkup: new ReplyKeyboardRemove()
                    );
                }
                else if (message == "Одиночные звуки")
                {
                    Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "Начинаем игру!",
                        replyMarkup: new ReplyKeyboardRemove()
                        );
                    
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
                        replyMarkup: replyMarkup);
                    await botClient.SendTextMessageAsync(chatId,Facts.getRandomFact());
                    
                }
                else if (message == "Ещё хочу!")
                {
                    await botClient.SendTextMessageAsync(chatId,Facts.getRandomFact());
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
                        replyMarkup: replyKeyboardMarkup);
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
                        replyMarkup: replyMarkup);
                    await botClient.SendTextMessageAsync(chatId, "В разделе \"интересные факты\" ты сможешь узнать что-то новое в мире" +
                                                                 " гитары.");
                    await botClient.SendTextMessageAsync(chatId, "В разделе \"Табы\" ты сможешь выбрать табулатуры из моих" +
                                                                 " любимых групп и скачать себе её на телефон.");
                    await botClient.SendTextMessageAsync(chatId, "В разделе \"Викторина\" ты сможешь сыграть в игру." +
                                                                 " Выбрав категорию, тебе надо будет отгадать  парочку(а может и нет) аккордов/нот ");
                }
                else if (message == "Сплин")
                {
                    InlineKeyboardMarkup inlineKeyboard = new(new[]
                    {
                        // first row
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData(text: "Время назад", callbackData: "11"),
                            InlineKeyboardButton.WithCallbackData(text: "Мороз по коже", callbackData: "12"),
                        },
                        // second row
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData(text: "Романс", callbackData: "21"),
                            InlineKeyboardButton.WithCallbackData(text: "Мое сердце", callbackData: "22"),
                        },
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData(text: "Весь этот бред", callbackData: "21"),
                            InlineKeyboardButton.WithCallbackData(text: "Топай", callbackData: "22"),
                        },
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData(text: "Передайте Гарри Поттеру", callbackData: "21"),
                            InlineKeyboardButton.WithCallbackData(text: "Выхода нет", callbackData: "22"),
                        },
                    });

                    Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "A message with an inline keyboard markup",
                        replyMarkup: inlineKeyboard
                        );
                }
                else if (message == "Нервы")
                {
                    InlineKeyboardMarkup inlineKeyboard = new(new[]
                    {
                        // first row
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData(text: "батареи", callbackData: "11"),
                            InlineKeyboardButton.WithCallbackData(text: "кофе мой друг", callbackData: "12"),
                        },
                        // second row
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData(text: "май вай", callbackData: "21"),
                            InlineKeyboardButton.WithCallbackData(text: "самый дорогой человек", callbackData: "22"),
                        }
                    });

                    Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "A message with an inline keyboard markup",
                        replyMarkup: inlineKeyboard
                    );
                }
                else if (message == "Звери")
                {
                    InlineKeyboardMarkup inlineKeyboard = new(new[]
                    {
                        // first row
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData(text: "Всё,что тебя касается", callbackData: "11"),
                            InlineKeyboardButton.WithCallbackData(text: "До скорой встречи", callbackData: "12"),
                        },
                        // second row
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData(text: "Для тебя", callbackData: "21"),
                            InlineKeyboardButton.WithCallbackData(text: "Дожди-пистолеты", callbackData: "22"),
                        },
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData(text: "Напитки покрепче", callbackData: "21"),
                            InlineKeyboardButton.WithCallbackData(text: "Не беда", callbackData: "22"),
                        },
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData(text: "Районы", callbackData: "21"),
                            InlineKeyboardButton.WithCallbackData(text: "Танцуй", callbackData: "22"),
                        },
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData(text: "Я с тобой", callbackData: "21"),
                            InlineKeyboardButton.WithCallbackData(text: "Ты так прекрасна", callbackData: "22"),
                        },
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData(text: "Просто такая сильная любовь", callbackData: "21"),
                            InlineKeyboardButton.WithCallbackData(text: "Тебе", callbackData: "22"),
                        }
                    });

                    Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "Легендарные звери!",
                        replyMarkup: inlineKeyboard
                    );
                }
                else if (message == "Мои любимые")
                {
                    InlineKeyboardMarkup inlineKeyboard = new(new[]
                    {
                        // first row
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData(text: "Березы", callbackData: "11"),
                            InlineKeyboardButton.WithCallbackData(text: "Девочка-Пай", callbackData: "12"),
                        },
                        // second row
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData(text: "Кто такая Элиз?", callbackData: "21"),
                            InlineKeyboardButton.WithCallbackData(text: "Лирическая", callbackData: "22"),
                        },
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData(text: "Самый лучший день", callbackData: "21"),
                            InlineKeyboardButton.WithCallbackData(text: "Седьмой лепесток", callbackData: "22"),
                        },
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData(text: "Супермаркет", callbackData: "21"),
                            InlineKeyboardButton.WithCallbackData(text: "Этот город", callbackData: "22"),
                        }
                    });

                    Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "Мои лучшие песни,вот они!",
                        replyMarkup: inlineKeyboard
                    );
                }
                else
                {
                    await botClient.SendTextMessageAsync(chatId, "Неверный выбор. Попробуйте еще раз.");
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
    
    private async Task chooseTabs(ChatId chatId,String mainPath,int idSong,String group)
    {
        SongTab[] Group = new SongTab[4];
        switch (group)
        {
            case "Сплин" :
                Group = SongTab.getSplinTabs();
                break;
            case "Звери" :
                Group = SongTab.getZveriTabs();
                break;
            case "Мои любимые" :
                Group = SongTab.getMyFavouriteTabs();
                break;
            case "Нервы" :
                Group = SongTab.getNervesTabs();
                break;
        }
        var filePath = mainPath + $"{Group[idSong]}";
        await using Stream stream = File.OpenRead(filePath);
        Message message = await botClient.SendDocumentAsync(
            chatId: chatId,
            document: new InputOnlineFile(content: stream, fileName: "tabs"),
            caption: "The Tragedy of Hamlet,\nPrince of Denmark");
    }
    
    private async void BotOnCallbackQueryReceived(object sender, Telegram.Bot.Args.CallbackQueryEventArgs callbackQueryEventArgs)
    {
        var callbackQuery = callbackQueryEventArgs.CallbackQuery;
    
        if (callbackQuery.Data == "button1")
        {
            // выполнить действия, соответствующие нажатию кнопки button1
        }
        else if (callbackQuery.Data == "button2")
        {
            // выполнить действия, соответствующие нажатию кнопки button2
        }
        else
        {
            // кнопка неизвестна, ничего не делаем
        }
    }

    
}