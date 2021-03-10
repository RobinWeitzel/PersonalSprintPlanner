using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessLibrary.Models
{
    public enum Color
    {
        Transparent = 0,
        Blue = 1, // 0078D7
        Teal = 2, // 00B7C3
        Pink = 3, // E3008C
        Lila = 4, // 744DA9
        Green = 5, // 018574
    }

    public class Board : IEquatable<Board>
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public Color Color { get; set; }
        public List<Color> Colors
        {
            get
            {
                return Enum.GetValues(typeof(Color)).Cast<Color>().ToList();
            }
        }

        public bool Equals(Board other)
        {
            return other.ID == ID;
        }
    }
}
