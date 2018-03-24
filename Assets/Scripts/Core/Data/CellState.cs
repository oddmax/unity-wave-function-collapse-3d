namespace Core.Data
{
    public class CellState
    {
        public float EntropyLevel;
        
        /// <summary>
        /// Going to be null if Entropy level is more than 0
        /// </summary>
        public byte? TileIndex;

        public CellState(float entropyLevel, byte? tileIndex)
        {
            EntropyLevel = entropyLevel;
            TileIndex = tileIndex;
        }
    }
}