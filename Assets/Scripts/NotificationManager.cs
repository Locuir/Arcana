using System.Collections;
using UnityEngine;
using TMPro;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance;

    public CanvasGroup NotificationCanvasGroup;
    public TMP_Text HeadingText;
    public TMP_Text BodyText;
    public AudioSource NotificationAudio;
    public AudioClip NotificationSound;

    public float FadeInDuration = 0.25f;
    public float DisplayDuration = 2f;
    public float FadeOutDuration = 0.4f;

    private Coroutine notificationCoroutine;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        NotificationCanvasGroup.alpha = 0f;
    }

    public void Show(string heading, string text)
    {
        if (notificationCoroutine != null)
            StopCoroutine(notificationCoroutine);

        notificationCoroutine = StartCoroutine(ShowNotification(heading, text));
    }

    private IEnumerator ShowNotification(string heading, string text)
    {
        HeadingText.text = heading;
        BodyText.text = text;

        NotificationAudio.PlayOneShot(NotificationSound);

        yield return StartCoroutine(Fade(0f, 1f, FadeInDuration));

        yield return new WaitForSeconds(DisplayDuration);

        yield return StartCoroutine(Fade(1f, 0f, FadeOutDuration));

        notificationCoroutine = null;
    }

    private IEnumerator Fade(float start, float end, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            NotificationCanvasGroup.alpha =
                Mathf.Lerp(start, end, elapsed / duration);

            yield return null;
        }

        NotificationCanvasGroup.alpha = end;
    }
}