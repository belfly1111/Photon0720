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
    [SerializeField] GameObject Skillmaneger_Stage_1;

    public static int LocalPlayerRule;


    public bool LightReady;
    public bool DarkReady;

    private void Awake()
    {
        LocalPlayerRule = -1;
        LightReady = false;
        DarkReady = false;  
    }

    private void Update()
    {
        if(LightReady && DarkReady)
        {
            Skillmaneger_Stage_1.SetActive(true);
        }
    }

    public void setLight()
    {
        PhotonManeger.Light = true;
        PhotonManeger.Dark = false;
        LightBtn.SetActive(false);
        DarkBtn.SetActive(false);

        // '1' 인 경우 'Light'
        LocalPlayerRule = 1;

        Debug.Log("Light를 선택하셨습니다.");
        PhotonNetwork.Instantiate("Light", new Vector3(-10, 0.5f, 0), Quaternion.identity);
        photonView.RPC("DestroyBtn", RpcTarget.OthersBuffered, LocalPlayerRule);
        photonView.RPC("setLightReady", RpcTarget.AllBuffered);

    }

    public void setDark()
    {
        PhotonManeger.Light = true;
        PhotonManeger.Dark = false;
        LightBtn.SetActive(false);
        DarkBtn.SetActive(false);

        // '0' 인 경우 'Dark'
        LocalPlayerRule = 0;

        Debug.Log("Dark를 선택하셨습니다.");
        PhotonNetwork.Instantiate("Dark", new Vector3(10, 0.5f, 0), Quaternion.identity);
        photonView.RPC("DestroyBtn", RpcTarget.OthersBuffered, LocalPlayerRule);
        photonView.RPC("setDarkReady", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void DestroyBtn(int AnotherSideRule)
    {
        if (AnotherSideRule == 1) LightBtn.SetActive(false);
        else if (AnotherSideRule == 0) DarkBtn.SetActive(false);
    }

    [PunRPC]
    void setLightReady()
    {
        LightReady = true;
    }

    [PunRPC]
    void setDarkReady()
    {
        DarkReady = true;
    }
}
