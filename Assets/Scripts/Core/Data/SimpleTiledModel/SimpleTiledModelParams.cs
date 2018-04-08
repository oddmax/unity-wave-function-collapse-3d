namespace Core.Data.SimpleTiledModel
{
    public class SimpleTiledModelParams : WaveFunctionCollapseModelParams
    {
        public const string DEFAULT_SUBSET = "Default";
       
        /// <summary>
        /// Determines if the output solutions are tilable. It's usefull for creating things like tileable textures,
        /// but also has a surprising influence on the output. When working with WFC, it's often a good idea to toggle
        /// Periodic Output on and off, checking if either setting influences the results in a favorable way.
        /// </summary>
        public bool Periodic { get; private set; }
        
        /// <summary>
        /// Defines which subset of tiles to use from Input data
        /// </summary>
        public string SubsetName { get; private set; }

        public bool Black { get; set; }

        public SimpleTiledModelParams(int width, int height, int depth, bool periodic = false, string subsetName = DEFAULT_SUBSET) : base(width, height, depth)
        {
            Periodic = periodic;
            SubsetName = subsetName;
            Black = false;
        }
    }
}