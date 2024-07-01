using Assets.Scripts.Game.Core.GoalBase;
using Assets.Scripts.Game.Core.LevelBase;
using Assets.Scripts.Game.Core.Managers;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GoalGenerator goalGenerator;
        [SerializeField] private MoveCounter moveCounter;

        public void PrepareUI(LevelData levelData)
        {
            goalGenerator.GenerateGoals(levelData);
            moveCounter.SetMoveCount(levelData.MoveCount);
        }
    }
}
