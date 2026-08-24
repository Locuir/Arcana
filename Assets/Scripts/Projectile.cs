using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 30f;
    public int damage;
    public float lifeTime = 5f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyStatus enemy = other.GetComponentInParent<EnemyStatus>();

        if (enemy == null)
            return;

        enemy.TakeDamage(damage);

        Destroy(gameObject);
    }
}