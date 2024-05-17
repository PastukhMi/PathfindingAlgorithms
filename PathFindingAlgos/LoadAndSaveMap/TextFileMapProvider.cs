using System.IO;
using System.Linq;

namespace PathFindingAlgos
{
    public class TextFileMapProvider : IMapProvider
    {
        private readonly string _fileName = "map.txt";
        public void SaveMap(TileType[,] map)
        {
            using (var filestream = File.Create(_fileName))
            {
                using (var writer = new StreamWriter(filestream))
                {
                    for (int x = 0; x < map.GetLength(0); x++)
                    {
                        for (int y = 0; y < map.GetLength(1); y++)
                        {
                            writer.Write(map[x, y] == TileType.Empty ? " " : "#");
                        }
                        writer.WriteLine();
                    }
                }
            }
        }
        public TileType[,] LoadMap()
        {
            int height = File.ReadLines(_fileName).Count();
            TileType[,] map;

            using (var filestream = File.OpenRead(_fileName))
            {
                using (var reader = new StreamReader(filestream))
                {
                    int width = reader.ReadLine().Length;

                    filestream.Seek(0, SeekOrigin.Begin);
                    reader.DiscardBufferedData();
                    reader.BaseStream.Seek(0, SeekOrigin.Begin);

                    map = new TileType[height, width];

                    for (int x = 0; x < height; x++)
                    {
                        var line = reader.ReadLine();
                        for (int y = 0; y < width; y++)
                        {
                            map[x, y] = line[y] == ' ' ? TileType.Empty : TileType.Wall;
                        }
                    }
                }
            }

            return map;
        }
    }
}
