using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitRonce : MonoBehaviour
{
void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("Ronce")) // Vérifie le type d'objet touché
    {
        // Récupérer la normale de la collision
        Vector2 collisionNormal = collision.contacts[0].normal;

        // Calculer la force de répulsion
        float intensity = 10f; // Ajustez selon l'effet désiré
        Vector2 repulsionForce = -collisionNormal * intensity;

        // Appliquer la force à l'objet
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(repulsionForce, ForceMode2D.Impulse);
        }
    }
}

}
