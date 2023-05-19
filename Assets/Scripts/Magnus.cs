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
        // La formule de la force r�sultante de l'effet magnus est la suivante: F = 4/3 * PI * densit� air * rayonBalle^3 
        // La direction de la force r�sultante est orthogonale � la vitesse lin�aire et la vitesse angulaire
        // https://en.wikipedia.org/wiki/Magnus_effect
        var direction = Vector3.Cross(rb.angularVelocity, rb.velocity);
        var magnitude = 4 / 3f * Mathf.PI * densiteAir * Mathf.Pow(rayon, 3);
        rb.AddForce(magnitude * direction);
    }
}
 