using Assets.Scripts.Game.Core.NumberBase;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.Game.Mechanics
{
    public class ScaleAnimation : MonoBehaviour
    {
        [HideInInspector] public Number Number;

        protected string _tweenID;

        private void Awake() => _tweenID = GetInstanceID() + "TweenID";

        public void Highlight()
        {
            DOTween.Kill(_tweenID);
            Number.transform.DORotate(new Vector3(0, 0, 359), 0.25f, RotateMode.FastBeyond360).OnComplete(() =>
            {
                Number.transform.DOScale(Vector3.zero, 0.25f);
                Number.transform.DORotate(new Vector3(0, 0, 0), 0f);
            });
        }

        public void Expand()
        {
            DOTween.Kill(_tweenID);
            Number.transform.DOScale(Vector3.one * 1f, 0.1f);
        }

        public void Shrink()
        {
            DOTween.Kill(_tweenID);
            Number.transform.DOScale(Vector3.one * 0.6f, 0.1f);
        }
    }
}
