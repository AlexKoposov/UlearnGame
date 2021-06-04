using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Project_Jumper
{
    public class Map
    {
        public MapCell[,] Level { get; private set; }
        public int LevelTimeSeconds { get; private set; }
        public int BestLevelTime { get; private set; }
        public int Width => Level.GetLength(0);
        public int Height => Level.GetLength(1);
        public readonly Point StartPosition;
        private int levelId = 0;
        private List<string> AvailableLevels { get; set; }
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

        public void ChangeToNextLevel()
        {
            ChangeLevel();
            if (levelId == AvailableLevels.Count - 1)
            {
                var currentForm = (GameWindow)Application.OpenForms["GameWindow"];
                currentForm.IsLastLevelCompleted = true;
            }
        }

        public void ChangeToPrevLevel()
        {
            if (levelId == AvailableLevels.Count - 1)
            {
                var currentForm = (GameWindow)Application.OpenForms["GameWindow"];
                currentForm.IsLastLevelCompleted = false;
            }
            ChangeLevel(-1);
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
            UpdateBestTime(false);
        }

        public void UpdateBestTime(bool isNewRecord = true)
        {
            if (LevelTimeSeconds < BestLevelTime || !isNewRecord)
            {
                if (isNewRecord) BestLevelTime = LevelTimeSeconds;
                converted[levelId] = Tuple.Create(BestLevelTime, converted[levelId].Item2);
                LevelConverter.WriteChanges(converted);
            }
        }

        private void ChangeLevel(int step = 1)
        {
            levelId += step;
            if (levelId >= 0 && levelId < AvailableLevels.Count)
            {
                Level = MapBuilder.CreateMap(AvailableLevels[levelId]);
                BestLevelTime = converted[levelId].Item1;
            }
            else if (levelId >= 0)
            {
                levelId = AvailableLevels.Count - 1;
                var currentForm = (GameWindow)Application.OpenForms["GameWindow"];
                currentForm.IsLastLevelCompleted = true;
            }
            else levelId = 0;
        }
    }
}
