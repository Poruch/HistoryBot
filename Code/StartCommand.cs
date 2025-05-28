using TBotLogic.Commands;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableTypes;

namespace HistoryBot.Code
{
    internal class StartCommand : IBotCommand
    {
        BotCommand botCommand;
        public BotCommand BotCommand
        {
            get => botCommand;
        }
        public StartCommand()
        {
            botCommand = new BotCommand("start", "Start work");
        }


        public IBotStateMachine? Logic(TelegramBotClient bot, Message message, string[] parameters)
        {
            DefaultBotStateMachine stateMachine = new DefaultBotStateMachine(bot,message.Chat.Id);
            stateMachine.InitialState();
            return stateMachine;
        }
    }
}
