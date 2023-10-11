using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class SoundManager : Singleton<SoundManager> {
    public AudioSource audioSource;
    public AudioClip[] soundClips;

    private void Start() {
    }


    public void PlaySound(int clipIndex) {
        if (clipIndex < 0 || clipIndex >= soundClips.Length) {
            Debug.LogWarning("Invalid clip index");
            return;
        }
        audioSource.PlayOneShot(soundClips[clipIndex]);

    }
}
