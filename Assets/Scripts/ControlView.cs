using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlView : MonoBehaviour
{
    [SerializeField] private float speed = 1;

    private float x;
    private float y;
    void Update()
    {
        y += speed * Input.GetAxis("Mouse X");
        x -= speed * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(x, y, 0f);
    }
}
