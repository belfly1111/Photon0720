using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light_Switch : MonoBehaviour
{
    public bool onSwitch;
    private void Awake()
    {
        onSwitch = false;
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name == "Light(Clone)")
        {
            onSwitch = true;
        }
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.name == "Light(Clone)")
        {
            onSwitch = false;
        }
    }
}
