using UnityEngine;

public class StaminaSystem : MonoBehaviour
{
    public float maxStamina;
    public float currentStamina;

    public float sprintDrain = 15f;
    public float staminaRegen = 20f;

    public float sprintRecoveryPercent = 0.2f;

    private bool isRegenerating;
    private bool exhausted;

    public PlayerStats Stats;

    void Start()
    {
        maxStamina = Stats.Stamina;
        currentStamina = maxStamina;
    }

    void Update()
    {
        if (isRegenerating)
        {
            currentStamina += staminaRegen * Time.deltaTime;
            currentStamina = Mathf.Clamp(
                currentStamina,
                0f,
                maxStamina
            );
        }

        if (currentStamina <= 0f)
        {
            currentStamina = 0f;
            exhausted = true;
        }

        if (exhausted &&
            currentStamina >= maxStamina * sprintRecoveryPercent)
        {
            exhausted = false;
        }
    }

    public bool UseStamina(float amount)
    {
        if (currentStamina <= 0f)
            return false;

        currentStamina -= amount;

        currentStamina = Mathf.Clamp(
            currentStamina,
            0f,
            maxStamina
        );

        if (currentStamina <= 0f)
        {
            currentStamina = 0f;
            exhausted = true;
        }

        return true;
    }

    public bool CanSprint()
    {
        return !exhausted && currentStamina > 0f;
    }

    public void StartRegeneration()
    {
        isRegenerating = true;
    }

    public void StopRegeneration()
    {
        isRegenerating = false;
    }

    public bool HasStamina(float amount)
    {
        return currentStamina >= amount;
    }

    public bool IsExhausted()
    {
        return exhausted;
    }

    public float GetStaminaPercent()
    {
        return currentStamina / maxStamina;
    }
}