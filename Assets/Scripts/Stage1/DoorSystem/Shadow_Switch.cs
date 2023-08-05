using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow_Switch : MonoBehaviour
{
    public bool onSwitch;
    private void Awake()
    {
        onSwitch = false;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name == "Dark(Clone)")
        {
            onSwitch = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.name == "Dark(Clone)")
        {
            onSwitch = false;
        }
    }
}
