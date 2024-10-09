using Assets.Scripts.Game.Core.BoardBase;
using Assets.Scripts.Game.Core.Managers;
using Assets.Scripts.Game.Core.Managers.PoolSystem;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Game.Core.GoalBase
{
    public class GoalAnimationManager : MonoBehaviour
    {
        private float _animationSpeed = 1500f;
        private ColorLibrary _colorLibrary;

        private void Start()
        {
            _colorLibrary = ServiceProvider.GetColorLibrary;
        }

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
            string animationPrefabID = "GoalAnimation";
            Vector3 startPosition = Camera.main.WorldToScreenPoint(cell.transform.position);
            GameObject animation = PoolingSystem.Instance.InstantiatePoolObject(animationPrefabID, startPosition);
            animation.GetComponent<Image>().color = _colorLibrary.GetColorForNumberType(goal.GoalNumberType);

            float _animationTime = Vector3.Magnitude(goal.transform.position - startPosition) / _animationSpeed;
            animation.transform.DOMove(goal.transform.position, _animationTime).SetEase(Ease.InOutSine).OnComplete(() =>
                {
                    PoolingSystem.Instance.DestroyPoolObject(animation);
                    EventManager.OnGoalAnimationCompleted?.Invoke(goal);
                });
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
