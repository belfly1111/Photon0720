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

    // 사운드 관리를 위해 추가한 변수
    public AudioClip[] walkAudio;
    public AudioClip[] jumpAudio;

    public bool DS = false;

    // 정밀한 점프를 위해 추가한 변수
    [SerializeField] private BoxCollider2D RB_groundCheck;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] int jumpSpeed = 5;

    // 대화 & 상호작용 관련 변수.
    private InteractiveObject _interactiveObject;
    public InteractiveObject InteractiveObject { set { _interactiveObject = value; } }
    [SerializeField] Vector3 previousPosition;
    public static bool inEvent;
    
    public bool aired;
    // 플레이어가 죽었을 때 추가 조작을 막기 위한 변수
    // 플레이어가 죽었을 때 세이브 포인트로 이동하기 위한 변수를 저장하는 변수
    public bool isDead;
    private Vector2 SavePointPosition;

    public bool isGround;
    //Vector3 curPos;

    void Awake()
    {
        SavePointPosition = new Vector2(-25, 0.5f);
        this.isDead = false;
        inEvent = false;
        isGround = true;
        aired = true;
    }

    void Start()
    {
        PV = this.GetComponent<PhotonView>();
    }

    void Update()
    {
        if (PV.IsMine)
        {
            if (this.isDead)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    transform.position = SavePointPosition;
                    isDead = false;
                }
                return;
            }

            if (!inEvent)
            {
                isGround = IsGrounded();
                // ← → 이동
                float axis = Input.GetAxisRaw("Horizontal");
                RB.velocity = new Vector2(3 * axis, RB.velocity.y);

                if (axis != 0)
                {
                    // 재접속시 filpX를 동기화해주기 위해서 AllBuffered
                    PV.RPC("FlipXRPC", RpcTarget.AllBuffered, axis);
                    if (isGround)
                    {
                        PV.RPC("SoundRPC", RpcTarget.All, 1);
                    }
                }

                // ↑ 점프, 바닥체크
                if (Input.GetKeyDown(KeyCode.Space) && isGround)
                {
                    aired = true;
                    PV.RPC("JumpRPC", RpcTarget.All);
                    PV.RPC("SoundRPC", RpcTarget.All, 0);
                }

                //상호작용 - 07.28 상호 작용 중 다른 키의 입력을 못받게 수정함.
                //상호작용 - 08.01 땅에 붙어있어야 대화가 가능함.
                if (Input.GetKeyDown(KeyCode.F) && isGround)
                {
                    inEvent = true;
                    previousPosition = gameObject.transform.position;
                    Interaction();
                }
            }
            else if (inEvent)
            {
                // 상호작용 - 07.28 상호 작용 중 다른 키의 입력을 못받게 수정함.
                // 상호작용 - 08.01 상호 작용 중 플레이어의 좌표를 고정함.
                // 만일 상호작용할 애들이 없다면 좌표를 고정시키지는 않는다.
                // 
                if (_interactiveObject != null)
                {
                    gameObject.transform.position = previousPosition;
                }
                else if (_interactiveObject == null)
                {
                    return;
                }

                if (Input.GetKeyDown(KeyCode.F))
                {
                    Interaction();
                }
            }
        }
        else
        {
            if (!DS)
            {
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
            audio.Stop();
        }
        if (type == 1)
        {
            if (!audio.isPlaying)
            {
                audio.clip = walkAudio[Random.Range(0, 9)];
                audio.Play();
            }
        }
        if (type == 2)
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
        else if (stream.IsReading)
        {
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
        }
    }

    public bool IsGrounded()
    {
        return Physics2D.BoxCast(RB_groundCheck.bounds.center, RB_groundCheck.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    public void Interaction()
    {
        _interactiveObject.Interaction();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DeadZone"))
        {
            this.isDead = true;
        }
        if (other.CompareTag("SavePoint"))
        {
            SavePointPosition = other.transform.position;
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && aired)
        {
            PV.RPC("SoundRPC", RpcTarget.All, 2);
            aired = false;
        }
    }
}