using Core.Data.OverlappingModel;
using Core.Data.SimpleTiledModel;
using UnityEngine;

namespace Core.InputProviders
{
    public abstract class InputDataProvider : MonoBehaviour, IInputDataProvider
    {
        public abstract InputOverlappingData GetInputOverlappingData();

        public abstract InputSimpleTiledModelData GetInputSimpleTiledData();
    }
}