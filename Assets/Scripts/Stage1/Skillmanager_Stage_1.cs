using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEditor;
using Cinemachine;
using System;

//빛: 대쉬
//어둠: 그림자
//오브젝트: 지형생성, 반사 +) 유리오브젝트는 빛 투과하고 못지나감

public class Skillmanager_Stage_1 : MonoBehaviourPun
{
    public PhotonView PV;

    [SerializeField] GameObject Light;
    [SerializeField] GameObject Dark;
    [SerializeField] CinemachineVirtualCamera VM;
    [SerializeField] private float dashingPower = 4f;
    [SerializeField] private float dashingTime = 0.2f;
    public bool skillcool = false;

    void Start()
    {
        Light = GameObject.Find("Light(Clone)");
        Dark = GameObject.Find("Dark(Clone)");

        PV = this.GetComponent<PhotonView>();

        if(StageUI_1.LocalPlayerRule == 1)
        {
            VM.Follow = Light.GetComponentInChildren<Transform>();
        }
        else if (StageUI_1.LocalPlayerRule == 0)
        {
            VM.Follow = Dark.GetComponentInChildren<Transform>();
        }
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.V))
        {
            UseSkill();
        }
    }

    #region Nomalregion
    //스킬 사용
    public void UseSkill()
    {
        // 플레이어가 선택한게 'Light'인 경우
        if (StageUI_1.LocalPlayerRule == 1 && !skillcool)
        {
            StartCoroutine("Dash");
        }

        // 플레이어가 선택한게 'Dark'인 경우
        else if (StageUI_1.LocalPlayerRule == 0 && !skillcool)
        {
            StartCoroutine("Teleport");
        }
    }
    IEnumerator Teleport()
    {
        skillcool = true;
        Debug.Log("TP코드실행");
        PV.RPC("TP", RpcTarget.AllBuffered);
        yield return new WaitForSeconds(3f);

        skillcool = false;
        Debug.Log("스킬 재사용 가능!");
    }
    IEnumerator Dash()
    {
        skillcool = true;
        Debug.Log("Dash코드실행");

        PV.RPC("DASH", RpcTarget.AllBuffered);
        yield return new WaitForSeconds(3f);

        skillcool = false;
        Debug.Log("스킬 재사용 가능!");
    }

    IEnumerator LerpDash(Vector2 dir){
        Rigidbody2D rb = Light.GetComponent<Rigidbody2D>();
        Vector2 DPos = rb.position + dir.normalized * dashingPower;
        float elapsedTime = 0f;
        while (elapsedTime < dashingTime){
            rb.position = Vector2.Lerp(rb.position, DPos, elapsedTime / dashingTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        rb.position = DPos;

    }
    #endregion

    #region PunRPC
    //Dark의 스킬 구현
    [PunRPC]
    void TP()
    {
        Vector3 curPos = Dark.transform.position;
        SpriteRenderer SR = Dark.GetComponent<SpriteRenderer>();

        if (SR.flipX == true)
        {
            Dark.transform.position = curPos + new Vector3(-7f, 0, 0);
        }
        else if(SR.flipX == false)
        {
            Dark.transform.position = curPos + new Vector3(7f, 0, 0);
        }
    }

    [PunRPC]
    void DASH()
    {
        Vector2 dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        StartCoroutine(LerpDash(dir));
    }
    #endregion

}