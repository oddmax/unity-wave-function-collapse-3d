namespace Core.Data
{
    public class WaveFunctionCollapseModelParams
    {
        public WaveFunctionCollapseModelParams(int width, int depth)
        {
            Width = width;
            Depth = depth;
        }

        /// <summary>
        /// X dimension of the output data
        /// </summary>
        public int Width { get; private set; }
        
        /// <summary>
        /// Z dimension of the output data
        /// </summary>
        public int Depth { get; private set; }
    }
}