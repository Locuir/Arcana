using UnityEngine;

public class RunManager : MonoBehaviour
{
    public static RunManager Instance;

    public float SurvivalTime { get; private set; }
    public int WavesSurvived { get; private set; }
    public PlayerStats stats;
    private bool runActive = true;


    public static int FinalLevel;
    public static int FinalWaves;
    public static float FinalTime;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if (!runActive)
            return;

        SurvivalTime += Time.deltaTime;
    }

    public void WaveCompleted()
    {
        WavesSurvived++;
    }

    public void EndRun()
    {
        if (!runActive)
            return;

        runActive = false;

        FinalLevel = stats.Level;
        FinalWaves = WavesSurvived;
        FinalTime = SurvivalTime;

        UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver");
    }
}