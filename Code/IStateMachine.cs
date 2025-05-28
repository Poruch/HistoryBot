using Telegram.BotAPI.AvailableTypes;

namespace TBotLogic.Commands
{
    internal interface IBotStateMachine
    {
        delegate bool State(Message message);
        bool Perform(Message message);
        IBotStateMachine NextState();
        bool IsBlock { get; }
    }
}
