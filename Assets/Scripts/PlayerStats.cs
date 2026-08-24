using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    [Header("Level")]
    public int Level = 1;
    public int EXP = 0;
    public int RequiredEXP = 100;
    public int AvailableStatPoints = 0;

    [Header("Level Rewards")]
    public WeaponData Level3Weapon;
    public WeaponData Level5Weapon;

    [Header("Stats")]
    public int Vigor { get; private set; } = 1;
    public int Endurance { get; private set; } = 1;
    public int Strength { get; private set; } = 1;
    public int Dexterity { get; private set; } = 1;
    public int Luck { get; private set; } = 1;
    public int Faith { get; private set; } = 1;
    public int Vitality { get; private set; } = 1;

    [Header("Derived Stats")]
    public int MaxHP;
    public int CurrentHP;

    public int HP
    {
        get { return CurrentHP; }
        set { CurrentHP = value; }
    }

    public int AttackPower;
    public int Stamina;
    public int SkillPower;
    public int PhysicalDef;
    public float CritChacne;
    public int MagicDef;
    public float AttackSpeed;

    private bool IsDead;

    private void Awake()
    {
        CalculateDrivedStats();
        CurrentHP = MaxHP;
    }

    public void CalculateDrivedStats()
    {
        MaxHP = 50 + (Vigor * 15);

        AttackPower = 20 + (Strength * 5);
        SkillPower = 20 + (Faith * 5);
        PhysicalDef = 10 + (Vitality * 3);
        Stamina = 100 + (Endurance * 10);
        CritChacne = 5 + ((float)Dexterity * 0.5f) + ((float)Luck * 0.5f);
        AttackSpeed = 1 + (Dexterity * 0.01f);

        if (CurrentHP > MaxHP)
            CurrentHP = MaxHP;
    }

    public void TakeDamage(int damage)
    {
        if (IsDead)
            return;

        if (damage <= 0)
            return;

        damage -= PhysicalDef;

        if (damage < 1)
            damage = 1;

        CurrentHP -= damage;

        if (CurrentHP < 0)
            CurrentHP = 0;

        Debug.Log(
            "PLAYER DAMAGE: " +
            damage +
            " | HP: " +
            CurrentHP +
            "/" +
            MaxHP
        );

        if (CurrentHP <= 0)
            Die();
    }

    public void Heal(int amount)
    {
        if (IsDead)
            return;

        if (amount <= 0)
            return;

        CurrentHP += amount;

        if (CurrentHP > MaxHP)
            CurrentHP = MaxHP;
    }

    private void Die()
    {
        if (IsDead)
            return;

        IsDead = true;

        Debug.Log("PLAYER DIED");

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }

    public void AddEXP(int amount)
    {
        if (amount <= 0)
            return;

        EXP += amount;

        while (EXP >= RequiredEXP)
        {
            EXP -= RequiredEXP;
            LevelUp();
        }
    }

    public void AddCardXP(int amount)
    {
        AddEXP(amount);
    }

    private void LevelUp()
    {
        Level++;
        AvailableStatPoints++;

        RequiredEXP =
            Mathf.RoundToInt(RequiredEXP * 1.25f);

        CalculateDrivedStats();

        CurrentHP = MaxHP;

        Debug.Log("LEVEL UP! Level: " + Level);

        if (Level == 3)
            GiveLevelReward(Level3Weapon);

        if (Level == 5)
            GiveLevelReward(Level5Weapon);
    }

    private void GiveLevelReward(WeaponData weapon)
    {
        if (weapon == null)
            return;

        if (InventoryManger.Instance == null)
            return;

        bool added =
            InventoryManger.Instance.AddWeapon(weapon);

        if (added)
        {
            Debug.Log(
                "LEVEL REWARD → " +
                weapon.ItemName +
                " ADDED"
            );
        }
        else
        {
            Debug.LogWarning(
                "LEVEL REWARD → INVENTORY FULL"
            );
        }
    }

    public void IncreaseVigor()
    {
        Vigor++;
        CalculateDrivedStats();
    }

    public void IncreaseEndurance()
    {
        Endurance++;
        CalculateDrivedStats();
    }

    public void IncreaseStrength()
    {
        Strength++;
        CalculateDrivedStats();
    }

    public void IncreaseDexterity()
    {
        Dexterity++;
        CalculateDrivedStats();
    }

    public void IncreaseLuck()
    {
        Luck++;
        CalculateDrivedStats();
    }

    public void IncreaseFaith()
    {
        Faith++;
        CalculateDrivedStats();
    }

    public void IncreaseVitality()
    {
        Vitality++;
        CalculateDrivedStats();
    }
}