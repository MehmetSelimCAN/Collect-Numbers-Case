using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using Assets.Scripts.Extensions;

namespace Assets.Scripts.Game.Core.Managers
{
    public class SceneController : MonoSingleton<SceneController>
    {
        public bool loadingInProgress { get; private set; }

        public void LoadScene(string scene)
        {
            if (loadingInProgress)
                return;

            StartCoroutine(LoadSceneCo(scene));
        }

        private IEnumerator LoadSceneCo(string sceneName)
        {
            loadingInProgress = true;
            yield return new WaitForSeconds(2);

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.name.Contains("Game"))
                    yield return UnloadSceneCo(scene);
                else if (scene.name.Contains("Test"))
                {
                    sceneName = "Test";
                }
            }

            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            var loadedScene = SceneManager.GetSceneByName(sceneName);
            if (loadedScene.name.Contains("Game") || loadedScene.name.Contains("Test"))
            {
                SceneManager.SetActiveScene(loadedScene);
                yield return new WaitForSeconds(0.2f);
                EventManager.OnSceneLoaded.Invoke();
            }

            loadingInProgress = false;
        }

        public void UnloadScene(string sceneName)
        {
            Scene scene = SceneManager.GetSceneByName(sceneName);
            SceneManager.SetActiveScene(SceneManager.GetSceneAt(0));
            StartCoroutine(UnloadSceneCo(scene));
        }

        private IEnumerator UnloadSceneCo(Scene scene)
        {
            yield return SceneManager.UnloadSceneAsync(scene.buildIndex);
        }
    }
}
