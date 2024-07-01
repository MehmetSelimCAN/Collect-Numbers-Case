using Assets.Scripts.SO;
using UnityEngine;

namespace Assets.Scripts.Game.EditorBase
{
    public class EditorNumber : MonoBehaviour
    {
        private NumberData _numberData;
        public NumberData NumberData { get { return _numberData; } }
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public void ChangeNumberData(NumberData numberData)
        {
            _numberData = numberData;
            _spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/" + numberData.NumberType.ToString());
        }
    }
}
