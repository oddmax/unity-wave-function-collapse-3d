namespace Core.Data
{
    public class WaveFunctionCollapseModelParams
    {
        public WaveFunctionCollapseModelParams(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public int Width { get; private set; }
        public int Height { get; private set; }
    }
}