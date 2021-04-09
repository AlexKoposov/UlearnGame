using System;
using System.Collections.Generic;
using System.Text;

namespace Project_Jumper
{
    public class Map
    {
        public char[,] Level { get; private set; }
        public int Width => Level.GetLength(0);
        public int Height => Level.GetLength(1);

        public Map(int width, int height)
        {
            Level = new char[width, height];
            CreateLevel();
        }

        void CreateLevel()
        {
            for (var i = 0; i < Width; i++)
                for (var j = 0; j < Height; j++)
                {
                    if (i == 0
                        || j == 0
                        || i == Width - 1
                        || j == Height - 1)
                        Level[i, j] = 'X';
                }
        }
    }
}
