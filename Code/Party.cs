namespace HistoryBot.Code
{
    internal class Party
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Ages { get; set; }

        public string Leaders { get; set; }
        public Dictionary<string, string> Events { get => events; set => events = value; }

        public Party(StreamReader textReader)
        {
            FromFile(textReader);
        }

        List<string> titles = new List<string>()
        {
            "Событие",
            "Название",
            "Описание",
            "Партия",
            "Источники",
            "Годы",
            "Реакция",
            "Лидеры"
        };

        Dictionary<string, string> events = new Dictionary<string, string>();
        
        public void FromFile(StreamReader textReader)
        {
            
            string title = textReader.ReadLine();
            string currentLine = "";

            string GetTitle()
            {
                string result = "";
                currentLine = textReader.ReadLine();
                while (!titles.Contains(currentLine) && !textReader.EndOfStream)
                {
                    result += currentLine;
                    currentLine = textReader.ReadLine();
                }
                //if(textReader.EndOfStream && currentLine != "")
                //{
                //    result += currentLine;
                //}
                return result;
            }

            while (!textReader.EndOfStream)
            {
                if (title == "Название")
                {
                    Name = GetTitle();
                    title = currentLine;
                }
                if(title == "Лидеры")
                {
                    Leaders = GetTitle();
                    title = currentLine;
                }
                if (title == "Годы")
                {
                    Ages = GetTitle();
                    title = currentLine;
                }
                if (title == "Описание")
                {
                    Description = GetTitle();
                    title = currentLine;
                }
                if (title == "Событие")
                {
                    string name = GetTitle(); 
                    string description = GetTitle();
                    Events.Add(name, description);
                    title = currentLine;
                }
            }
        }

        public override string ToString()
        {
            return $"Партия: {Name}";
        }

    }
}
