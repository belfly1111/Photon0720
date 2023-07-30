using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class TouchTrigger : MonoBehaviourPun
{
    public Skillmanager_Stage_1 SM;

    void Awake(){

        SM = GameObject.FindObjectOfType<Skillmanager_Stage_1>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Trigger")){
            SM.canDash = true;
        }
    }
    
    void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Trigger")){
            SM.canDash = false;
        }
    }


}
