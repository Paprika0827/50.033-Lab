using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.Events;

public class HUDManager2 : MonoBehaviour{
    private Vector3[] scoreTextPosition = {
        new Vector3(-747, 473, 0),
        new Vector3(0, 0, 0)
        };
    private Vector3[] restartButtonPosition = {
        new Vector3(844, 455, 0),
        new Vector3(0, -150, 0)
    };

    public IntVariable gameScore;

    public GameObject scoreText;
    public GameObject highscoreText;
    public GameObject loadingText;
    public GameObject mainMenu;
    public Transform restartButton;
    public GameObject PauseMenu;
    public GameObject PauseButton;
    public GameObject loadingPanel;
    public Image loadingImage;
    public float fadeDuration = 3.0f;
    public UnityEvent start;

    private string[] level = { "World1-1", "World1-2" };
    public int currenetLevel = 2;




    public void Awake() {
        loadingPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        PauseMenu.SetActive(false);
        PauseButton.SetActive(false);
        mainMenu.SetActive(true);
        scoreText.SetActive(false);
        restartButton.gameObject.SetActive(false);
        mainMenu.SetActive(false);
        loadingPanel.SetActive(true);
        loadingText.GetComponent<TextMeshProUGUI>().text = level[currenetLevel - 1];
        StartCoroutine(FadeOutLoadingScreen());
    }

    public GameObject gameOverPanel;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }


    public void ChangeScene() {
        mainMenu.SetActive(false);
        loadingPanel.SetActive(true);
        loadingText.GetComponent<TextMeshProUGUI>().text = level[currenetLevel-1];
        StartCoroutine(FadeOutLoadingScreen());
    }

    public void MainMenu() {
        loadingPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        PauseMenu.SetActive(false);
        PauseButton.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void GameStart() {
        scoreText.GetComponent<TextMeshProUGUI>().text = "Score: " + gameScore.Value.ToString("D6");
        // hide gameover panel
        loadingPanel.SetActive(false);
        mainMenu.SetActive(false);
        gameOverPanel.SetActive(false);
        PauseMenu.SetActive(false);
        PauseButton.SetActive(true);
        scoreText.SetActive(true);
        restartButton.gameObject.SetActive(true);
        scoreText.transform.localPosition = scoreTextPosition[0];
        restartButton.localPosition = restartButtonPosition[0];
    }
    
    public void GamePause() {
        PauseMenu.SetActive(true);
        PauseButton.SetActive(false);
        //SoundManager.Instance.PlaySound(4);
    }

    public void GameResume() {
        PauseButton.SetActive(true);
        PauseMenu.SetActive(false);
        //SoundManager.Instance.PlaySound(4);
    }

    public void SetScore(int score) {
        gameScore.Value += score;
        scoreText.GetComponent<TextMeshProUGUI>().text = "Score: " + gameScore.Value.ToString("D6");
    }

    public void GameRestart() {
        gameScore.Value = 0;
        scoreText.GetComponent<TextMeshProUGUI>().text = "Score: " + gameScore.Value.ToString("D6");
        // hide gameover panel
        loadingPanel.SetActive(false);
        mainMenu.SetActive(false);
        gameOverPanel.SetActive(false);
        PauseMenu.SetActive(false);
        PauseButton.SetActive(true);
        scoreText.SetActive(true);
        restartButton.gameObject.SetActive(true);
        scoreText.transform.localPosition = scoreTextPosition[0];
        restartButton.localPosition = restartButtonPosition[0];
    }

    public void GameOver() {
        loadingPanel.SetActive(false);
        gameOverPanel.SetActive(true);
        PauseMenu.SetActive(false);
        PauseButton.SetActive(false);
        scoreText.transform.localPosition = scoreTextPosition[1];
        restartButton.localPosition = restartButtonPosition[1];
        // set highscore
        highscoreText.GetComponent<TextMeshProUGUI>().text = "TOP- " + gameScore.previousHighestValue.ToString("D6");
        // show
        highscoreText.SetActive(true);
    }

    private IEnumerator FadeOutLoadingScreen() {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration) {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            loadingImage.color = new Color(1f, 1f, 1f, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        loadingImage.color = new Color(1f, 1f, 1f, 0f);
        loadingPanel.SetActive(false);
        start.Invoke();
    }
}
