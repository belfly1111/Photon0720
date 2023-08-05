using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenOrDoor : MonoBehaviour
{
    public Light_Switch LS;
    public Shadow_Switch SS;
    public GameObject OrGate;


    private void Update()
    {
        if(LS.onSwitch || SS.onSwitch)
        {
            OrGate.gameObject.SetActive(false);
        }
        else{
            OrGate.gameObject.SetActive(true);
        }
    }



}