using Unity.Netcode;
using UnityEngine;

public class CollisionMultijoueur : NetworkBehaviour
{
    // Layers
    private static int StickyZoneLayer = 6;
    private static int AcidZoneLayer = 7;
    private static int TrouLayer = 8;
    private const int layerBouleDeFeu = 9;
    private static int ondeLayer = 14;
    // Matériaux
    private bool seDissout;
    private bool seConsolide;
    private float alpha = -1.1f;
    private Material materiel;
    // Sons
    [SerializeField] private AudioSource mortSFX;
    [SerializeField] private AudioSource finNiveauSFX;
    [SerializeField] private AudioSource ressuscitementSFX;
    // Composants joueur
    private Saut composantSaut;
    private Rigidbody rb;
    // Logique jeu
    private GestionJeuMultijoueur gestionnaireJeu;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        materiel = GetComponent<Renderer>().material;
        composantSaut = GetComponent<Saut>();
        gestionnaireJeu = GameObject.Find("GestionnaireJeu").GetComponent<GestionJeuMultijoueur>();
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkDespawn();
        AjusterCollisionsJoueursClientRpc();
        AjusterCollisionJoueurLocal();
    }
    
    private void AjusterCollisionJoueurLocal()
    {
        var listeJoueurs = GameObject.FindGameObjectsWithTag("Player");
        foreach (var joueur in listeJoueurs)
        {
            for (int i = 0; i < listeJoueurs.Length; i++)
            {
                if (!(joueur == listeJoueurs[i]))
                {
                    Physics.IgnoreCollision(joueur.GetComponent<Collider>(),listeJoueurs[i].GetComponent<Collider>());
                }
            }
        }
    }
    
    [ClientRpc]
    private void AjusterCollisionsJoueursClientRpc()
    {
        AjusterCollisionJoueurLocal();
    }
    
    void Update()
    {
        if (seDissout)
        {
            alpha += Time.deltaTime;
            materiel.SetFloat("_Alpha", alpha);
            if (alpha >= 1f)    
            {
                seDissout = false;
                Ressusciter();
            }
        }
        if (seConsolide)
        {
            composantSaut.enabled = false;
            materiel.SetColor("_DissolveColor", materiel.GetColor("_Color"));
            alpha -= Time.deltaTime;
            materiel.SetFloat("_Alpha", alpha);
            if (alpha <= -1.1f)
            {
                seConsolide = false;
                alpha = -1.1f;
                materiel.SetFloat("_Alpha", alpha);
                composantSaut.enabled = true;
            }
        }
    }
    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        int collidedLayer = collision.contacts[0].otherCollider.gameObject.layer;
        if (collidedLayer == StickyZoneLayer)
        {
            rb.velocity = Vector3.zero;
            rb.useGravity = false;
        }
        else if (collidedLayer == AcidZoneLayer || collidedLayer == 14)
        {
            mortSFX.Play();
            materiel.SetColor("_DissolveColor", materiel.GetColor("_AcidDissolveColor"));
            rb.velocity = Vector3.zero;
            rb.useGravity = false;
            seDissout = true;
            composantSaut.enabled = false;
        }
        else if (collidedLayer == TrouLayer)
        {
            finNiveauSFX.Play();
            rb.velocity = Vector3.zero;
            rb.useGravity = false;
            gestionnaireJeu.ArriverTrou((int)OwnerClientId);
            composantSaut.enabled = false;
        }
        else if (collidedLayer == layerBouleDeFeu)
        {
            mortSFX.Play();
            composantSaut.enabled = false;
            materiel.SetColor("_DissolveColor", materiel.GetColor("_FireDissolveColor"));
            rb.velocity = Vector3.zero;
            rb.useGravity = false;
            seDissout = true;
        }
        else if (collidedLayer == ondeLayer)
        {
            Vector3 force = collision.transform.parent.rotation.eulerAngles * 2;
            rb.AddRelativeForce(force);
            Debug.Log(force);
        }
    }
    private void OnCollisionExit(UnityEngine.Collision other)
    {
        rb.useGravity = true;
    }
    private void Ressusciter()
    {
        ressuscitementSFX.Play();
        gestionnaireJeu.Ressusciter(transform);
        transform.rotation = Quaternion.Euler(0, -90, 0);
        seConsolide = true;
    }
    public void CollisionLaser()
    {
        mortSFX.Play();
        ChangerCouleurApparition("_LaserDissolveColor");
        Détruire();
    }
    private void Détruire()
    {
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        seDissout = true;
        composantSaut.enabled = false; 
    }
    private void ChangerCouleurApparition(string couleur) => materiel.SetColor("_DissolveColor", materiel.GetColor(couleur));
}
