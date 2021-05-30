using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Project_Jumper
{
    class LevelConverter
    {
        private readonly static string currentPath = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName.ToString();

        public static List<Tuple<int, string>> GetAllLevels()
        {
            var result = new List<Tuple<int, string>>();
            var rawLevels = File.ReadAllText(Path.Combine(currentPath, "Levels.txt")).Split('-', StringSplitOptions.RemoveEmptyEntries);
            foreach (var str in rawLevels)
            {
                var splited = str.Split('|', StringSplitOptions.RemoveEmptyEntries);
                result.Add(Tuple.Create(ConvertToLevelTime(splited[0]), splited[1]));
            }
            return result;
        }

        public static void WriteChanges(List<Tuple<int, string>> input)
        {
            var converted = string.Join('-', input
                .Select(x => string.Join('|', ConvertToDefaultTime(x.Item1), x.Item2)));
            File.WriteAllText(Path.Combine(currentPath, "Levels.txt"), converted);
        }

        public static int ConvertToLevelTime(string input)
        {
            var splited = input.Split(':', StringSplitOptions.RemoveEmptyEntries);
            return int.Parse(splited[0]) * 60 + int.Parse(splited[1]);
        }

        public static string ConvertToDefaultTime(int timeInSeconds) =>
            $"{timeInSeconds / 60}:{string.Format("{0:00}", timeInSeconds % 60)}";
    }
}
