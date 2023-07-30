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
    [SerializeField] CinemachineVirtualCamera VM;
    [SerializeField] private float dashingPower = 4f;
    [SerializeField] private float dashingTime = 0.2f;
    public bool canSkill = true;
    public bool canPassive= true;
    public bool canDash = false;


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
        if (Input.GetKey(KeyCode.V))
        {
            UseSkill();
        }
        if(Input.GetKeyDown(KeyCode.K)){
            UniqueSkill();

        }
    }

    #region Nomalregion
    //스킬 사용
    public void UseSkill()
    {
        // 플레이어가 선택한게 'Light'인 경우
        if (PhotonManeger.LocalPlayerRule == 1 && canSkill && canDash)
        {
            StartCoroutine("Dash");
        }

        // 플레이어가 선택한게 'Dark'인 경우
        else if (PhotonManeger.LocalPlayerRule == 0 && canSkill)
        {
            return;
        }
    }

    public void UniqueSkill(){
        if(PhotonManeger.LocalPlayerRule == 1 && canPassive){
            StartCoroutine("flashTime");
        }

        else if(PhotonManeger.LocalPlayerRule == 0 && canPassive){
            StartCoroutine("Teleport");
        }
    }

    #endregion

    //Dark 텔레포트 (구현중)
    #region  DarkSkill
    IEnumerator Teleport()
    {
        canSkill = false;
        Debug.Log("TP코드실행");
        PV.RPC("TP", RpcTarget.AllBuffered);
        //TP하고 나서 스킬 쿨을 조정해야함
        yield return new WaitForSeconds(3f);
        canSkill = true;
        Debug.Log("스킬 재사용 가능!");
    }

    IEnumerator RCTeleport(Vector3 SPos)
    {
        //Sobj에 Shadow정보 저장
        GameObject Sobj = Shadow;

        //Shadow의 현재 위치에 "123" 프리팹 생성
        GameObject Dport = Shadow;
        Shadow = Instantiate(Dport,SPos,Quaternion.identity);
        Debug.Log("5초 시작");
        yield return new WaitForSeconds(5f);
        Debug.Log("5초 끝");
        //5초가 지난후 Shadow(123프리팹)의 위치를 가져와 Shadow에 Sobj를 넣고 텔레포트
        Vector3 TPos = Shadow.transform.position;
        //Shadow(123프리팹)오브젝트를 파괴하고 실행
        
        Shadow = Sobj;
        Shadow.transform.position = TPos;
        Destroy(GameObject.Find("123(Clone)"));

    }
// 구버전 위치 지정 텔레포트
    /*    IEnumerator Teleport()
        {
            canSkill = false;
            Debug.Log("TP코드실행");
            PV.RPC("TP", RpcTarget.AllBuffered);
            yield return new WaitForSeconds(3f);
            canSkill = true;
            Debug.Log("스킬 재사용 가능!");
        }*/

    #endregion

    //Light 대쉬 (유리라는 오브젝트를 통해 추가적 대쉬를 할건지 결정 필요)
    #region LightSkill
    IEnumerator Dash()
    {
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
        if (RCh.collider != null)
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
        Debug.Log("TP RPC 실행");
        //지금 조작하고 있는 Shadow의 움직임을 제한함.
        Rigidbody2D RB = Shadow.GetComponent<Rigidbody2D>();
        RB.constraints = RigidbodyConstraints2D.FreezeAll;
        Vector3 SPos = Shadow.transform.position;
        Debug.Log("RCTeleport 실행");
        StartCoroutine(RCTeleport(SPos));
        //Shadow움직임 제한 풀기
        RB.constraints = RigidbodyConstraints2D.FreezeRotation;
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