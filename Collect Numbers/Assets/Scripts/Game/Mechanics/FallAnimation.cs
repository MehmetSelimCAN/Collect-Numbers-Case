using Assets.Scripts.Game.Core.BoardBase;
using Assets.Scripts.Game.Core.NumberBase;
using UnityEngine;

namespace Assets.Scripts.Game.Mechanics
{
    public class FallAnimation : MonoBehaviour
    {
        public Number Number;
        public bool IsFalling { get; private set; }
        [HideInInspector] public Cell TargetCell;

        private static float _startVel = 10f;
        private static float _maxSpeed = 20f;

        private float _vel = _startVel;

        private Vector3 _targetPosition;

        public void FallTo(Cell targetCell)
        {
            if (TargetCell != null && targetCell.Y >= TargetCell.Y) return;
            TargetCell = targetCell;
            Number.Cell = TargetCell;
            _targetPosition = TargetCell.transform.position;
            IsFalling = true;
        }

        public void Update()
        {
            if (!IsFalling) return;

            _vel = _vel >= _maxSpeed ? _maxSpeed : _vel;
            var p = Number.transform.position;
            p.y -= _vel * Time.deltaTime;
            if (p.y <= _targetPosition.y)
            {
                IsFalling = false;
                TargetCell = null;
                p.y = _targetPosition.y;
                _vel = _startVel;
            }

            Number.transform.position = p;
        }
    }
}