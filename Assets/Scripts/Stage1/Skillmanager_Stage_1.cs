using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEditor;
using Cinemachine;
using System;

//빛: 발광, 대쉬
//그림자 : 지형 파괴, 텔레포트

public class Skillmanager_Stage_1 : MonoBehaviourPun
{
    public PhotonView PV;

    [SerializeField] GameObject Light;
    [SerializeField] GameObject Shadow;
    [SerializeField] CinemachineVirtualCamera VM;
    [SerializeField] private float dashingPower = 4f;
    [SerializeField] private float dashingTime = 0.2f;
    public bool skillcool = false;

    void Start()
    {
        Light = GameObject.Find("Light(Clone)");
        Shadow = GameObject.Find("Dark(Clone)");

        PV = this.GetComponent<PhotonView>();

        if(PhotonManeger.LocalPlayerRule == 1)
        {
            VM.Follow = Light.GetComponentInChildren<Transform>();
        }
        else if (PhotonManeger.LocalPlayerRule == 0)
        {
            VM.Follow = Shadow.GetComponentInChildren<Transform>();
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
        if (PhotonManeger.LocalPlayerRule == 1 && !skillcool)
        {
            StartCoroutine("Dash");
        }

        // 플레이어가 선택한게 'Dark'인 경우
        else if (PhotonManeger.LocalPlayerRule == 0 && !skillcool)
        {
            StartCoroutine("Teleport");
        }
    }
<<<<<<< Updated upstream

    //Dark 텔레포트 (구현중)
    #region  DarkSkill

    IEnumerator Teleport()
    {
        skillcool = true;
        Debug.Log("TP코드실행");
        PV.RPC("TP", RpcTarget.AllBuffered);
        yield return new WaitForSeconds(3f);

        skillcool = false;
        Debug.Log("스킬 재사용 가능!");
    }

    IEnumerator RCTeleport(){
        //TP 실행시 
        Rigidbody2D rb = Dark.GetComponent<Rigidbody2D>();
        yield return new WaitForSeconds(3f);

    }

    #endregion

    //Light 대쉬 (유리라는 오브젝트를 통해 추가적 대쉬를 할건지 결정 필요)
    #region LightSkill
=======
>>>>>>> Stashed changes
    IEnumerator Dash()
    {
        skillcool = true;
        Debug.Log("Dash코드실행");

        PV.RPC("DASH", RpcTarget.AllBuffered);
        yield return new WaitForSeconds(3f);

        skillcool = false;
        Debug.Log("스킬 재사용 가능!");
    }
    IEnumerator Teleport()
    {
        skillcool = true;
        Debug.Log("TP코드실행");
        PV.RPC("TP", RpcTarget.AllBuffered);
        yield return new WaitForSeconds(3f);

<<<<<<< Updated upstream
    
=======
        skillcool = false;
        Debug.Log("스킬 재사용 가능!");
    }
>>>>>>> Stashed changes
    IEnumerator LerpDash(Vector2 dir){
        Rigidbody2D rb = Light.GetComponent<Rigidbody2D>();
        Vector2 DPos = rb.position + dir.normalized * dashingPower;

        //layerMask에서 플레이어 레이어 삭제
        int layerMask = ~(1 << LayerMask.NameToLayer("Player"));
        //레이케스트 중심이 (0,0)일 경우 발에서 레이케스트를 쏘기 때문에 지하로 들어가는 것을 Vector2(0,0.5f)를 추가함으로 정 가운데서 레이케스트를 쏨
        RaycastHit2D RCh = Physics2D.Raycast(rb.position + new Vector2(0,0.5f), dir.normalized, dir.normalized.magnitude * dashingPower, layerMask);

        //레이캐스트를 통해 충돌한 경우 그 위치까지 설정
        if (RCh.collider != null)
        {
            DPos = RCh.point - new Vector2(0,0.5f);
            // 캐릭터의 콜라이더의 크기만큼 충돌후 위치를 수정함
            DPos.x -= dir.x * 0.375f;
            DPos.y -= dir.y * 0.5f;
        }

        //Lerp구문을 이용한 대쉬 모션 구현 
        float elapsedTime = 0f;
        while (elapsedTime < dashingTime){
            rb.position = Vector2.Lerp(rb.position, DPos, elapsedTime / dashingTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rb.position = DPos;
    }
    #endregion
    
    #endregion

    #region PunRPC
    //Dark의 스킬 구현
    [PunRPC]
    void TP()
<<<<<<< Updated upstream
    {   
        StartCoroutine(RCTeleport());
=======
    {
        Vector3 curPos = Shadow.transform.position;
        SpriteRenderer SR = Shadow.GetComponent<SpriteRenderer>();

        if (SR.flipX == true)
        {
            Shadow.transform.position = curPos + new Vector3(-7f, 0, 0);
        }
        else if(SR.flipX == false)
        {
            Shadow.transform.position = curPos + new Vector3(7f, 0, 0);
        }
>>>>>>> Stashed changes
    }

    [PunRPC]
    void DASH()
    {
        Vector2 dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        StartCoroutine(LerpDash(dir));
    }
    #endregion

}