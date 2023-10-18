
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMushroomPowerup : BasePowerup {
    // setup this object's type
    // instantiate variables

    public float direction = 1;
    protected override void Start() {
        base.Start(); // call base class Start()
        SpawnPowerup();
        
        this.type = PowerupType.MagicMushroom;
    }

    void OnCollisionEnter2D(Collision2D col) {
        Debug.Log("Collided with " + col.gameObject.tag);
        if (col.gameObject.CompareTag("Player") && spawned) {
            // TODO: do something when colliding with Player
            ApplyPowerup(col.gameObject);
            // then destroy powerup (optional)
            DestroyPowerup();

        } else if (col.gameObject.layer == 7) // else if hitting Pipe, flip travel direction
          {
            if (spawned) {
                direction *= -1;
                rigidBody.AddForce(Vector2.right * 3 * (goRight ? 1 : -1), ForceMode2D.Impulse);

            }
        }
    } 

    // interface implementation
    public override void SpawnPowerup() {
        spawned = true;
        this.gameObject.GetComponent<AudioSource>().Play();
        //rigidBody.AddForce(Vector2.right * 3, ForceMode2D.Impulse); // move to the right
        
    }


    

    // interface implementation
    public void ApplyPowerup(GameObject i) {
        // try
        MarioStateController mario;
        bool result = i.TryGetComponent<MarioStateController>(out mario);
        if (result) {
            mario.SetPowerup(this.powerupType);
            Debug.Log("Mario got MagicMushroom");
        }
    }
    private void Update() {
        rigidBody.velocity += Vector2.right * 0.1f * direction;
    }

    public override void ApplyPowerup(MonoBehaviour i) {
        //throw new System.NotImplementedException();
    }
}