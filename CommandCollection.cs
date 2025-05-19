
using System.Linq;
using System.Reflection;
using TBotLogic.Commands;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableTypes;

namespace TBotLogic
{
    internal class CommandCollection
    {
        Dictionary<string, IBotCommand> commands;
        TelegramBotClient client;
        public CommandCollection(TelegramBotClient telegramBot)
        {
            commands = new Dictionary<string, IBotCommand>();
            client = telegramBot;
            List<Type> commandsTypes = new List<Type>(Assembly
                .GetExecutingAssembly().GetTypes()
                .Where(type => typeof(IBotCommand).IsAssignableFrom(type) && !type.IsInterface));
            foreach (Type type in commandsTypes)
            {
                IBotCommand command = (IBotCommand)Activator.CreateInstance(type)!;
                if (command != null)
                    commands.Add(command.BotCommand.Command, command);
            }
        }
        public BotCommand[] GetBotCommands()
        {
            return commands.Values.Select((command, index) => { return command.BotCommand; }).ToArray();
        }
        public IBotStateMachine? Command(string name, Message message, string[] parameters)
        {
            if (commands.ContainsKey(name))
                return commands[name].Logic(client, message, parameters);
            return null;
        }
    }
}
