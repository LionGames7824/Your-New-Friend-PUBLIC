
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace YourNewFriend
{
    public class LoadingScene : MonoBehaviour
    {
        public GameObject LoadingScreen;

        public void LoadScene(int sceneId)
        {
            StartCoroutine(
                LoadSceneAsync(SceneManager.LoadSceneAsync(sceneId))
            );
        }

        public void LoadScene(string sceneName)
        {
            StartCoroutine(
                LoadSceneAsync(SceneManager.LoadSceneAsync(sceneName))
            );
        }

        private IEnumerator LoadSceneAsync(AsyncOperation operation)
        {
            LoadingScreen.SetActive(true);

            while (!operation.isDone)
            {
                yield return null;
            }
        }
    }
}
