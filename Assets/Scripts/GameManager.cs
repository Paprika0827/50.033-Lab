using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager: MonoBehaviour {
    // events
    public IntVariable gameScore;
    public PlayerParams playerParams;
    public Level levelparams;
    private int currentLevelIndex = 0;
    private LevelData currentLevel;
    public int CurrentLevel {
        get {
            return currentLevelIndex;
        }
        set {
            currentLevelIndex = value;
            currentLevel = Levels.levels[currentLevelIndex];
        }
    }

    private int score = 0;

    public void Awake() {
        if (this.playerParams != null) {
            playerParams.ReloadParams();
        }
        if (this.levelparams != null) {
            levelparams.ReloadParams();
        }
    }
    void Start() {

    }

    public void Back() { 
        Time.timeScale = 0.0f;
    }
    public void GameStart() {
       Time.timeScale = 1.0f;
    }

    public void ChangeScene(int lv) {
        CurrentLevel = lv;
        Debug.Log("Change scene to "+currentLevel.Scene);
        Time.timeScale = 1.0f;
        if (lv != 0) {
            SceneManager.LoadScene(currentLevel.Scene, LoadSceneMode.Single);
        }
        //StartCoroutine(LoadSceneAsync());

    }
    public string sceneToLoad;

    private IEnumerator LoadSceneAsync() {
        // 创建异步操作
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(currentLevel.Scene, LoadSceneMode.Single);

        // 阻止场景的自动切换
        asyncOperation.allowSceneActivation = false;

        // 等待场景加载完成
        while (!asyncOperation.isDone) {
            // 在加载完成前进行一些操作

            // 可以显示进度条或其他UI效果

            // 检查是否加载完成
            if (asyncOperation.progress >= 0.9f) {
                // 允许场景切换
                asyncOperation.allowSceneActivation = true;

                // 在这里执行场景加载后的操作
                OnSceneLoaded();
            }

            yield return null;
        }
    }

    private void OnSceneLoaded() {
        // 在这里执行场景加载完成后的操作
        Debug.Log("Scene loaded: " + sceneToLoad);

    }
    public void ChangeSceneCallback() { 
        Time.timeScale = 1.0f;
    }
    // Update is called once per frame
    void Update() {

    }


    public void GameRestart() {
        // reset score
        gameScore.Value = 0;
        SetScore(gameScore.Value);
        Time.timeScale = 1.0f;
    }

    public void GamePause() {
        Time.timeScale = 0.0f;
    }

    public void GameResume() {
        Time.timeScale = 1.0f;
    }


    public void SceneSetup(Scene current, Scene next) {
        SetScore(gameScore.Value);
    }


    public void IncreaseScore(int increment) {
        gameScore.ApplyChange(increment);
        SetScore(gameScore.Value);
    }

    public void SetScore(int score) {
    }


    public void ResetHighestValue() {
        gameScore.ResetHighestValue();
    }



    public void GameOver() {
        Time.timeScale = 0.0f;
    }
}