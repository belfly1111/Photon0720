using Photon.Pun;
using System.Collections;
using UnityEngine;

//빛: 대쉬
//어둠: 그림자
//오브젝트: 지형생성, 반사 +) 유리오브젝트는 빛 투과하고 못지나감

public class Skillmanager_Stage_1 : MonoBehaviourPun
{
    [SerializeField] private PhotonView PV;
    
    #region NormalFunction
    private void CooTimeEnd()
    {
        Debug.Log("쿨타임 끝");
    }
    #endregion

    #region PunRPCFunction
    [PunRPC]
    IEnumerator CoolTimeCRT(float cool)
    {
        yield return new WaitForSeconds(cool);
        PV.RPC("CoolTimeEnd", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void Teleport()
    {
        Debug.Log("TP코드실행");
        StartCoroutine(CoolTimeCRT(7f));
    }

    [PunRPC]
    private void Dash()
    {
        Debug.Log("Dash코드실행");
        StartCoroutine(CoolTimeCRT(7f));
    }
    #endregion
}
