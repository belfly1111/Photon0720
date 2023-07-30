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

    //상호작용하는 인스턴스의 정보를 저장
    private InteractiveObject _interactiveObject;

    // 정밀한 점프를 위해 추가한 변수
    [SerializeField] private BoxCollider2D RB_groundCheck;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] int jumpSpeed = 5;

    // 대화 & 상호작용 관련 변수
    public InteractiveObject InteractiveObject { set { _interactiveObject = value; } }
    private bool inEvent;

    public bool isGround;
    Vector3 curPos;

    void Awake()
    {
        inEvent = false;
        isGround = true;
    }

    void Start()
    {
        PV = this.GetComponent<PhotonView>();
    }

    void Update()
    {
        if (PV.IsMine && !inEvent)
        {
            // ← → 이동
            float axis = Input.GetAxisRaw("Horizontal");
            RB.velocity = new Vector2(3 * axis, RB.velocity.y);

            if (axis != 0) PV.RPC("FlipXRPC", RpcTarget.AllBuffered, axis);
            // 재접속시 filpX를 동기화해주기 위해서 AllBuffered

            // ↑ 점프, 바닥체크
            if (Input.GetKeyDown(KeyCode.Space) && IsGrounded()) PV.RPC("JumpRPC", RpcTarget.All);

            //상호작용 - 07.28 상호 작용 중 다른 키의 입력을 못받게 수정함.
            if (Input.GetKeyDown(KeyCode.F))
            {
                inEvent = true;
                Interaction();
                inEvent = false;
            }
            if(RB.name == "123(Clone)"){
                float ver = Input.GetAxisRaw("Vertical");
                RB.velocity = new Vector2(axis,ver);
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, currPos, Time.deltaTime * 10f);
            transform.rotation = Quaternion.Lerp(transform.rotation, currRot, Time.deltaTime * 10f);
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
        isGround = false;
        RB.velocity = new Vector2(RB.velocity.x, jumpSpeed);
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

    //isground 바닥 체크 - 그러나 Shadow_Animation_Controller.cs 는 여전히 OnCollisionEnter 방식을 사용중.
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(RB_groundCheck.bounds.center, RB_groundCheck.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
    private void Interaction()
    {
        //만일 상호작용할 애들이 없다면 반환한다.
        if(_interactiveObject == null)
        {
            return;
        }

        _interactiveObject.Interaction();
    }
}