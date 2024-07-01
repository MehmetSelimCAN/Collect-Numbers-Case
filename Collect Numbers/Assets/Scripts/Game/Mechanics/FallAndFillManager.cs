using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Game.Core.LevelBase;
using Assets.Scripts.Game.Core.Managers;
using Assets.Scripts.Game.Core.NumberBase;
using Assets.Scripts.SO;
using Assets.Scripts.Game.Core.BoardBase;
using Grid = Assets.Scripts.Game.Core.BoardBase.Grid;

namespace Assets.Scripts.Game.Mechanics
{
    public class FallAndFillManager : MonoBehaviour
    {
        private Cell[] _fillingCells;

        private LevelData _levelData;

        private bool _isActive;
        private bool _isChecked;

        private NumberFactory _numberFactory;

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            EventManager.OnCellExploded += StartFalls;
        }

        private void UnsubscribeEvents()
        {
            EventManager.OnCellExploded -= StartFalls;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        public void Init(LevelData levelData)
        {
            _levelData = levelData;
            _numberFactory = ServiceProvider.GetNumberFactory;

            CreateFillingCells();
        }

        private void CreateFillingCells()
        {
            var cellList = new List<Cell>();
            for (var y = 0; y < Grid.Rows; y++)
            {
                for (var x = 0; x < Grid.Cols; x++)
                {
                    var cell = Grid.Cells[x, y];
                    if (cell != null && cell.IsFillingCell)
                    {
                        cellList.Add(cell);
                    }
                }
            }

            _fillingCells = cellList.ToArray();
        }

        private void StartFalls(Cell cell)
        {
            EventManager.OnCellFallsStarted?.Invoke();
            _isActive = true;
        }

        private void StopFalls()
        {
            _isActive = false;
            EventManager.OnCellFallsFinished?.Invoke();
        }

        private void DoFalls()
        {
            for (var y = 0; y < Grid.Rows; y++)
            {
                for (var x = 0; x < Grid.Cols; x++)
                {
                    var cell = Grid.Cells[x, y];
                    if (cell.Number != null && cell.FirstCellBelow != null && cell.FirstCellBelow.Number == null)
                    {
                        cell.Number.Fall();
                    }
                }
            }
        }

        private void DoFills()
        {
            for (var i = 0; i < _fillingCells.Length; i++)
            {
                var cell = _fillingCells[i];
                if (cell.Number == null)
                {
                    List<NumberData> availableNumbers = _levelData.AvailableNumbers;
                    cell.Number = _numberFactory.Create(cell, availableNumbers[Random.Range(0, availableNumbers.Count)]);
                    var offsetY = 0.0f;
                    var targetCellBelow = cell.GetFallTarget().FirstCellBelow;
                    if (targetCellBelow != null)
                    {
                        if (targetCellBelow.Number != null)
                        {
                            offsetY = targetCellBelow.Number.transform.position.y + 1;
                        }
                    }

                    var pos = cell.transform.position;
                    pos.y += 1.5f;
                    pos.y = pos.y > offsetY ? pos.y : offsetY;

                    if (!cell.HasNumber) continue;

                    cell.Number.transform.position = pos;
                    cell.Number.Fall();
                }
            }
        }

        private bool IsFallingAnything()
        {
            for (var y = 0; y < Grid.Rows; y++)
            {
                for (var x = 0; x < Grid.Cols; x++)
                {
                    var cell = Grid.Cells[x, y];
                    if (cell.HasNumber && cell.Number.FallAnimation.IsFalling)
                    {
                        _isChecked = false;
                        return true;
                    }
                }
            }

            return false;
        }

        private void CheckFellMatches()
        {
            for (var y = 0; y < Grid.Rows; y++)
            {
                for (var x = 0; x < Grid.Cols; x++)
                {
                    var cell = Grid.Cells[x, y];
                    if (cell.HasNumber)
                    {
                        cell.Grid.CheckMatches(cell);
                    }
                }
            }

            _isChecked = true;
        }

        public void Update()
        {
            if (!_isActive) return;
            DoFalls();
            DoFills();

            if (!IsFallingAnything() && !_isChecked)
            {
                StopFalls();
                CheckFellMatches();
            }
        }
    }
}
