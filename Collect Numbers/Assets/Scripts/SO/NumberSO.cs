using Assets.Scripts.Game.Core.Enums;
using System;
using UnityEngine;

namespace Assets.Scripts.SO
{
    [CreateAssetMenu(fileName = "Number", menuName = "Number/Number")]
    public class NumberSO : ScriptableObject
    {
        public NumberData NumberData;
    }

    [Serializable]
    public class NumberData
    {
        public NumberType NumberType;
        public bool IsUpgradeable;
    }
}
