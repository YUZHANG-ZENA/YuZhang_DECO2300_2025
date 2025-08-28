using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float lookSpeed = 2f;

    private float yaw = 0f;
    private float pitch = 0f;

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            yaw += lookSpeed * Input.GetAxis("Mouse X");
            pitch -= lookSpeed * Input.GetAxis("Mouse Y");
            pitch = Mathf.Clamp(pitch, -80f, 80f);

            transform.eulerAngles = new Vector3(pitch, yaw, 0f);
        }

        float h = Input.GetAxis("Horizontal"); // A D
        float v = Input.GetAxis("Vertical");   // W S
        float upDown = 0f;

        if (Input.GetKey(KeyCode.E)) upDown = 1f; 
        if (Input.GetKey(KeyCode.Q)) upDown = -1f; 

        Vector3 dir = new Vector3(h, upDown, v);
        transform.Translate(dir * moveSpeed * Time.deltaTime);
    }
}

