using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Movement : MonoBehaviour
{
    // global variables
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;
    public float speed = 10;
    private Rigidbody2D marioBody;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI finalText;
    public GameObject enemies;
    private Vector3 startPos;
    public Canvas gameOverCanvas;
    public Canvas inGameCanvas;
    // Start is called before the first frame update
    void Start()
    {
        HideGameOverUI();
        marioSprite = GetComponent<SpriteRenderer>();
        Application.targetFrameRate =  30;
        marioBody = GetComponent<Rigidbody2D>();   
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // toggle state
      if (Input.GetKeyDown("a") && faceRightState){
          faceRightState = false;
          marioSprite.flipX = true;
      }

      if (Input.GetKeyDown("d") && !faceRightState){
          faceRightState = true;
          marioSprite.flipX = false;
      }
        
    }
    // FixedUpdate is called 50 times a second
    public float maxSpeed = 20;
    public float upSpeed = 10;
    private bool onGroundState = true;

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground")) onGroundState = true;
    }
    void  FixedUpdate()
    {
        if (Input.GetKeyDown("space") && onGroundState){
        marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
        onGroundState = false;
        }
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        // Debug.Log(moveHorizontal+"ee");
        if (Mathf.Abs(moveHorizontal) > 0){
            Vector2 movement = new Vector2(moveHorizontal, 0);
            // check if it doesn't go beyond maxSpeed
            if (marioBody.velocity.magnitude < maxSpeed)
                    marioBody.AddForce(movement * speed);
        }

        // stop
        if (Input.GetKeyUp("a") || Input.GetKeyUp("d")){
            // stop
            marioBody.velocity = Vector2.zero;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
  {
      if (other.gameObject.CompareTag("Enemy"))
      {
          Debug.Log("Collided with goomba!");
          finalText.text = "Score: " + jumpOverGoomba.score.ToString();
          ShowGameOverUI();
          Time.timeScale = 0.0f;
      }
  }
    private void HideGameOverUI(){
        gameOverCanvas.enabled =false;
        inGameCanvas.enabled = true;
    }
    private void ShowGameOverUI(){
        gameOverCanvas.enabled = true;
        inGameCanvas.enabled = false;
    }
    public void RestartButtonCallback(int input)
    {
        Debug.Log("Restart!");
        // reset everything
        ResetGame();
        // resume time
        Time.timeScale = 1.0f;
    }
    public JumpOverGoomba jumpOverGoomba;
    private void ResetGame()
    {
        HideGameOverUI();
        // reset position
        marioBody.transform.position = startPos;
        // reset sprite direction
        faceRightState = true;
        marioSprite.flipX = false;
        // reset score
        scoreText.text = "Score: 0";
        // reset Goomba
        foreach (Transform eachChild in enemies.transform)
        {
            eachChild.transform.localPosition = eachChild.GetComponent<EnemyMovement>().startPosition;
        }
        jumpOverGoomba.score = 0;

    }
}
