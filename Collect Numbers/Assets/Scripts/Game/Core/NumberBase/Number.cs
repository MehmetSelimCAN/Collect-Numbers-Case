using Assets.Scripts.Game.Core.BoardBase;
using Assets.Scripts.Game.Core.Enums;
using Assets.Scripts.Game.Core.Managers;
using Assets.Scripts.Game.Core.Managers.PoolSystem;
using Assets.Scripts.Game.Mechanics;
using Assets.Scripts.SO;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.Core.NumberBase
{
    public class Number : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;

        private NumberData _numberData;
        public NumberData NumberData { get { return _numberData; } }
        private NumberListSO _numbersListSO;

        public FallAnimation FallAnimation;
        public ScaleAnimation ScaleAnimation;

        private Cell _cell;
        public Cell Cell
        {
            get { return _cell; }
            set
            {
                if (_cell == value) return;

                var oldCell = _cell;
                _cell = value;

                if (oldCell != null && oldCell.Number == this)
                {
                    oldCell.Number = null;
                }

                if (value != null)
                {
                    value.Number = this;
                    gameObject.name = _cell.gameObject.name + " " + GetType().Name;
                }
            }
        }

        private ImageLibrary _imageLibrary;
        private NumberFactory _numberFactory;

        public void PrepareNumber(NumberData numberData, Cell cell, NumberBase numberBase)
        {
            _numbersListSO = Resources.Load<NumberListSO>("Numbers/Number List");
            _numberData = numberData;
            _cell = cell;
            gameObject.name = _cell.gameObject.name + " " + GetType().Name;

            FallAnimation = numberBase.FallAnimation;
            FallAnimation.Number = this;

            ScaleAnimation = numberBase.ScaleAnimation;
            ScaleAnimation.Number = this;

            _imageLibrary = ServiceProvider.GetImageLibrary;
            _numberFactory = ServiceProvider.GetNumberFactory;

            if (TryGetComponent(out SpriteRenderer spriteRenderer))
            {
                _spriteRenderer = spriteRenderer;
            }

            if (_spriteRenderer == null)
                _spriteRenderer = AddSprite(_imageLibrary.GetSpriteForNumberType(_numberData.NumberType));
            else
                UpdateSprite();
        }

        public void ChangeNumberData(NumberData numberData)
        {
            if (numberData.NumberType == NumberType.Unique)
            {
                Number uniqueNumber = _numberFactory.Create(_cell, numberData);
                uniqueNumber.transform.position = _cell.transform.position;
                uniqueNumber.transform.localScale = _cell.Number.transform.localScale;
                uniqueNumber.Expand();
                _cell.Number = uniqueNumber;
                _cell = null;
                PoolingSystem.Instance.DestroyPoolObject(gameObject);
            }
            else
            {
                _numberData = numberData;
                UpdateSprite();
            }
        }

        public void IncreaseNumber()
        {
            NumberSO currentNumberSO = _numbersListSO.numbers.list.Find(x => x.NumberData.NumberType == _numberData.NumberType);
            int currentNumberIndex = _numbersListSO.numbers.list.IndexOf(currentNumberSO);
            NumberData numberData = _numbersListSO.numbers.list[currentNumberIndex + 1].NumberData;
            ChangeNumberData(numberData);
        }

        private SpriteRenderer AddSprite(Sprite sprite)
        {
            var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite;

            return spriteRenderer;
        }

        private void UpdateSprite()
        {
            _spriteRenderer.sprite = _imageLibrary.GetSpriteForNumberType(_numberData.NumberType);
        }

        public void Fall()
        {
            FallAnimation.FallTo(Cell.GetFallTarget());
        }

        public void Expand()
        {
            ScaleAnimation.Expand();
        }

        public void Shrink()
        {
            ScaleAnimation.Shrink();
        }

        public virtual void TryToExecute(bool isExecutedFromMatch, List<Cell> toBeDestroyedCells)
        {
            RemoveNumber();
        }

        private void RemoveNumber()
        {
            EventManager.OnCellExploded?.Invoke(Cell);

            Cell.Number = null;
            Cell = null;

            PoolingSystem.Instance.DestroyPoolObject(gameObject);
        }
    }
}
