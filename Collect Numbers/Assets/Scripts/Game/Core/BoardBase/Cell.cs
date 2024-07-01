using Assets.Scripts.Game.Core.Managers;
using Assets.Scripts.Game.Core.NumberBase;
using Assets.Scripts.SO;
using UnityEngine;

namespace Assets.Scripts.Game.Core.BoardBase
{
    public class Cell : MonoBehaviour
    {
        [HideInInspector] public int X;
        [HideInInspector] public int Y;
        [HideInInspector] public Vector3Int Position { get { return new Vector3Int(X, Y, 0); } }

        [HideInInspector] public Cell FirstCellBelow;
        [HideInInspector] public bool IsFillingCell;

        private Number _number;
        public Number Number
        {
            get
            {
                return _number;
            }
            set
            {
                if (_number == value) return;

                var oldItem = _number;
                _number = value;

                if (oldItem != null && Equals(oldItem.Cell, this))
                {
                    oldItem.Cell = null;
                }
                if (value != null)
                {
                    value.Cell = this;
                }
            }
        }

        public bool HasNumber => Number != null;

        public Grid Grid { get; private set; }

        private NumberFactory _numberFactory;

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            EventManager.OnCellClicked += OnClicked;
        }

        public void OnClicked(Cell clickedCell)
        {
            if (clickedCell == this)
            {
                if (_number.NumberData.IsUpgradeable)
                {
                    _number.IncreaseNumber();
                    EventManager.OnCellUpgraded?.Invoke(this);
                }
            }
        }

        private void UnsubscribeEvents()
        {
            EventManager.OnCellClicked -= OnClicked;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        public void Prepare(int x, int y, Grid grid)
        {
            X = x;
            Y = y;
            transform.localPosition = new Vector3(x, y);

            Grid = grid;
            IsFillingCell = Y == Grid.Rows - 1;

            _numberFactory = ServiceProvider.GetNumberFactory;

            UpdateLabel();
            UpdateBottomNeighbour(Grid);
        }

        private void UpdateBottomNeighbour(Grid grid)
        {
            var down = grid.GetBottomNeighbour(this);

            if (down != null) FirstCellBelow = down;
        }

        private void UpdateLabel()
        {
            var cellName = X + ":" + Y;
            gameObject.name = "Cell " + cellName;
        }

        public Number InsertNumber(NumberData numberData)
        {
            if (_number != null) return _number;

            _number = _numberFactory.Create(this, numberData);
            return _number;
        }

        public Cell GetFallTarget()
        {
            var targetCell = this;
            while (targetCell.FirstCellBelow != null && targetCell.FirstCellBelow.Number == null)
            {
                targetCell = targetCell.FirstCellBelow;
            }
            return targetCell;
        }
    }
}
