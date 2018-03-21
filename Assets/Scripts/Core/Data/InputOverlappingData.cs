using System.Collections.Generic;

namespace Core.Data
{
    public class InputOverlappingData
    {
        public int Width;
        public int Depth;

        public OverlappingModelTile[,] tiles;
        public byte[,] tilesIndexIds;

        public Dictionary<string, TileConfig> Configs = new Dictionary<string, TileConfig>();
        public List<string> TilesIdsList = new List<string>();
        public List<OverlappingModelTile> list = new List<OverlappingModelTile>();

        public InputOverlappingData(int width, int depth)
        {
            this.Width = width;
            this.Depth = depth;
            
            tiles = new OverlappingModelTile[width, depth];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < depth; y++)
                {
                    tiles[x, y] = OverlappingModelTile.Empty;
                }
            }
            
            
            
        }

        public byte[,] GetSampleMatrix()
        {
            tilesIndexIds = new byte[Width, Depth];
            
            TilesIdsList.Clear();

            for (int y = 0; y < Depth; y++)
            for (int x = 0; x < Width; x++)
            {
                OverlappingModelTile tile = tiles[x, y];

                var i = 0;
                foreach (var tileId in TilesIdsList)
                {
                    if (tileId == tile.Id) break;
                    i++;
                }

                if (i == TilesIdsList.Count)
                {
                    TilesIdsList.Add(tile.Id);
                }
                tilesIndexIds[x, y] = (byte) i;
            }

            return tilesIndexIds;
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