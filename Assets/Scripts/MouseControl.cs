using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseControl : MonoBehaviour
{
    [SerializeField] private float speed = 1;

    private float x;
    private float y;

    void Update()
    {
        Cursor.lockState = CursorLockMode.Confined;
        x += speed * Input.GetAxis("Mouse X");
        y -= speed * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(0, x, 0f);
    }
}
