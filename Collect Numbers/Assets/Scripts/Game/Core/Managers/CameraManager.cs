using Grid = Assets.Scripts.Game.Core.BoardBase.Grid;
using UnityEngine;

namespace Assets.Scripts.Game.Core.Managers
{
    public class CameraManager : MonoBehaviour
    {
        private float yOffset = 0.5f;
        private float orthographicSizeOffset = 1;

        public void PrepareCamera()
        {
            var cam = GetComponent<Camera>();
            float XPosition = (Grid.Cols - 1) / 2f;
            float YPosition = (Grid.Rows - 1) / 2f;
            cam.transform.position = new Vector3(XPosition, YPosition + yOffset, -10);
            cam.orthographicSize = Mathf.Max(Grid.Cols, Grid.Rows) + orthographicSizeOffset;
        }
    }
}
