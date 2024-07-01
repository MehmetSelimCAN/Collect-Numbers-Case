using Assets.Scripts.Game.Core.LevelBase;
using UnityEngine;

namespace Assets.Scripts.Game.Core.GoalBase
{
    public class GoalGenerator : MonoBehaviour
    {
        [SerializeField] private Goal goalPrefab;
        [SerializeField] private Transform goalsParent;

        public void GenerateGoals(LevelData levelData)
        {
            ClearGoals();

            foreach (var goalData in levelData.GoalDatas)
            {
                Goal goal = Instantiate(goalPrefab, goalsParent);
                goal.Initialize(goalData.NumberType, goalData.GoalCount);
            }
        }

        public void ClearGoals()
        {
            for (int i = 0; i < goalsParent.childCount; i++)
            {
                Destroy(goalsParent.GetChild(i).gameObject);
            }
        }
    }
}
