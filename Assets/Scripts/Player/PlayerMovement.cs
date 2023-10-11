using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Events;
using static UnityEditor.PlayerSettings;

public class PlayerMovement : Singleton<PlayerMovement>
{
    private Vector3 startPos;
    
    [System.NonSerialized]
    public bool alive = true;
    private bool onGroundState = true;
    private bool jumpedState = false;
    private bool faceRightState = true;
    private bool invincibleState = false;
    private bool bigState = false;

    public Transform gameCamera;

    public GameObject marioPrefab;
    public GameObject bigMarioPrefab;
    public GameObject mario;

    public SpriteRenderer marioSprite;
    public Rigidbody2D marioBody;
    public Animator marioAnimator;
    public AudioSource marioAudio;
    public AudioClip marioDeath;




    override public void Awake() {
        base.Awake();
        GameManager.Instance.gameRestart.AddListener(GameRestart);
        GameManager.Instance.changeLevel.AddListener(ChangeScene);
        startPos = Levels.levels[0].MarioPosition;
        
        
    }

    public void ChangeScene(int level) {
        startPos = Levels.levels[level].MarioPosition;
        transform.position = startPos;

    }
  

    void Update() {
        if (mario == null) {
            mario = Instantiate(marioPrefab, startPos, Quaternion.identity);
            marioSprite = mario.GetComponent<SpriteRenderer>();
            marioBody = mario.GetComponent<Rigidbody2D>();
            marioAnimator = mario.GetComponent<Animator>();
            marioAudio = mario.GetComponent<AudioSource>();
            marioAnimator.SetBool("onGround", onGroundState);
        }
        if (marioAnimator!=null) marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.velocity.x));
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
/*        if (alive) {
            if (mario == null) {
                init(marioPrefab, startPos);
            }
        }*/
        if (alive && moving) {
            Move(faceRightState == true ? 1 : -1);
        }
        if(mario!=null) transform.position = mario.transform.position;
    }

    void Move(int value) {
        if (mario == null) { return; }
        Vector2 movement = new Vector2(value, 0);
        // check if it doesn't go beyond maxSpeed
        if (marioBody.velocity.magnitude < Constants.MarioMaxSpeed)
            marioBody.AddForce(movement * Constants.MarioSpeed);
    }

    public void MoveCheck(int value) {
        if (mario == null) { return; }
        if (value == 0) {
            moving = false;
        } else {
            FlipMarioSprite(value);
            moving = true;
            Move(value);
        }
    }

    

    public void Jump() {
        if (mario == null) { return; }
        if (alive && onGroundState) {
            // jump
            marioBody.AddForce(Vector2.up * Constants.MarioUpSpeed, ForceMode2D.Impulse);
            onGroundState = false;
            jumpedState = true;
            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);
            PlayJumpSound();
        }
    }


    public void JumpHold() {
        if (mario == null) { return; }
        if (alive && jumpedState) {
            // jump higher
            marioBody.AddForce(Vector2.up * Constants.MarioUpSpeed * 30, ForceMode2D.Force);
            jumpedState = false;

        }
    }


    void Start() {
        Application.targetFrameRate = 30;
        if (mario != null) {
            Debug.Log("Destroy mario on Awake");
            Destroy(mario);
        }
        mario = Instantiate(marioPrefab, startPos, Quaternion.identity);
        marioSprite = mario.GetComponent<SpriteRenderer>();
        marioBody = mario.GetComponent<Rigidbody2D>();
        marioAnimator = mario.GetComponent<Animator>();
        marioAudio = mario.GetComponent<AudioSource>();
        marioAnimator.SetBool("onGround", onGroundState);
    }


    public void ApplyStarMan() {
        if (mario == null) { return; }
        invincibleState = true;
        StartCoroutine(ToggleAfterDelay(Constants.StarmanTime));
    }

    public void ApplyMushroom() {
        if (mario == null) { return; }
        // change sprite
        bigState = true;
        if (mario != null) {
            Debug.Log("Destroy mario");
            Destroy(mario);
        }
        mario = Instantiate(bigMarioPrefab, transform.position + 0.5f * transform.up, Quaternion.identity);
        marioSprite = mario.GetComponent<SpriteRenderer>();
        marioBody = mario.GetComponent<Rigidbody2D>();
        marioAnimator = mario.GetComponent<Animator>();
        marioAudio = mario.GetComponent<AudioSource>();
        marioAnimator.SetBool("onGround", onGroundState);

    }

    private IEnumerator ToggleAfterDelay(float delay) {

        yield return new WaitForSeconds(delay);
        invincibleState = false;
        
        Debug.Log("Starman Stopped");
    }



    public void OnCollisionEnter2D(Collision2D col) {
        if (mario == null) { return; }
        if ((col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("Obstacles")) && !onGroundState) {
            onGroundState = onGroundCheck();
            
            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);
        }
        if (col.gameObject.CompareTag("Enemy")) {
            if (invincibleState == true) {
                // 增加分数
                Debug.Log("kicked enemy!");
                GameManager.Instance.IncreaseScore(800);

                Vector3 pos = col.gameObject.transform.position;
                EnemyManager.Instance.PlaceSprite(pos);

                Destroy(col.gameObject);
                return;
            }
            // 获取碰撞方向
            Vector2 collisionDirection = col.contacts[0].normal;

            // 如果是从上方碰撞
            if (collisionDirection == Vector2.up) {
                // 增加分数
                Debug.Log("Jumped on enemy!");
                GameManager.Instance.IncreaseScore(800);

                Vector3 pos = col.gameObject.transform.position;
                EnemyManager.Instance.PlaceSprite(pos);

                Destroy(col.gameObject);
            } else {
                if (bigState) {
                    // 变小
                    bigState = false;
                    if (mario != null) {
                        Debug.Log("Destroy mario become small");
                        Destroy(mario);
                    }
                    mario = Instantiate(marioPrefab, transform.position-0.5f*transform.up, Quaternion.identity);
                    marioSprite = mario.GetComponent<SpriteRenderer>();
                    marioBody = mario.GetComponent<Rigidbody2D>();
                    marioAnimator = mario.GetComponent<Animator>();
                    marioAudio = mario.GetComponent<AudioSource>();
                    marioAnimator.SetBool("onGround", onGroundState);
                    invincibleState = true;
                    StartCoroutine(ToggleAfterDelay(0.5f));
                } else {
                    // 游戏结束
                    marioAnimator.SetTrigger("die");
                    marioAudio.PlayOneShot(marioDeath);
                    marioBody.AddForce(Vector2.up * Constants.deathImpulse, ForceMode2D.Impulse);
                    alive = false;
                    StartCoroutine(GameOver());
                    IEnumerator GameOver() {
                        yield return new WaitForSeconds(0.5f);
                        GameManager.Instance.GameOver();
                    }
                }
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

    public void GameRestart() {

        Debug.Log("Restart!");
        faceRightState = true;
        marioSprite.flipX = false;
        onGroundState = true;
        alive = true;
        if (mario != null) {
            Debug.Log("Destroy mario on Reset");
            Destroy(mario);
        }
        mario = Instantiate(marioPrefab, startPos, Quaternion.identity);
        marioSprite = mario.GetComponent<SpriteRenderer>();
        marioBody = mario.GetComponent<Rigidbody2D>();
        marioAnimator = mario.GetComponent<Animator>();
        marioAudio = mario.GetComponent<AudioSource>();
        marioAnimator.SetBool("onGround", onGroundState);
        
    }

    public bool onGroundCheck() {
        if (mario == null) { return false; }
        if (!bigState) {
            if (Physics2D.BoxCast(mario.transform.position, Constants.BoxSize, 0, -mario.transform.up, Constants.MaxDistance, Constants.LayerMask)) {
                //Debug.Log("on ground");
                return true;
            } else {
                //Debug.Log("not on ground");
                return false;
            }
        } else {
        if (Physics2D.BoxCast(mario.transform.position, Constants.BoxSize, 0, -mario.transform.up, Constants.MaxDistance+1, Constants.LayerMask)) {
                //Debug.Log("on ground"); 
                return true;
            } else {
                //Debug.Log("not on ground");
                return false;
            }
        }
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        if (mario != null) {
            Gizmos.DrawCube(mario.transform.position - mario.transform.up * Constants.MaxDistance, Constants.BoxSize);
        }
    }

    void PlayJumpSound() {
        // play jump sound
        if (mario == null) { return; }
        marioAudio.PlayOneShot(marioAudio.clip);

    }


}
