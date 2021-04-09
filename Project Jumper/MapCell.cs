using System;
using System.Collections.Generic;
using System.Text;

namespace Project_Jumper
{
    public class MapCell
    {
        public readonly string Name;
        public readonly bool IsFriendly;
        public readonly bool Collision;

        public MapCell(string name, bool isFriendly, bool collision)
        {
            Name = name;
            IsFriendly = isFriendly;
            Collision = collision;
        }
    }
}
