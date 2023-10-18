using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BuffStateController : StateController {
    public MarioState shouldBeNextState = MarioState.Normal;
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
        Color color = spriteRenderer.color;
        while (string.Equals(currentState.name, "Invincible", StringComparison.OrdinalIgnoreCase)) {
            float t = Mathf.PingPong(Time.time * Constants.StarFlashSpeed, 1f);
            Color rainbowColor = Color.HSVToRGB(t, 1f, 1f);
            spriteRenderer.color = rainbowColor;
            yield return null;
        }

        spriteRenderer.color = color;
    }

    // this should be added to the GameRestart EventListener as callback
    public void GameRestart() {
        // clear powerup
        currentPowerupType = PowerupType.Default;
        // set the start state
        TransitionToState(startState);
    }

    public void SetPowerup(PowerupType i) {
        // currentPowerupType = i;
        currentPowerupType = i;
    }

}
