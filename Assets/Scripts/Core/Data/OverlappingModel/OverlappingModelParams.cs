namespace Core.Data.OverlappingModel
{
    public class OverlappingModelParams : WaveFunctionCollapseModelParams
    {
        /// <summary>
        ///  Represents the width & height of the patterns that the overlap model breaks the input into.
        /// As it solves, it attempts to match up these subpatterns with each other.
        /// A higher pattern size will capture bigger features of the input, but is computationally more intensive,
        /// and may require a larger input sample to achieve reliable solutions.
        /// </summary>
        public int PatternSize { get; private set; }
        
        /// <summary>
        /// Represents which additional symmetries of the input pattern are digested. 0 is just the original input,
        /// 1-8 adds mirrored and rotated variations. These variations can help flesh out the patterns in your input,
        /// but aren't necessary. They also only work with unidirectional tiles, and are undesirable
        /// when your final game tiles have direction dependent graphics or functionality.
        /// </summary>
        public int Symmetry { get; set; }
        
        /// <summary>
        /// Represents whether the input pattern is tiling. If true, when WFC digests the input into N pattern chunks
        /// it will create patterns connecting the right & bottom edges to the left & top.
        /// If you use this setting, you'll need to make sure your input "makes sense" accross these edges.
        /// </summary>
        public bool PeriodicInput { get; set; }
        
        /// <summary>
        /// Determines if the output solutions are tilable. It's usefull for creating things like tileable textures,
        /// but also has a surprising influence on the output. When working with WFC, it's often a good idea to toggle
        /// Periodic Output on and off, checking if either setting influences the results in a favorable way.
        /// </summary>
        public bool PeriodicOutput { get; set; }
        
        /// <summary>
        /// When not 0, this assigns a pattern for the bottom row of the output.
        /// It's mainly useful for "vertical" worlds, where you want a distinct ground and sky separation.
        /// The value corresponds to the overlap models internal pattern indexes,
        /// so some experimentation is needed to figure out a suitable value.
        /// </summary>
        public int Ground { get; set; }

        public OverlappingModelParams(int width, int height, int depth, int patternSize) : base(width, height, depth)
        {
            this.PatternSize = patternSize;
        }
    }
}