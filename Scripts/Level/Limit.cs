using UnityEngine;

public class TeleportOnContact : MonoBehaviour
{
    private PlayerHealth PlayerHealth;
    // Position où le joueur sera téléporté
    public Vector3 teleportPosition = new Vector3(-33.12f, -5.49f, 0f);

    // Vérifie si l'objet entrant est le joueur
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Vérifie si l'objet qui entre en collision a le tag "Player"
        if (other.CompareTag("Player"))
        {
            // Téléporte le joueur à la position définie
            other.transform.position = teleportPosition;
            Debug.Log("Joueur téléporté à : " + teleportPosition);
        }
        if (gameObject.CompareTag("Limit"))
        {

            Debug.Log("Collision avec une Limit");
            PlayerHealth PlayerHealth = GetComponent<PlayerHealth>();
            
            if (PlayerHealth != null)
            {
                // Appelle la méthode TakeDamage du script PlayerHealth
                PlayerHealth.TakeDamage(1);
            }
         }
    }
}
