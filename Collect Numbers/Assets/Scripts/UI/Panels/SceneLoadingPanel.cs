using Assets.Scripts.Game.Core.Managers;

namespace Assets.Scripts.UI.Panels
{
    public class SceneLoadingPanel : BaseFadePanel
    {
        protected override float FadeOutDuration => base.FadeOutDuration + 0.5f;

        private void OnEnable()
        {
            ShowPanelAnimated();
            EventManager.OnSceneRefreshed += ShowPanel;
            EventManager.OnSceneLoaded += HidePanelAnimated;
        }

        private void OnDisable()
        {
            EventManager.OnSceneRefreshed -= ShowPanel;
            EventManager.OnSceneLoaded -= HidePanelAnimated;
        }
    }
}
