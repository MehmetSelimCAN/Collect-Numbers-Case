using Assets.Scripts.Game.Core.BoardBase;
using UnityEngine;

namespace Assets.Scripts.Game.Core.Managers
{
    public class InputManager : MonoBehaviour
    {
        private Camera _camera;

        private const string CellCollider = "CellCollider";

        private Vector2 _firstTouchPosition;
        private Vector2 _secondTouchPosition;
        private Cell _touchedCell;

        private bool _canTouch = true;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            EventManager.OnSceneLoaded += EnableInput;

            EventManager.OnCellFallsStarted += DisableInput;
            EventManager.OnCellFallsFinished += EnableInput;

            EventManager.OnMoveCountFinished += DisableInput;

            EventManager.OnLevelFinished += DisableInput;
        }

        private void DisableInput()
        {
            _canTouch = false;
        }

        private void EnableInput()
        {
            _canTouch = true;
        }

        private void UnsubscribeEvents()
        {
            EventManager.OnSceneLoaded -= EnableInput;

            EventManager.OnCellFallsStarted -= DisableInput;
            EventManager.OnCellFallsFinished -= EnableInput;

            EventManager.OnMoveCountFinished -= DisableInput;

            EventManager.OnLevelFinished -= DisableInput;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        private void Update()
        {
            if (!_canTouch) return;

            HandleInput();
        }

        private void HandleInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _firstTouchPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                _touchedCell = GetCellUnderMousePosition(_firstTouchPosition);
                if (_touchedCell != null)
                {
                    _touchedCell.Number.Shrink();
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (_touchedCell == null) return;
                _touchedCell.Number.Expand();

                _secondTouchPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                var secondTouchedCell = GetCellUnderMousePosition(_secondTouchPosition);
                if (secondTouchedCell == null) return;

                if (_touchedCell.GetInstanceID() == secondTouchedCell.GetInstanceID())
                {
                    EventManager.OnCellClicked?.Invoke(_touchedCell);
                }
            }
        }

        private Cell GetCellUnderMousePosition(Vector2 firstTouchPosition)
        {
            var firstCellHit = Physics2D.OverlapPoint(firstTouchPosition) as BoxCollider2D;
            if (firstCellHit == null) return null;
            if (!firstCellHit.CompareTag(CellCollider)) return null;

            Cell clickedCell = firstCellHit.gameObject.GetComponent<Cell>();
            if (!clickedCell.HasNumber) return null;

            return clickedCell;
        }
    }
}
