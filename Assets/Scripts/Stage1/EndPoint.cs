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
    public Image Endingimg;
    public TMP_Text EndingDialog;
    PhotonManeger PM;
    Animator animator;

    private void Awake()
    {
        PM = FindObjectOfType<PhotonManeger>();
        Endingimg.enabled = false;
        EndingDialog.enabled = false;
        animator = Endingimg.GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PV.RPC("EndingStart", RpcTarget.All);
    }

    [PunRPC]
    void EndingStart()
    {
        Endingimg.enabled = true;
        EndingDialog.enabled = true;
        animator.SetBool("isEnding", true);
        Invoke("GoTitle", 3f);
    }

    void GoTitle()
    {
        PM.LeaveRoom();
        PM.Disconnect();
        SceneManager.LoadScene("Title");
        Destroy(PM);
    }
}
