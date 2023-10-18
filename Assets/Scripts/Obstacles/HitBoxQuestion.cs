using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxQuestion : MonoBehaviour
{
    public GameObject powerupPrefab;

    public bool canBeBumped = true;
    public bool powerupGenerated = false;
    public bool coin = false;

    private Rigidbody2D rigidbody;
    public Animator questionBoxAnimator;
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
        Vector2 collisionDirection = collision.contacts[0].normal;
        Debug.Log(collisionDirection);
        if (collision.gameObject.CompareTag("Player") && canBeBumped && collisionDirection == Vector2.up)
        {
            BumpBrick();
            StartCoroutine(SetActiveFalse());
            IEnumerator SetActiveFalse(){
                yield return new WaitForSeconds(0.5f);
                rigidbody.bodyType  = RigidbodyType2D.Static;
            }
            
        }
    }

    private void BumpBrick()
    {
    
        Debug.Log("bumped");
        if (!powerupGenerated)
        {
            GeneratePowerup();
            Debug.Log("generated");
        }

        canBeBumped = false;
        questionBoxAnimator.SetBool("canBump",false);
    }

    private void GeneratePowerup()
    {
        Instantiate(powerupPrefab, transform.position + Vector3.up, Quaternion.identity);
        powerupGenerated = true;
        if (coin) {
            //SoundManager.Instance.PlaySound(0);
        } else {
            //SoundManager.Instance.PlaySound(2);
        }
    }

    public void ResetGame()
    {
        canBeBumped = true;
        powerupGenerated = false;
        rigidbody.bodyType  = RigidbodyType2D.Dynamic;
        questionBoxAnimator.SetBool("canBump",true);
    }
}
