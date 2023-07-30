using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveObj : MonoBehaviour
{
    public Rigidbody2D RB;
    public SpriteRenderer SR;

    // Update is called once per frame
    void Update()
    {
        float axis = Input.GetAxisRaw("Horizontal");
        float vert = Input.GetAxisRaw("Vertical");

        RB.velocity = new Vector2(axis,vert);
    }
}
