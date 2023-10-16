using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffState {
    Default = -1,
    Invincible = 0,
}

public class BuffStateController : StateController {
    public BuffState shouldBeNextState = BuffState.Default;
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
        while (string.Equals(currentState.name, "Invincible", StringComparison.OrdinalIgnoreCase)) {
            // Toggle the visibility of the sprite renderer
            spriteRenderer.enabled = !spriteRenderer.enabled;

            // Wait for the specified blink interval
            yield return new WaitForSeconds(Constants.flickerInterval);
        }

        spriteRenderer.enabled = true;
    }

    // this should be added to the GameRestart EventListener as callback
    public void GameRestart() {
        // clear powerup
        // set the start state
        TransitionToState(startState);
    }

    public void SetPowerup(PowerupType i) {
        // currentPowerupType = i;
    }

}
