namespace Core.Data
{
    public class WaveFunctionCollapseModelParams
    {
        public WaveFunctionCollapseModelParams(int width, int height, int depth)
        {
            Width = width;
            Height = height;
            Depth = depth;
        }

        /// <summary>
        /// X dimension of the output data
        /// </summary>
        public int Width { get; private set; } 
        
        /// <summary>
        /// Y dimension of the output data
        /// </summary>
        public int Height { get; private set; }
        
        /// <summary>
        /// Z dimension of the output data
        /// </summary>
        public int Depth { get; private set; }
    }
}