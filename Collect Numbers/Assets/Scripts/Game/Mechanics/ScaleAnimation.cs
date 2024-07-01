using Assets.Scripts.Game.Core.NumberBase;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.Game.Mechanics
{
    public class ScaleAnimation : MonoBehaviour
    {
        public Number Number;

        protected string _tweenID;

        private void Awake() => _tweenID = GetInstanceID() + "TweenID";

        public void Expand()
        {
            DOTween.Kill(_tweenID);
            Number.transform.DOScale(new Vector3(1f, 1f), 0.1f);
        }

        public void Shrink()
        {
            DOTween.Kill(_tweenID);
            Number.transform.DOScale(new Vector3(0.6f, 0.6f), 0.1f);
        }
    }
}
