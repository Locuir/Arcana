using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    public PlayerMovement playerMovement;

    private CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        if (playerMovement == null)
            playerMovement = FindObjectOfType<PlayerMovement>();

        HideCrosshair();
    }

    void Update()
    {
        if (playerMovement == null)
            return;

        bool show =
            playerMovement.IsBowEquipped() &&
            playerMovement.animator.GetBool("IsAiming");

        if (show)
            ShowCrosshair();
        else
            HideCrosshair();
    }

    void ShowCrosshair()
    {
        canvasGroup.alpha = 1f;
    }

    void HideCrosshair()
    {
        canvasGroup.alpha = 0f;
    }
}