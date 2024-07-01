using Assets.Scripts.Game.Core.BoardBase;
using Assets.Scripts.Game.Core.Enums;
using Assets.Scripts.Game.Core.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Game.Core.GoalBase
{
    public class Goal : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI goalCountText;
        [SerializeField] private Image numberTypeImage;

        private NumberType _goalNumberType;
        public NumberType GoalNumberType { get { return _goalNumberType; } }

        private int _goalCount;
        private bool _isCompleted;

        private ImageLibrary _imageLibrary;

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            EventManager.OnCellExploded += CheckGoal;
            EventManager.OnGoalAnimationCompleted += OnGoalAnimationCompleted;
        }

        private void CheckGoal(Cell cell)
        {
            if (cell.Number.NumberData.NumberType == _goalNumberType && !_isCompleted)
            {
                EventManager.OnGoalChanged?.Invoke(this, cell);
                DecreaseGoalCount();
            }
        }

        private void DecreaseGoalCount()
        {
            if (_isCompleted) return;

            _goalCount--;
            if (_goalCount <= 0)
            {
                _isCompleted = true;
                EventManager.OnGoalCompleted?.Invoke(this);
            }
        }

        private void OnGoalAnimationCompleted(Goal goal)
        {
            if (goal != this) return;

            if (_goalCount <= 0)
            {
                UpdateGoalCountText("Done");
            }
            else
            {
                UpdateGoalCountText(_goalCount.ToString());
            }
        }

        private void UnsubscribeEvents()
        {
            EventManager.OnCellExploded -= CheckGoal;
            EventManager.OnGoalAnimationCompleted -= OnGoalAnimationCompleted;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        public void Initialize(NumberType goalNumberType, int goalCount)
        {
            _goalNumberType = goalNumberType;
            _goalCount = goalCount;
            _imageLibrary = ServiceProvider.GetImageLibrary;

            UpdateGoalCountText(_goalCount.ToString());
            UpdateNumberTypeImage(_goalNumberType);
        }

        private void UpdateGoalCountText(string goalCount)
        {
            goalCountText.SetText(goalCount);
        }

        private void UpdateNumberTypeImage(NumberType goalNumberType)
        {
            numberTypeImage.sprite = _imageLibrary.GetSpriteForNumberType(goalNumberType);
        }
    }
}
