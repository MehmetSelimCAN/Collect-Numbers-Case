using Assets.Scripts.SO;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Game.EditorBase
{
    public class EditorNumber : MonoBehaviour
    {
        private NumberData _numberData;
        public NumberData NumberData { get { return _numberData; } }

        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private TextMeshPro _numberText;

        [SerializeField] private Color[] _numberColors;
        [SerializeField] private string[] _numberTexts;

        public void ChangeNumberData(NumberData numberData)
        {
            _numberData = numberData;
            _spriteRenderer.color = _numberColors[(int)numberData.NumberType - 1];
            _numberText.SetText(_numberTexts[(int)numberData.NumberType - 1]);
        }
    }
}
