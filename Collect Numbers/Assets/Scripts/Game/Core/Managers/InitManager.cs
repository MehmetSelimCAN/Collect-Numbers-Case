using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Game.Core.Managers
{
    public class InitManager : MonoBehaviour
    {
        [SerializeField] private SceneController sceneController;

        private void Start()
        {
            Invoke("LoadMenuScene", 0.5f);
        }

        public void LoadMenuScene()
        {
            StartCoroutine(LoadMenuSceneCo());
        }

        private IEnumerator LoadMenuSceneCo()
        {
            yield return SceneManager.LoadSceneAsync("UIScene", LoadSceneMode.Additive);
            sceneController.LoadScene("GameScene");
            Destroy(gameObject);
        }
    }
}
