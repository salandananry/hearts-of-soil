using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance { get; private set; }

    [SerializeField] private UIPanelTransition loadingScreen;
    [SerializeField] private Slider progressBar;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
    public void SceneChange(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine(sceneName));
    }
    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        loadingScreen.Open();

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        operation.allowSceneActivation = false;

        float displayedProgress = 0f;

        while (displayedProgress < 1f)
        {
            float targetProgress =
                Mathf.Clamp01(operation.progress / 0.9f);

            displayedProgress = Mathf.MoveTowards(
                displayedProgress,
                targetProgress,
                Time.unscaledDeltaTime * 0.8f
            );

            progressBar.value = displayedProgress;

            if (displayedProgress >= 0.99f &&
                operation.progress >= 0.9f)
            {
                break;
            }

            yield return null;
        }

        progressBar.value = 1f;

        operation.allowSceneActivation = true;

        yield return new WaitUntil(() => operation.isDone);

        loadingScreen.Close();
    }
}