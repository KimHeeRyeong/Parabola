using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] float speed = 3.0f;
    // Update is called once per frame
    void Update()
    {
        transform.position += (Input.GetAxis("Vertical") * transform.forward
            + Input.GetAxis("Horizontal") * transform.right)*Time.deltaTime*speed;
    }
}
