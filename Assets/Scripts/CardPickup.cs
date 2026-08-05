using UnityEngine;

public class CardPickup : MonoBehaviour
{
    public ItemData Item;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {


            if (InventoryManger.Instance.Add(Item))
            {
                Destroy(gameObject);
            }

        }
    }

}
