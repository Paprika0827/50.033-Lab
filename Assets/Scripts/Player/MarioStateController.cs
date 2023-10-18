using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum MarioState {
    Default = -1,
    SmallMario = 0,
    SuperMario = 1,
    FireMario = 2,
    InvincibleSmallMario = 3,
    DeadMario = 99,
    Normal = 10,
    Invincible = 11,
}



public class MarioStateController : StateController {
    public MarioState shouldBeNextState = MarioState.Default;
    private SpriteRenderer spriteRenderer;

    public override void Start() {
        base.Start();
        GameRestart(); // clear powerup in the beginning, go to start state
    }

    public void SetRendererToFlicker() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(BlinkSpriteRenderer());
    }
    private IEnumerator BlinkSpriteRenderer() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        while (string.Equals(currentState.name, "InvincibleSmallMario", StringComparison.OrdinalIgnoreCase)) {
            // Toggle the visibility of the sprite renderer
            spriteRenderer.enabled = !spriteRenderer.enabled;

            // Wait for the specified blink interval
            yield return new WaitForSeconds(Constants.FlickerInterval);
        }

        spriteRenderer.enabled = true;
    }

    // this should be added to the GameRestart EventListener as callback
    public void GameRestart() {
        // clear powerup
        currentPowerupType = PowerupType.Default;
        // set the start state
        TransitionToState(startState);
    }

    public void SetPowerup(PowerupType i) {
        currentPowerupType = i;
    }

    public void Fire() {
        this.currentState.DoEventTriggeredActions(this, ActionType.Attack);
    }
}