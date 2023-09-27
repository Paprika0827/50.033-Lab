using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
     public float coinSpeed = 2f;
     public Animator coinAnimator;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.up * coinSpeed;
        Destroy(gameObject, 1f); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
