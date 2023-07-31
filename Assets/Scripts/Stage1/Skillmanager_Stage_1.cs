using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEditor;
using Cinemachine;
using System;
using UnityEngine.Rendering.Universal;

//빛: 발광, 대쉬
//그림자 : 지형 파괴, 텔레포트

public class Skillmanager_Stage_1 : MonoBehaviourPun
{
    [SerializeField] PhotonView PV;
    [SerializeField] GameObject Light;
    [SerializeField] GameObject Shadow;
    [SerializeField] GameObject Dport;
    [SerializeField] CinemachineVirtualCamera VM;
    [SerializeField] private float dashingPower = 4f;
    [SerializeField] private float dashingTime = 0.2f;
    public bool canSkill = true;
    public bool canPassive= true;
    public bool canDash = false;
    public moveSetOrigin mso;


    void FindPrefab()
    {
        Light = GameObject.Find("Light(Clone)");
        Shadow = GameObject.Find("Dark(Clone)");
    }

    void FindPrefabCam()
    {
        if (PhotonManeger.LocalPlayerRule == 1)
        {
            VM.Follow = Light.transform.GetChild(0);
        }
        else if (PhotonManeger.LocalPlayerRule == 0)
        {
            VM.Follow = Shadow.transform.GetChild(0);
        }
    }

    void Start()
    {
        Invoke("FindPrefab", 1f);
        Invoke("FindPrefabCam", 1f);
        PV = this.GetComponent<PhotonView>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.V) && !moveSetOrigin.inEvent)
        {
            UseSkill();
        }
        if(Input.GetKeyDown(KeyCode.K) && !moveSetOrigin.inEvent)
        {
            UniqueSkill();
        }
    }

    #region Nomalregion
    //스킬 사용
    public void UseSkill()
    {
        // 플레이어가 선택한게 'Light'인 경우
        if (PhotonManeger.LocalPlayerRule == 1 && canSkill && canDash && !Light_AnimationController.isDead_Light)
        {
            StartCoroutine("Dash");
        }

        // 플레이어가 선택한게 'Dark'인 경우
        else if (PhotonManeger.LocalPlayerRule == 0 && canSkill && !Shadow_AnimationController.isDead_Shadow)
        {
            return;
        }
    }

    public void UniqueSkill(){

        if (PhotonManeger.LocalPlayerRule == 1 && canPassive && !Light_AnimationController.isDead_Light)
        {
            StartCoroutine("flashTime");
        }

        else if (PhotonManeger.LocalPlayerRule == 0 && canPassive && !Shadow_AnimationController.isDead_Shadow)
        {
            moveSetOrigin mso = Shadow.GetComponent<moveSetOrigin>();
            Debug.Log("고유스킬 시작");
            if (mso.isGround)
            {
                Debug.Log("텔포시작");
                StartCoroutine("Teleport");
            }
            else Debug.Log("텔포실패 이유: isGround = " + mso.isGround);
        }

        else if (PhotonManeger.LocalPlayerRule == 0 && canPassive && !Shadow_AnimationController.isDead_Shadow)
        {
            StartCoroutine("Teleport");
        }
    }
    #endregion

    //Dark 텔레포트 (구현중)
    #region  DarkSkill
    IEnumerator Teleport()
    {   
        if(!PV.IsMine) yield return null;

        canPassive = false;
        Rigidbody2D RB = Shadow.GetComponent<Rigidbody2D>();

        //TP에서는 위치만 가져오도록 하기 + 캐릭터 안보이게
        PV.RPC("TP", RpcTarget.AllBuffered);

        //Shadow 위치 포지션 가져오고 위치 바꾸기 시작
        Vector3 SPos = Shadow.transform.position;
        StartCoroutine(SetMark(SPos));

        yield return new WaitForSeconds(8f);
        canPassive = true;
        Debug.Log("스킬 재사용 가능!");
    }

    IEnumerator SRdisabled(Rigidbody2D RB){
        moveSetOrigin mso = Light.GetComponent<moveSetOrigin>();
        SpriteRenderer SR = Shadow.GetComponent<SpriteRenderer>();
        SR.enabled = false;
        yield return new WaitForSeconds(5.3f);
        SR.enabled = true;
        mso.DS = false;
        RB.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    IEnumerator SetMark(Vector3 SPos)
    {   
        GameObject Marker = Instantiate(Dport,SPos,Quaternion.identity);
        GameObject TMP = Shadow;
        Shadow = Marker;
        yield return new WaitForSeconds(5f);
        
        Vector2 MP = Shadow.transform.position;
        Shadow = TMP;
        Shadow.transform.position = MP;
        Destroy(Marker);
        //Shadow움직임 제한 풀기
    }
    #endregion

    //Light 대쉬 (유리라는 오브젝트를 통해 추가적 대쉬를 할건지 결정 필요)
    #region LightSkill
    IEnumerator Dash()
    {
        if(!PV.IsMine) yield return null;
        canSkill = false;
        Debug.Log("Dash코드실행");

        PV.RPC("DASH", RpcTarget.AllBuffered);
        yield return new WaitForSeconds(3f);

        canSkill = true;
        Debug.Log("스킬 재사용 가능!");
    }
    
    IEnumerator LerpDash(Vector2 dir)
    {
        Rigidbody2D rb = Light.GetComponent<Rigidbody2D>();
        Vector2 DPos = rb.position + dir.normalized * dashingPower;

        //layerMask에서 플레이어 레이어 삭제
        int layerMask = ~(1 << LayerMask.NameToLayer("Player"));
        //레이케스트 중심이 (0,0)일 경우 발에서 레이케스트를 쏘기 때문에 지하로 들어가는 것을 Vector2(0,0.5f)를 추가함으로 정 가운데서 레이케스트를 쏨
        RaycastHit2D RCh = Physics2D.Raycast(rb.position + new Vector2(0, 0.5f), dir.normalized, dir.normalized.magnitude * dashingPower, layerMask);

        //레이캐스트를 통해 충돌한 경우 그 위치까지 설정
        if (RCh.collider != null && !RCh.collider.CompareTag("Trigger")) 
        {
            DPos = RCh.point - new Vector2(0, 0.5f);
            // 캐릭터의 콜라이더의 크기만큼 충돌후 위치를 수정함
            DPos.x -= dir.x * 0.375f;
            DPos.y -= dir.y * 0.5f;
        }

        //Lerp구문을 이용한 대쉬 모션 구현 
        float elapsedTime = 0f;
        while (elapsedTime < dashingTime)
        {
            rb.position = Vector2.Lerp(rb.position, DPos, elapsedTime / dashingTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rb.position = DPos;
    }

    IEnumerator flashTime(){
        
        canPassive = false;
        Debug.Log("패시브 쿨 Off");
        PV.RPC("Flash",RpcTarget.AllBuffered);
        Debug.Log("Flash실행");
        yield return new WaitForSeconds(3f);
        PV.RPC("Flash",RpcTarget.AllBuffered);
        canPassive = true;
        Debug.Log("패시브 쿨 On");
    }

    
    #endregion


    #region PunRPC
    //Dark 고유스킬 텔레포트
    [PunRPC]
    void TP()
    {
        moveSetOrigin mso = Light.GetComponent<moveSetOrigin>();
        Rigidbody2D RB = Shadow.GetComponent<Rigidbody2D>();
        mso.DS = true;
        
        StartCoroutine(SRdisabled(RB));

        RB.constraints = RigidbodyConstraints2D.FreezeAll;
    }


    //Light 스킬 대쉬
    [PunRPC]
    void DASH()
    {
        Vector2 dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        StartCoroutine(LerpDash(dir));
    }

    //Light 고유스킬 발광
    [PunRPC]
    void Flash(){
        GameObject BL = Light.transform.GetChild(1).gameObject;
        BL.SetActive(!BL.activeSelf);
    }

    #endregion
}