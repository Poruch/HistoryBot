using System.Text.RegularExpressions;
using TBotLogic;
using TBotLogic.Commands;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.AvailableTypes;
using Telegram.BotAPI.Extensions;
using Telegram.BotAPI.GettingUpdates;

namespace BotTemplateSample
{
    public sealed class MyBot : SimpleTelegramBotBase
    {
        private static string botLink = "https://api.telegram.org/file/bot";
        public static TelegramBotClient? Bot = null;
        private static User? Me = null;
        private static CommandCollection? commands = null;

        private IBotStateMachine stateMachine;

        public MyBot()
        {
            // Provides a better way to extract commands using regular expressions.
            SetCommandExtractor(Me.Username!);
            stateMachine = null;
        }

        public static void SetBot(string Token)
        {
            Bot = new TelegramBotClient(Token);
            botLink += Token;
            Me = Bot.GetMe();
            commands = new CommandCollection(Bot);
            Bot.SetMyCommands(commands.GetBotCommands());
        }

        public override void OnUpdate(Update update)
        {
            Console.WriteLine(
                "New update with id: {0}. Type: {1}",
                update.UpdateId,
                update.GetUpdateType()
            );
            base.OnUpdate(update);
        }

        public static string GetLink()
        {
            return botLink;
        }
        protected override void OnMessage(Message message)
        {

            // Ignore user 777000 (Telegram)
            if (message.From?.Id == TelegramConstants.TelegramId)
            {
                return;
            }

            Console.WriteLine("New message from chat id: {0}", message.Chat.Id);
            Console.WriteLine("Message Text: {0}", message.Text ?? "|:O");

            if (message.Chat.Type == ChatTypes.Private) // Private Chats
            {
                if (stateMachine != null)
                    if (stateMachine.Perform(message))
                    {
                        if (stateMachine.NextState() == null)
                        {
                            stateMachine = null;
                        }
                    }
            }

            // Group chats
            // Check if the message contains a command
            if (message.Entities?.Any(e => e.Type == "bot_command") is not true)
            {
                return;
            }

            if(stateMachine != null)
            if (stateMachine.IsBlock) return;
            // If the command includes a mention, you should verify that it is for your bot, otherwise you will need to ignore the command.
            string? pattern = $@"^\/(?<COMMAND>\w*)(?:|@{Me.Username})(?:$|\s(?<PARAMETERS>.*))";
            Match? match = Regex.Match(message.Text!, pattern, RegexOptions.IgnoreCase);
            if (!match.Success)
            {
                return;
            }

            string? command = match.Groups.Values.Single(v => v.Name == "COMMAND").Value; // Get command name
            string? @params =
                match.Groups.Values.SingleOrDefault(v => v.Name == "PARAMETERS")?.Value
                ?? string.Empty;

            Console.WriteLine("New command: {0}", command);
            OnCommand(message, command, @params);
        }

        protected override void OnCommand(Message message, string cmd, string parameters)
        {
            string[]? args = parameters.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            Console.WriteLine("Params: {0}", args.Length);
            stateMachine = commands.Command(cmd, message, args) ?? stateMachine;
        }

        protected override void OnBotException(BotRequestException exp)
        {
            Console.WriteLine("New BotException: {0}", exp.Message);
            Console.WriteLine("Error Code: {0}", exp.ErrorCode);
            Console.WriteLine();
        }

        protected override void OnException(Exception exp)
        {
            Console.WriteLine("New Exception: {0}", exp.Message);
            Console.WriteLine();
        }
    }
}