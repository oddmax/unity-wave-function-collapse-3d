using Core.Data;

namespace Core
{
    public interface IInputDataProvider
    {
        InputOverlappingData GetInputOverlappingData();
        InputSimpleTiledModelData GetInputSimpleTiledData();
    }
}