namespace Core.Data
{
    public class WaveFunctionCollapseModelParams
    {
        public WaveFunctionCollapseModelParams(int width, int depth)
        {
            Width = width;
            Depth = depth;
        }

        public int Width { get; private set; }
        public int Depth { get; private set; }
    }
}