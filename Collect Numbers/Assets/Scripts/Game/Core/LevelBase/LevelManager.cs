using Assets.Scripts.Extensions;
using Assets.Scripts.Game.Core.GoalBase;
using Assets.Scripts.Game.Core.Managers;
using Assets.Scripts.Game.Mechanics;
using Assets.Scripts.UI;
using Grid = Assets.Scripts.Game.Core.BoardBase.Grid;
using UnityEngine;

namespace Assets.Scripts.Game.Core.LevelBase
{
    public class LevelManager : MonoSingleton<LevelManager>
    {
        [SerializeField] private Grid grid;
        private UIManager uiManager;
        private GoalManager goalManager;
        private LevelData _levelData;
        private FallAndFillManager FallAndFillManager;
        private CameraManager CameraManager;

        protected override void Awake()
        {
            base.Awake();
            uiManager = FindObjectOfType<UIManager>();
            goalManager = FindObjectOfType<GoalManager>();
            FallAndFillManager = FindObjectOfType<FallAndFillManager>();
            CameraManager = FindObjectOfType<CameraManager>();
        }

        private void Start()
        {
            _levelData = LevelDataFactory.CreateLevelData("Level" + PlayerPrefs.GetInt("LastLevel", 1));

            EventManager.OnLevelStarted?.Invoke();
            PrepareGrid();
            PrepareLevel();
            PrepareUI();
            PrepareGoalManager();
            PrepareFillAndFallManager();
            PrepareCamera();
        }

        private void PrepareGrid()
        {
            grid.Prepare();
        }

        private void PrepareLevel()
        {
            for (var x = 0; x < _levelData.GridData.GetLength(0); x++)
            {
                for (var y = 0; y < _levelData.GridData.GetLength(1); y++)
                {
                    var cell = Grid.Cells[x, y];

                    var numberData = _levelData.GridData[x, y];
                    var number = cell.InsertNumber(numberData);

                    if (number == null) return;

                    cell.Number = number;
                    number.transform.position = cell.transform.position;
                }
            }
        }

        private void PrepareGoalManager()
        {
            goalManager.SetGoalsCount(_levelData.GoalDatas.Count);
        }

        private void PrepareUI()
        {
            uiManager.PrepareUI(_levelData);
        }

        private void PrepareCamera()
        {
            CameraManager.PrepareCamera();
        }

        private void PrepareFillAndFallManager()
        {
            FallAndFillManager.Init(_levelData);
        }

        public void ReloadLevel()
        {
            EventManager.OnSceneRefreshed?.Invoke();
            SceneController.Instance.LoadScene("GameScene");
        }

        public void LoadNextLevel()
        {
            EventManager.OnSceneRefreshed?.Invoke();
            SceneController.Instance.LoadScene("GameScene");
        }
    }
}
