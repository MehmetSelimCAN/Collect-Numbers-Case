using Assets.Scripts.Game.Core.BoardBase;
using Assets.Scripts.Game.Core.Enums;
using Assets.Scripts.Game.Core.GoalBase;
using Assets.Scripts.SO;
using System.Collections.Generic;
using UnityEngine;
using Grid = Assets.Scripts.Game.Core.BoardBase.Grid;

namespace Assets.Scripts.Game.Core.LevelBase
{
    public class LevelData
    {
        public List<CellData> NumberDatas;
        public List<GoalData> GoalDatas;

        public List<NumberData> AvailableNumbers;
        public List<NumberData> RandomNumbers;

        public int GridWidth;
        public int GridHeight;
        public int MoveCount;

        private string _levelName;

        public LevelData(string levelName)
        {
            _levelName = levelName;
            NumberDatas = new List<CellData>();
            GoalDatas = new List<GoalData>();
            AvailableNumbers = new List<NumberData>();
            RandomNumbers = new List<NumberData>();
        }

        public NumberData[,] GridData { get; private set; }

        private NumberSO emptyNumber;

        public void Initialize()
        {
            TextAsset levelTextAsset = Resources.Load("Levels/" + _levelName, typeof(TextAsset)) as TextAsset;
            if (levelTextAsset == null)
            {
                levelTextAsset = Resources.Load("Levels/RandomLevel", typeof(TextAsset)) as TextAsset;
            }

            string dataText = levelTextAsset.text;

            ReadDataFromJson(dataText);
            AssignGridSize();

            emptyNumber = Resources.Load<NumberSO>("Numbers/Empty");
            GridData = new NumberData[Grid.Cols, Grid.Rows];

            int gridNumber = 0;
            for (var x = 0; x < Grid.Rows; x++)
            {
                for (var y = 0; y < Grid.Cols; y++)
                {
                    if (NumberDatas[gridNumber].NumberData.NumberType == NumberType.Random)
                    {
                        GridData[y, x] = emptyNumber.NumberData;
                    }
                    else
                    {
                        GridData[y, x] = NumberDatas[gridNumber].NumberData;
                    }

                    gridNumber++;
                }
            }

            gridNumber = 0;
            for (var x = 0; x < Grid.Rows; x++)
            {
                for (var y = 0; y < Grid.Cols; y++)
                {
                    if (NumberDatas[gridNumber].NumberData.NumberType == NumberType.Random)
                    {
                        List<NumberData> tempNumberList = RandomNumbers;
                        NumberType randomNumberType = tempNumberList[UnityEngine.Random.Range(0, tempNumberList.Count)].NumberType;

                        while (IsThereAnyMatch(x, y, randomNumberType))
                        {
                            tempNumberList.Remove(tempNumberList.Find(x => x.NumberType == randomNumberType));
                            randomNumberType = tempNumberList[UnityEngine.Random.Range(0, RandomNumbers.Count)].NumberType;
                        }

                        GridData[y, x] = RandomNumbers.Find(x => x.NumberType == randomNumberType);
                    }

                    gridNumber++;
                }
            }
        }

        private bool IsThereAnyMatch(int x, int y, NumberType randomNumberType)
        {
            //Check y axis matches
            return x > 0 && GridData[y, x - 1].NumberType == randomNumberType
                    && x < GridHeight - 1 && GridData[y, x + 1].NumberType == randomNumberType ||
                    x > 0 && GridData[y, x - 1].NumberType == randomNumberType
                    && x > 1 && GridData[y, x - 2].NumberType == randomNumberType ||
                    x < GridHeight - 1 && GridData[y, x + 1].NumberType == randomNumberType
                    && x < GridHeight - 2 && GridData[y, x + 2].NumberType == randomNumberType ||
                    //Check x axis matches
                    y > 0 && GridData[y - 1, x].NumberType == randomNumberType
                    && y < GridWidth - 1 && GridData[y + 1, x].NumberType == randomNumberType ||
                    y > 0 && GridData[y - 1, x].NumberType == randomNumberType
                    && y > 1 && GridData[y - 2, x].NumberType == randomNumberType ||
                    y < GridWidth - 1 && GridData[y + 1, x].NumberType == randomNumberType
                    && y < GridWidth - 2 && GridData[y + 2, x].NumberType == randomNumberType;
        }

        private void ReadDataFromJson(string data)
        {
            LevelData levelData = JsonUtility.FromJson<LevelData>(data);

            GridWidth = levelData.GridWidth;
            GridHeight = levelData.GridHeight;
            NumberDatas = levelData.NumberDatas;
            GoalDatas = levelData.GoalDatas;
            AvailableNumbers = levelData.AvailableNumbers;
            RandomNumbers = levelData.RandomNumbers;
            MoveCount = levelData.MoveCount;
        }

        public void AssignGridSize()
        {
            Grid.Rows = GridHeight;
            Grid.Cols = GridWidth;
            Grid.Cells = new Cell[GridWidth, GridHeight];
        }
    }
}
