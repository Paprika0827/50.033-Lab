using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassCollide : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        PlayerMovementOld.Instance.OnCollisionEnter2D(collision);
    }
}
 