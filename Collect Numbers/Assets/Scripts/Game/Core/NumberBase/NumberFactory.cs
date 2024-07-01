using Assets.Scripts.Game.Core.BoardBase;
using Assets.Scripts.Game.Core.Enums;
using Assets.Scripts.Game.Core.Managers;
using Assets.Scripts.Game.Core.Managers.PoolSystem;
using Assets.Scripts.Interfaces;
using Assets.Scripts.SO;
using UnityEngine;

namespace Assets.Scripts.Game.Core.NumberBase
{
    public class NumberFactory : MonoBehaviour, IProvidable
    {
        [SerializeField] private NumberBase numberBasePrefab;

        private void Awake()
        {
            ServiceProvider.Register(this);
        }

        public Number Create(Cell cell, NumberData numberData)
        {
            Number number = null;
            switch (numberData.NumberType)
            {
                case NumberType.Number1:
                case NumberType.Number2:
                case NumberType.Number3:
                case NumberType.Number4:
                case NumberType.Number5:
                    number = CreateNumber(cell, numberData);
                    break;
                case NumberType.Unique:
                    number = CreateUniqueNumber(cell, numberData);
                    break;
                default:
                    Debug.LogWarning("Can not create item: " + numberData.NumberType);
                    break;
            }

            return number;
        }

        private Number CreateNumber(Cell cell, NumberData numberData)
        {
            var numberBase = PoolingSystem.Instance.InstantiatePoolObject("Number").GetComponent<NumberBase>();
            var number = numberBase.gameObject.AddComponent<Number>();
            number.PrepareNumber(numberData, cell, numberBase);

            return number;
        }

        private Number CreateUniqueNumber(Cell cell, NumberData numberData)
        {
            var numberBase = PoolingSystem.Instance.InstantiatePoolObject("Number").GetComponent<NumberBase>();
            var number = numberBase.gameObject.AddComponent<UniqueNumber>();
            number.PrepareNumber(numberData, cell, numberBase);

            return number;
        }
    }
}
