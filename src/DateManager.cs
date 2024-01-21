using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BleedCal
{
    public sealed class DateManager
    {
        private static DateManager _instance = null;
        private readonly string saveFileName = "perioddates.txt";

        public List<DateOnly> periodDays;

        private DateManager() 
        { 
            periodDays = new List<DateOnly>();
        }

        public static DateManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DateManager();
                }

                return _instance;
            }
        }

        public void SaveDataToFile()
        {
            var stringBuilder = new StringBuilder();

            foreach (var day in periodDays)
            {
                stringBuilder.AppendLine($"{day.ToString()},");
            }

            var targetFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), saveFileName);

            if (!File.Exists(targetFile))
            {
                File.Create(targetFile);
            }

            File.WriteAllText(targetFile, stringBuilder.ToString());
        }

        public void LoadData()
        {
            var targetFile = Path.Combine(FileSystem.Current.AppDataDirectory, saveFileName);

            if (!File.Exists(targetFile))
            {
                return;
            }

            var fileContent = File.ReadAllLines(targetFile);

            foreach (string line in fileContent)
            {
                periodDays.Add(DateOnly.Parse(line));
            }
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            foreach(var day in periodDays)
            {
                stringBuilder.AppendLine(day.ToString());
            }

            return stringBuilder.ToString();
        }
    }
}
