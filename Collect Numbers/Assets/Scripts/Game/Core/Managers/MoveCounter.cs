using Assets.Scripts.Game.Core.BoardBase;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Game.Core.Managers
{
    public class MoveCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI remainingMoveCountText;
        private int _remainingMoveCount;

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            EventManager.OnCellUpgraded += SpentMove;
        }

        private void SpentMove(Cell cell)
        {
            _remainingMoveCount--;
            UpdateRemainingMoveCountText();

            if (_remainingMoveCount == 0)
                EventManager.OnMoveCountFinished?.Invoke();
        }

        private void UnsubscribeEvents()
        {
            EventManager.OnCellUpgraded -= SpentMove;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        public void SetMoveCount(int moveCount)
        {
            _remainingMoveCount = moveCount;
            UpdateRemainingMoveCountText();
        }

        private void UpdateRemainingMoveCountText()
        {
            remainingMoveCountText.SetText(_remainingMoveCount.ToString());
        }
    }
}
