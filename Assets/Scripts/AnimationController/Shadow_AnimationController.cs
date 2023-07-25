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

    void Start()
    {
        Animator = GetComponent<Animator>();
        SR = GetComponent<SpriteRenderer>();   
        PV = GetComponent<PhotonView>();
    }

    void Update()
    {
        // 별다른 입력이 없으면 항상 idle 상태이다.
        // 또한 Walking 상태도 아니다.
        Animator.SetBool("isIdle", true);
        Animator.SetBool("isWalking", false);
        if (PV.IsMine)
        {
            axis = Input.GetAxisRaw("Horizontal");
            // 가로축에 어떠한 입력이 있었을 때, isWalking 상태를 활성화한다.
            if (axis != 0)
            {
                PV.RPC("WalkingAnimationRPC", RpcTarget.AllBuffered);
            }
            // 점프 입력이 있었을 때, isJumping 상태를 활성화한다.
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PV.RPC("JumpAnimationRPC", RpcTarget.AllBuffered);
            }
        }
    }

    [PunRPC]
    void WalkingAnimationRPC()
    {
        Animator.SetBool("isWalking", true);
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
