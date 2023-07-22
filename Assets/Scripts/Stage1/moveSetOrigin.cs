using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class moveSetOrigin : MonoBehaviourPunCallbacks, IPunObservable
{
    public Rigidbody2D RB;
    public SpriteRenderer SR;
    public PhotonView PV;


    public bool isGround;
    Vector3 curPos;

    void Awake()
    {
        isGround = true;
    }

    void Update()
    {
        if (PV.IsMine)
        {
            // ← → 이동
            float axis = Input.GetAxisRaw("Horizontal");
            RB.velocity = new Vector2(4 * axis, RB.velocity.y);

            if (axis != 0) PV.RPC("FlipXRPC", RpcTarget.AllBuffered, axis); // 재접속시 filpX를 동기화해주기 위해서 AllBuffered
            
            // ↑ 점프, 바닥체크
            //isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(0, -0.5f), 0.07f, 1 << LayerMask.NameToLayer("Ground"));
            if (Input.GetKeyDown(KeyCode.Space) && isGround) PV.RPC("JumpRPC", RpcTarget.All);
        }
        // IsMine이 아닌 것들은 부드럽게 위치 동기화
        else if ((transform.position - curPos).sqrMagnitude >= 100) transform.position = curPos;
        else transform.position = Vector3.Lerp(transform.position, curPos, Time.deltaTime * 10);
    }


    [PunRPC]
    void FlipXRPC(float axis)
    {
        SR.flipX = axis == -1;
    }

    [PunRPC]
    void JumpRPC()
    {
        isGround = false;
        RB.velocity = Vector2.zero;
        RB.AddForce(Vector2.up * 700);
    }


    [PunRPC]
    void DestroyRPC() => Destroy(gameObject);


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else
        {
            curPos = (Vector3)stream.ReceiveNext();
        }
    }
}