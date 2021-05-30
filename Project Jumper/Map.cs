using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Project_Jumper
{
    public class Map
    {
        public MapCell[,] Level { get; private set; }
        public List<string> AvailableLevels { get; private set; }
        public int LevelTimeSeconds { get; private set; }
        public int BestLevelTime { get; private set; }
        public int Width => Level.GetLength(0);
        public int Height => Level.GetLength(1);
        public readonly Point StartPosition;
        private int levelId = 0;
        private readonly List<Tuple<int, string>> converted;

        public Map()
        {
            converted = LevelConverter.GetAllLevels();
            BestLevelTime = converted[levelId].Item1;
            AvailableLevels = converted
                .Select(t => t.Item2)
                .ToList();
            StartPosition = new Point(1, 1);
            Level = MapBuilder.CreateMap(AvailableLevels[levelId]);
        }

        public void ChangeLevel()
        {
            if (++levelId < AvailableLevels.Count)
            {
                Level = MapBuilder.CreateMap(AvailableLevels[levelId]);
                BestLevelTime = converted[levelId].Item1;
            }
            else levelId = AvailableLevels.Count - 1;
        }

        public void IncreaseTime()
        {
            LevelTimeSeconds++;
        }

        public void ResetTime()
        {
            LevelTimeSeconds = 0;
        }

        public void ResetBestTime()
        {
            BestLevelTime = 9999;
            UpdateBestTime(9999);
        }

        public void UpdateBestTime(int newTime = -1)
        {
            if (LevelTimeSeconds < BestLevelTime
                || BestLevelTime == 9999)
            {
                if (newTime == -1)
                    BestLevelTime = LevelTimeSeconds;
                converted[levelId] = Tuple.Create(BestLevelTime, converted[levelId].Item2);
                LevelConverter.WriteChanges(converted);
            }
        }
    }
}
