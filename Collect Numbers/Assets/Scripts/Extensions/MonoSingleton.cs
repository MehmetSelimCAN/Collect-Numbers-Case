using UnityEngine;

namespace Assets.Scripts.Extensions
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.LogError(typeof(T).ToString() + " is missing.");
                }

                return _instance;
            }
        }



        protected virtual void Awake()
        {
            if (_instance != null)
            {
                Debug.LogWarning("Second instance of " + typeof(T) + " created. Automatic self-destruct triggered.");
                Destroy(this.gameObject);
            }
            _instance = this as T;

            Init();
        }


        void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }

        public virtual void Init() { }
    }
}