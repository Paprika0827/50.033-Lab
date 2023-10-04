using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    // global variables
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;
    public float speed = 10;
    private Rigidbody2D marioBody;
    public GameManager gameManager;

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

    public float maxSpeed = 20;
    public float upSpeed = 20;
    private bool onGroundState = true;

    private bool jumpedState = false;

    public EnemyManager enemyManager;

    void Update() {
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.velocity.x));
    }

    void FlipMarioSprite(int value) {
        if (value == -1 && faceRightState) {
            faceRightState = false;
            marioSprite.flipX = true;
            if (marioBody.velocity.x > 0.05f)
                marioAnimator.SetTrigger("onSkid");

        } else if (value == 1 && !faceRightState) {
            faceRightState = true;
            marioSprite.flipX = false;
            if (marioBody.velocity.x < -0.05f)
                marioAnimator.SetTrigger("onSkid");
        }
    }
    private bool moving = false;
    void FixedUpdate() {
        if (alive && moving) {
            Move(faceRightState == true ? 1 : -1);
        }
    }

    void Move(int value) {

        Vector2 movement = new Vector2(value, 0);
        // check if it doesn't go beyond maxSpeed
        if (marioBody.velocity.magnitude < maxSpeed)
            marioBody.AddForce(movement * speed);
    }

    public void MoveCheck(int value) {
        if (value == 0) {
            moving = false;
        } else {
            FlipMarioSprite(value);
            moving = true;
            Move(value);
        }
    }

    

    public void Jump() {
        if (alive && onGroundState) {
            // jump
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
            jumpedState = true;
            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);

        }
    }


    public void JumpHold() {
        if (alive && jumpedState) {
            // jump higher
            marioBody.AddForce(Vector2.up * upSpeed * 30, ForceMode2D.Force);
            jumpedState = false;

        }
    }

    void Start() {
        marioSprite = GetComponent<SpriteRenderer>();
        Application.targetFrameRate = 30;
        marioBody = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        marioAnimator.SetBool("onGround", onGroundState);
    }




    void OnCollisionEnter2D(Collision2D col) {
        if ((col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("Obstacles")) && !onGroundState) {
            onGroundState = jumpOverGoomba.onGroundCheck();

            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);
        }
        if (col.gameObject.CompareTag("Enemy")) {
            // 获取碰撞方向
            Vector2 collisionDirection = col.contacts[0].normal;

            // 如果是从上方碰撞
            if (collisionDirection == Vector2.up) {
                // 增加分数
                Debug.Log("Jumped on enemy!");
                gameManager.IncreaseScore(800);

                Vector3 pos = col.gameObject.transform.position;
                enemyManager.PlaceSprite(pos);

                Destroy(col.gameObject);
            } else {
                // 游戏结束
                marioAnimator.Play("mario_die");
                marioAudio.PlayOneShot(marioDeath);
                alive = false;
            }
        }
    }

/*    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Enemy") && alive) {
            Debug.Log("Collided with goomba!");
            marioAnimator.Play("mario_die");
            marioAudio.PlayOneShot(marioDeath);
            alive = false;

        }
    }*/
    void PlayDeathImpulse() {
        Debug.Log("Death Impulse");
        marioBody.AddForce(Vector2.up * deathImpulse, ForceMode2D.Impulse);
    }

    public void RestartButtonCallback(int input) {
        Debug.Log("Restart!");
        // reset everything
        ResetGame();
        // resume time



    }
    public JumpOverGoomba jumpOverGoomba;
    private void ResetGame() {
        // reset position
        marioBody.transform.position = startPos;
        // reset sprite direction
        faceRightState = true;
        marioSprite.flipX = false;
        marioAnimator.SetTrigger("gameRestart");
        alive = true;
        gameCamera.position = new Vector3(0, 0, -10);
    }





    void PlayJumpSound() {
        // play jump sound

        marioAudio.PlayOneShot(marioAudio.clip);

    }

    void GameOverScene() {
        gameManager.GameOver();
    }


}
