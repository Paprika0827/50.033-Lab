using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxQuestion : MonoBehaviour
{
    public GameObject coinPrefab;

    public bool canBeBumped = true;
    public bool coinGenerated = false;

    private Rigidbody2D rigidbody;
    public GameManager gameManager;
    public Animator questionBoxAnimator;
    public AudioSource coinAudio;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && canBeBumped)
        {
            BumpBrick();
            StartCoroutine(SetActiveFalse());
            IEnumerator SetActiveFalse()
    {
        yield return new WaitForSeconds(0.5f);
        rigidbody.bodyType  = RigidbodyType2D.Static;
    }
            
        }
    }

    private void BumpBrick()
    {
    
        Debug.Log("bumped");
        if (!coinGenerated)
        {
            GenerateCoin();
            Debug.Log("generated");
        }

        canBeBumped = false;
        questionBoxAnimator.SetBool("canBump",false);
    }

    private void GenerateCoin()
    {
        Instantiate(coinPrefab, transform.position + Vector3.up, Quaternion.identity);
        coinGenerated = true;
        gameManager.IncreaseScore(200);
        coinAudio.PlayOneShot(coinAudio.clip);
    }
    public void RestartButtonCallback(int input)
    {
        //Debug.Log("Restart!");
        // reset everything
        ResetGame();
        // resume time
        
    }
    private void ResetGame()
    {
        canBeBumped = true;
        coinGenerated = false;
        rigidbody.bodyType  = RigidbodyType2D.Dynamic;
        questionBoxAnimator.SetBool("canBump",true);
    }
}
