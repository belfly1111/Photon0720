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

    private void Awake()
    {
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
            axis = Input.GetAxisRaw("Horizontal");
            if (axis != 0)
            {
                PV.RPC("WalkingAnimationRPC", RpcTarget.AllBuffered);
            }
            else
            {
                Animator.SetBool("isWalking", false);
                Animator.SetBool("isIdle", true);
            }

            // 점프 입력이 있었을 때, isJumping 상태를 활성화한다.
            if (Input.GetKeyDown(KeyCode.Space) && isGround)
            {
                PV.RPC("JumpAnimationRPC", RpcTarget.AllBuffered);
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
}
