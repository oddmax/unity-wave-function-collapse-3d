namespace Core.Data
{
    public class CellState
    {
        public double EntropyLevel { get; private set; }
        
        /// <summary>
        /// Going to be null if Entropy level is more than 0
        /// </summary>
        public ITile Tile { get; private set; }

        public CellState(double entropyLevel, ITile tileIndex)
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