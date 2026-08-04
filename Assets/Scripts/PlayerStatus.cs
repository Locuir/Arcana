using UnityEngine;

public class PlayerStatus : MonoBehaviour
{

    public float Health;
    public float MaxHealth = 100;
    bool IsDead;

    void Start()
    {
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
