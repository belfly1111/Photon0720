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


public class Skillmanager : MonoBehaviourPun
{
    private bool isDarkCharacter = false;
    private bool isUsingSkill = false;
    public PhotonView PV;

    void Update() {
        if(!PV.IsMine) return;

        if(Input.GetKeyDown(KeyCode.O) && isDarkCharacter) {
            PV.RPC("Useskill",RpcTarget.AllBuffered);
        }
        else if(Input.GetKeyDown(KeyCode.O) && !isDarkCharacter){
            UseSkill();
        }
    }

    //코루틴 및 기본 설정
    [PunRPC]

    //쿨타임 코루틴
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

    //스킬 사용
    public void UseSkill(){
        //스킬 사용중이면 스킬사용 X
        if(isUsingSkill) return;
        //Dark인가 확인
        if(isDarkCharacter && !IsInvoking("Teleport")){
            PV.RPC("Teleport", RpcTarget.AllBuffered);
        }
        //Dark가 아닐경우
        else if(!isDarkCharacter && !IsInvoking("Dash")){
            PV.RPC("Dash", RpcTarget.AllBuffered);
        }
    }

    //쿨타임이 다 끝났을때
    private void CooTimeEnd(){
        Debug.Log("쿨타임 끝");
    }


    //Dark의 스킬 구현
    [PunRPC]

    private void Teleport(){
        //TP코드
        Debug.Log("TP코드실행");
        Vector3 desiredPosition = new Vector3(5f, 5f, 0);
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
        //Dash 이펙트

        //CoolTimeCoRoutine 실행
        StartCoroutine(CoolTimeCRT(7f));
    }
    
}
