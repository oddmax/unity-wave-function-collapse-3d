using Core.Data;
using Core.Data.OverlappingModel;

namespace Core.Model.New
{
    public class OvelappingModel2dWrapper : IModel3d
    {
        public OverlappingModel2d Model { get; private set; }
        public WaveFunctionCollapseModelParams ModelParam { get; private set; }

        public OvelappingModel2dWrapper(InputOverlappingData inputData, OverlappingModelParams modelParam)
        {
            ModelParam = modelParam;
            Model = new OverlappingModel2d(inputData, modelParam);
        }
        
        public CellState GetCellStateAt(int x, int y, int z)
        {
            return Model.GetCellStateAt(x, z);
        }

    }
}