using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 포톤 환경에서의 namespace이다.
using Photon.Pun;
using Photon.Realtime;
using System.Reflection;
using UnityEditor;

public class StageUI_1 : MonoBehaviourPun
{
    [SerializeField] GameObject LightBtn;
    [SerializeField] GameObject DarkBtn;

    public int LocalPlayerRule = 0;

    public void setLight()
    {
        PhotonManeger.Light = true;
        PhotonManeger.Dark = false;
        LightBtn.SetActive(false);
        DarkBtn.SetActive(false);
        LocalPlayerRule = 1;

        PhotonNetwork.Instantiate("Light", new Vector3(-10, 0.5f, 0), Quaternion.identity);
        photonView.RPC("DestroyBtn", RpcTarget.OthersBuffered, LocalPlayerRule);
    }

    public void setDark()
    {
        PhotonManeger.Light = true;
        PhotonManeger.Dark = false;
        LightBtn.SetActive(false);
        DarkBtn.SetActive(false);
        LocalPlayerRule = 0;

        PhotonNetwork.Instantiate("Dark", new Vector3(10, 0.5f, 0), Quaternion.identity);
        photonView.RPC("DestroyBtn", RpcTarget.OthersBuffered, LocalPlayerRule);
    }

    [PunRPC]
    private void DestroyBtn(int AnotherSideRule)
    {
        if (AnotherSideRule == 1) LightBtn.SetActive(false);
        else if (AnotherSideRule == 0) DarkBtn.SetActive(false);
    }

}
