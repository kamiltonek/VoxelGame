using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform playerPosition;
    private float smoothSpeed = 0.5F;
    private Vector3 offset = new Vector3(2, 6, -6);
    private Vector3 velocity = Vector3.zero;
    private void Awake()
    {
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        Vector3 desiredPosition = playerPosition.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
    }
}
