using UnityEngine;
using UnityEngine.UI;

public class StaminaBarSlider : MonoBehaviour
{
    public Slider StaminaBar;
    public Slider DamagedTakenBar;
    public StaminaSystem Stamina;
    public float lerpSpeed = 0.05f;

    void Start()
    {
        StaminaBar.maxValue = Stamina.maxStamina;
        DamagedTakenBar.maxValue = Stamina.maxStamina;

        StaminaBar.value = Stamina.currentStamina;
        DamagedTakenBar.value = Stamina.currentStamina;
    }

    void Update()
    {
        if (StaminaBar.value != Stamina.currentStamina)
        {
            StaminaBar.value = Stamina.currentStamina;
        }

        if (StaminaBar.value != DamagedTakenBar.value)
        {
            DamagedTakenBar.value = Mathf.Lerp(
                DamagedTakenBar.value,
                Stamina.currentStamina,
                lerpSpeed
            );
        }
    }
}