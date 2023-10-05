using System.Collections.Generic;
using System.IO;

namespace StateOfClone.GameMap
{
    public interface IHexUnit
    {
        IHexGrid Grid { get; set; }
        IHexCell Location { get; set; }
        float Orientation { get; set; }
        int HexSpeed { get; }
        int VisionRange { get; }

        void Die();
        int GetMoveCost(IHexCell fromCell, IHexCell toCell, HexDirection direction);
        bool IsValidDestination(IHexCell cell);
        void Save(BinaryWriter writer);
        void Travel(List<IHexCell> path);
        void ValidateLocation();
    }
}
