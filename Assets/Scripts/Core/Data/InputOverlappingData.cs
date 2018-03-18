using System.Collections.Generic;

namespace Core.Data
{
    public class InputOverlappingData
    {
        public int Width;
        public int Height;

        public OverlappingModelTile[,] tiles;

        public Dictionary<string, TileConfig> Configs = new Dictionary<string, TileConfig>();
        public List<OverlappingModelTile> list = new List<OverlappingModelTile>();

        public InputOverlappingData(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            
            tiles = new OverlappingModelTile[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    tiles[x, y] = OverlappingModelTile.Empty;
                }
            }
        }

        public void SetTile(int x, int y, string tileId, int rotation)
        {
            TileConfig tileConfig;
            if (Configs.ContainsKey(tileId))
            {
                tileConfig = Configs[tileId];
            }
            else
            {
                tileConfig = new TileConfig(tileId);
            }
            var tile = new OverlappingModelTile(tileConfig, rotation);
            tiles[x, y] = tile;
        }

        public OverlappingModelTile GetTileAt(int x, int y)
        {
            return tiles[x, y];
        }
    }
}