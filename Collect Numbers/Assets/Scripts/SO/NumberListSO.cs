using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.SO
{
    [CreateAssetMenu(fileName = "Number List", menuName = "Number/Number List")]
    public class NumberListSO : ScriptableObject
    {
        public NumberList numbers;
    }

    [Serializable]
    public class NumberList
    {
        public List<NumberSO> list;
    }
}
