using Assets.Scripts.Game.Core.Enums;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Game.Core.Managers
{
    public class ImageLibrary : MonoBehaviour, IProvidable
    {
        [SerializeField] private Sprite number1Sprite;
        [SerializeField] private Sprite number2Sprite;
        [SerializeField] private Sprite number3Sprite;
        [SerializeField] private Sprite number4Sprite;
        [SerializeField] private Sprite number5Sprite;
        [SerializeField] private Sprite uniqueSprite;

        private void Awake()
        {
            ServiceProvider.Register(this);
        }

        public Sprite GetSpriteForNumberType(NumberType numberType)
        {
            switch (numberType)
            {
                case NumberType.Number1:
                    return number1Sprite;
                case NumberType.Number2:
                    return number2Sprite;
                case NumberType.Number3:
                    return number3Sprite;
                case NumberType.Number4:
                    return number4Sprite;
                case NumberType.Number5:
                    return number5Sprite;
                case NumberType.Unique:
                    return uniqueSprite;
                default:
                    return number1Sprite;
            }
        }
    }
}
