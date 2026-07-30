using System.Collections.Generic;
using UnityEngine;

public class WeaponDamge : MonoBehaviour
{


    HashSet<EnemyStatus> Enemieshit = new HashSet<EnemyStatus>();


    void Start()
    {
        
    }

    public Collider WeaponHitBox;

    // Update is called once per frame
    void Update()
    {



        
    }



    public void EnableHitBox()
    {
        Enemieshit.Clear();
        WeaponHitBox.enabled = true;


    }
    public void DisableHitBox()
    {

        WeaponHitBox.enabled = false;


    }

    private void OnTriggerEnter(Collider other)
    {
            //Debug.Log("Touched: " + other.name);
            //Debug.Log("Tag = " + other.tag);

            if (!other.GetComponentInParent<EnemyStatus>())
            return;

            EnemyStatus enemy = other.GetComponentInParent<EnemyStatus>();

        if (enemy != null && !Enemieshit.Contains(enemy))
        {
            Enemieshit.Add(enemy);
            enemy.TakeDamage(10);

        }


    }

    }



