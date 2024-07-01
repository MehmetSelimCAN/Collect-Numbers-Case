namespace Assets.Scripts.Game.Core.LevelBase
{
    public static class LevelDataFactory
    {
        public static LevelData CreateLevelData(string levelName)
        {
            LevelData levelData = new LevelData(levelName);
            levelData.Initialize();
            return levelData;
        }
    }
}
