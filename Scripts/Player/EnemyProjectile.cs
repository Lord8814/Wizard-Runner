using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour
{
    public float speed = 40f;
    public int damage = 1;
    private Animator animator;
    private bool hasCollided = false;

    void Start()
    {
        animator = GetComponent<Animator>(); // Assurer que l'animator est initialisé
        if (animator == null)
        {
            Debug.LogError("Animator non trouvé sur le projectile !");
        }

        // Lancer la coroutine qui gère la destruction après 10 secondes
        StartCoroutine(ExplosionTimer());
    }

    void Update()
    {
        // Déplacement du projectile
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        // Vérifier si le projectile doit être "flippé" en fonction de sa direction
        if (speed > 0 && transform.localScale.x < 0) // Si le projectile va à droite mais est orienté vers la droite
        {
            Flip();
        }
        else if (speed < 0 && transform.localScale.x > 0) // Si le projectile va à gauche mais est orienté vers la gauche
        {
            Flip();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasCollided) return; // Empêche de gérer plusieurs collisions

        // Si le projectile touche un ennemi
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyHealth>().TakeDamage(damage);
            speed = 0f;

            // Déclencher l'animation d'explosion
            if (animator != null)
            {
                animator.SetTrigger("hit");
            }

            Destroy(gameObject, 2f); // Détruire le projectile après l'animation
            hasCollided = true; // Marquer comme ayant eu une collision

            StopCoroutine(ExplosionTimer()); // Arrêter la coroutine de destruction automatique
        }
        // Si le projectile touche le sol ou un autre objet
        else if (collision.CompareTag("Ground"))
        {
            speed = 0f;
            if (animator != null)
            {
                animator.SetTrigger("hit");
            }
            Destroy(gameObject, 2f); // Détruire le projectile après l'animation
            hasCollided = true; // Marquer comme ayant eu une collision

            StopCoroutine(ExplosionTimer()); // Arrêter la coroutine de destruction automatique
        }
    }

    private IEnumerator ExplosionTimer()
    {
        yield return new WaitForSeconds(5f); // Attendre 10 secondes

        if (!hasCollided) // Vérifier si aucune collision n'a eu lieu
        {
            // Déclencher l'animation d'explosion
            if (animator != null)
            {
                animator.SetTrigger("hit");
            }

            Destroy(gameObject, 2f); // Détruire le projectile après l'animation
        }
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1; // Inverser l'échelle sur l'axe X
        transform.localScale = localScale;
    }
}
