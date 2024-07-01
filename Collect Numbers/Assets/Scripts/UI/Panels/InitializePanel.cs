using Assets.Scripts.Game.Core.Managers;

namespace Assets.Scripts.UI.Panels
{
    public class InitializePanel : BaseFadePanel
    {
        protected override float FadeOutDuration => base.FadeOutDuration + 0.5f;

        private void OnEnable()
        {
            ShowPanelAnimated();
            EventManager.OnSceneLoaded += HidePanelAnimated;
        }

        private void OnDisable()
        {
            EventManager.OnSceneLoaded -= HidePanelAnimated;
        }
    }
}
