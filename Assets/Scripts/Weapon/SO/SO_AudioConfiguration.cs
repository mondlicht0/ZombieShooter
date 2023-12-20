using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Audio Config", menuName = "Guns/Audio Config", order = 3)]
public class SO_AudioConfiguration : ScriptableObject
{
    [Range(0f, 1f)]
    public float Volume = 1f;
    public AudioClip[] FireClips;
    public AudioClip EmptyClip;
    public AudioClip ReloadClip;
    public AudioClip LastBulletClip;

    public void PlayShootingClip(AudioSource source, bool isLastBullet = false)
    {
        if (isLastBullet && LastBulletClip != null)
        {
            source.PlayOneShot(LastBulletClip, Volume);
        }

        else
        {
            source.PlayOneShot(FireClips[Random.Range(0, FireClips.Length)], Volume);
        }
    }

    public void PlayOutOfAmmoClip(AudioSource source)
    {
        if (EmptyClip != null)
        {
            source.PlayOneShot(EmptyClip, Volume);
        }
    }

    public void PlayReloadClip(AudioSource source)
    {
        if (ReloadClip != null)
        {
            source.PlayOneShot(ReloadClip, Volume);
        }
    }
}
