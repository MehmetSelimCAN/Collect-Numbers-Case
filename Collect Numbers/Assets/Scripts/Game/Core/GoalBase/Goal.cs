using Assets.Scripts.Game.Core.BoardBase;
using Assets.Scripts.Game.Core.Enums;
using Assets.Scripts.Game.Core.Managers;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Game.Core.GoalBase
{
    public class Goal : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI goalCountText;
        [SerializeField] private TextMeshProUGUI goalText;
        [SerializeField] private Image numberTypeImage;

        private NumberType _goalNumberType;
        public NumberType GoalNumberType { get { return _goalNumberType; } }

        private int _goalCount;
        private bool _isCompleted;

        private ColorLibrary _colorLibrary;
        private TextLibrary _textLibrary;

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
                UpdateGoalCountText("<sprite name=\"Tick\">");
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

            _colorLibrary = ServiceProvider.GetColorLibrary;
            _textLibrary = ServiceProvider.GetTextLibrary;

            UpdateGoalCountText(_goalCount.ToString());
            UpdateGoalText();
            UpdateNumberTypeImage();
        }

        private void UpdateGoalCountText(string goalCount)
        {
            goalCountText.SetText(goalCount);
            numberTypeImage.transform.DOScale(Vector3.one * 1.5f, 0.2f).OnComplete(() => numberTypeImage.transform.DOScale(Vector3.one, 0.2f));
            goalText.transform.DOScale(Vector3.one * 1.5f, 0.2f).OnComplete(() => goalText.transform.DOScale(Vector3.one, 0.2f));
        }

        private void UpdateGoalText()
        {
            string goalString = _textLibrary.GetTextForNumberType(_goalNumberType);
            goalText.SetText(goalString);
        }

        private void UpdateNumberTypeImage()
        {
            numberTypeImage.color = _colorLibrary.GetColorForNumberType(_goalNumberType);
        }
    }
}
