using System.Collections.Generic;
using UnityEngine;

namespace Core.Data.OverlappingModel
{
    public class InputOverlappingData
    {
        public readonly int Width;
        public readonly int Depth;

        public OverlappingModelTile[,] tiles;
        public byte[,] tilesIndexIds;

        public List<OverlappingModelTile> TilesSortedByIds = new List<OverlappingModelTile>();
        private TileConfigData<TileConfig> tileConfigData;

        public InputOverlappingData(TileConfigData<TileConfig> tileConfigData, int width, int depth)
        {
            this.tileConfigData = tileConfigData;
            Width = width;
            Depth = depth;
            
            tiles = new OverlappingModelTile[width, depth];

            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < depth; z++)
                {
                    tiles[x, z] = OverlappingModelTile.Empty;
                }
            }
        }

        public byte[,] GetSampleMatrix()
        {
            tilesIndexIds = new byte[Width, Depth];
            
            TilesSortedByIds.Clear();

            for (var z = 0; z < Depth; z++)
            for (var x = 0; x < Width; x++)
            {
                var tile = tiles[x, z];

                int i;
                for (i = 0; i < TilesSortedByIds.Count; i++)
                {
                    if (TilesSortedByIds[i].Id == tile.Id) break;
                }

                if (i == TilesSortedByIds.Count)
                {
                    TilesSortedByIds.Add(tile);
                }
                tilesIndexIds[x, z] = (byte) i;
            }

            return tilesIndexIds;
        }

        public OverlappingModelTile GetTileById(byte id)
        {
            if (id < TilesSortedByIds.Count)
            {
                return TilesSortedByIds[id];
            }

            return null;
        }
        
        public void SetTile(TileConfig tileConfig, int x, int z, int rotation)
        {
            var tile = new OverlappingModelTile(tileConfig, rotation);
            tiles[x, z] = tile;
        }

        public OverlappingModelTile GetTileAt(int x, int z)
        {
            return tiles[x, z];
        }
    }
}