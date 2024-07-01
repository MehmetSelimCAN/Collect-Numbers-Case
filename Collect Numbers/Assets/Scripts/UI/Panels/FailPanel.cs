using Assets.Scripts.Game.Core.LevelBase;
using Assets.Scripts.Game.Core.Managers;

namespace Assets.Scripts.UI.Panels
{
    public class FailPanel : BaseFadePanel
    {
        public bool IsButtonInteracted { get; private set; }

        private void OnEnable()
        {
            EventManager.OnSceneLoaded += OnSceneLoaded;
            EventManager.OnLevelFailed += ShowPanelAnimated;
        }

        private void OnSceneLoaded()
        {
            IsButtonInteracted = false;
            HidePanel();
        }

        private void OnDisable()
        {

            EventManager.OnSceneLoaded -= OnSceneLoaded;
            EventManager.OnLevelFailed -= ShowPanelAnimated;
        }

        public void Restart()
        {
            if (IsButtonInteracted)
                return;

            IsButtonInteracted = true;
            HidePanelAnimated();

            LevelManager.Instance.ReloadLevel();
        }
    }
}
