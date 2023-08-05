using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingFloor_Vertical : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    public float moveDistance = 5.0f;
    private Vector2 initialPosition;
    private bool movingUp = true;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        float newYPosition = Mathf.PingPong(Time.time * moveSpeed, moveDistance) + initialPosition.y;

        if (!movingUp)
        {
            newYPosition = initialPosition.y + moveDistance - newYPosition;
        }

        transform.position = new Vector2(transform.position.x, newYPosition);
    }
}
