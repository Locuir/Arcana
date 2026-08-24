using UnityEngine;

public class WeaponSlashEffect : MonoBehaviour
{
    public ParticleSystem Slash;

    public void PlaySlash()
    {
        Slash.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        Slash.Play();
    }
}