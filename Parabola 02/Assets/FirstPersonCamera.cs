using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
       private const float Y_ANGLE_MIN = -50.0f;
    private const float Y_ANGLE_MAX = 50.0f;

    public float pitchSpeed = 50.0f;
    public float yawSpeed = 50.0f;
    private float currentX = 0.0f;
    private float currentY = 15.0f;

    private void Update()
    {
        currentX += Input.GetAxis("Mouse X")* pitchSpeed * Time.deltaTime;
        currentY += Input.GetAxis("Mouse Y") * yawSpeed* Time.deltaTime;

        currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
        
    }

    private void LateUpdate()
    {
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        transform.rotation = rotation;
    }
}
