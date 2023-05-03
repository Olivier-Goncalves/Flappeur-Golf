using UnityEngine;
 
[RequireComponent(typeof(Rigidbody))]
public class Magnus : MonoBehaviour
{
    [SerializeField]
    private float rayon = 1f;
    [SerializeField]
    private float densiteAir = 0.3f;
 
    private Rigidbody rb;
 
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
 
    void FixedUpdate()
    {
        var direction = Vector3.Cross(rb.angularVelocity, rb.velocity);
        var magnitude = 4 / 3f * Mathf.PI * densiteAir * Mathf.Pow(rayon, 3);
        rb.AddForce(magnitude * direction);
    }
}
 