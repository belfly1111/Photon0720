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
    public GameObject KeyDoor;
    public GameObject Key;
    public bool IsDark = false;
    
    void Start(){
        RB = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
    }

    void Update(){
        if(IsDark){
            RB.transform.position = SM.Shadow.transform.position + new Vector3(0f,1.5f,0f);
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject == KeyDoor){
            Destroy(KeyDoor);
            Key.SetActive(false);
            SM.IsKey = false;
        }
    }

    void OnTriggerStay2D(Collider2D other) {
        if(other.gameObject.name == "Dark(Clone)"){
            IsDark = true;
            SM.IsKey = true;
        }
        else{
            IsDark = false;
            SM.IsKey = false;
        }
    }
}
