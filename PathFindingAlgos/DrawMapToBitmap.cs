using System;
using System.Collections.Generic;
using System.Drawing;
        
namespace PathFindingAlgos          
{
    public class DrawMapToBitmap 
    {
        public event EventHandler MapChanged;
        public Bitmap Bitmap { get; private set; }

        private readonly float BorderThickness;
        private int MaxColumnCount;
        private int MaxRowCount;
        private float TileWidth;
        private float TileHeight;

        private readonly IMapProvider _mapProvider;
        private TileType[,] _map;

        private Graphics _graphics; 

        /* Draw Map Sheme
         +----------------+
         | +--+ +--+ +--+ |
         | |  | |  | |  | |
         | +--+ +--+ +--+ |
         | +--+ +--+ +--+ |
         | |  | |  | |  | |
         | +--+ +--+ +--+ |
         +----------------+*/

        private readonly Dictionary<TileType, Brush> _brushes = 
            new Dictionary<TileType, Brush>
        {
            { TileType.Empty, Brushes.Gray },
            { TileType.Wall, Brushes.GreenYellow }
        };

        public DrawMapToBitmap(float borderThickness, int width, int heigth)
        {
            _mapProvider = new TextFileMapProvider();
            BorderThickness = borderThickness;

            InitializeMap();
            CreateBitmap(width, heigth);
            DrawMap();
        }

        private void InitializeMap()
        {
            _map = _mapProvider.LoadMap();
            MaxColumnCount = _map.GetLength(1);
            MaxRowCount = _map.GetLength(0);
        }

        private void CreateBitmap(int width, int heigth)
        {
            TileWidth = CalculateTileSize(width, MaxColumnCount);
            TileHeight = CalculateTileSize(heigth, MaxRowCount);

            Bitmap = new Bitmap(width, heigth);
            _graphics = Graphics.FromImage(Bitmap);
            _graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            _graphics.Clear(Color.Black);
        }

        public void ChangeBitmapSize(int width, int heigth)
        {
            CreateBitmap(width, heigth);
            DrawMap();

            MapChanged?.Invoke(this, EventArgs.Empty);
        }

        public void ChangeTile(Point pointOnBitmap, TileType type)
        {
            int columnNum = ConvertBitmapCoordToMap(pointOnBitmap.X, TileWidth),
                rowNum = ConvertBitmapCoordToMap(pointOnBitmap.Y, TileHeight);

            if (CheckCoordsOutOfRange(columnNum, rowNum))
            {
                return;
            }

            _map[rowNum, columnNum] = type;
            DrawTile(rowNum, columnNum);

            MapChanged?.Invoke(this, EventArgs.Empty);
        }

        private bool CheckCoordsOutOfRange(int columnNum, int rowNum)
        {
            return columnNum < 0 || columnNum >= MaxColumnCount || rowNum < 0 || rowNum >= MaxRowCount;
        }

        public void DrawMap()
        {
            _graphics.Clear(Color.Black);
            for (int rowCounter = 0; rowCounter < MaxRowCount; rowCounter++)
            {
                for (int columnCounter = 0; columnCounter < MaxColumnCount; columnCounter++)
                {
                    DrawTile(rowCounter, columnCounter);
                }
            }
        }

        public void DrawTile(int rowNum, int columnNum)
        {
            var tileType = _map[rowNum, columnNum];
            var brush = _brushes[tileType];

            float x = ConvertMapCoordToBitmap(columnNum, TileWidth),
                  y = ConvertMapCoordToBitmap(rowNum, TileHeight);

            _graphics.FillRectangle(brush, x, y, TileWidth, TileHeight);
        }

        public void SaveMap()
        {
            _mapProvider.SaveMap(_map);
        }

        // TODO: This calculation does not take into account one additional border
        // So we change tile, when bitmap coord is in border before tile
        // |b|tile1|b|tile2|b|
        // \__t1__/\__t2__/
        private int ConvertBitmapCoordToMap(int bitmapCoord, float tileSize)
        {
            return (int)(bitmapCoord / (tileSize + BorderThickness));
        }
        private float ConvertMapCoordToBitmap(int mapCoord, float tileSize)
        {
            int bordersNumber = mapCoord + 1;
            return mapCoord * tileSize + bordersNumber * BorderThickness;
        }
        private float CalculateTileSize(int bitmapLength, int maxTileCount)
        {
            int bordersNumber = maxTileCount + 1;
            return (bitmapLength - bordersNumber * BorderThickness) / maxTileCount;
        }
    }
}
