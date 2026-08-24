using UnityEngine;

public class LevelRewardSystem : MonoBehaviour
{
    public static LevelRewardSystem Instance;

    [System.Serializable]
    public class LevelReward
    {
        public int Level;
        public WeaponData Weapon;
    }

    public LevelReward[] Rewards;

    private void Awake()
    {
        Instance = this;
    }

    public void GiveReward(int level)
    {
        foreach (LevelReward reward in Rewards)
        {
            if (reward.Level != level)
                continue;

            if (reward.Weapon == null)
                continue;

            bool added =
                InventoryManger.Instance.AddWeapon(
                    reward.Weapon
                );

            if (added)
            {
                Debug.Log(
                    "LEVEL REWARD → " +
                    reward.Weapon.ItemName +
                    " | LEVEL " +
                    level
                );
            }
            else
            {
                Debug.LogWarning(
                    "LEVEL REWARD → INVENTORY FULL"
                );
            }
        }
    }
}