using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Events;
using static UnityEditor.PlayerSettings;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 startPos;
    
    public bool alive = true;
    private bool onGroundState = true;
    private bool jumpedState = false;
    private bool faceRightState = true;

    public BoolVariable marioFaceRight;
    public GameObject mario;

    public SpriteRenderer marioSprite;
    public Rigidbody2D marioBody;
    public Animator marioAnimator;
    public AudioSource marioAudio;
    public AudioClip marioDeath;
    
    public PlayerParams playerParams;


    private void updateMarioShouldFaceRight(bool value) {
        faceRightState = value;
        marioFaceRight.SetValue(faceRightState);
        Debug.Log("Mario should face right: " + marioFaceRight.Value);
    }

    public void Awake() {
        //startPos = Levels.levels[0].MarioPosition;
        startPos = transform.position;
        if (this.playerParams != null) {
            playerParams.ReloadParams();
        }
    }

    public void ChangeScene(int level) {
        startPos = Levels.levels[level].MarioPosition;
        transform.position = startPos;
    }
  

    void Update() {
        if (marioAnimator!=null) marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.velocity.x));
    }

    void FlipMarioSprite(int value) {
        if (value == -1 && faceRightState) {
            updateMarioShouldFaceRight(false);
            marioSprite.flipX = true;
            if (marioBody.velocity.x > 0.05f)
                marioAnimator.SetTrigger("onSkid");

        } else if (value == 1 && !faceRightState) {
            updateMarioShouldFaceRight(true);
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
            marioBody.AddForce(Vector2.up * Constants.MarioUpSpeed * 20, ForceMode2D.Force);
            jumpedState = false;

        }
    }


    void Start() {
        Application.targetFrameRate = 30;
        onGroundState = onGroundCheck();
        // update animator state
        marioAnimator.SetBool("onGround", onGroundState);
    }




    public void OnCollisionEnter2D(Collision2D col) {
        if (mario == null) { return; }
        if ((col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("Obstacles")) && !onGroundState) {
            onGroundState = onGroundCheck();
            
            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);
        }
        if (col.gameObject.CompareTag("Enemy")) {
            // 获取碰撞方向
            Vector2 collisionDirection = col.contacts[0].normal;

            // 如果是从上方碰撞
            if (collisionDirection == Vector2.up) {
                // 增加分数
            } else {
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

        
    }

    public bool onGroundCheck() {
        Vector2 Boxsize = gameObject.GetComponent<BoxCollider2D>().size;
        if (Physics2D.BoxCast(mario.transform.position, Boxsize, 0, -mario.transform.up, Constants.MaxDistance, Constants.LayerMask)) {
            //Debug.Log("on ground");
            return true;
        } else {
            //Debug.Log("not on ground");
            return false;
        }
    }
     
    void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Vector2 Boxsize = gameObject.GetComponent<BoxCollider2D>().size;
        Gizmos.DrawCube(mario.transform.position - mario.transform.up * Constants.MaxDistance, Boxsize);
    }

    void PlayJumpSound() {
        // play jump sound
        if (mario == null) { return; }
        marioAudio.PlayOneShot(marioAudio.clip);

    }


}
