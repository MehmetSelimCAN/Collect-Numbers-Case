using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.Core.Managers.PoolSystem
{
    [System.Serializable]
    public class Pool
    {
        public string PoolID;
        public GameObject Prefab;
        public int InitSize;

        public List<GameObject> cloneObjects = new List<GameObject>();
    }
}
