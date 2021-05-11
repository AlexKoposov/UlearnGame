using System;
using System.Linq;

namespace Project_Jumper
{
    class MapCreator
    {
        public static MapCell[,] CreateMap(string map, string separator = "\r\n")
        {
            var rows = map.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries)
                .Reverse()
                .ToArray();
            if (rows.Select(z => z.Length).Distinct().Count() != 1)
                throw new Exception($"Wrong test map '{map}'");
            var result = new MapCell[rows[0].Length, rows.Length];
            for (var x = 0; x < rows[0].Length; x++)
                for (var y = 0; y < rows.Length; y++)
                    result[x, y] = CreateMapCellBySymbol(rows[y][x]);
            return result;
        }

        private static MapCell CreateMapCellBySymbol(char c) =>
            c switch
            {
                'X' => new MapCell("Border", true, true, false),
                'B' => new MapCell("Block", true, true, false),
                'S' => new MapCell("Spike", false, true, false),
                '*' => new MapCell("Saw", false, true, false),
                'J' => new MapCell("JumpOrb", true, false, true),
                'G' => new MapCell("GravityOrb", true, false, true),
                _ => new MapCell("Space", true, false, false)
            };
    }
}