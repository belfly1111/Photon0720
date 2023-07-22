using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEditor;

//빛: 대쉬
//어둠: 그림자
//오브젝트: 지형생성, 반사 +) 유리오브젝트는 빛 투과하고 못지나감


public class Skillmanager_Stage_1 : MonoBehaviourPun
{
    private PlayerRole PR;
    private bool isUsingSkill = false;
    public PhotonView PV;
    public Skillmanager_Stage_1 SM;

    public enum PlayerRole{
        Dark,
        Light,
        Null
    }


    void Start() {
        //초기화
        PV = this.GetComponent<PhotonView>();
        SM = this.GetComponent<Skillmanager_Stage_1>();

        //isDarkCharacter 인지 확인 필요
        if(PV.IsMine){
            //처음에 Null로 고정시킴
            PR = PlayerRole.Null;
            if(PV.Owner.NickName == "Dark"){
                PR = PlayerRole.Dark;
            }
            else if(PV.Owner.NickName == "Light"){
                PR = PlayerRole.Light;
            }
           }
    }


    void Update() {
        if(!PV.IsMine) return;

        if(Input.GetKeyDown(KeyCode.O)) {
            UseSkill();
        }
    }

    
    #region Nomalregion

    //스킬 사용
    public void UseSkill(){
        //스킬 사용중이면 스킬사용 X
        if(isUsingSkill) return;

        //Dark인가 확인
        if(PR == PlayerRole.Dark && !IsInvoking("Teleport")){
            //PV.RPC("Teleport", RpcTarget.AllBuffered);
            Teleport();
        }

        //Dark가 아닐경우
        else if(PR == PlayerRole.Light && !IsInvoking("Dash")){
            //PV.RPC("Dash", RpcTarget.AllBuffered);
            Dash();
        }
    }

    //Player 역할 고정
    public void SetPR(PlayerRole role){
        PR = role;
        Debug.Log("플레이어 역할 변경: " + PR);
    }

    #endregion


    #region PunRPCScripts

    //쿨타임 코루틴
    [PunRPC]
    public IEnumerator CoolTimeCRT(float cool){
        //스킬 사용중
        isUsingSkill = true;
        //스킬 사용 후 쿨타임 기다리기
        yield return new WaitForSeconds(cool);
        //스킬 사용끝
        isUsingSkill = false;
        //쿨타임 끝난걸 RPC선언
        PV.RPC("CoolTimeEnd", RpcTarget.AllBuffered);
    }
    //쿨타임이 다 끝났을때
    [PunRPC]
    private void CoolTimeEnd(){
        Debug.Log("쿨타임 끝");
    }


    //Dark의 스킬 구현
    [PunRPC]
    private void Teleport(){
        //TP코드
        Debug.Log("TP코드실행");
        Vector3 desiredPosition = new Vector3(-5, 5f, 0);
        PV.transform.position = desiredPosition;
        //TP이펙트가 있을 경우

        //CoolTimeCoRouTine실행
        StartCoroutine(CoolTimeCRT(7f));
    }


    //Light의 스킬 구현
    [PunRPC]
    private void Dash(){
        //Dash 코드
        Debug.Log("Dash코드실행");
        Vector3 desiredPosition = new Vector3(5, 5f, 0);
        PV.transform.position = desiredPosition;
        //Dash 이펙트

        //CoolTimeCoRoutine 실행
        StartCoroutine(CoolTimeCRT(7f));
    }

    #endregion
}

