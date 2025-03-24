using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;         // Santé maximale de l'ennemi
    private int currentHealth;       // Santé actuelle
    private bool isFacingRight = true; // Indique si l'ennemi fait face à la droite
    private Animator animator;       // Référence à l'Animator
    private Rigidbody2D rb;          // Référence au Rigidbody2D pour obtenir la direction

    void Start()
    {
        currentHealth = maxHealth;   // Initialisation de la santé
        animator = GetComponent<Animator>(); // Récupérer l'Animator
        rb = GetComponent<Rigidbody2D>();   // Récupérer le Rigidbody2D (si nécessaire pour la direction)

        // Vérification de la présence des composants nécessaires
        if (animator == null)
        {
            Debug.LogError("Animator non trouvé sur l'ennemi !");
        }

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D non trouvé sur l'ennemi !");
        }
    }

    // Méthode appelée pour infliger des dégâts à l'ennemiaaa
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("Hit");    // Réduction de la santé
        Debug.Log($"Santé de l'ennemi : {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();                  // Appeler la méthode Die si la santé atteint 0
        }
    }

    // Gestion de la mort de l'ennemi
    void Die()
    {
        
        Debug.Log("L'ennemi est mort !");
        
        // Jouer l'animation de mort si l'Animator est présent
        if (animator != null)
        {
            animator.SetBool("Dead", true);
        }

        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = false; // Désactiver le collider
        }

        // Gérer l'orientation (flip) en fonction de la direction
        if (rb != null)
        {
            Vector2 direction = rb.velocity; // Obtenir la direction du mouvement

            if (direction.x > 0 && !isFacingRight) // Si l'ennemi se déplace vers la droite et regarde à gauche
            {
                Flip();
            }
            else if (direction.x < 0 && isFacingRight) // Si l'ennemi se déplace vers la gauche et regarde à droite
            {
                Flip();
            }
        }

        // Supprimer l'objet après un délai pour permettre à l'animation de se jouer
        Destroy(gameObject, 1.6f); // Délai de 1 seconde avant la destruction
    }

    // Inverser l'orientation de l'ennemi
    void Flip()
    {
        isFacingRight = !isFacingRight; // Inverser l'état d'orientation

        // Inverser l'échelle du GameObject pour simuler le flip visuel
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
