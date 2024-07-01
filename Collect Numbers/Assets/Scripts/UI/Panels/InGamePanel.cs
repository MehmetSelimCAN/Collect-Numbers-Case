using Assets.Scripts.Game.Core.Managers;

namespace Assets.Scripts.UI.Panels
{
    public class InGamePanel : BasePanel
    {
        private void OnEnable()
        {
            EventManager.OnLevelStarted += ShowPanel;
        }

        private void OnDisable()
        {
            EventManager.OnLevelStarted -= ShowPanel;
        }
    }
}
