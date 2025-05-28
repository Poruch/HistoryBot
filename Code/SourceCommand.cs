using TBotLogic.Commands;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.AvailableTypes;

namespace HistoryBot.Code
{
    internal class SourceCommand : IBotCommand
    {
        BotCommand botCommand;
        public BotCommand BotCommand
        {
            get => botCommand;
        }
        public SourceCommand()
        {
            botCommand = new BotCommand("getsources", "Get sources");
        }


        public IBotStateMachine? Logic(TelegramBotClient bot, Message message, string[] parameters)
        {
            string result = System.IO.File.OpenText("D:\\Repos\\HistoryBot\\Texts\\Sources.txt").ReadToEnd();
            bot.SendMessage(message.Chat.Id, result,null,null, "HTML");
            return null;
        }
    }
}
