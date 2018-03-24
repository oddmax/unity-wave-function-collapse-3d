namespace Core.Data
{
    public class OverlappingModelParams : WaveFunctionCollapseModelParams
    {
        public int PatternSize { get; private set; }
        public int Symmetry { get; set; }
        public bool PeriodicInput { get; set; }
        public bool PeriodicOutput { get; set; }
        public int Ground { get; set; }

        public OverlappingModelParams(int width, int depth, int patternSize) : base(width, depth)
        {
            this.PatternSize = patternSize;
        }
    }
}