using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ȯ�濡���� namespace�̴�.
using Photon.Pun;
using Photon.Realtime;
using System.Reflection;
using UnityEditor;

public class DarkMove : MonoBehaviour
{
    public PhotonView PV;
    public SpriteRenderer SR;
    void Update()
    {
        if (PhotonManeger.Dark)
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
