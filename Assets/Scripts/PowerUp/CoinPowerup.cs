
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CoinPowerup : BasePowerup {
    // setup this object's type
    // instantiate variables

    public float direction = 1;
    public UnityEvent<int> ScoreUp;
    protected override void Start() {
        base.Start(); // call base class Start()
        SpawnPowerup();
        this.type = PowerupType.Coin;
    }


    // interface implementation
    public override void SpawnPowerup() {
        spawned = true;
        this.gameObject.GetComponent<AudioSource>().Play();
        ApplyPowerup(this);
    }




    // interface implementation
    public void ApplyPowerup(GameObject i) {
        // try
        MarioStateController mario;
        ScoreUp.Invoke(100);
        bool result = i.TryGetComponent<MarioStateController>(out mario);
        if (result) {
            mario.SetPowerup(this.powerupType);
            Debug.Log("Mario got MagicMushroom");
        }
    }
    private void Update() {
    }

    private IEnumerator DestroyAfterTime() {
        yield return new WaitForSeconds(0.5f);
        DestroyPowerup();
    }

    public override void ApplyPowerup(MonoBehaviour i) {
        //throw new System.NotImplementedException();
        StartCoroutine(DestroyAfterTime());
    }
}