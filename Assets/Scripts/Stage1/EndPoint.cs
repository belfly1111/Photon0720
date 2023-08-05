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


    float elapsedTime = 0;

    private void Awake()
    {
        Endingimg.enabled = false;
        EndingDialog.enabled = false;
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
        StartCoroutine(GoTitle());
    }

    IEnumerator GoTitle()
    {
        yield return new WaitForSeconds(3f);
        PhotonNetwork.Disconnect();
        Destroy(PhotonManeger.instance);
        SceneManager.LoadScene("Title");
    }
}
