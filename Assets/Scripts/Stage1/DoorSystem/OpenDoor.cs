using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    [SerializeField] GameObject Door;
    public Light_Switch LS;
    public Shadow_Switch SS;

    private void Update()
    {
        if(LS.onSwitch && SS.onSwitch)
        {
            Destroy(gameObject);
        }
    }



}
