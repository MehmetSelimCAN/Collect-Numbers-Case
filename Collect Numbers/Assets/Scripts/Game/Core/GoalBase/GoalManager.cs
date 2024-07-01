using Assets.Scripts.Game.Core.Managers;
using UnityEngine;

namespace Assets.Scripts.Game.Core.GoalBase
{
    public class GoalManager : MonoBehaviour
    {
        private int _goalsCount;
        private int _completedGoalCount;

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            EventManager.OnGoalCompleted += OnGoalCompleted;
            EventManager.OnSceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded()
        {
            _completedGoalCount = 0;
        }

        private void OnGoalCompleted(Goal completedGoal)
        {
            _completedGoalCount++;
            if (_completedGoalCount == _goalsCount)
            {
                EventManager.OnAllGoalsCompleted?.Invoke();
            }
        }

        private void UnsubscribeEvents()
        {
            EventManager.OnGoalCompleted -= OnGoalCompleted;
            EventManager.OnSceneLoaded -= OnSceneLoaded;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        public void SetGoalsCount(int goalCount)
        {
            _goalsCount = goalCount;
        }
    }
}
