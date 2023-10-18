using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyMovement : MonoBehaviour
{
    private float originalX;
    private float maxOffset = 5.0f;
    private float enemyPatroltime = 2.0f;
    private bool isDead = true;
    public UnityEvent<int> ScoreUp;
    public float TravelTime { get { return enemyPatroltime; } set { enemyPatroltime = value; } }
    private int moveRight = -1;
    private Vector2 velocity;

    private Rigidbody2D enemyBody;
    public Sprite deadSprite;
    private Collider2D enemyCollider;
    private Vector3 startPosition;
    private AudioSource enemyAudio;


    void Start()
    {
        enemyBody = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<Collider2D>();
        enemyAudio = GetComponent<AudioSource>();
        // get the starting position
        // startPosition = transform.position;
        originalX = transform.position.x;
        startPosition = transform.position;
        ComputeVelocity();
    }
    void ComputeVelocity()
    {
        velocity = new Vector2((moveRight) * maxOffset / enemyPatroltime, 0);
    }
    void Movegoomba()
    {
        enemyBody.MovePosition(enemyBody.position + velocity * Time.fixedDeltaTime);
    }
    public void GameStart() {
        isDead = false;
    }
    public void GameOver() {
        isDead = true;
    }
    public void GameRestart() {
        isDead = false;
        enemyCollider.enabled = true;
        transform.localScale = new Vector3(1, 1, 1);
        transform.position = startPosition;
    }
    public void KillGoomba() {
        enemyAudio.PlayOneShot(enemyAudio.clip);
        enemyCollider.enabled = false;
        ScoreUp.Invoke(500);
        isDead = true;
        transform.localScale = new Vector3(1, 0.2f, 1);
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
    }
    void Update()
    {
        if (isDead) { return; }
        if (Mathf.Abs(enemyBody.position.x - originalX) < maxOffset)
        {// move goomba
            Movegoomba();
        }
        else
        {
            // change direction
            moveRight *= -1;
            ComputeVelocity();
            Movegoomba();
        }
    }
    void OnTriggerEnter2D(Collider2D other)
  {
     Debug.Log(other.gameObject.name);
  }

}
