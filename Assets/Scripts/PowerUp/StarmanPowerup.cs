﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarManPowerup : BasePowerup {
    // setup this object's type
    // instantiate variables
    protected override void Start() {
        base.Start(); // call base class Start()
        this.type = PowerupType.StarMan;
        SpawnPowerup();
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.CompareTag("Player") && spawned) {
            // TODO: do something when colliding with Player
            ApplyPowerup(this);
            // then destroy powerup (optional)
            DestroyPowerup();

        } else // else if hitting Pipe, flip travel direction
          {
            if (spawned) {
                if (col.gameObject.layer == 7 || col.gameObject.layer == 8) {
                    goRight = !goRight;
                    rigidBody.AddForce(Vector2.right * 3 * (goRight ? 1 : -1), ForceMode2D.Impulse);

                }
            }
        }
    }

    // interface implementation
    public override void SpawnPowerup() {
        spawned = true;

        rigidBody.AddForce(Vector2.right * 3, ForceMode2D.Impulse); // move to the right
    }


    // interface implementation
    public override void ApplyPowerup(MonoBehaviour i) {
        Debug.Log("StarManPowerup");
        SoundManager.Instance.PlaySound(3);
        PlayerMovement.Instance.ApplyStarMan();
        MusicManager.Instance.PlayMusic(2);
    }
}