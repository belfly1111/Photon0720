using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingFloor : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    public float moveDistance = 5.0f;
    private Vector2 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        // 수평으로 이동하는 코드 예시
        float newPositionX = Mathf.PingPong(Time.time * moveSpeed, moveDistance) + initialPosition.x;
        transform.position = new Vector2(newPositionX, transform.position.y);
    }
}
