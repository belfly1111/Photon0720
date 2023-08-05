using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEditor;
using Cinemachine;
using System;
using UnityEngine.Rendering.Universal;


public class Keyobj : MonoBehaviourPun
{
    public Skillmanager_Stage_1 SM;
    public Rigidbody2D RB;
    public SpriteRenderer SR;
    public bool IsDark = false;
    
    void Awake(){
        SM = GameObject.FindObjectOfType<Skillmanager_Stage_1>();
    }

    void Update(){
        if(IsDark){
            RB.transform.position = SM.Shadow.transform.position + new Vector3(0f,0.5f,0f);
        }
    }

    void OnTriggerStay2D(Collider2D other) {
        if(other.gameObject.name == "Dark(Clone)"){
            IsDark = true;
        }
        else{
            IsDark = false;
        }
        
    }
}
