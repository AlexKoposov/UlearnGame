using System;
using System.Collections.Generic;
using System.Text;

namespace Project_Jumper
{
    public class Map
    {
        public MapCell[,] Level { get; private set; }
        public int Width => Level.GetLength(0);
        public int Height => Level.GetLength(1);

        public Map(int width, int height)
        {
            Level = new MapCell[width, height];
            CreateSimpleLevel();
        }

        void CreateSimpleLevel()
        {
            for (var i = 0; i < Width; i++)
                for (var j = 0; j < Height; j++)
                {
                    if (i == 0
                        || j == 0
                        || i == Width - 1
                        || j == Height - 1)
                        Level[i, j] = new MapCell("Border", true, true);
                    else if (i % 6 == 0 && j % 4 == 0)
                        Level[i, j] = new MapCell("Block", true, true);
                    else Level[i, j] = new MapCell("Space", true, false);
                }
            Level[6, 20] = new MapCell("Space", true, false);
        }
    }
}
