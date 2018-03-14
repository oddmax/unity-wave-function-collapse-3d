using Core.Data;

namespace Core.InputProviders
{
    public interface IInputDataProvider
    {
        InputOverlappingData GetInputOverlappingData();
        InputSimpleTiledModelData GetInputSimpleTiledData();
    }
}