using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject projectilePrefab; // Le prefab du projectile
    public Transform firePoint; // Le point où le projectile apparaît

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J)) // Modifier la touche selon ton choix
        {
            Attack();
        }
    }

    void Attack()
    {
        // Instancier le projectile
        
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // Ajuster la direction selon l'orientation du joueur
        ProjectileScript ps = projectile.GetComponent<ProjectileScript>();
        ps.speed *= transform.localScale.x; // Supposant que l'échelle X indique la direction du joueur

    }
}
