using UnityEngine;
 
[RequireComponent(typeof(Rigidbody))]
public class Magnus : MonoBehaviour
{
    public float radius = 0.5f;
    public float airDensity = 0.1f;
 
    private Rigidbody rb;
 
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
 
    void FixedUpdate()
    {
        var direction = Vector3.Cross(rb.angularVelocity, rb.velocity);
        var magnitude = 4 / 3f * Mathf.PI * airDensity * Mathf.Pow(radius, 3);
        rb.AddForce(magnitude * direction);
    }
}
 