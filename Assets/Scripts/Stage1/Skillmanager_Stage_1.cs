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
<<<<<<< HEAD
        skillcool = false;
        PV = this.GetComponent<PhotonView>();
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.V))
        {
            if(Light == null) Light = GameObject.Find("Light(Clone)");
            if(Dark == null) Dark = GameObject.Find("Dark(Clone)");

            //Dark 와 Light가 동시에 존재해야 스킬사용
            if(Dark != null && Light != null) UseSkill();
        }
    }


    #region Nomalregion

    //스킬 사용
    public void UseSkill()
    {
        // 플레이어가 선택한게 없을 경우
        if (StageUI_1.LocalPlayerRule == -1) return;

        // 플레이어가 선택한게 'Light'인 경우
        else if (StageUI_1.LocalPlayerRule == 1 && !skillcool)
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
=======
>>>>>>> parent of 7e6ee60 (Shadow 능력생성.)
        Debug.Log("TP코드실행");
        StartCoroutine(CoolTimeCRT(7f));
    }

<<<<<<< HEAD
    IEnumerator Dash()
=======
    [PunRPC]
    private void Dash()
>>>>>>> parent of 7e6ee60 (Shadow 능력생성.)
    {
        Debug.Log("Dash코드실행");
        StartCoroutine(CoolTimeCRT(7f));
    }
    #endregion
<<<<<<< HEAD

    #region PunRPC
    //Dark의 스킬 구현
    [PunRPC]
    void TP(){
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
        Rigidbody2D RD = Light.GetComponent<Rigidbody2D>();

        if (SR.flipX == true)
        {
            RD.AddForce(new Vector2(DashPower, 20), ForceMode2D.Impulse);
        }
        else if (SR.flipX == false)
        {
            RD.AddForce(new Vector2(DashPower, 20), ForceMode2D.Impulse);
        }
    }

    #endregion
}
=======
}
>>>>>>> parent of 7e6ee60 (Shadow 능력생성.)
