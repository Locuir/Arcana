using UnityEngine;
using UnityEngine.UI;

public class HealthBarSlider : MonoBehaviour
{

    public Slider HealthBar;
    public Slider DamagedTakenBar;
    public PlayerStatus Player;
    public float lerpSpeed = 0.05f;


    void Start()
    {
        HealthBar.maxValue = Player.MaxHealth;
        DamagedTakenBar.maxValue = Player.MaxHealth;

        HealthBar.value = Player.Health;
        DamagedTakenBar.value = Player.Health;

        Debug.Log(Player.Health);
    }

    // Update is called once per frame
    void Update()
    {

        if (HealthBar.value != Player.Health)
        {

            HealthBar.value = Player.Health;


        }

        if (HealthBar.value != DamagedTakenBar.value)
        {

            DamagedTakenBar.value = Mathf.Lerp(DamagedTakenBar.value , Player.Health , lerpSpeed );

        }

    }
}
