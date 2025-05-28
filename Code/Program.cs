
using Newtonsoft.Json;
using Telegram.BotAPI.GettingUpdates;
namespace BotTemplateSample
{
    class Config
    {
        public Config(string token, string connectionString)
        {
            this.token = token;
            this.connectionString = connectionString;
        }

        private string token = "";
        private string connectionString = "";

        public string Token { get => token; }
        public string ConnectionString { get => connectionString; }
    }



    class Program
    {
        static Dictionary<long, MyBot> botInstances = new Dictionary<long, MyBot>();
        static void Main()
        {
            Console.WriteLine("Start!");
            var file = System.IO.File.OpenText("D:\\Repos\\HistoryBot\\config.json");
            string str = file.ReadToEnd();
            var config = JsonConvert.DeserializeObject<Config>(str);

            if (config == null) return;
            MyBot.SetBot(config.Token);
            MyBot.Bot?.DeleteWebhook();

            //RegisterManager.SetDB(config.ConnectionString);


            // Long Polling: Start
            var updates = MyBot.Bot?.GetUpdates();
            //if(updates.Count() != 0)
            //updates = MyBot.Bot.GetUpdates(updates.Last().UpdateId + 1);
            while (true)
            {
                if (updates?.Any() ?? false)
                {
                    foreach (var update in updates)
                    {
                        if (botInstances.ContainsKey(update.Message?.Chat.Id ?? 0))
                        {
                            botInstances[update.Message.From.Id].OnUpdate(update);
                        }
                        else
                        {
                            var botInstance = new MyBot();
                            botInstance.OnUpdate(update);
                            botInstances.Add(update.Message.From.Id, botInstance);
                        }
                    }
                    var offset = updates.Last().UpdateId + 1;
                    updates = MyBot.Bot?.GetUpdates(offset);
                }
                else
                {
                    updates = MyBot.Bot?.GetUpdates();
                }
            }
        }
    }
}