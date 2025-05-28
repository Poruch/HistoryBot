using TBotLogic.Commands;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableTypes;

namespace HistoryBot.Code
{
    internal interface IBotCommand
    {
        BotCommand BotCommand { get; }
        IBotStateMachine? Logic(TelegramBotClient bot, Message message, string[] parameters);
    }
}
