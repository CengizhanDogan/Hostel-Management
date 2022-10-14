using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip moneySound;
    public AudioClip MoneySound => moneySound;

    [SerializeField] private AudioSource purchaseSound;

    [SerializeField] private AudioSource upgradeSound;

    [SerializeField] private AudioSource doorSound;

    public void PlaySound(AudioClip clip, float volume)
    {
        if (audioSource.isPlaying) return;

        audioSource.volume = volume;
        audioSource.PlayOneShot(clip);
    }
    public void DoorSound()
    {
        if (doorSound.isPlaying) return;

        doorSound.Play();
    }
    public void UpgradeSound()
    {
        if (upgradeSound.isPlaying) return;

        upgradeSound.Play();
    }
    public void PurchaseSound()
    {
        if (purchaseSound.isPlaying) return;

        purchaseSound.Play();
    }
}
