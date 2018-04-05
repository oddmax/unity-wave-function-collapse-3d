using Core.Data;

namespace Core.Model
{
    public interface IModel
    {
        CellState GetCellStateAt(int x, int y);

        WaveFunctionCollapseModelParams ModelParam { get; }
    }
}