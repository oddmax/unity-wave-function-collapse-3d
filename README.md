# unity-wave-function-collapse-3d
Unity plugin for procedural generation of levels, enviroments or 3d models based on Wave Function Collapse algorithm. Wave Function Collapse algorithm is employed to assemble 2d and 3d tile-based visually impressive levels, dioramas, buildings, etc from handcrafted tilesets.

Watch a video demonstration of plugin in action on YouTube: https://www.youtube.com/watch?v=Mkvh4rosiF0 and https://www.youtube.com/watch?v=JO8OW2zg0gY

### Features
* 


### How to use
There are two versions of algorithm: Overlapping Model and Simple Tiled Model 

### Theory behind
Constraint solving is neither a traditional nor well-known approach to procedural content generation (PCG). Nevertheless, this approach can be surprisingly effective for building controllable content generators. Algorithm works similar to Sudoku. Basically you can say that it's a Sudoku solver on steroids :)

### Based on
Based on the [Wave Function Collapse algorithm](https://github.com/mxgmn/WaveFunctionCollapse) made by [mxgmn](https://github.com/mxgmn). You can find there more theory behind an algorithm and some impressive examples.

### Feature plans
* Finish ready-to-use Unity plugin for quick level/content generation which can be super useful for rapid prototyping or game-jams and put it to Unity Asset Store
* Add possibility to easily use additional heuristics to make generated results more hand-crafted or, for instance, make levels walkable
* Add possibility to use additional constraints (like predefine some parts of output)
* Add backtracking to resolve rare contradictions
* Improved Editor support

