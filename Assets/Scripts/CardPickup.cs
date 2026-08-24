using UnityEngine;

public class CardPickup : MonoBehaviour
{
    public ItemData Item;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        Debug.Log(
            "CARD PICKUP | " +
            Item.ItemName +
            " | TYPE: " +
            Item.Type +
            " | ID: " +
            Item.ID
        );

        bool added = InventoryManger.Instance.Add(Item);

        Debug.Log("CARD ADDED: " + added);

        if (added)
            Destroy(gameObject);
    }

}