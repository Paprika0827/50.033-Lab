using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "PluggableSM/Actions/TriggerEvent")]
public class TriggerEvent : Action {
    public UnityEvent buttonEvent;
    public override void Act(StateController controller) {
        buttonEvent.Invoke();
    }
}