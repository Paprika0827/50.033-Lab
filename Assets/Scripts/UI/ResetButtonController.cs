using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Events;

public class ResetButtonController : MonoBehaviour {
    public UnityEvent gameRestart;

    public void ButtonClick() {
        gameRestart.Invoke();
    } 

}