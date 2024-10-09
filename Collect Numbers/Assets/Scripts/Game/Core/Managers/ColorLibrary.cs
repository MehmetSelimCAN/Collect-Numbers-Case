using Assets.Scripts.Game.Core.Enums;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Game.Core.Managers
{
    public class ColorLibrary : MonoBehaviour, IProvidable
    {
        [SerializeField] private Color _number1Color;
        [SerializeField] private Color _number2Color;
        [SerializeField] private Color _number3Color;
        [SerializeField] private Color _number4Color;
        [SerializeField] private Color _number5Color;
        [SerializeField] private Color _uniqueColor;

        private void Awake()
        {
            ServiceProvider.Register(this);
        }

        public Color GetColorForNumberType(NumberType numberType)
        {
            switch (numberType)
            {
                case NumberType.Number1:
                    return _number1Color;
                case NumberType.Number2:
                    return _number2Color;
                case NumberType.Number3:
                    return _number3Color;
                case NumberType.Number4:
                    return _number4Color;
                case NumberType.Number5:
                    return _number5Color;
                case NumberType.Unique:
                    return _uniqueColor;
                default:
                    return _number1Color;
            }
        }
    }
}
