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

    // 정밀한 점프를 위해 추가한 변수
    [SerializeField] private BoxCollider2D RB_groundCheck;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] int jumpSpeed = 5;

    public bool isGround;
    Vector3 curPos;

    void Awake()
    {
        isGround = true;
    }

    void Start()
    {
        PV = this.GetComponent<PhotonView>();
    }

    void Update()
    {
        if (PV.IsMine)
        {
            // ← → 이동
            float axis = Input.GetAxisRaw("Horizontal");
            RB.velocity = new Vector2(3 * axis, RB.velocity.y);

            if (axis != 0) PV.RPC("FlipXRPC", RpcTarget.AllBuffered, axis);
            // 재접속시 filpX를 동기화해주기 위해서 AllBuffered

            // ↑ 점프, 바닥체크
            if (Input.GetKeyDown(KeyCode.Space) && IsGrounded()) PV.RPC("JumpRPC", RpcTarget.All);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, currPos, Time.deltaTime * 10f);
            transform.rotation = Quaternion.Lerp(transform.rotation, currRot, Time.deltaTime * 10f);
        }

        // IsMine이 아닌 것들은 부드럽게 위치 동기화
        //else if ((transform.position - curPos).sqrMagnitude >= 100) transform.position = curPos;
        //else transform.position = Vector3.Lerp(transform.position, curPos, Time.deltaTime * 10);
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
        RB.velocity = new Vector2(RB.velocity.x, jumpSpeed);
        
        // 기존 사용 코드
        //RB.velocity = Vector2.zero;
        //RB.AddForce(Vector2.up * 350);
    }


    [PunRPC]
    void DestroyRPC() => Destroy(gameObject);


    private Vector3 currPos;
    private Quaternion currRot;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);

        }
        else if(stream.IsReading)
        {
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
        }
    }

/*    //isground 바닥 체크 - 기존 사용하던 코드
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGround = true;
        }
    }*/

    //isground 바닥 체크2 - 그러나 Shadow_Animation_Controller.cs 는 여전히 OnCollisionEnter 방식을 사용중.
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(RB_groundCheck.bounds.center, RB_groundCheck.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}