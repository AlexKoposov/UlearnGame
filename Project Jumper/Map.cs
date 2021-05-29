using System.Drawing;

namespace Project_Jumper
{
    public class Map
    {
        public MapCell[,] Level { get; private set; }
        public int Width => Level.GetLength(0);
        public int Height => Level.GetLength(1);
        public int LevelTimeSeconds { get; private set; }
        public readonly Point Start;

        private const string sandbox = @"
XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
X                                   X
X                                   X
X                                   X
X                   G               X
X                                   X
X                                   X
X                                   X
X                                   X
X                                   X
X                   G               X
X                                   X
X             J           J         X
X       F                           X
X   *   F             B             X
X   *   F          B                X
X   *           B                   X
X            B                      X
X         B           G       J  SSSX
XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";

        private const string heightTest = @"
XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
X                             X
X                             X
XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";

        private const string jumpTest = @"
XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
X                                     X
X                                     X
X                  G                  X
X                                     X
X                                     X
X                                     X
X                                     X
X                                     X
X                                     X
X                                     X
X                                     X
X                                     X
X                                     X
X                  G                  X
X                                     X
X                                     X
XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";

        private const string level1 = @"
XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
XF                                  X
XF                                  X
XF                                  X
XBBBB                               X
X       J                           X
X    *         J                    X
X       *  B                        X
X             *  * BBB              X
X                         J    J    X
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

        private const string level2 = @"
XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
XFFB   *                            X
X  B  *                             X
X  B *                              X
X  B*           BBB    J            X
X  B            B          BB       X
X               B * * *             X
X         B * * B                BBBX
X   G     B                  J      X
X         B          G              X
X         BSSSSSS                   X
X * * * * BBBBBBB     BBBBB      *  X
X               B     B             X
X               B     B     *       X
X                     B          *  X
X      B  G           B             X
X      B                    *       X
X   B  B                            X
X   BSSBSSSSSSSSSSSSSSSSSSSSSSSSSSSSX
XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";

        public void IncreaseTime()
        {
            LevelTimeSeconds++;
        }

        public void ResestTime()
        {
            LevelTimeSeconds = 0;
        }

        public Map()
        {
            Start = new Point(1, 1);
            Level = MapCreator.CreateMap(sandbox);
        }
    }
}
