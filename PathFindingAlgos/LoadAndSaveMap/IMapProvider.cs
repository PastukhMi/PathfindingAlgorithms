namespace PathFindingAlgos
{
    public interface IMapProvider
    {
        TileType[,] LoadMap();
        void SaveMap(TileType[,] map);
    }
}
