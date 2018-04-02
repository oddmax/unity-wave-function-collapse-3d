using Core.Data;
using Core.Data.OverlappingModel;
using Core.Data.SimpleTiledModel;

namespace Core.InputProviders
{
    public interface IInputDataProvider
    {
        InputOverlappingData GetInputOverlappingData();
        InputSimpleTiledModelData GetInputSimpleTiledData();
    }
}