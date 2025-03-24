using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 3f;          // Vitesse de déplacement
    public float detectionRange = 5f;    // Portée pour détecter le joueur
    public Transform player;             // Référence au joueur
    public int damage = 1;               // Dégâts infligés par l'ennemi
    private Rigidbody2D rb;
    private Animator animator;           // Référence à l'Animator du drone
    private Vector3 lastPosition;        // Dernière position du drone pour calculer la vitesse
    private bool isFacingRight = true;   // Indicateur pour savoir si le drone regarde vers la droite

    void Start()
    {
        // Récupère le Rigidbody2D
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();  // Récupère l'Animator

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D manquant sur " + gameObject.name);
        }

        // Cherche automatiquement le joueur si aucune référence n'est donnée
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
            if (player == null)
            {
                Debug.LogError("Aucun joueur trouvé avec le tag 'Player'.");
            }
        }

        lastPosition = transform.position;  // Initialisation de la dernière position
    }

    void Update()
    {
        // Si player est assigné, on fait les vérifications
        if (player != null)
        {
            // Vérifie si le joueur est dans la portée de détection
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= detectionRange)
            {
                // Se déplace vers le joueur
                Vector2 direction = (player.position - transform.position).normalized;
                rb.velocity = direction * moveSpeed;

                

                // Calculer la vitesse à partir de la vélocité du Rigidbody
                float speed = rb.velocity.magnitude;

                // Si la vitesse est inférieure à un seuil très bas, considère-le comme 0
                if (speed < 0.05f)
                    {
                      speed = 0;
                    }

                // Mettre à jour le paramètre "Speed" dans l'Animator
                animator.SetFloat("Speed", speed);
                


                // Gérer le flip en fonction du déplacement sur l'axe X
                if (direction.x > 0 && !isFacingRight) // Si le drone va vers la droite et qu'il ne regarde pas vers la droite
                {
                    Flip();
                }
                else if (direction.x < 0 && isFacingRight) // Si le drone va vers la gauche et qu'il regarde vers la droite
                {
                    Flip();
                }
            }
            else
            {
                rb.velocity = Vector2.zero; // Arrête le drone si le joueur est hors de portée
                animator.SetFloat("Speed", 0); // L'animation doit être arrêtée si le drone ne se déplace pas
            }
        }
        else
        {
            Debug.LogWarning("Le joueur n'a pas été trouvé. Le script ne peut pas fonctionner correctement.");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Vérifie si le joueur possède un composant PlayerHealth
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            else
            {
                Debug.LogWarning("Le joueur ne possède pas de composant PlayerHealth.");
            }
        }
    }

    // Fonction pour effectuer le flip du drone
    void Flip()
    {
        // Inverser la direction du mouvement (sur l'axe X)
        isFacingRight = !isFacingRight;

        // Inverser l'échelle du drone pour simuler le flip visuel
        Vector3 localScale = transform.localScale;
        localScale.x *= -1; // Inverser l'échelle sur l'axe X
        transform.localScale = localScale; // Appliquer le changement
    }
}
