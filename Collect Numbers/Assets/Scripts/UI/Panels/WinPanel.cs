using Assets.Scripts.Game.Core.LevelBase;
using Assets.Scripts.Game.Core.Managers;

namespace Assets.Scripts.UI.Panels
{
    public class WinPanel : BaseFadePanel
    {
        public bool IsButtonInteracted { get; private set; }

        private void OnEnable()
        {
            EventManager.OnSceneLoaded += OnSceneLoaded;
            EventManager.OnLevelSuccesful += ShowPanelAnimated;
        }

        private void OnSceneLoaded()
        {
            IsButtonInteracted = false;
            HidePanel();
        }

        private void OnDisable()
        {
            EventManager.OnSceneLoaded -= OnSceneLoaded;
            EventManager.OnLevelSuccesful -= ShowPanelAnimated;
        }

        public void Next()
        {
            if (IsButtonInteracted)
                return;

            IsButtonInteracted = true;
            HidePanelAnimated();

            LevelManager.Instance.LoadNextLevel();
        }
    }
}
