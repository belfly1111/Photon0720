using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Photon.Pun;
using Photon.Realtime;
using System.Reflection;
using UnityEditor;

public class PlayerMove : MonoBehaviourPunCallbacks
{
    public PhotonView PV;
    public SpriteRenderer SR;

    void Update()
    {
        if (photonView.IsMine == true && PhotonNetwork.IsConnected == true)
        {
            float axis = Input.GetAxisRaw("Horizontal");
            transform.Translate(new Vector3(axis * Time.deltaTime * 7, 0, 0));

            if (axis != 0) PV.RPC("FlipXRPC", RpcTarget.AllBuffered, axis);
        }
    }

    [PunRPC]
    void FlipXRPC(float axis)
    {
        SR.flipX = axis == -1;
    }
}