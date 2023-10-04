using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxBrics : MonoBehaviour
{
    public GameObject coinPrefab;

    public GameManager gameManager;

    public bool canBeBumped = true;
    public bool coinGenerated = false;

    private bool init_canBeBumped;
    private bool init_coinGenerated;

    public AudioSource coinAudio;

    // Start is called before the first frame update
    void Start()
    {
        init_canBeBumped = canBeBumped;
        init_coinGenerated = coinGenerated;
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
        canBeBumped = init_canBeBumped;
        coinGenerated = init_coinGenerated;

    }
}
