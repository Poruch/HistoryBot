using System.IO;
using TBotLogic.Commands;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.AvailableTypes;

namespace HistoryBot.Code
{
    internal class DefaultBotStateMachine : IBotStateMachine
    {
        public bool IsBlock => false;
        TelegramBotClient client;
        long Id { get; set; }
        public DefaultBotStateMachine(TelegramBotClient client, long id)
        {
            string path = "D:\\Repos\\HistoryBot\\Texts\\";
            DirectoryInfo directoryInfo = new DirectoryInfo(path);

            var files = directoryInfo.GetFiles();
            foreach (var file in files)
            {
                if (file.Name == "Events.txt")
                {
                    var reader = file.OpenText();
                    string line = reader.ReadLine();
                    while (!reader.EndOfStream)
                    {
                        if (line == "Событие")
                        {
                            Event newEvent = new Event(reader);
                            events.Add(newEvent.Name, newEvent);
                        }
                    }
                }
                else if (file.Name == "Sources.txt")
                {

                }
                else
                    parties.Add(new Party(file.OpenText()));
            }
            this.client = client;
            this.Id = id;
            states = new List<Func<Message, bool>>()
            {
                GetNumberParty,
                GetPartyOrEvents,
                GetPartyOrEvent,
                GetEvent,
            };
        }
        List<Func<Message,bool>> states { get; set; }
        public List<Party> parties = new List<Party>();
        public Dictionary<string,Event> events = new Dictionary<string,Event>();


        int currentState = 0;
        int supportNumber = 0;

        Party currentParty;

        //private bool GetPartyOrEvent(Message message)
        //{

        //}
        private bool GetNumberParty(Message message)
        {
            bool isSuccess = int.TryParse(message.Text, out supportNumber);
            if (!isSuccess)
            {
                client.SendMessage(Id,"Вы ввели число не правильно");
            }
            if(supportNumber == 0 || supportNumber > parties.Count)
            {
                client.SendMessage(Id, "Партии с таким номером нет");
                isSuccess = false;
            }
            if (isSuccess)
            {
                currentParty = parties[supportNumber-1];
                client.SendMessage(Id, "*" + currentParty.Name + "*\n" + currentParty.Ages + " " + currentParty.Leaders, null, null, "Markdown");
                client.SendMessage(Id, currentParty.Description == "" ? "Пусто": currentParty.Description);
                
                client.SendMessage(Id, "Вы также можете можете узнать о:\n" +
                    "1. Другой партии\n" +
                    "2. О некоторой деятельности партии\n");
                currentState = 1;
            }
            return isSuccess;
        }
        private bool GetPartyOrEvents(Message message)
        {
            bool isSuccess = int.TryParse(message.Text, out supportNumber);
            if (!isSuccess)
            {
                client.SendMessage(Id, "Вы ввели число не правильно");
            }
            if (supportNumber == 0 || supportNumber > 2)
            {
                client.SendMessage(Id, "Варианта с таким номером нет");
                isSuccess = false;
            }
            if (isSuccess)
            {
                if(supportNumber == 1)
                {
                    InitialState();
                }
                if(supportNumber == 2)
                {
                    client.SendMessage(Id, "Некоторые события в которых участровола эта партия:");
                    int i = 1;
                    foreach (KeyValuePair<string, string> @event in currentParty.Events)
                    {
                        client.SendMessage(Id, $"{i++}. " + @event.Key + "\nРеакция:" + @event.Value);
                    }
                    client.SendMessage(Id, "1. Выбрать другую партию\n" +
                                           "2. Пояснить событие\n");
                    currentState = 2;
                }
            }
            return isSuccess;
        }
        private bool GetPartyOrEvent(Message message)
        {
            bool isSuccess = int.TryParse(message.Text, out supportNumber);
            if (!isSuccess)
            {
                client.SendMessage(Id, "Вы ввели число не правильно");
            }
            if (supportNumber == 0 || supportNumber > 2)
            {
                client.SendMessage(Id, "Варианта с таким номером нет");
                isSuccess = false;
            }
            if (isSuccess)
            {
                if (supportNumber == 1)
                {
                    InitialState();
                }
                if (supportNumber == 2)
                {
                    client.SendMessage(Id, "Напишите название события");
                    currentState = 3;
                }
            }
            return isSuccess;
        }
        public bool GetEvent(Message message)
        {
            bool isSuccess = events.ContainsKey(message.Text);
            if (!isSuccess)
            {
                client.SendMessage(Id, "Такого события нет");
            }
            if (isSuccess)
            {
                Event currentEvent = events[message.Text];
                client.SendMessage(Id,"*"+ currentEvent.Name + "*\n" + currentEvent.Ages,null,null, "Markdown");
                client.SendMessage(Id, currentEvent.Description == "" ? "Пусто" : currentEvent.Description);

                client.SendMessage(Id, "Можете можете узнать о:\n" +
                    "1. Другой партии\n" +
                    "2. Другом событии");
                currentState = 1;
            }
            return isSuccess;
        }
        private void PrintParties()
        {
            int i = 1;
            foreach (var party in parties)
            {
                client.SendMessage(Id, (i++).ToString() + ". " + party.ToString());
            }
        }
        public void InitialState()
        {            
            client.SendMessage(Id, "Выберите номер партии о которой хотите узнать");
            PrintParties();
            currentState = 0;
        }

        public IBotStateMachine NextState()
        {
            return this;
        }

        public bool Perform(Message message)
        {
            return states[currentState].Invoke(message);
        }
    }
}
