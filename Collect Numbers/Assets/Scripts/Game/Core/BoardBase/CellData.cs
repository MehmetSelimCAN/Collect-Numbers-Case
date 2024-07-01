using Assets.Scripts.SO;
using System;
using UnityEngine;

namespace Assets.Scripts.Game.Core.BoardBase
{
    [Serializable]
    public class CellData
    {
        public Vector2 Position;
        public NumberData NumberData;
    }
}
