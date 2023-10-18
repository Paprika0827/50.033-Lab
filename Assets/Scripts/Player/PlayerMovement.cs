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
    
    public bool alive = false;
    public bool Invincible = false;
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
        //Debug.Log("Mario should face right: " + marioFaceRight.Value);
    }

    public void Awake() {
        //startPos = Levels.levels[0].MarioPosition;
        startPos = transform.position;
        alive = false;
    }
  

    void Update() {
        if (!alive) { return; }
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
        if (!alive) { return; }
        Vector2 movement = new Vector2(value, 0);
        // check if it doesn't go beyond maxSpeed
        if (marioBody.velocity.magnitude < Constants.MarioMaxSpeed)
            marioBody.AddForce(movement * Constants.MarioSpeed);
    }

    public void MoveCheck(int value) {
        if (!alive) { return; }
        if (value == 0) {
            moving = false;
        } else {
            FlipMarioSprite(value);
            moving = true;
            Move(value);
        }
    }

    

    public void Jump() {
        if (!alive) { return; }
        if (onGroundState) {
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
        if (!alive) { return; }
        if (jumpedState) {
            // jump higher
            marioBody.AddForce(Vector2.up * Constants.MarioUpSpeed * 30, ForceMode2D.Force);
            jumpedState = false;

        }
    }


    void Start() {
        Application.targetFrameRate = 30;
        onGroundState = onGroundCheck();
        // update animator state
        marioAnimator.SetBool("onGround", onGroundState);
    }

    public void ToggleInvinsibility() {
        Invincible = !Invincible;
    }

    public void DamageMario() {
        // GameOverAnimationStart(); // last time Mario dies right away

        // pass this to StateController to see if Mario should start game over
        // since both state StateController and MarioStateController are on the same gameobject, it's ok to cross-refer between scripts
        GetComponent<MarioStateController>().SetPowerup(PowerupType.Damage);

    }

    public void OnCollisionEnter2D(Collision2D col) {
        if (!alive) { return; }
        if ((col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("Obstacles")) && !onGroundState) {
            onGroundState = onGroundCheck();
            
            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);
        }
        if (col.gameObject.CompareTag("Enemy")) {
            // ��ȡ��ײ����
            Vector2 collisionDirection = col.contacts[0].normal;

            // ����Ǵ��Ϸ���ײ
            if (collisionDirection == Vector2.up || Invincible) {
                col.gameObject.GetComponent<EnemyMovement>().KillGoomba();
                // ���ӷ���
            } else {
                DamageMario();
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
        alive = true;
        transform.position = startPos;
        marioBody.velocity = Vector2.zero;
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

    public void GameOver() {
        marioAudio.PlayOneShot(marioDeath);
        marioBody.AddForce(Vector2.up * Constants.DeathImpulse, ForceMode2D.Impulse);
        alive = false;
    }

}
