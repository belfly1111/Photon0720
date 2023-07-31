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

    public AudioClip[] walkAudio;
    public AudioClip[] jumpAudio;

    public bool DS = false;

    // 정밀한 점프를 위해 추가한 변수
    [SerializeField] private BoxCollider2D RB_groundCheck;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] int jumpSpeed = 5;

    // 대화 & 상호작용 관련 변수. 이때 inEvent변수와 curTextLine 변수는 정적 변수로 다른 곳에서도 참조할 수 있게 하였다.
    private InteractiveObject _interactiveObject;
    public InteractiveObject InteractiveObject { set { _interactiveObject = value; } }
    public static bool inEvent;


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
        if (PV.IsMine)
        {
            if(!inEvent)
            {
                isGround = IsGrounded();
                // ← → 이동
                float axis = Input.GetAxisRaw("Horizontal");
                RB.velocity = new Vector2(3 * axis, RB.velocity.y);

                if (axis != 0)
                {
                    // 재접속시 filpX를 동기화해주기 위해서 AllBuffered
                    PV.RPC("FlipXRPC", RpcTarget.AllBuffered, axis);
                    PV.RPC("SoundRPC", RpcTarget.All, 1);
                }

                // ↑ 점프, 바닥체크
                if (Input.GetKeyDown(KeyCode.Space) && isGround)
                {
                    PV.RPC("JumpRPC", RpcTarget.All);
                    PV.RPC("SoundRPC", RpcTarget.All,2);
                }

                //상호작용 - 07.28 상호 작용 중 다른 키의 입력을 못받게 수정함.
                if (Input.GetKeyDown(KeyCode.F))
                {
                    inEvent = true;
                    Interaction();
                }
            }
            else if(inEvent)
            {
                //상호작용 - 07.28 상호 작용 중 다른 키의 입력을 못받게 수정함.
                if (Input.GetKeyDown(KeyCode.F))
                {
                    Interaction();
                }
            }
        }
        else
        {
            if(!DS){
                transform.position = Vector3.Lerp(transform.position, currPos, Time.deltaTime * 10f);
                transform.rotation = Quaternion.Lerp(transform.rotation, currRot, Time.deltaTime * 10f);
            }
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

    [PunRPC]
    void SoundRPC(int type)
    {
        AudioSource audio = GetComponent<AudioSource>();
        if (type == 0)
        {
        //    audio.Stop();
        }
        if(type == 1)
        {
            if (!audio.isPlaying)
            {
                audio.clip = walkAudio[Random.Range(0,9)];
                audio.Play();
            }
        }
        if(type == 2)
        {
            if (!audio.isPlaying)
            {
                audio.clip = jumpAudio[Random.Range(0, 2)];
                audio.Play();
            }
        }
    }

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

    public void Interaction()
    {
        //만일 상호작용할 애들이 없다면 반환한다.
        if (_interactiveObject == null)
        {
            return;
        }

        _interactiveObject.Interaction();

    }
}