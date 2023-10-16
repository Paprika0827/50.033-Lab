using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "PluggableSM/Actions/SetupAnimator")]
public class SetupAnimator : Action {
    public RuntimeAnimatorController animatorController;
    public override void Act(StateController controller) {
        controller.gameObject.GetComponent<Animator>().runtimeAnimatorController = animatorController;
        controller.gameObject.GetComponent<Animator>().SetBool("onGround", true);
        controller.gameObject.transform.position = controller.gameObject.transform.position + controller.gameObject.transform.up * 0.52f;

    }
}