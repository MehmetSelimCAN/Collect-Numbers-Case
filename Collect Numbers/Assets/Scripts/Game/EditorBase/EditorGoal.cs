using Assets.Scripts.Game.Core.Enums;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Game.EditorBase
{
    public class EditorGoal : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private TextMeshPro goalCountText;
        [SerializeField] private TextMeshPro goalNumberText;

        [HideInInspector] public NumberType NumberType;
        [HideInInspector] public int GoalCount;

        [SerializeField] private Color _toBeFilledColor;
        [SerializeField] private Color[] _numberColors;

        [SerializeField] private string[] _numberTexts;

        public void ChangeGoalImage(NumberType numberType)
        {
            if (numberType == NumberType.Random)
            {
                spriteRenderer.color = _toBeFilledColor;
                goalNumberText.SetText("Select Type");
            }
            else
            {
                spriteRenderer.color = _numberColors[(int)NumberType - 1];
                goalNumberText.SetText(_numberTexts[(int)NumberType - 1]);
            }
        }

        public void ChangeGoalCountText(int goalCount)
        {
            goalCountText.SetText(goalCount.ToString());
        }
    }
}
