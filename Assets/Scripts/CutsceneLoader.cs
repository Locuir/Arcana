using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CutsceneLoader : MonoBehaviour
{
    [SerializeField] private PlayableDirector director;
    [SerializeField] private string gameSceneName;

    private void Start()
    {
        director.stopped += OnCutsceneFinished;
    }

    private void OnCutsceneFinished(PlayableDirector playableDirector)
    {
        StartCoroutine(LoadGameScene());
    }

    private System.Collections.IEnumerator LoadGameScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(gameSceneName);

        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f)
        {
            yield return null;
        }

        operation.allowSceneActivation = true;
    }
}