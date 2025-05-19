using Telegram.BotAPI.AvailableTypes;
using Telegram.BotAPI;
namespace TBotLogic.Commands
{
    internal interface IBotCommand
    {
        BotCommand BotCommand { get; }
        IBotStateMachine? Logic(TelegramBotClient bot, Message message, string[] parameters);
    }
}
