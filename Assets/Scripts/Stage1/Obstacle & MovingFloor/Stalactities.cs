using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Stalactities : MonoBehaviourPun
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PhotonView PV;

    public Vector2 rayOrigin;
    public LayerMask playerLayer; // 플레이어를 감지하기 위한 레이어 마스크
    public AudioClip[] ac;

    private bool isFalling = false;

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
        rayOrigin = transform.position;
    }


    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, 5f, playerLayer);
        if(hit.collider == null) return;
        if (hit.collider.CompareTag("Player") && !isFalling)
        {
            AudioSource a = GetComponent<AudioSource>();
            a.clip = ac[0];
            a.Play();
            Debug.Log(hit.collider.name);
            isFalling = true;
            rb.gravityScale = 1; // 중력 적용하여 떨어지도록 함
        }
    }


    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Ground"))
        {
            AudioSource a = GetComponent<AudioSource>();
            a.clip = ac[1];
            Debug.Log("종유석 땅에 닿음 다시 제자리로.");
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
            gameObject.transform.position = rayOrigin;
            a.Play();
            isFalling = false;
        }
    }
}
