using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public Transform cameraTransform;  // La caméra à suivre
    public Vector2 parallaxFactor;    // Le facteur de parallaxe (x, y)
    
    private Vector3 previousCameraPosition;

    void Start()
    {
        // Enregistrer la position initiale de la caméra
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
        previousCameraPosition = cameraTransform.position;
    }

    void Update()
    {
        // Calculer la différence de mouvement de la caméra
        Vector3 deltaMovement = cameraTransform.position - previousCameraPosition;

        // Appliquer le mouvement au fond
        transform.position += new Vector3(deltaMovement.x * parallaxFactor.x, deltaMovement.y * parallaxFactor.y, 0);

        // Mettre à jour la position précédente de la caméra
        previousCameraPosition = cameraTransform.position;
    }
}
