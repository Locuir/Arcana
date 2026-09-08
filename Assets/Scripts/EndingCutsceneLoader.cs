using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class EndingCutsceneLoader : MonoBehaviour
{
    [SerializeField] private PlayableDirector director;
    [SerializeField] private string mainMenuScene = "MainMenu";

    private void Start()
    {
        director.stopped += OnCutsceneFinished;
    }

    private void OnCutsceneFinished(PlayableDirector playableDirector)
    {
        SceneManager.LoadScene(mainMenuScene);
    }

    private void OnDestroy()
    {
        if (director != null)
            director.stopped -= OnCutsceneFinished;
    }
}