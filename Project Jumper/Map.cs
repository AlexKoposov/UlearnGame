using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Project_Jumper
{
    public class Map
    {
        public MapCell[,] Level { get; private set; }
        public int Width => Level.GetLength(0);
        public int Height => Level.GetLength(1);
        public readonly Point startPos;

        private const string playground = @"
XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
X                                   X
X                                   X
X                                   X
X                                   X
X                                   X
X                                   X
X                                   X
X                                   X
X                                   X
X                   G               X
X                                   X
X             J           J         X
X                                   X
X   *                 B             X
X   *              B                X
X   *           B                   X
X            B                      X
X         B           G       J  SSSX
XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";

        private const string heightTest = @"
XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
X                             X
X                             X
XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";

        private const string level1 = @"
XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
X                                   X
X                                   X
X                                   X
XBBB                                X
X       J                           X
X    *         J                    X
X       *  *                        X
X             *  * BBB              X
X                         J         X
X                        * *        X
X                               BBB X
X                                   X
X                                   X
X                         BBBB      X
X         B    J    B               X
X      B  B         B  J            X
X   B  B  B         B               X
X   BSSBSSBSSSSSSSSSBSSSSSSSSSSSSSSSX
XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";

        public Map()
        {
            startPos = new Point(1, 17);
            Level = MapCreator.CreateMap(level1);
        }
    }
}
