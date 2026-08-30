using UnityEngine;

public class DragonFireball : MonoBehaviour
{
    public float Speed = 30f;
    public int Damage = 20;
    public float LifeTime = 5f;
    public float TurnSpeed = 8f;

    private Transform target;
    private bool initialized;

    public void Initialize(Vector3 targetPosition)
    {
        GameObject playerObj = GameObject.Find("PlayerObj");

        if (playerObj != null)
            target = playerObj.transform;

        Vector3 direction =
            target != null
                ? target.position - transform.position
                : targetPosition - transform.position;

        if (direction.sqrMagnitude > 0.01f)
        {
            transform.rotation =
                Quaternion.LookRotation(direction.normalized);
        }

        initialized = true;

        Destroy(gameObject, LifeTime);
    }

    private void Update()
    {
        if (!initialized)
            return;

        if (target != null)
        {
            Vector3 direction =
                target.position - transform.position;

            if (direction.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation =
                    Quaternion.LookRotation(direction.normalized);

                transform.rotation =
                    Quaternion.Slerp(
                        transform.rotation,
                        targetRotation,
                        TurnSpeed * Time.deltaTime
                    );
            }
        }

        transform.position +=
            transform.forward * Speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerStatus player =
            other.GetComponentInParent<PlayerStatus>();

        if (player == null)
            return;

        player.TakeDamage(Damage);

        Destroy(gameObject);
    }
}