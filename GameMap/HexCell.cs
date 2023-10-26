using Godot;
using System.IO;

namespace StateOfClone.GameMap
{
    public class HexCell
    {
        /// <summary>
        /// Hexagonal coordinates unique to the cell.
        /// </summary>
        public HexCoordinates Coordinates { get; set; }

    }
}