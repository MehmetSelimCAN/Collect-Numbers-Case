using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using Assets.Scripts.Game.Core.GoalBase;
using Assets.Scripts.Game.Core.LevelBase;
using Assets.Scripts.Game.EditorBase;
using Assets.Scripts.SO;
using Assets.Scripts.Game.Core.BoardBase;
using Assets.Scripts.Game.Core.Enums;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
namespace Assets.Scripts.Editor
{
    public class LevelEditor : EditorWindow
    {
        private string _levelName = string.Empty;

        private int _gridWidth;
        private int _gridHeight;

        private float _numberWidth = 1f;
        private float _numberHeight = 1f;

        private int _goalCount;
        private float goalsYPosition = 3.5f;
        private float goalsXOffset = -1.5f;
        private int[] goalCountsArray = new[] { 0, 0, 0, 0, 0 };

        private int _moveCount;


        private Transform _numbersParent;
        public EditorNumber NumberPrefab;

        public EditorGoal GoalPrefab;
        private Transform _goalsParent;

        public NumberListSO EditorNumbers;

        private NumberListSO _availableNumbersSO;
        private NumberListSO _randomNumbersSO;

        private List<NumberData> _availableNumbersList = new List<NumberData>();
        private List<NumberData> _randomNumbersList = new List<NumberData>();

        [SerializeField] private NumberSO _randomNumberSO;
        private NumberSO _currentNumberSO;

        private EditorNumber[] _numbers;
        private EditorGoal[] _goals;

        private static int selectedNumberTypeIndex;
        private static string[] numberTypeTexts;
        private static int numberTypeGridHorizontalLength = 3;

        private bool _mouseLeftDown;

        private const string SavePath = @"Resources/Levels/";
        private const string Extension = @"json";

        private void OnEnable()
        {
            SceneView.duringSceneGui += OnSceneGUI;

            selectedNumberTypeIndex = EditorNumbers.numbers.list.IndexOf(_randomNumberSO);
            numberTypeTexts = new string[EditorNumbers.numbers.list.Count];
            for (int i = 0; i < numberTypeTexts.Length; i++)
            {
                numberTypeTexts[i] = EditorNumbers.numbers.list[i].NumberData.NumberType.ToString();
            }

            _currentNumberSO = EditorNumbers.numbers.list.Find(x => x == _randomNumberSO);
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
            ClearGrid();
            ClearGoals();
        }

        [MenuItem("Editor/Level Editor")]
        public static void Initialize()
        {
            GetWindow<LevelEditor>("Level Editor");
        }

        private void OnGUI()
        {
            GUILayout.Label("Grid Size");
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("X", GUILayout.Width(20));
            EditorGUI.BeginChangeCheck();
            _gridWidth = EditorGUILayout.IntField(_gridWidth, GUILayout.Width(30));
            EditorGUILayout.LabelField("Y", GUILayout.Width(20));
            _gridHeight = EditorGUILayout.IntField(_gridHeight, GUILayout.Width(30));

            EditorGUILayout.EndHorizontal();

            if (EditorGUI.EndChangeCheck())
            {
                CreateGrid();
            }

            EditorGUI.BeginChangeCheck();

            GUILayout.Label("Goal Count");
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Count", GUILayout.Width(40));
            _goalCount = EditorGUILayout.IntField(_goalCount, GUILayout.Width(30));

            EditorGUILayout.EndHorizontal();

            if (EditorGUI.EndChangeCheck())
            {
                CreateGoals();
            }

            EditorGUI.BeginChangeCheck();

            GUILayout.Label("Goals Counts");
            EditorGUILayout.BeginHorizontal();

            for (int i = 0; i < _goalCount; i++)
            {
                EditorGUILayout.LabelField("Goal " + (i + 1), GUILayout.Width(40));
                goalCountsArray[i] = EditorGUILayout.IntField(goalCountsArray[i], GUILayout.Width(30));
            }

            EditorGUILayout.EndHorizontal();

            if (EditorGUI.EndChangeCheck())
            {
                UpdateGoalCounts();
            }


            GUILayout.Label("Move Count");
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Count", GUILayout.Width(40));
            _moveCount = EditorGUILayout.IntField(_moveCount, GUILayout.Width(30));

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Fill Number Types", GUILayout.Width(150));
            _availableNumbersSO = EditorGUILayout.ObjectField(_availableNumbersSO, typeof(NumberListSO), false, GUILayout.Height(EditorGUIUtility.singleLineHeight)) as NumberListSO;

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Random Number Types", GUILayout.Width(150));
            _randomNumbersSO = EditorGUILayout.ObjectField(_randomNumbersSO, typeof(NumberListSO), false, GUILayout.Height(EditorGUIUtility.singleLineHeight)) as NumberListSO;

            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.Label("Number Type");
            GUILayout.Space(5);

            selectedNumberTypeIndex = GUILayout.SelectionGrid(selectedNumberTypeIndex, numberTypeTexts, numberTypeGridHorizontalLength);
            _currentNumberSO = EditorNumbers.numbers.list[selectedNumberTypeIndex];

            GUILayout.Space(15);
            GUILayout.Label("Level Name");

            _levelName = GUILayout.TextField(_levelName);
            if (!string.IsNullOrEmpty(_levelName))
            {
                GUILayout.Label("Will be saved as: " + _levelName + "." + Extension);
            }

            GUILayout.Space(15);
            if (GUILayout.Button("Save"))
            {
                if (_gridWidth <= 0 || _gridHeight <= 0)
                {
                    EditorUtility.DisplayDialog("Save", "you must give grid size in correct range. [1, 10]", "OK");
                    return;
                }
                else
                {
                    Save(_levelName);
                }
            }
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
            Event currentEvent = Event.current;

            switch (currentEvent.type)
            {
                case EventType.MouseDown:
                    {
                        if (currentEvent.button == 0)
                        {
                            _mouseLeftDown = true;
                            currentEvent.Use();

                        }

                        break;
                    }

                case EventType.MouseUp:
                    {
                        if (currentEvent.button == 0)
                        {
                            _mouseLeftDown = false;
                            currentEvent.Use();

                        }

                        break;
                    }
            }

            if (_mouseLeftDown)
            {
                Ray ray = HandleUtility.GUIPointToWorldRay(currentEvent.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hitInfo))
                {
                    if (hitInfo.collider.TryGetComponent(out EditorNumber numberUnderMouse))
                    {
                        numberUnderMouse.ChangeNumberData(_currentNumberSO.NumberData);
                    }

                    if (hitInfo.collider.TryGetComponent(out EditorGoal goalUnderMouse))
                    {
                        goalUnderMouse.NumberType = _currentNumberSO.NumberData.NumberType;
                        goalUnderMouse.ChangeGoalImage(_currentNumberSO.NumberData.NumberType);
                    }
                }
            }
        }

        private void Save(string levelName)
        {
            if (string.IsNullOrEmpty(_levelName))
            {
                EditorUtility.DisplayDialog("Save", "You must give your level a name to save it", "OK");
                return;
            }

            LevelData LevelData = new LevelData(levelName);
            LevelData.GridWidth = _gridWidth;
            LevelData.GridHeight = _gridHeight;
            LevelData.MoveCount = _moveCount;

            _numbers = (EditorNumber[])FindObjectsByType(typeof(EditorNumber), FindObjectsSortMode.InstanceID);
            _goals = (EditorGoal[])FindObjectsByType(typeof(EditorGoal), FindObjectsSortMode.InstanceID);

            foreach (var number in _numbers)
            {
                CellData numberData = new CellData();
                numberData.Position = number.transform.position;
                numberData.NumberData = number.NumberData;
                LevelData.NumberDatas.Add(numberData);
            }

            foreach (var goal in _goals)
            {
                GoalData goalData = new GoalData();
                goalData.NumberType = goal.NumberType;
                goalData.GoalCount = goal.GoalCount;
                LevelData.GoalDatas.Add(goalData);
            }

            foreach (NumberSO numberSO in _availableNumbersSO.numbers.list)
            {
                _availableNumbersList.Add(numberSO.NumberData);
            }

            foreach (NumberSO numberSO in _randomNumbersSO.numbers.list)
            {
                _randomNumbersList.Add(numberSO.NumberData);
            }

            LevelData.AvailableNumbers = _availableNumbersList;
            LevelData.RandomNumbers = _randomNumbersList;

            var data = JsonUtility.ToJson(LevelData);
            string path = $@"{Application.dataPath}/{SavePath}" + levelName + "." + Extension;

            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(data);
                }
            }

            AssetDatabase.Refresh();
        }

        public string ReadDataFromText(string path)
        {
            string data = null;
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(fs))
                    {
                        data = reader.ReadToEnd();
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }

            return data;
        }

        public void ClearGrid()
        {
            DestroyImmediate(GameObject.Find("Numbers Parent"));
            _numbers = FindObjectsOfType<EditorNumber>();

            for (int i = 0; i < _numbers.Length; i++)
            {
                DestroyImmediate(_numbers[i].gameObject);
            }

            _currentNumberSO = EditorNumbers.numbers.list.Find(x => x == _randomNumberSO);
        }

        public void ClearGoals()
        {
            DestroyImmediate(GameObject.Find("Goals Parent"));
            _goals = FindObjectsOfType<EditorGoal>();
            for (int i = 0; i < _goals.Length; i++)
            {
                DestroyImmediate(_goals[i].gameObject);
            }
        }

        private void CreateGrid()
        {
            ClearGrid();
            _numbersParent = new GameObject("Numbers Parent").transform;

            for (int j = _gridHeight - 1; j >= 0; j--)
            {
                for (int i = _gridWidth - 1; i >= 0; i--)
                {
                    CreateNumber(new Vector2(i * _numberWidth,
                                            j * _numberHeight),
                                            _currentNumberSO.NumberData);
                }
            }

            SetPosition(_gridWidth, _gridHeight);
        }

        private void CreateGoals()
        {
            _currentNumberSO = EditorNumbers.numbers.list.Find(x => x == _randomNumberSO);
            ClearGoals();
            _goalsParent = new GameObject("Goals Parent").transform;
            for (int i = 0; i < _goalCount; i++)
            {
                var goalsCount = goalCountsArray[i];
                CreateGoal(new Vector2(i + goalsXOffset,
                                        goalsYPosition),
                                        _currentNumberSO.NumberData.NumberType, goalsCount);
            }
            SetPosition(_gridWidth, _gridHeight);
        }

        public void CreateNumber(Vector2 position, NumberData TestNumber)
        {
            var number = Instantiate(NumberPrefab, position, Quaternion.identity, _numbersParent);
            number.ChangeNumberData(TestNumber);
        }

        public void CreateGoal(Vector2 position, NumberType numberType, int goalCount)
        {
            var goal = Instantiate(GoalPrefab, position, Quaternion.identity, _goalsParent);

            goal.GoalCount = goalCount;
            goal.NumberType = numberType;

            goal.ChangeGoalImage(numberType);
            goal.ChangeGoalCountText(goalCount);
        }

        public void UpdateGoalCounts()
        {
            _goals = (EditorGoal[])FindObjectsByType(typeof(EditorGoal), FindObjectsSortMode.InstanceID);
            for (int i = _goals.Length - 1, j = 0; i >= 0; i--, j++)
            {
                _goals[i].GoalCount = goalCountsArray[j];
                _goals[i].ChangeGoalCountText(goalCountsArray[j]);
            }
        }

        private void SetPosition(int width, int height)
        {
            float newWidth = width % 2 == 0 ? (width / 2) - 0.5f : (width / 2);
            float newHeight = height % 2 == 0 ? (height / 2) - 0.5f : (height / 2);

            if (_numbersParent != null)
                _numbersParent.transform.position = new Vector3(-newWidth * _numberWidth,
                                                             -newHeight * _numberHeight,
                                                             +10);
        }
    }
}

#endif
