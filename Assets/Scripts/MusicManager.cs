using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum BGM {

}

public class MusicManager: MonoBehaviour{
    public AudioSource audioSource;
    public AudioClip[] musicClips;
    public AudioClip starmanClip;
    public int currentClipIndex = 0;
    bool isStarman = false;

    private void Start() {
        // 设置初始音乐
        //GameManager.Instance.gameStart.AddListener(GameStart);
        //GameManager.Instance.gamePause.AddListener(PauseMusic);
        //GameManager.Instance.gameResume.AddListener(ResumeMusic);
        //GameManager.Instance.changeLevel.AddListener(OnLevel);
    }
    public void SetCurrentClipIndex(int index) {
        if (index < 0 || index >= musicClips.Length) {
            Debug.LogError("MusicManager: SetCurrentClipIndex: index out of range");
            return;
        }
        currentClipIndex = index;
    }

    public void PlayMusic() {
        if (currentClipIndex < 0 || currentClipIndex >= musicClips.Length) {
            Debug.LogError("MusicManager: PlayMusic: currentClipIndex out of range");
            return;
        }
        if (audioSource == null) {
            Debug.LogError("MusicManager: PlayMusic: audioSource is null");
            return;
        }
        audioSource.clip = musicClips[currentClipIndex];
        audioSource.Play();
    }

    public void StopMusic() {
        if (audioSource != null) {
            audioSource.Stop();
        }
    }

    public void StartFromBeginning() {
        if (audioSource != null) {
            audioSource.time = 0;
            audioSource.Play();
        }
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

    public void ToggleStarState() {
        if (isStarman) {
            isStarman = false;
            audioSource.clip = musicClips[currentClipIndex];
            audioSource.Play();
        } else {
            isStarman = true;
            audioSource.clip = starmanClip;
            audioSource.Play();
        }
    }
}
