using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject InventoryPanel;
    public PlayerMovement PlayerMovement;
    public WeaponAttack WeaponAttack;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            bool isOpen = !InventoryPanel.activeSelf;
            InventoryPanel.SetActive(isOpen);

            if (isOpen)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                PlayerMovement.enabled  = false;
                WeaponAttack.enabled = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                PlayerMovement.enabled = true;
                WeaponAttack.enabled = true;
            }
        }
    }
}