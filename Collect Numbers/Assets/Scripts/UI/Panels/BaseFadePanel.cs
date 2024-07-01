using DG.Tweening;
using System;

namespace Assets.Scripts.UI.Panels
{
    public class BaseFadePanel : BasePanel
    {
        public bool IsPanelShowing { get; protected set; }
        protected virtual float FadeInDuration => 0.5f;
        protected virtual float FadeOutDuration => 0.5f;
        protected virtual float ShowDelay => 0;
        protected virtual float HideDelay => 0;

        protected virtual float MaxFade => 1f;
        protected virtual float MinFade => 0f;

        protected string _fadeTweenID;

        private void Awake() => _fadeTweenID = GetInstanceID() + "FadeTweenID";

        public virtual void ShowPanelAnimated()
        {
            if (IsPanelShowing)
                return;

            IsPanelShowing = true;
            FadeTween(MaxFade, ShowDelay, FadeInDuration, ShowPanel);
        }

        public virtual void HidePanelAnimated()
        {
            if (!IsPanelShowing)
                return;

            IsPanelShowing = false;
            FadeTween(MinFade, HideDelay, FadeOutDuration, HidePanel);
        }

        public override void ShowPanel()
        {
            IsPanelShowing = true;
            SetPanel(1, true, true);
        }

        public override void HidePanel()
        {
            IsPanelShowing = false;
            SetPanel(0, false, false);
        }

        protected virtual void FadeTween(float endValue, float delay, float duration, Action onComplete = null)
        {
            DOTween.Kill(_fadeTweenID);
            CanvasGroup.DOFade(endValue, duration).SetId(_fadeTweenID).SetDelay(delay).SetEase(Ease.Linear).OnComplete(() =>
            {
                onComplete?.Invoke();
            });
        }
    }
}
