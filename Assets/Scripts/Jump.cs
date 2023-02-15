using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Jump : MonoBehaviour
{
    [SerializeField] private float jumpStrength = 100;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Vector3 velocity = _rigidbody.velocity;
            _rigidbody.velocity = velocity / 1.5f - new Vector3(0, velocity.y / 2, 0);
            _rigidbody.AddRelativeForce(new Vector3(0, jumpStrength * 10, jumpStrength * 12));
        }
    }
}
