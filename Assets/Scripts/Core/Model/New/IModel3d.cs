using Core.Data;

namespace Core.Model
{
    public interface IModel3d
    {
        CellState GetCellStateAt(int x, int y, int z);

        WaveFunctionCollapseModelParams ModelParam { get; }
    }
}