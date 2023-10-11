using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum BGM {
    Ground = 0,
    Underground = 1,
    Starman = 2,
}

public class MusicManager : Singleton<MusicManager>{
    public AudioSource audioSource;
    public AudioClip[] musicClips;
    private int currentClipIndex = -1;

    private void Start() {
        // 设置初始音乐
        //GameManager.Instance.gameStart.AddListener(GameStart);
        GameManager.Instance.gamePause.AddListener(PauseMusic);
        GameManager.Instance.gameResume.AddListener(ResumeMusic);
        GameManager.Instance.changeLevel.AddListener(OnLevel);
    }
    public void OnLevel(int level) {
        PlayMusic(level);
    }
    public void PlayMusic(int clipIndex) {
        if (clipIndex < 0 || clipIndex >= musicClips.Length) {
            Debug.LogWarning("Invalid clip index");
            return;
        }
        if (currentClipIndex == clipIndex) {   
            return;
        }
        if (clipIndex == 2) {
            int tmp = currentClipIndex;
            StartCoroutine(ToggleAfterDelay(Constants.StarmanTime, tmp));
        }
        audioSource.Stop();
        currentClipIndex = clipIndex;
        audioSource.clip = musicClips[currentClipIndex];
        audioSource.Play();
    }

    public void PauseMusic() {
        if (audioSource != null && audioSource.isPlaying) {
            audioSource.Pause();
        }
    }

    public void ResumeMusic() {
        if (audioSource != null && !audioSource.isPlaying) {
            audioSource.UnPause();
        }
    }

    private IEnumerator ToggleAfterDelay(float delay,int newone) {
        yield return new WaitForSeconds(delay);
        audioSource.Stop();
        currentClipIndex = newone;
        audioSource.clip = musicClips[newone];
        audioSource.Play();
        Debug.Log("Music" + (BGM)newone);
    }
 
}
