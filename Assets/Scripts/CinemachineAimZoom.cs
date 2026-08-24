using UnityEngine;
using Unity.Cinemachine;

public class CinemachineAimZoom : MonoBehaviour
{
    public float normalFOV = 60f;
    public float aimFOV = 50f;
    public float zoomSpeed = 8f;

    private CinemachineCamera cinemachineCamera;
    private PlayerMovement playerMovement;

    void Start()
    {
        cinemachineCamera = GetComponent<CinemachineCamera>();
        playerMovement = FindObjectOfType<PlayerMovement>();

        if (cinemachineCamera != null)
        {
            cinemachineCamera.Lens.FieldOfView = normalFOV;
        }
    }

    void Update()
    {
        if (cinemachineCamera == null || playerMovement == null)
            return;

        bool aiming =
            playerMovement.animator.GetBool("IsAiming");

        float targetFOV =
            aiming ? aimFOV : normalFOV;

        cinemachineCamera.Lens.FieldOfView =
            Mathf.Lerp(
                cinemachineCamera.Lens.FieldOfView,
                targetFOV,
                zoomSpeed * Time.deltaTime
            );
    }
}