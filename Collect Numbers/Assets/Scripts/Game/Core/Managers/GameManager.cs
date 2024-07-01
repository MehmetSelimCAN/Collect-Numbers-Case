using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game.Core.Managers
{
    public class GameManager : MonoBehaviour
    {
        private bool _isFallingAnything;
        private bool _isAllGoalsCompleted;
        private bool _isMoveCountFinished;
        private bool _isLevelCompleted;

        private WaitForSeconds moveCountDelay = new WaitForSeconds(0.8f);
        private WaitForSeconds levelCompletedDelay = new WaitForSeconds(1f);

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            EventManager.OnMoveCountFinished += OnMoveCountFinished;
            EventManager.OnAllGoalsCompleted += OnAllGoalsCompleted;
            EventManager.OnCellFallsFinished += OnCellFallsFinished;
            EventManager.OnCellFallsStarted += OnCellFallsStarted;
        }

        private void OnMoveCountFinished()
        {
            EventManager.OnLevelFinished?.Invoke();
            StartCoroutine(MoveCountFinishedWithDelay());
        }

        private IEnumerator MoveCountFinishedWithDelay()
        {
            _isMoveCountFinished = true;
            yield return moveCountDelay;

            if (_isFallingAnything) yield break;

            if (_isAllGoalsCompleted)
            {
                StartCoroutine(SuccesfulWithDelay());
            }
            else
            {
                StartCoroutine(FailedWithDelay());
            }
        }

        private void OnAllGoalsCompleted()
        {
            _isAllGoalsCompleted = true;
            
            if (!_isFallingAnything && !_isLevelCompleted)
            {
                StartCoroutine(SuccesfulWithDelay());
            }
        }

        private void OnCellFallsFinished()
        {
            _isFallingAnything = false;
            if (_isMoveCountFinished)
            {
                if (!_isLevelCompleted)
                {
                    if (_isAllGoalsCompleted)
                    {
                        StartCoroutine(SuccesfulWithDelay());
                    }
                    else
                    {
                        StartCoroutine(FailedWithDelay());
                    }
                }
            }
            else if (_isAllGoalsCompleted && !_isLevelCompleted)
            {
                StartCoroutine(SuccesfulWithDelay());
            }
        }

        private void OnCellFallsStarted()
        {
            _isFallingAnything = true;
        }

        private IEnumerator SuccesfulWithDelay()
        {
            EventManager.OnLevelFinished?.Invoke();

            if (_isLevelCompleted) yield break;
            _isLevelCompleted = true;

            yield return levelCompletedDelay;

            int lastLevel = PlayerPrefs.GetInt("LastLevel", 1);
            PlayerPrefs.SetInt("LastLevel", lastLevel + 1);

            EventManager.OnLevelSuccesful?.Invoke();
        }

        private IEnumerator FailedWithDelay()
        {
            EventManager.OnLevelFinished?.Invoke();

            yield return levelCompletedDelay;
            EventManager.OnLevelFailed?.Invoke();
        }

        private void UnsubscribeEvents()
        {
            EventManager.OnMoveCountFinished -= OnMoveCountFinished;
            EventManager.OnAllGoalsCompleted -= OnAllGoalsCompleted;
            EventManager.OnCellFallsFinished -= OnCellFallsFinished;
            EventManager.OnCellFallsStarted -= OnCellFallsStarted;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }
    }
}
