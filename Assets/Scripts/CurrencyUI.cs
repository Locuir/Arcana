using TMPro;
using UnityEngine;

public class CurrencyUI : MonoBehaviour
{
    public CurrencySystem currencySystem;
    public TMP_Text essenceText;

    void Start()
    {
        UpdateUI();
    }

    void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        essenceText.text = currencySystem.Essence.ToString();
    }
}