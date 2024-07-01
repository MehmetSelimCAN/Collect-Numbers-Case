using Assets.Scripts.Game.Core.Enums;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Game.EditorBase
{
    public class EditorGoal : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private TextMeshPro goalText;

        [HideInInspector] public NumberType NumberType;
        [HideInInspector] public int GoalCount;

        public void ChangeGoalImage(NumberType numberType)
        {
            if (numberType == NumberType.Random)
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/ToBeFilled");
            }
            else
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/" + numberType.ToString());
            }
        }

        public void ChangeGoalText(int goalCount)
        {
            goalText.SetText(goalCount.ToString());
        }
    }
}
