using Assets.Scripts.Game.Core.BoardBase;
using Assets.Scripts.Game.Core.Enums;
using Assets.Scripts.Game.Core.Managers;
using Assets.Scripts.Game.Core.Managers.PoolSystem;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.Game.Core.GoalBase
{
    public class GoalAnimationManager : MonoBehaviour
    {
        private float _animationTime = 0.75f;
        
        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            EventManager.OnGoalChanged += PlayAnimation;
        }

        private void PlayAnimation(Goal goal, Cell cell)
        {
            string animationPrefabID = GetAnimationPrefabIDForNumberType(cell.Number.NumberData.NumberType);
            GameObject animation = PoolingSystem.Instance.InstantiatePoolObject(animationPrefabID, cell.transform.position);
            PoolingSystem.Instance.DestroyPoolObject(animation, _animationTime);

            Vector3 targetPosition = Camera.main.ScreenToWorldPoint(goal.transform.position);
            animation.transform.DOMove(targetPosition, _animationTime).OnComplete(() => EventManager.OnGoalAnimationCompleted?.Invoke(goal));
        }

        private string GetAnimationPrefabIDForNumberType(NumberType numberType)
        {
            switch (numberType)
            {
                case NumberType.Number1:
                    return "Number1GoalAnimation";
                case NumberType.Number2:
                    return "Number2GoalAnimation";
                case NumberType.Number3:
                    return "Number3GoalAnimation";
                case NumberType.Number4:
                    return "Number4GoalAnimation";
                case NumberType.Number5:
                    return "Number5GoalAnimation";
                default:
                    return null;
            }
        }

        private void UnsubscribeEvents()
        {
            EventManager.OnGoalChanged -= PlayAnimation;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }
    }
}
