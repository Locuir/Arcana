using UnityEngine;

public class EnemyStatus : MonoBehaviour
{

    public int Health = 30;
    public int MaxHealth = 30;
    


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void TakeDamage(int DamageTaken)
    {
        Health -= DamageTaken;
        Debug.Log("Dagame Taken");
        Debug.Log($"Health = {Health}");
        CheckDeath();



    }

    void CheckDeath()
    {

        if (Health <= 0)
        {
            Debug.Log("Dead");


        }

    }

}
