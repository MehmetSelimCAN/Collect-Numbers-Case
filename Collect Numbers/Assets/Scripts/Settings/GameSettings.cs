using UnityEngine;

namespace Assets.Scripts.Settings
{
    public class GameSettings : MonoBehaviour
    {
        [Header("CONFIG")]
        [SerializeField] private int _targetFrameRate = 60;

        private void Awake()
        {
            Application.targetFrameRate = _targetFrameRate;
        }
    }
}
