using Assets.Scripts.Game.Core.BoardBase;
using System.Collections.Generic;

namespace Assets.Scripts.Game.Core.NumberBase
{
    public class UniqueNumber : Number
    {
        private bool _isAlreadyExploded;

        public override void TryToExecute(bool isExecutedFromMatch, List<Cell> toBeDestroyedCells)
        {
            if (_isAlreadyExploded) return;

            if (isExecutedFromMatch)
            {
                _isAlreadyExploded = true;
                for (int i = 0; i < Grid.Cols; i++)
                {
                    if (Grid.Cells[i, Cell.Y].HasNumber)
                    {
                        if (toBeDestroyedCells.Contains(Grid.Cells[i, Cell.Y])) continue;

                        Grid.Cells[i, Cell.Y].Number.TryToExecute(false, toBeDestroyedCells);
                    }
                }

                for (int i = 0; i < Grid.Rows; i++)
                {
                    if (Grid.Cells[Cell.X, i].HasNumber)
                    {
                        if (toBeDestroyedCells.Contains(Grid.Cells[Cell.X, i])) continue;

                        Grid.Cells[Cell.X, i].Number.TryToExecute(false, toBeDestroyedCells);
                    }
                }
            }

            base.TryToExecute(isExecutedFromMatch, toBeDestroyedCells);
        }
    }
}
