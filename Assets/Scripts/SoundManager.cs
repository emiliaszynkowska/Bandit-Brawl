using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip music;

    public AudioClip buttonHover;
    public AudioClip buttonClick;

    public AudioClip ring;
    public AudioClip complete;
    public AudioClip weaponSwing;
    public AudioClip jump;
    public AudioClip damage;
    public AudioClip blockedDamage;
    public AudioClip slam;
    public AudioClip block;
    public AudioClip death;
    public AudioClip win;
    public AudioClip fire;
    public AudioClip fight;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.clip = music;
    }

    public void PlayMusic()
    {
        audioSource.Play();
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }

    public void PlayFight()
    {
        audioSource.PlayOneShot(fight);
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    public void PlayButtonHover()
    {
        audioSource.PlayOneShot(buttonHover);
    }

    public void PlayButtonClick()
    {
        audioSource.PlayOneShot(buttonClick);
    }

    public void PlayRing()
    {
        audioSource.PlayOneShot(ring);
    }

    public void PlayWeaponSwing()
    {
        audioSource.PlayOneShot(weaponSwing);
    }

    public void PlayJump()
    {
        audioSource.PlayOneShot(jump);
    }

    public void PlayDamage()
    {
        audioSource.PlayOneShot(damage);
    }

    public void PlayBlockedDamage()
    {
        audioSource.PlayOneShot(blockedDamage);
    }

    public void PlaySlam()
    {
        audioSource.PlayOneShot(slam);
    }

    public void PlayBlock()
    {
        audioSource.PlayOneShot(block);
    }

    public void PlayDeath()
    {
        audioSource.PlayOneShot(death);
    }

    public void PlayWin()
    {
        audioSource.PlayOneShot(win);
    }

    public void PlayFire()
    {
        audioSource.PlayOneShot(fire);
    }
}
