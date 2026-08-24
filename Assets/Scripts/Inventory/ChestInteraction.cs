using UnityEngine;

public class ChestInteraction : MonoBehaviour
{
    public float interactionDistance = 3f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryOpenChest();
        }
    }

    private void TryOpenChest()
    {
        Chest[] chests =
            FindObjectsByType<Chest>(
                FindObjectsSortMode.None
            );

        float closestDistance = interactionDistance;
        Chest closestChest = null;

        foreach (Chest chest in chests)
        {
            float distance =
                Vector3.Distance(
                    transform.position,
                    chest.transform.position
                );

            if (distance <= closestDistance)
            {
                closestDistance = distance;
                closestChest = chest;
            }
        }

        if (closestChest == null)
            return;

        if (InventoryUI.Instance == null)
            return;

        if (ChestUI.Instance == null)
            return;

        InventoryUI.Instance.OpenInventoryWithChest(
            closestChest
        );
    }
}