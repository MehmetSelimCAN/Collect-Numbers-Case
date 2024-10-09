using Assets.Scripts.Game.Core.Enums;
using Assets.Scripts.Game.Core.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.Core.BoardBase
{
    public class Grid : MonoBehaviour
    {
        public static int Rows;
        public static int Cols;

        public Transform CellsParent;

        [SerializeField] private Cell cellPrefab;
        public static Cell[,] Cells;

        public void Prepare()
        {
            CreateCells();
            PrepareCells();
        }

        private void CreateCells()
        {
            for (int y = 0; y < Rows; y++)
            {
                for (int x = 0; x < Cols; x++)
                {
                    var cell = Instantiate(cellPrefab, Vector3.zero, Quaternion.identity, CellsParent);
                    Cells[x, y] = cell;
                }
            }
        }

        private void PrepareCells()
        {
            for (int y = 0; y < Rows; y++)
            {
                for (int x = 0; x < Cols; x++)
                {
                    Cells[x, y].Prepare(x, y, this);
                }
            }
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            EventManager.OnCellUpgraded += CheckMatches;
        }

        public void CheckMatches(Cell clickedCell)
        {
            int x = clickedCell.Position.x;
            int y = clickedCell.Position.y;
            NumberType clickedNumberType = clickedCell.Number.NumberData.NumberType;

            List<Cell> sameTypeCells = new List<Cell>();
            Queue<Cell> toCheck = new Queue<Cell>();
            toCheck.Enqueue(clickedCell);

            if (IsThereAnyMatchInXAxis(x, y, clickedNumberType))
            {
                FindMatchesInXAxis(ref toCheck, ref sameTypeCells, clickedNumberType);
            }

            toCheck.Enqueue(clickedCell);

            if (IsThereAnyMatchInYAxis(x, y, clickedNumberType))
            {
                FindMatchesInYAxis(ref toCheck, ref sameTypeCells, clickedNumberType);
            }

            if (sameTypeCells.Count > 2)
            {
                DestroyMatchedCells(sameTypeCells);
            }
        }

        private void UnsubscribeEvents()
        {
            EventManager.OnCellUpgraded -= CheckMatches;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        private bool IsThereAnyMatchInXAxis(int x, int y, NumberType clickedNumberType)
        {
            return (x > 0 && Cells[x - 1, y].HasNumber && Cells[x - 1, y].Number.NumberData.NumberType == clickedNumberType
                    && x < Cols - 1 && Cells[x + 1, y].HasNumber && Cells[x + 1, y].Number.NumberData.NumberType == clickedNumberType) ||
                    (x > 0 && Cells[x - 1, y].HasNumber && Cells[x - 1, y].Number.NumberData.NumberType == clickedNumberType
                    && x > 1 && Cells[x - 2, y].HasNumber && Cells[x - 2, y].Number.NumberData.NumberType == clickedNumberType) ||
                    (x < Cols - 1 && Cells[x + 1, y].HasNumber && Cells[x + 1, y].Number.NumberData.NumberType == clickedNumberType
                    && x < Cols - 2 && Cells[x + 2, y].HasNumber && Cells[x + 2, y].Number.NumberData.NumberType == clickedNumberType);
        }

        private bool IsThereAnyMatchInYAxis(int x, int y, NumberType clickedNumberType)
        {
            return (y > 0 && Cells[x, y - 1].HasNumber && Cells[x, y - 1].Number.NumberData.NumberType == clickedNumberType
                && y < Rows - 1 && Cells[x, y + 1].HasNumber && Cells[x, y + 1].Number.NumberData.NumberType == clickedNumberType) ||
                (y > 0 && Cells[x, y - 1].HasNumber && Cells[x, y - 1].Number.NumberData.NumberType == clickedNumberType
                && y > 1 && Cells[x, y - 2].HasNumber && Cells[x, y - 2].Number.NumberData.NumberType == clickedNumberType) ||
                (y < Rows - 1 && Cells[x, y + 1].HasNumber && Cells[x, y + 1].Number.NumberData.NumberType == clickedNumberType
                && y < Rows - 2 && Cells[x, y + 2].HasNumber && Cells[x, y + 2].Number.NumberData.NumberType == clickedNumberType);
        }

        private void FindMatchesInXAxis(ref Queue<Cell> toCheck, ref List<Cell> sameTypeCells, NumberType clickedNumberType)
        {
            Vector3Int[] xOffsets = new[] { Vector3Int.left, Vector3Int.right };

            while (toCheck.Count > 0)
            {
                var currentCheckingCell = toCheck.Dequeue();
                sameTypeCells.Add(currentCheckingCell);

                foreach (var xOffset in xOffsets)
                {
                    var nextCellPosition = currentCheckingCell.Position + xOffset;

                    if (nextCellPosition.x < Cols && nextCellPosition.x >= 0)
                    {
                        var nextCell = Cells[nextCellPosition.x, nextCellPosition.y];

                        if (sameTypeCells.Contains(nextCell)) continue;

                        if (nextCell.HasNumber && nextCell.Number.NumberData.NumberType == clickedNumberType)
                        {
                            toCheck.Enqueue(nextCell);
                        }
                    }
                }
            }
        }

        private void FindMatchesInYAxis(ref Queue<Cell> toCheck, ref List<Cell> sameTypeCells, NumberType clickedNumberType)
        {
            Vector3Int[] yOffsets = new[] { Vector3Int.down, Vector3Int.up };

            while (toCheck.Count > 0)
            {
                var currentCheckingCell = toCheck.Dequeue();
                sameTypeCells.Add(currentCheckingCell);

                foreach (var yOffset in yOffsets)
                {
                    var nextCellPosition = currentCheckingCell.Position + yOffset;

                    if (nextCellPosition.y < Rows && nextCellPosition.y >= 0)
                    {
                        var nextCell = Cells[nextCellPosition.x, nextCellPosition.y];

                        if (sameTypeCells.Contains(nextCell)) continue;

                        if (nextCell.HasNumber && nextCell.Number.NumberData.NumberType == clickedNumberType)
                        {
                            toCheck.Enqueue(nextCell);
                        }
                    }
                }
            }
        }

        private void DestroyMatchedCells(List<Cell> cellsToBeDestroyed)
        {
            for (var i = 0; i < cellsToBeDestroyed.Count; i++)
            {
                if (!cellsToBeDestroyed[i].HasNumber) continue;

                var destroyedCell = cellsToBeDestroyed[i];
                var number = destroyedCell.Number;
                number.TryToExecute(true, cellsToBeDestroyed);
            }
        }

        public Cell GetBottomNeighbour(Cell cell)
        {
            var x = cell.X;
            var y = cell.Y;
            y -= 1;

            if (x >= Cols || x < 0 || y >= Rows || y < 0) return null;

            return Cells[x, y];
        }
    }
}
