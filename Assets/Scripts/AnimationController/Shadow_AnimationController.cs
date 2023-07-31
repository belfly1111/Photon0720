using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Shadow_AnimationController : MonoBehaviour
{
    private Animator Animator;
    public SpriteRenderer SR;
    public PhotonView PV;

    private float axis;
    public bool isGround;

    /// <summary>
    /// 아래 정적 변수 isDead_Dark는 플레이어가 죽었을 때 입력을 하지 못하게 막는 변수이다.
    /// 이미 movesetOrigin에 같은 역할을 하는 같은 이름의 변수가 존재한다. 
    /// 그러나 movesetOrigin의 isDead 정적 변수로 선언하면 같은 스크립트를 가진 Light, Dark 모두
    /// 둘 중 한명이 죽으면 스킬 발동, 이동이 모두 막히기 때문에 각 light, Dark에 따로 부착된 
    /// 이 애니메이션 컨트롤러 스크립트를 간접적으로 사용한다.
    /// </summary>
    public static bool isDead_Shadow;

    private void Awake()
    {
        isDead_Shadow = false;
        isGround = true;
    }

    void Start()
    {
        Animator = GetComponent<Animator>();
        SR = GetComponent<SpriteRenderer>();
        PV = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (PV.IsMine)
        {
            if (isDead_Shadow)
            {
                if (Input.GetKey(KeyCode.R))
                {
                    Animator.SetBool("isIdle", true);
                    Animator.SetBool("isWalking", false);
                    Animator.SetBool("isJumping", false);

                    isDead_Shadow = false;
                }
            }
            else
            {
                axis = Input.GetAxisRaw("Horizontal");
                if (axis != 0)
                {
                    Animator.SetBool("isWalking", true);
                    Animator.SetBool("isIdle", false);
                    //PV.RPC("WalkingAnimationRPC", RpcTarget.AllBuffered);
                }
                else
                {
                    Animator.SetBool("isWalking", false);
                    Animator.SetBool("isIdle", true);
                }

                // 점프 입력이 있었을 때, isJumping 상태를 활성화한다.
                if (Input.GetKeyDown(KeyCode.Space) && isGround)
                {
                    Animator.SetBool("isJumping", true);
                    Animator.SetBool("isIdle", false);
                    Animator.SetBool("isWalking", false);
                    // PV.RPC("JumpAnimationRPC", RpcTarget.AllBuffered);
                }
            }
        }
    }

    [PunRPC]
    void WalkingAnimationRPC()
    {
        Animator.SetBool("isWalking", true);
        Animator.SetBool("isIdle", false);
    }

    [PunRPC]
    void JumpAnimationRPC()
    {
        Animator.SetBool("isJumping", true);
        Animator.SetBool("isIdle", false);
        Animator.SetBool("isWalking", false);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            Animator.SetBool("isJumping", false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DeadZone"))
        {
            isDead_Shadow = true;
            Animator.SetTrigger("isDead");
        }
    }
}
