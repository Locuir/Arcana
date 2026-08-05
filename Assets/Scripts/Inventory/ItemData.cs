using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public int ID;
    public string ItemName;
    public Sprite Icon;
    public string Description;
}