using Assets.Scripts.Game.Core.Enums;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Game.Core.Managers
{
    public class TextLibrary : MonoBehaviour, IProvidable
    {
        [SerializeField] private string _number1Text = "1";
        [SerializeField] private string _number2Text = "2";
        [SerializeField] private string _number3Text = "3";
        [SerializeField] private string _number4Text = "4";
        [SerializeField] private string _number5Text = "5";
        [SerializeField] private string _uniqueText = "";

        private void Awake()
        {
            ServiceProvider.Register(this);
        }

        public string GetTextForNumberType(NumberType numberType)
        {
            switch (numberType)
            {
                case NumberType.Number1:
                    return _number1Text;
                case NumberType.Number2:
                    return _number2Text;
                case NumberType.Number3:
                    return _number3Text;
                case NumberType.Number4:
                    return _number4Text;
                case NumberType.Number5:
                    return _number5Text;
                case NumberType.Unique:
                    return _uniqueText;
                default:
                    return _number1Text;
            }
        }
    }
}
