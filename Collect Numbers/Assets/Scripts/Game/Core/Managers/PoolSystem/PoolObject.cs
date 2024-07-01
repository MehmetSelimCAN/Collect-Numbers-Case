using UnityEngine;

namespace Assets.Scripts.Game.Core.Managers.PoolSystem
{
    public class PoolObject : MonoBehaviour
    {
        public string poolID { get; private set; }

        private Vector3 _defaultScale;

        private void Awake()
        {
            _defaultScale = transform.localScale;
        }

        public void Initialize(string newID)
        {
            poolID = newID;
        }

        public void OnBackToPool()
        {
            transform.localScale = _defaultScale;
        }
    }
}
