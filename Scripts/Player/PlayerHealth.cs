using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;      // Santé maximale
    private int currentHealth;    // Santé actuelle
    [SerializeField] private Animator animator;


    void Start()
    {
        currentHealth = maxHealth; // Initialise la santé
        if (maxHealth <= 0)
        {
            Debug.LogWarning("La santé maximale n'est pas configurée correctement !");
            maxHealth = 1; // Valeur par défaut pour éviter les problèmes
        }

        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Le composant Animator est manquant !");
        }
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0)
        {
            Debug.LogWarning("Dégâts invalides reçus !");
            return;
        }

        currentHealth -= damage;
        animator.SetTrigger("Hit");
        NotifyDamage();

        Debug.Log($"Santé du joueur : {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public void Heal(int amount)
    {
        // Augmente la santé
        currentHealth += amount;

        Debug.Log($"Santé après soin : {currentHealth}");
        NotifyHealth();
    }

    void Die()
    {
        animator.SetTrigger("Dead");
        Debug.Log("Le joueur est mort !");
        SceneManager.LoadScene("MainMenu");
        // Logique supplémentaire pour Game Over ou réinitialisation

    }


    private void NotifyDamage()
    {
        // Parcourt tous les enfants et leur envoie un message
        foreach (Transform child in transform)
        {
            child.SendMessage("OnParentTakeDamage", SendMessageOptions.DontRequireReceiver);
        }
    }
    private void NotifyHealth()
    {
        // Parcourt tous les enfants et leur envoie un message
        foreach (Transform child in transform)
        {
            child.SendMessage("OnParentTakeHealth", SendMessageOptions.DontRequireReceiver);
        }
    }
}
