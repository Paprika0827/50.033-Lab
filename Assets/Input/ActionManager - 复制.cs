using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActionManagerss : MonoBehaviour
{
    public MarioActions marioActions;
    public PlayerInput playerInput;
    private InputAction jumpHoldAction;

    void Start() {
        // must match the actions name
        marioActions = new MarioActions();
        marioActions.gameplay.Enable();
        marioActions.gameplay.jump.performed += OnJumpAction;
        marioActions.gameplay.jumphold.performed += OnJumpHoldAction;
        marioActions.gameplay.move.started += OnMove;
        marioActions.gameplay.move.canceled += OnMove;
        jumpHoldAction = playerInput.actions["jumphold"];
        jumpHoldAction.performed += OnJumpHoldAction;
    }

    void OnMove(InputAction.CallbackContext context) {
        if (context.started) {
            Debug.Log("move started");
        }
        if (context.canceled) {
            Debug.Log("move stopped");
        }

        float move = context.ReadValue<float>();
        Debug.Log($"move value: {move}"); // will return null when not pressed

        // TODO
    }
    public void OnJumpHold(InputValue value) {
        Debug.Log($"OnJumpHold performed with value {value.Get()}");
        // TODO

    }

    public void OnJumpHoldAction(InputAction.CallbackContext context) {
        if (context.started)
            Debug.Log("JumpHold was started");
        else if (context.performed) {
            Debug.Log("JumpHold was performed");
        } else if (context.canceled)
            Debug.Log("JumpHold was cancelled");
    }

    // called twice, when pressed and unpressed
    public void OnJumpAction(InputAction.CallbackContext context) {
        if (context.started)
            Debug.Log("Jump was started");
        else if (context.performed) {
            Debug.Log("Jump was performed");
        } else if (context.canceled)
            Debug.Log("Jump was cancelled");

    }

    // called twice, when pressed and unpressed
    public void OnMoveAction(InputAction.CallbackContext context) {
        if (context.started) {
            Debug.Log("move started");
            float move = context.ReadValue<float>();
            Debug.Log($"move value: {move}"); // will return null when not pressed
        }
        if (context.canceled) {
            Debug.Log("move stopped");
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
