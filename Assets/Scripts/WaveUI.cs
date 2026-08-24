using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveUI : MonoBehaviour
{
    public Slider PhaseBar;
    public TMP_Text PhaseText;
    public TMP_Text WaveText;

    public Sprite KillBarSprite;
    public Sprite PrepareBarSprite;

    public Image PhaseBarFill;

    private void Update()
    {
        if (WaveManager.Instance == null)
            return;

        UpdateWave();
        UpdatePhase();
    }

    void UpdateWave()
    {
        WaveText.text =
            "WAVE " +
            WaveManager.Instance.currentWave;
    }

    void UpdatePhase()
    {
        WaveManager manager =
            WaveManager.Instance;

        if (manager.currentPhase ==
            WaveManager.WavePhase.KillMonsters)
        {
            PhaseText.text =
                "KILL THE MONSTERS";

            if (manager.MaxEnemies > 0)
            {
                PhaseBarFill.fillAmount =
                    (float)manager.CurrentEnemies /
                    manager.MaxEnemies;
            }

            PhaseBarFill.sprite =
                KillBarSprite;
        }
        else if (manager.currentPhase ==
                 WaveManager.WavePhase.PrepareLoadout)
        {
            PhaseText.text =
                "PREPARE YOUR LOADOUT";

            if (manager.prepareTime > 0)
            {
                PhaseBarFill.fillAmount =
                    manager.CurrentPhaseTime /
                    manager.prepareTime;
            }

            PhaseBarFill.sprite =
                PrepareBarSprite;
        }
    }
}