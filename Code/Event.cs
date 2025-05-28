using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistoryBot.Code
{
    internal class Event
    {
        List<string> titles = new List<string>()
        {
            "Событие",
            "Название",
            "Описание",
            "Партия",
            "Источники",
            "Годы",
            "Реакция"
        };
        public string Name {  get; set; }
        public string Description { get; set; }
        public string Ages { get; set; }
        public Event(StreamReader textReader)
        {
            string currentLine = textReader.ReadLine();
            string GetTitle()
            {
                string result = "";
                currentLine = textReader.ReadLine();
                while (!titles.Contains(currentLine) && !textReader.EndOfStream)
                {
                    result += currentLine;
                    currentLine = textReader.ReadLine();
                }
                return result;
            }
            if (currentLine == "Название")
            {
                Name = GetTitle();
            }
            if(currentLine == "Годы")
            {
                Ages = GetTitle();
            }
            if(currentLine == "Описание")
            {
                Description = GetTitle();
            }
        }
        public override string ToString()
        {
            return Name + "\n" + Ages + "\n" + Description;
        }
    }
}
