using UnityEngine;

public class PlayerStatus : MonoBehaviour
{

    public float Health;
    public PlayerStats Stats;
    public int MaxHealth;
    bool IsDead;

    void Start()
    {
        MaxHealth = Stats.HP;
        Health = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void TakeDamage(int DamageTaken)
    {


        Health -= DamageTaken;
        CheckDeath();

    }


        void CheckDeath()
        {
            if (IsDead) return;

            if (Health <= 0)
            {
                IsDead = true;
                Debug.Log("Dead");

                
            }
        }

    


}
