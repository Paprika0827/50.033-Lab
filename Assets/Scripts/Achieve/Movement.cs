using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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
    public Animator marioAnimator;   
    public AudioSource marioAudio;
    public AudioClip marioDeath;

    public Transform gameCamera;

    public float deathImpulse;
    [System.NonSerialized]
    public bool alive = true;

    void Start()
    {
        HideGameOverUI();
        marioSprite = GetComponent<SpriteRenderer>();
        Application.targetFrameRate =  30;
        marioBody = GetComponent<Rigidbody2D>();   
        startPos = transform.position;
        marioAnimator.SetBool("onGround", onGroundState);
        SceneManager.activeSceneChanged += SetStartingPosition;
    }

    // Update is called once per frame
    void Update()
    {
        // toggle state
      if (Input.GetKeyDown("a") && faceRightState){
          faceRightState = false;
          marioSprite.flipX = true;
          if (marioBody.velocity.x > 0.1f)
                marioAnimator.SetTrigger("onSkid");
      }

      if (Input.GetKeyDown("d") && !faceRightState){
          faceRightState = true;
          marioSprite.flipX = false;
          if (marioBody.velocity.x < -0.1f)
                marioAnimator.SetTrigger("onSkid");
      }

      marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.velocity.x));
        
    }
    // FixedUpdate is called 50 times a second
    public float maxSpeed = 20;
    public float upSpeed = 20;
    private bool onGroundState = true;


    void OnCollisionEnter2D(Collision2D col)
    {
        if ((col.gameObject.CompareTag("Ground")|| col.gameObject.CompareTag("Enemy") || col.gameObject.CompareTag("Obstacles")) && !onGroundState)
        {
            onGroundState = true;

            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);
        }
    }
    void  FixedUpdate()
    {
        if (alive)
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

            marioAnimator.SetBool("onGround", onGroundState);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
  {
      if (other.gameObject.CompareTag("Enemy") && alive)
      {
          Debug.Log("Collided with goomba!");
          marioAnimator.Play("mario_die");
          marioAudio.PlayOneShot(marioDeath);
          alive = false;
          finalText.text = "Score: " + jumpOverGoomba.score.ToString();
          
      }
  }
    void PlayDeathImpulse()
    {
        Debug.Log("Death Impulse");
        marioBody.AddForce(Vector2.up * deathImpulse, ForceMode2D.Impulse);
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
        marioAnimator.SetTrigger("gameRestart");
        alive = true;
        gameCamera.position = new Vector3(0, 0, -10);

    }
        public void GameRestart()
    {
        // reset position
        marioBody.transform.position = new Vector3(-5.33f, -4.69f, 0.0f);
        // reset sprite direction
        faceRightState = true;
        marioSprite.flipX = false;

        // reset animation
        marioAnimator.SetTrigger("gameRestart");
        alive = true;

        // reset camera position
        gameCamera.position = new Vector3(0, 0, -10);
    }

    

        void PlayJumpSound()
    {
        // play jump sound

        marioAudio.PlayOneShot(marioAudio.clip);
        
    }
    
    void GameOverScene()
    {
        // stop time
        Time.timeScale = 0.0f;
        // set gameover scene
        ShowGameOverUI();
    }
    public void SetStartingPosition(Scene current, Scene next)
    {
        if (next.name == "World-1-2")
        {
            // change the position accordingly in your World-1-2 case
            this.transform.position = new Vector3(-10.2399998f, -4.3499999f, 0.0f);
        }
    }
}
