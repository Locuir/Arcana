using UnityEngine;

public class CheatManager : MonoBehaviour
{
    public ItemData Wolf;
    public ItemData Slime;
    public ItemData Goblin;
    public ItemData Skeleton;

    public WeaponData Sword;
    public WeaponData Axe;

    public CurrencySystem currencySystem;

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    AddItem(Wolf);
        //}
        //else if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    AddItem(Slime);
        //}
        //else if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    AddItem(Goblin);
        //}
         if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            AddItem(Skeleton);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            AddItem(Sword);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            AddItem(Axe);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            currencySystem.AddEssence(100);
            Debug.Log("100 Added");
        }
    }

    private void AddItem(ItemData item)
    {
        if (item == null)
        {
            Debug.Log("Item is not assigned!");
            return;
        }

        Debug.Log("Adding item: " + item.ItemName);
        InventoryManger.Instance.Add(item);
    }
}