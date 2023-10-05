namespace StateOfClone.GameMap
{
    public interface IHexGridChunk
    {
        void AddCell(int index, IHexCell cell);
        void Refresh();
        void ShowUI(bool visible);
    }
}