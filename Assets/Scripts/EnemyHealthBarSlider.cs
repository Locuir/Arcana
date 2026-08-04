using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarSlider : MonoBehaviour
{

    public Slider HealthBar;
    public Slider DamagedTakenBar;
    public EnemyStatus Enemy;
    public float lerpSpeed = 0.05f;


    void Start()
    {
        HealthBar.maxValue = Enemy.MaxHealth;
        DamagedTakenBar.maxValue = Enemy.MaxHealth;

        HealthBar.value = Enemy.Health;
        DamagedTakenBar.value = Enemy.Health;

        Debug.Log(Enemy.Health);
    }

    // Update is called once per frame
    void Update()
    {

        if (HealthBar.value != Enemy.Health)
        {

            HealthBar.value = Enemy.Health;


        }

        if (HealthBar.value != DamagedTakenBar.value)
        {

            DamagedTakenBar.value = Mathf.Lerp(DamagedTakenBar.value, Enemy.Health, lerpSpeed);

        }

    }
}
