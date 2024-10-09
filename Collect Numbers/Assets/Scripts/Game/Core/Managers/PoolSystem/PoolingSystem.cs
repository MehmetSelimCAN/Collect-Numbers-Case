using Assets.Scripts.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.Core.Managers.PoolSystem
{
    public class PoolingSystem : MonoSingleton<PoolingSystem>
    {
        [SerializeField] private List<Pool> _pools = new List<Pool>();
        [SerializeField] private int _defaultSize = 20;
        private Transform _canvasTransform;

        protected override void Awake()
        {
            _canvasTransform = GameObject.Find("Canvas").transform;
            InitPoolObjects();
            base.Awake();
        }

        private void InitPoolObjects()
        {
            int _tempSize = _defaultSize;

            for (int i = 0; i < _pools.Count; i++)
            {
                if (_pools[i].InitSize != 0)
                    _tempSize = _pools[i].InitSize;

                for (int j = 0; j < _tempSize; j++)
                {
                    GameObject obj;
                    if (_pools[i].PoolID == "GoalAnimation")
                    {
                        obj = Instantiate(_pools[i].Prefab, _canvasTransform);
                    }
                    else
                    {
                        obj = Instantiate(_pools[i].Prefab, transform);
                    }
                    obj.SetActive(false);

                    if (!obj.TryGetComponent(out PoolObject _))
                        obj.AddComponent<PoolObject>().Initialize(_pools[i].PoolID);

                    _pools[i].cloneObjects.Add(obj);
                }
            }
        }

        public GameObject InstantiatePoolObject(string ID)
        {
            for (int i = 0; i < _pools.Count; i++)
            {
                if (string.Equals(_pools[i].PoolID, ID))
                {
                    for (int j = 0; j < _pools[i].cloneObjects.Count; j++)
                    {
                        if (!_pools[i].cloneObjects[j].activeInHierarchy)
                        {
                            _pools[i].cloneObjects[j].SetActive(true);

                            return _pools[i].cloneObjects[j];
                        }
                    }

                    GameObject obj;
                    if (ID == "GoalAnimation")
                    {
                        obj = Instantiate(_pools[i].Prefab, _canvasTransform);
                    }
                    else
                    {
                        obj = Instantiate(_pools[i].Prefab, transform);
                    }

                    if (!obj.TryGetComponent(out PoolObject _))
                        obj.AddComponent<PoolObject>().Initialize(_pools[i].PoolID);

                    _pools[i].cloneObjects.Add(obj);
                    return obj;
                }
            }

            return null;
        }

        public GameObject InstantiatePoolObject(string ID, Vector3 position)
        {
            GameObject obj = InstantiatePoolObject(ID);
            if (obj)
            {
                obj.transform.position = position;
                return obj;
            }
            else
                return null;
        }

        public void DestroyPoolObject(GameObject cloneObj)
        {
            cloneObj.transform.position = transform.position;
            cloneObj.transform.rotation = transform.rotation;
            cloneObj.GetComponent<PoolObject>().OnBackToPool();

            if (cloneObj.GetComponent<PoolObject>().poolID == "GoalAnimation")
            {
                cloneObj.transform.SetParent(_canvasTransform);
            }
            else
            {
                cloneObj.transform.SetParent(transform);
            }

            cloneObj.SetActive(false);
        }

        public void DestroyPoolObject(GameObject cloneObj, float waitTime)
        {
            StartCoroutine(DestroyPoolObjectCo(cloneObj, waitTime));
        }

        private IEnumerator DestroyPoolObjectCo(GameObject clone, float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            DestroyPoolObject(clone);
        }
    }
}
