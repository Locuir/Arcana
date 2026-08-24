using UnityEngine;

public class WeaponToolbar : MonoBehaviour
{
    [Header("Slots")]
    public WeaponToolbarSlot[] slots;

    [Header("Weapon")]
    public WeaponManager weaponManager;

    private int currentSlot = -1;

    private void Start()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
                continue;

            slots[i].SlotIndex = i;
            slots[i].SetSelected(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SelectSlot(0);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            SelectSlot(1);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            SelectSlot(2);
    }

    public void SelectSlot(int index)
    {
        if (slots == null)
            return;

        if (index < 0 || index >= slots.Length)
            return;

        if (slots[index] == null)
            return;

        WeaponData weapon =
            slots[index].GetWeapon();

        if (weapon == null)
            return;

        if (weaponManager == null)
            return;

        currentSlot = index;

        UpdateSelection();

        weaponManager.EquipWeapon(weapon);
    }

    private void UpdateSelection()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
                continue;

            slots[i].SetSelected(i == currentSlot);
        }
    }

    public void RefreshSelection()
    {
        UpdateSelection();
    }

    public bool IsWeaponAlreadyEquipped(WeaponData weapon)
    {
        if (weapon == null)
            return false;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
                continue;

            if (slots[i].GetWeapon() == weapon)
                return true;
        }

        return false;
    }

    public bool TryAddWeaponToInventory(WeaponData weapon)
    {
        if (weapon == null)
            return false;

        if (InventoryManger.Instance == null)
            return false;

        return InventoryManger.Instance.Add(weapon);
    }

    public WeaponData GetCurrentWeapon()
    {
        if (currentSlot < 0 ||
            currentSlot >= slots.Length)
            return null;

        if (slots[currentSlot] == null)
            return null;

        return slots[currentSlot].GetWeapon();
    }
}