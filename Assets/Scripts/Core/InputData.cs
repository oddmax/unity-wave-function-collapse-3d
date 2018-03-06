namespace Core
{
    public class InputData
    {
        public int Width;
        public int Height;

        public TileConfig[,] tiles;

        public InputData(int width, int height)
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

        public TileConfig GetTileAt(int x, int y)
        {
            return tiles[x, y];
        }
    }
}