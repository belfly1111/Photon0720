using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class PlayerMove : MonoBehaviourPun
{
    // Send Rate를 조정할 PhotonTransformView
    [SerializeField]private PhotonTransformView photonTransformView;

    [SerializeField]private PhotonView PV;
    public SpriteRenderer SR;

    float axis = 0;
    float vertical = 0;

    public Rigidbody2D rigid;
    public bool isJump;
    Vector3 curPos;

    private void Awake()
    {
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
        photonTransformView = gameObject.GetComponent<PhotonTransformView>();
        rigid = gameObject.GetComponent<Rigidbody2D>();
        isJump = false;

        // Send Rate 값을 원하는 값으로 설정
        //photonTransformView.SendRate = 20; // 예시로 초당 20번의 패킷을 보내도록 설정
    }

    private void FixedUpdate()
    {
        Jump();
    }

    void Update()
    {
        defaultMove();
    }

    void defaultMove()
    {
        if (photonView.IsMine == true && PhotonNetwork.IsConnected == true)
        {
            axis = Input.GetAxisRaw("Horizontal");
            transform.Translate(new Vector3(axis * Time.deltaTime * 7, 0, 0));

            if (axis != 0) PV.RPC("FlipXRPC", RpcTarget.AllBuffered, axis);
        }

        // IsMine이 아닌 것들은 부드럽게 위치 동기화
        else if ((transform.position - curPos).sqrMagnitude >= 100) transform.position = curPos;
        else transform.position = Vector3.Lerp(transform.position, curPos, Time.deltaTime * 10);
    }

    void Jump()
    {
        if (Input.GetAxisRaw("Vertical") == 1 & photonView.IsMine == true && PhotonNetwork.IsConnected == true)
        {

            if (!isJump)
            {
                isJump = true;
                PV.RPC("JumpRPC", RpcTarget.AllBuffered);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isJump = false;
        }
    }

    [PunRPC]
    void FlipXRPC(float axis)
    {
        SR.flipX = axis == -1;
    }

    [PunRPC]
    void JumpRPC()
    {
        rigid.AddForce(Vector3.up * Time.deltaTime * 300f, ForceMode2D.Impulse);
    }

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