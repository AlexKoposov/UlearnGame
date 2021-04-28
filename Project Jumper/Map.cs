using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project_Jumper
{
    public class Map
    {
        public MapCell[,] Level { get; private set; }
        public int Width => Level.GetLength(0);
        public int Height => Level.GetLength(1);

        private const string startMap = @"
XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
X                             X
X                             X
X                             X
X                             X
X                             X
X                             X
X                             X
X                             X
X                             X
X                             X
X                     B       X
X                  B          X
X               B             X
X            B                X
X         B                SSSX
XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";

        private const string highTest = @"
XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
X                             X
X                             X
XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";

        public Map()
        {
            Level = MapCreator.CreateMap(startMap);
        }
    }
}
