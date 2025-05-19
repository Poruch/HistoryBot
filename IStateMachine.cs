using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
