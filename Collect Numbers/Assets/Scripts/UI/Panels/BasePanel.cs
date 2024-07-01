using UnityEngine;

namespace Assets.Scripts.UI.Panels
{
    [RequireComponent(typeof(CanvasGroup))]
    public class BasePanel : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;
        protected CanvasGroup CanvasGroup { get { return (_canvasGroup == null) ? _canvasGroup = GetComponent<CanvasGroup>() : _canvasGroup; } }

        public virtual void ShowPanel()
        {
            if (CanvasGroup.alpha > 0)
                return;

            SetPanel(1, true, true);
        }

        public virtual void HidePanel()
        {
            if (CanvasGroup.alpha == 0)
                return;

            SetPanel(0, false, false);
        }

        public void SetPanel(float alpha, bool interactable, bool blocksRaycast)
        {
            CanvasGroup.alpha = alpha;
            CanvasGroup.interactable = interactable;
            CanvasGroup.blocksRaycasts = blocksRaycast;
        }
    }
}
