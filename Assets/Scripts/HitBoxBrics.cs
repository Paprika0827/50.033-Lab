using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxBrics : MonoBehaviour
{
    public GameObject coinPrefab;

    public bool canBeBumped = true;
    public bool coinGenerated = false;

    public AudioSource coinAudio;

    // Start is called before the first frame update
    void Start()
    {

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
        coinAudio.PlayOneShot(coinAudio.clip);
    }
}
