using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    public CanvasGroup GameOverTitleCanvasGroup;
    public CanvasGroup LevelCanvasGroup;
    public CanvasGroup WavesCanvasGroup;
    public CanvasGroup TimeCanvasGroup;
    public CanvasGroup RatingCanvasGroup;

    public TMP_Text LevelText;
    public TMP_Text WavesText;
    public TMP_Text TimeText;
    public TMP_Text RatingText;

    public float TitleFadeDuration = 1.5f;
    public float StatsDelay = 1f;
    public float StatFadeDuration = 0.5f;
    public float BetweenStatsDelay = 0.3f;

    private void Start()
    {
        LevelText.text = RunManager.FinalLevel.ToString();
        WavesText.text = RunManager.FinalWaves.ToString();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        int minutes = Mathf.FloorToInt(RunManager.FinalTime / 60f);
        int seconds = Mathf.FloorToInt(RunManager.FinalTime % 60f);

        TimeText.text = $"{minutes:00}:{seconds:00}";

        RatingText.text = CalculateRating(
            RunManager.FinalLevel,
            RunManager.FinalWaves,
            RunManager.FinalTime
        );

        GameOverTitleCanvasGroup.alpha = 0f;
        LevelCanvasGroup.alpha = 0f;
        WavesCanvasGroup.alpha = 0f;
        TimeCanvasGroup.alpha = 0f;
        RatingCanvasGroup.alpha = 0f;

        StartCoroutine(ShowSequence());
    }

    private IEnumerator ShowSequence()
    {
        yield return StartCoroutine(FadeIn(GameOverTitleCanvasGroup, TitleFadeDuration));

        yield return new WaitForSecondsRealtime(StatsDelay);

        yield return StartCoroutine(FadeIn(LevelCanvasGroup, StatFadeDuration));

        yield return new WaitForSecondsRealtime(BetweenStatsDelay);

        yield return StartCoroutine(FadeIn(WavesCanvasGroup, StatFadeDuration));

        yield return new WaitForSecondsRealtime(BetweenStatsDelay);

        yield return StartCoroutine(FadeIn(TimeCanvasGroup, StatFadeDuration));

        yield return new WaitForSecondsRealtime(BetweenStatsDelay);

        yield return StartCoroutine(FadeIn(RatingCanvasGroup, StatFadeDuration));
    }

    private IEnumerator FadeIn(CanvasGroup canvasGroup, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / duration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

    private string CalculateRating(int level, int waves, float time)
    {
        float score = 0f;

        score += Mathf.Clamp01(level / 20f) * 40f;
        score += Mathf.Clamp01(waves / 15f) * 40f;
        score += Mathf.Clamp01(time / 1200f) * 20f;

        if (score >= 90f)
            return "S";

        if (score >= 80f)
            return "A";

        if (score >= 70f)
            return "B";

        if (score >= 60f)
            return "C";

        if (score >= 40f)
            return "D";

        return "F";
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Gameplay");
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}