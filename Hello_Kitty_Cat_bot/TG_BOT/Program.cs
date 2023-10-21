using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

var botClient = new TelegramBotClient("6462235249:AAHkDpEIoThLkivRyJRBhCaurEvUOSYC-Bs");

using CancellationTokenSource cts = new();

ReceiverOptions receiverOptions = new()
{
    AllowedUpdates = Array.Empty<UpdateType>()
};

botClient.StartReceiving(
    updateHandler: HandleUpdateAsync,
    pollingErrorHandler: HandlePollingErrorAsync,
    receiverOptions: receiverOptions,
    cancellationToken: cts.Token
);

var me = await botClient.GetMeAsync();

Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();

cts.Cancel();

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)

{

    if (update.Message is not { } message)
        return;
    if (message.Text is not { } messageText)
        return;

    var chatId = message.Chat.Id;
    var kol = "Комплекс педикюр 1900\r\n" + "Маникюр с покрытием 1200\r\n" + "Маникюр без покрытия 700\r\n" + "Педикюр стандарт 1000\r\n";
    var sost = "https://t.me/saharova_manic";
    var graf = "Понедельник - пятница с 9 до 20\r\n" + "Суббота с 10 до 18\r\n" + "Воскресенье с 10 до 16\r\n";
    Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");
    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
{
    new KeyboardButton[] { "Услуги", "Примеры работ", "График работы", "Контакты"},
})
    {
        ResizeKeyboard = true
    };
    if (messageText == "/start")
    {
        Message sentMessage = await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Поскорее записывайся",
            replyMarkup: replyKeyboardMarkup,
            cancellationToken: cancellationToken);

        await botClient.SendTextMessageAsync(chatId, sost, cancellationToken: cancellationToken);
    }
    if (messageText == "Услуги")
    {
        await botClient.SendTextMessageAsync(chatId, kol, cancellationToken: cancellationToken);
    }
    if (messageText == "График работы")
    {
        await botClient.SendTextMessageAsync(chatId, graf, cancellationToken: cancellationToken);
    }
    if (messageText == "Примеры работ")
    {
        await botClient.SendMediaGroupAsync(
        chatId: chatId,
        media: new IAlbumInputMedia[]
         {
                      new InputMediaPhoto(
                        InputFile.FromUri("https://github.com/angel007L/asdfg/blob/main/photo_2023-10-12%2022.54.11.jpeg?raw=true")),
                        new InputMediaPhoto(
                         InputFile.FromUri("https://github.com/angel007L/asdfg/blob/main/photo_2023-10-12%2022.54.13.jpeg?raw=true")),
         },
            cancellationToken: cancellationToken);
        await botClient.SendMediaGroupAsync(
            chatId: chatId,
        media: new IAlbumInputMedia[]
        {
                      new InputMediaPhoto(
                        InputFile.FromUri("https://github.com/angel007L/asdfg/blob/main/photo_2023-10-12%2022.54.15.jpeg?raw=true")),
                        new InputMediaPhoto(
                         InputFile.FromUri("https://github.com/angel007L/asdfg/blob/main/photo_2023-10-12%2022.54.17.jpeg?raw=true")),
         },
        cancellationToken: cancellationToken);
    }

}

Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException
            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
        _ => exception.ToString()
    };

    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
}