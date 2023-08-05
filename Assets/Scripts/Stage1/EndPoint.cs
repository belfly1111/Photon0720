using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;


public class EndPoint : MonoBehaviourPun
{
    public PhotonView PV;
    public GameObject EndingGameobject;
    PhotonManeger PM;

    private void Awake()
    {
        PM = FindObjectOfType<PhotonManeger>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PV.RPC("EndingStart", RpcTarget.All);
    }

    [PunRPC]
    void EndingStart()
    {
        EndingGameobject.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
