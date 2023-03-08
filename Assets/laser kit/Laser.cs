using UnityEngine;
using UnityEngine.Serialization;

public class Laser : MonoBehaviour
{

    private int bondMax = 20;

    private int compteur ;
    private LineRenderer laser;

    [FormerlySerializedAs("offSet")] [SerializeField]
    private Vector3 Vecteur;

    private void Start()
    {
        laser = GetComponent<LineRenderer>();
        laser.positionCount = 20;
    }
    private void Update()
    {
        compteur = 0;
        castLaser(transform.position + Vecteur, transform.up);
    }
    private void castLaser(Vector3 position , Vector3 direction)
    {
        laser.SetPosition(0, transform.position + Vecteur );
       
        for (int i=0; i< bondMax; i++ )
        {
            
            Ray rayon = new Ray(position, direction);
            RaycastHit hit;
            
            if(compteur < bondMax - 1)
                compteur++;
                //Debug.Log(_count);
            if(Physics.Raycast(rayon , out hit , 300 ))
            {
                
                position = hit.point;
                direction = Vector3.Reflect(direction, hit.normal);
                laser.SetPosition(compteur, hit.point);

                if (hit.transform.tag != "Mirror")
                {
                   
                    for (int j = (i + 1); j < bondMax; j++)
                    {
                        laser.SetPosition(j, hit.point);

                    }
                    break;
                }
                else 
                {
                   
                    laser.SetPosition(compteur, hit.point);
                }
            }
            else
            {
                
                laser.SetPosition(compteur, rayon.GetPoint(300));
              
            }
                

        }
      
    }
   
}
