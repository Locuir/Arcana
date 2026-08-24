using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource SFXSource;

    [Header("Bow")]
    public AudioClip BowLoad;
    public AudioClip BowRelease;
    [Header("Footsteps")]
    public AudioClip[] Footsteps;
    [Header("Sword")]
    public AudioClip[] SwordSlashes;
    [Header("Enemy Death")]
    public AudioClip[] EnemyDeathSounds;



    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    public void PlaySwordSlash()
    {
        if (SwordSlashes.Length == 0)
            return;

        AudioClip clip = SwordSlashes[Random.Range(0, SwordSlashes.Length)];
        SFXSource.PlayOneShot(clip);
    }
    public void PlayEnemyDeath()
    {
        if (EnemyDeathSounds.Length == 0)
            return;

        AudioClip clip = EnemyDeathSounds[Random.Range(0, EnemyDeathSounds.Length)];
        SFXSource.PlayOneShot(clip);
    }
    public void PlayBowLoad()
    {
        SFXSource.PlayOneShot(BowLoad);
    }

    public void PlayBowRelease()
    {
        SFXSource.PlayOneShot(BowRelease);
    }


    public void PlayFootstep()
    {
        if (Footsteps.Length == 0)
            return;

        AudioClip clip = Footsteps[Random.Range(0, Footsteps.Length)];
        SFXSource.PlayOneShot(clip);
    }
}