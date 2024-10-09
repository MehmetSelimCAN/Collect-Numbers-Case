using Assets.Scripts.Game.Core.BoardBase;
using Assets.Scripts.Game.Core.GoalBase;
using System;
using UnityEngine.Events;

namespace Assets.Scripts.Game.Core.Managers
{
    public static class EventManager
    {
        public static UnityAction OnSceneLoaded = delegate { };
        public static UnityAction OnSceneRefreshed = delegate { };

        public static UnityAction<Cell> OnCellClicked = delegate { };
        public static UnityAction<Cell> OnCellExploded = delegate { };
        public static UnityAction<Cell> OnCellUpgraded = delegate { };

        public static UnityAction<Goal, Cell> OnGoalChanged = delegate { };
        public static UnityAction<Goal> OnGoalAnimationCompleted = delegate { };
        public static UnityAction<Goal> OnGoalCompleted = delegate { };
        public static UnityAction OnAllGoalsCompleted = delegate { };

        public static UnityAction OnMoveCountFinished = delegate { };

        public static UnityAction OnLevelStarted = delegate { };
        public static UnityAction OnLevelFailed = delegate { };
        public static UnityAction OnLevelSuccesful = delegate { };
        public static UnityAction OnLevelFinished = delegate { };

        public static UnityAction OnCellFallsStarted = delegate { };
        public static UnityAction OnCellFallsFinished = delegate { };

        public static UnityAction OnCellExplodeAnimationStarted = delegate { };
        public static UnityAction OnCellExplodeAnimationEnded = delegate { };

    }
}
