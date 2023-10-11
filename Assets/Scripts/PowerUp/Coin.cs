using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : BasePowerup {
    public float coinSpeed = 2f;
    public Animator coinAnimator;

    protected override void Start() {
        base.Start();
        this.type = PowerupType.MagicMushroom;
        rigidBody.velocity = Vector2.up * coinSpeed;
        ApplyPowerup(this);
        Destroy(this.gameObject, 1f);
    }

    public override void SpawnPowerup() {
        throw new System.NotImplementedException();
    }

    public override void ApplyPowerup(MonoBehaviour i) {
        GameManager.Instance.IncreaseScore(200);
    }
}
