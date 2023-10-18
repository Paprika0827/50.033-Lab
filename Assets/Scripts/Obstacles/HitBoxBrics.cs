using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxBrics : MonoBehaviour
{
    public GameObject coinPrefab;


    public bool canBeBumped = true;
    public bool coinGenerated = false;
    public bool coin = false;

    private bool init_canBeBumped;
    private bool init_coinGenerated;


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
        Vector2 collisionDirection = collision.contacts[0].normal;

        if (collision.gameObject.CompareTag("Player") && canBeBumped && collisionDirection == Vector2.up)
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
        if (coin) {
            //SoundManager.Instance.PlaySound(0);
        } else {
            //SoundManager.Instance.PlaySound(2);
        }
    }

    public void ResetGame()
    {
        canBeBumped = init_canBeBumped;
        coinGenerated = init_coinGenerated;

    }
}
