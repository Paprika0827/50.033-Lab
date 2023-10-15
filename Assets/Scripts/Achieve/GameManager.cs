using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> {
    // events
    public IntVariable gameScore;
    public UnityEvent gameStart;
    public UnityEvent gameRestart;
    public UnityEvent<int> scoreChange;
    public UnityEvent<int> changeLevel;
    public UnityEvent<int> loading;
    public UnityEvent gameOver;
    public UnityEvent gamePause;
    public UnityEvent gameResume;
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

    override public void Awake() {
        base.Awake();
        if (this.playerParams != null) {
            playerParams.ReloadParams();
        }
        if (this.levelparams != null) {
            levelparams.ReloadParams();
        }
    }
    void Start() {
        Time.timeScale = 0.0f;
    }
    public void Back() { 
        HUDManager.Instance.MainMenu();
        Time.timeScale = 0.0f;
    }
    public void GameStart() {
        ChangeScene(0);
    }
    public void GameStartCallback() { 
        gameStart.Invoke();
    } 

    public void ChangeScene(int lv) {
        CurrentLevel = lv;
        Debug.Log("Change scene to "+currentLevel.Scene);
        Time.timeScale = 1.0f;
        if (lv != 0) {
            SceneManager.LoadScene(currentLevel.Scene, LoadSceneMode.Single);
        }
        //StartCoroutine(LoadSceneAsync());
        loading.Invoke(currentLevelIndex);
        changeLevel.Invoke(currentLevelIndex);

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
            loading.Invoke(currentLevelIndex);
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
        changeLevel.Invoke(currentLevelIndex);

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
        gameRestart.Invoke();
        Time.timeScale = 1.0f;
    }

    public void GamePause() {
        Time.timeScale = 0.0f;
        gamePause.Invoke();
    }

    public void GameResume() {
        Time.timeScale = 1.0f;
        gameResume.Invoke();
    }


    public void SceneSetup(Scene current, Scene next) {
        gameStart.Invoke();
        SetScore(gameScore.Value);
    }


    public void IncreaseScore(int increment) {
        gameScore.ApplyChange(increment);
        SetScore(gameScore.Value);
    }

    public void SetScore(int score) {
        scoreChange.Invoke(score);
    }


    public void ResetHighestValue() {
        gameScore.ResetHighestValue();
    }



    public void GameOver() {
        Time.timeScale = 0.0f;
        gameOver.Invoke();
    }
}