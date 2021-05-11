using System;
using System.Collections.Generic;
using System.Text;

namespace Project_Jumper
{
    public class MapCell
    {
        public readonly string Type;
        public readonly bool IsFriendly;
        public readonly bool Collision;
        public readonly bool IsOrb;

        public MapCell(string name, bool isFriendly, bool collision, bool isOrb)
        {
            Type = name;
            IsFriendly = isFriendly;
            Collision = collision;
            IsOrb = isOrb;
        }
    }
}
