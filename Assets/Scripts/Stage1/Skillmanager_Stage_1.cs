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
    public PhotonView PV;

    [SerializeField] GameObject Light;
    [SerializeField] GameObject Dark;
    [SerializeField] float DashPower = 500;
    bool skillcool;

    void Start()
    {
        Light = GameObject.Find("Light(Clone)");
        Dark = GameObject.Find("Dark(Clone)");
        skillcool = false;
        PV = this.GetComponent<PhotonView>();
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
        yield return new WaitForSeconds(7f);

        skillcool = false;
        Debug.Log("스킬 재사용 가능!");
    }
    IEnumerator Dash()
    {
        skillcool = true;
        Debug.Log("Dash코드실행");
        PV.RPC("DASH", RpcTarget.AllBuffered);
        yield return new WaitForSeconds(7f);
        Light.GetComponent<Rigidbody2D>().gravityScale = 1f;

        skillcool = false;
        Debug.Log("스킬 재사용 가능!");
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
            Dark.transform.position = curPos + new Vector3(-70f, 0, 0);
        }
        else if(SR.flipX == false)
        {
            Dark.transform.position = curPos + new Vector3(70f, 0, 0);
        }
    }

    //Light의 스킬 구현
    [PunRPC]
    void DASH()
    {
        SpriteRenderer SR = Light.GetComponent<SpriteRenderer>();
        Rigidbody2D RB = Light.GetComponent<Rigidbody2D>();

        if (SR.flipX == true)
        {
            RB.gravityScale = 0f;
            RB.velocity = new Vector2(Light.transform.localScale.x * -24f, 0f);
        }
        else if (SR.flipX == false)
        {
            RB.gravityScale = 0f;
            RB.velocity = new Vector2(Light.transform.localScale.x * 24f, 0f);
        }
    }

    #endregion
}