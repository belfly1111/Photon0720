using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class TouchTrigger : MonoBehaviourPun
{
    public Skillmanager_Stage_1 SM;
    public LayerMask _isTouchingLayer;

    void Awake(){
        SM = GameObject.FindObjectOfType<Skillmanager_Stage_1>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        int otherLayerMask = other.collider.gameObject.layer;
        if (otherLayerMask == LayerMask.NameToLayer("Scaffolding_Shadow"))
        {
            SM.canTP = false;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.collider.IsTouchingLayers(_isTouchingLayer))
        {
            SM.canTP = false;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.IsTouchingLayers(_isTouchingLayer))
        {
            SM.canTP = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Trigger")){
            SM.canDash = true;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Trigger"))
        {
            SM.canDash = false;
        }
    }


}
