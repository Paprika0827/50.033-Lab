
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarmanPowerup : BasePowerup {
    // setup this object's type
    // instantiate variables
    protected override void Start() {
        base.Start(); // call base class Start()
        SpawnPowerup();
        StartCoroutine(BlinkSpriteRenderer());
        this.type = PowerupType.StarMan;
    }
    public float direction = 1;

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
                //rigidBody.AddForce(Vector2.right * 3 * (goRight ? 1 : -1), ForceMode2D.Impulse);

            }
        }
    }

    private IEnumerator BlinkSpriteRenderer() {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        while (true) {
            float t = Mathf.PingPong(Time.time * Constants.StarFlashSpeed, 1f);
            Color rainbowColor = Color.HSVToRGB(t, 1f, 1f);
            spriteRenderer.color = rainbowColor;
            yield return null;
        }
    }

    // interface implementation
    public override void SpawnPowerup() {
        spawned = true;
        this.gameObject.GetComponent<AudioSource>().Play();
        rigidBody.AddForce(Vector2.right * 3, ForceMode2D.Impulse); // move to the right
    }


    // interface implementation
    public void ApplyPowerup(GameObject i) {
        // try
        BuffStateController mario;
        bool result = i.TryGetComponent<BuffStateController>(out mario);
        if (result) {
            mario.SetPowerup(this.powerupType);
            Debug.Log("Mario got Starman");
        }
    }

    public void Update() {
        rigidBody.velocity += Vector2.right * 0.1f * direction;
    }

    public override void ApplyPowerup(MonoBehaviour i) {
        //throw new System.NotImplementedException();
    }
}