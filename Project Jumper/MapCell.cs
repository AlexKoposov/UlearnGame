
namespace Project_Jumper
{
    public class MapCell
    {
        public readonly string Type;
        public readonly bool IsFriendly;
        public readonly bool Collision;
        public readonly bool IsOrb;
        public readonly bool IsPortal;

        public MapCell(string name, bool isFriendly, bool collision, bool isOrb, bool isPortal)
        {
            Type = name;
            IsFriendly = isFriendly;
            Collision = collision;
            IsOrb = isOrb;
            IsPortal = isPortal;
        }
    }
}
