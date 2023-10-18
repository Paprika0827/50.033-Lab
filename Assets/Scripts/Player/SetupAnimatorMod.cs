using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "PluggableSM/Actions/SetupAnimatorMod")]
public class SetupAnimatorMod : Action {
    public RuntimeAnimatorController animatorController;
    public override void Act(StateController controller) {
        controller.gameObject.transform.position += new Vector3(0, 0.5f, 0);
    }
}