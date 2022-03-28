# Wave Function Collapse content generator for Unity
Unity plugin for procedural generation of levels, environments or 3d models based on Wave Function Collapse algorithm. Wave Function Collapse algorithm is employed to assemble 2d and 3d tile-based visually impressive levels, dioramas, buildings, etc from handcrafted tilesets.

There are two models
* **Overlapping model** breaks an input pattern up into pattern chunks. And arrange them in output with constraint of local similarity (You can read more about this in [original repo](https://github.com/mxgmn/WaveFunctionCollapse)). In simple words, it produces output which looks similar to input.
* **Simple Tiled model** takes input and creates neigbor constraints (legal adjacencies for different tiles) for the tiles based on information if in input tile were located next to each other.

**Overlapping model output example (input on the left, generated output on the right)**
<p align="center"><img alt="overlapping model" src="https://i.imgur.com/okAaFfX.png"></p>

**Simple Tiled model output example**
<p align="center"><img alt="simple tiled model" src="https://i.imgur.com/zR83Baz.png"></p>

<p align="center"><img alt="main gif" src="https://i.imgur.com/nhhWdry.gif"></p>

Watch a video demonstration of plugin in action on YouTube: https://www.youtube.com/watch?v=Mkvh4rosiF0 and https://www.youtube.com/watch?v=JO8OW2zg0gY


### How to use
There are two versions of algorithm: Overlapping Model and Simple Tiled Model 
For Overlapping model:
* Implement IInputDataProvider class for your source or use existing implementation for TilePainterInputDataProvider
* Create a game object and add to it your InputDataProvider, this object is going to be source of input for WFC model
* Create a game object and add to it WaveFunctionCollapseRenderer, this object is goint to be a target for generated content
* Create a game object and add to it WaveFunctionCollapseGenerator, this object starts generation.
* Drag-and-drop InputDataProvider and WaveFunctionCollapseRenderer objects to corresponding fields in generator object
* (*Only for simple tiled model*) Create SymmetrySetsScriptableObject and set up symmetries for your tile prefabs. Look below to read more about symmetries. 
* Set up parameters of a generator to required in your case.
* Press "Generate Overlapping output" or "Generate Simple Tiled output" depending on your needs

#### Symmetry system for SimpleTiledModel 

Lists of all the possible pairs of adjacent tiles in practical tilesets can be quite long, so mxmgn implemented a symmetry system for tiles to shorten the enumeration. In this system each tile should be assigned with its symmetry type. 

* X - for tile with all possible directions
* L - for L shaped (like corners or L-turns), note that L-tile prefab should be located upside-down (Like 'Г')
* T - for stairs, T-turns, etc
* I - for part of the road or any tile which has same type of connection on opposite sides
* / - for any tile which has same type of connection on two neighbours sides

#### Explanation of parameters

Model Constructor

* `width,depth,height (int)` Dimensions of the output data. Height currently only supported for Simple Tiled model, for Overlapping use height equal 1.
* `pattern size (int)` Represents the width & height of the patterns that the overlap model breaks the input into. As it solves, it attempts to match up these subpatterns with each other. A higher pattern size will capture bigger features of the input, but is computationally more intensive, and may require a larger input sample to achieve reliable solutions.
* `periodic input (bool)` Represents whether the input pattern is tiling. If true, when WFC digests the input into N pattern chunks it will create patterns connecting the right & bottom edges to the left & top. If you use this setting, you'll need to make sure your input "makes sense" accross these edges.
* `periodic output (bool)` Determines if the output solutions are tilable. It's usefull for creating things like tileable textures, but also has a surprising influence on the output. When working with WFC, it's often a good idea to toggle Periodic Output on and off, checking if either setting influences the results in a favorable way.
* `symmetry (int)` Represents which additional symmetries of the input pattern are digested. 0 is just the original input, 1-8 adds mirrored and rotated variations. These variations can help flesh out the patterns in your input, but aren't necessary. They also only work with unidirectional tiles, and are undesirable when your final game tiles have direction dependent graphics or functionality.
* `ground (int)` When not 0, this assigns a pattern for the bottom row of the output. It's mainly useful for "vertical" words, where you want a distinct ground and sky separation. The value corresponds to the overlap models internal pattern indexes, so some experimentation is needed to figure out a suitable value.

### Theory behind
Constraint solving is neither a traditional nor well-known approach to procedural content generation (PCG). Nevertheless, this approach can be surprisingly effective for building controllable content generators.The goal of an algorithm for solving Constraint satisfaction problems (CSPs) (a solver) is to find a total assignment (an assignment for every variable) such that no constraints are violated. Algorithm works similar to Sudoku. Basically you can say that it's a Sudoku solver on steroids :)

### Based on
Based on the [Wave Function Collapse algorithm](https://github.com/mxgmn/WaveFunctionCollapse) made by [mxgmn](https://github.com/mxgmn). You can find there more theory behind an algorithm and some impressive examples.

### Feature plans
* Finish ready-to-use Unity plugin for quick level/content generation which can be super useful for rapid prototyping or game-jams and put it to Unity Asset Store
* Add possibility to easily use additional heuristics to make generated results more hand-crafted or, for instance, make levels walkable
* Add possibility to use additional constraints (like predefine some parts of output)
* Add backtracking to resolve rare contradictions
* Improved Editor support

