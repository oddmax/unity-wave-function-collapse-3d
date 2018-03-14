using System.Collections.Generic;

namespace Core.Data
{
    public class InputOverlappingData
    {
        public int Width;
        public int Height;

        public TileConfig[,] tiles;
        
        public List<TileConfig> list = new List<TileConfig>();

        public InputOverlappingData(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            
            tiles = new TileConfig[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    tiles[x, y] = TileConfig.Empty;
                }
            }
        }

        public void SetTile(int x, int y, string tileId, int rotation)
        {
            var tileConfig = new TileConfig(tileId);
            tileConfig.Rotation = rotation;
            tiles[x, y] = tileConfig;
        }

        public TileConfig GetTileAt(int x, int y)
        {
            return tiles[x, y];
        }
    }
}