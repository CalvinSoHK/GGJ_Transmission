using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LoadingImg_Controller : MonoBehaviour {

    //Event to fire on load
    public UnityEvent LOAD_EVENT;

    //Fires the event when called
    public void InvokeEvent()
    {
        LOAD_EVENT.Invoke();
    }
}
