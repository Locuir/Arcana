using TMPro;
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
    public WeaponData Level10Weapon;
    public WeaponData Level15Weapon;

    [Header("Starting Weapon")]
    public WeaponData StartingWeapon;

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

    public TMP_Text LevelText;

    private bool IsDead;

    private int TemporaryStrengthBonus;
    private int TemporaryVigorBonus;

    public float DamageMultiplier { get; private set; } = 1f;

    private void Awake()
    {
        CalculateDrivedStats();
        CurrentHP = MaxHP;

        if (StartingWeapon != null && InventoryManger.Instance != null)
        {
            InventoryManger.Instance.AddWeapon(StartingWeapon);
        }
    }
    public void SetDamageMultiplier(float multiplier)
    {
        DamageMultiplier = multiplier;
    }

    public void ResetDamageMultiplier()
    {
        DamageMultiplier = 1f;
    }
    public void CalculateDrivedStats()
    {
        MaxHP =
            50 +
            ((Vigor + TemporaryVigorBonus) * 15);

        AttackPower =
            20 +
            ((Strength + TemporaryStrengthBonus) * 5);

        SkillPower =
            20 +
            (Faith * 5);

        PhysicalDef =
            10 +
            (Vitality * 3);

        Stamina =
            100 +
            (Endurance * 10);

        CritChacne =
            5 +
            ((float)Dexterity * 0.5f) +
            ((float)Luck * 0.5f);

        AttackSpeed =
            1 +
            (Dexterity * 0.01f);

        if (CurrentHP > MaxHP)
            CurrentHP = MaxHP;
    }

    public void ActivateTemporaryStats(
        int strengthBonus,
        int vigorBonus
    )
    {
        int oldMaxHP = MaxHP;

        TemporaryStrengthBonus = strengthBonus;
        TemporaryVigorBonus = vigorBonus;

        CalculateDrivedStats();

        int hpIncrease = MaxHP - oldMaxHP;

        CurrentHP += hpIncrease;

        if (CurrentHP > MaxHP)
            CurrentHP = MaxHP;
    }

    public void RemoveTemporaryStats()
    {
        TemporaryStrengthBonus = 0;
        TemporaryVigorBonus = 0;

        CalculateDrivedStats();

        if (CurrentHP > MaxHP)
            CurrentHP = MaxHP;
    }

    public void LoseHealth(int amount)
    {
        if (IsDead)
            return;

        if (amount <= 0)
            return;

        CurrentHP -= amount;

        if (CurrentHP < 0)
            CurrentHP = 0;
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

        NotificationManager.Instance.Show(
            "LEVEL UP!",
            "You reached Level " + Level
        );

        AvailableStatPoints++;

        LevelText.text =
            Level.ToString();

        RequiredEXP =
            Mathf.RoundToInt(
                RequiredEXP * 1.25f
            );

        CalculateDrivedStats();

        CurrentHP = MaxHP;

        Debug.Log(
            "LEVEL UP! Level: " +
            Level
        );

        if (Level == 3)
            GiveLevelReward(Level3Weapon);

        if (Level == 5)
            GiveLevelReward(Level5Weapon);

        if (Level == 10)
            GiveLevelReward(Level10Weapon);

        if (Level == 15)
            GiveLevelReward(Level15Weapon);
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