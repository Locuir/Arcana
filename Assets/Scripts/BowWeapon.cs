using UnityEngine;

public class BowWeapon : MonoBehaviour
{
    public Transform projectileSpawnPoint;
    public GameObject projectilePrefab;
    public WeaponData weaponData;

    public float aimDistance = 1000f;
    public LayerMask aimLayers = ~0;

    public void Fire()
    {
        if (projectilePrefab == null)
            return;

        if (projectileSpawnPoint == null)
            return;

        if (weaponData == null)
            return;

        Camera cam = Camera.main;

        if (cam == null)
            return;

        Ray ray = cam.ViewportPointToRay(
            new Vector3(0.5f, 0.5f, 0f)
        );

        Vector3 targetPoint;

        if (Physics.Raycast(
            ray,
            out RaycastHit hit,
            aimDistance,
            aimLayers,
            QueryTriggerInteraction.Ignore))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint =
                ray.origin +
                ray.direction * aimDistance;
        }

        Vector3 direction =
            targetPoint -
            projectileSpawnPoint.position;

        if (direction.sqrMagnitude <= 0.001f)
            return;

        Quaternion projectileRotation =
            Quaternion.LookRotation(direction.normalized);

        GameObject projectile = Instantiate(
            projectilePrefab,
            projectileSpawnPoint.position,
            projectileRotation
        );

        Projectile projectileScript =
            projectile.GetComponent<Projectile>();

        if (projectileScript != null)
        {
            projectileScript.damage =
                weaponData.damage;
        }
    }
}