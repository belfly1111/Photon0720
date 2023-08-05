using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenOrDoor : MonoBehaviour
{
    public Light_Switch LS;
    public Shadow_Switch SS;
    public SpriteRenderer SR;
    public BoxCollider2D BC;
    public GameObject OrGate;

    public void Start(){
        SR = OrGate.GetComponent<SpriteRenderer>();
        BC = OrGate.GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if(LS.onSwitch || SS.onSwitch)
        {
            SR.enabled = false;
            BC.enabled = false;
        }
        else{
            SR.enabled = true;
            BC.enabled = true;
        }
    }



}