namespace Core.Data
{
    public class CellState
    {
        public float EntropyLevel;
        
        /// <summary>
        /// Going to be null if Entropy level is more than 0
        /// </summary>
        public ITile Tile { get; private set; }

        public CellState(float entropyLevel, ITile tileIndex)
        {
            EntropyLevel = entropyLevel;
            Tile = tileIndex;
        }

        public bool Collapsed
        {
            get { return Tile != null; }
        }
    }
}