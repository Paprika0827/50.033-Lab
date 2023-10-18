using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Events;

public class ButtonController : MonoBehaviour {
    public UnityEvent buttonEvent;

    public void ButtonClick() {
        buttonEvent.Invoke();
    } 
      
}