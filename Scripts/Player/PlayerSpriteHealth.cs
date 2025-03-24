using UnityEngine;

public class PlayerSpriteHealth : MonoBehaviour
{
    public Sprite[] healthSprites; // Tableau des sprites correspondant aux niveaux de vie
    public int maxHealth = 5; // Vie maximale du joueur
    private int currentHealth;
    private SpriteRenderer spriteRenderer;

    private int lastHealth = -1; // Pour optimiser UpdateSprite

    void Start()
    {
        // Initialisation du SpriteRenderer
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer est manquant sur ce GameObject !");
            return;
        }

        // Vérifie que le tableau de sprites est bien configuré
        if (healthSprites.Length != maxHealth + 1)
        {
            Debug.LogError("Le tableau healthSprites doit contenir exactement maxHealth + 1 sprites !");
        }

        // Initialise la santé et le sprite
        currentHealth = maxHealth;
        UpdateSprite();
    }

    public void OnParentTakeDamage()
    {
        Debug.Log($"{gameObject.name} a détecté que son parent a subi des dégâts.");
        currentHealth -= 1;

        // Clamp pour éviter une santé négative
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateSprite();
    }

    public void OnParentTakeHealth()
    {
        Debug.Log($"{gameObject.name} a détecté que son parent a récupéré de la vie.");
        currentHealth += 1;

        // Clamp pour éviter de dépasser la santé maximale
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (currentHealth == lastHealth) return; // Pas de changement, inutile de mettre à jour
        lastHealth = currentHealth;

        if (currentHealth >= 0 && currentHealth <= maxHealth && healthSprites.Length > currentHealth)
        {
            spriteRenderer.sprite = healthSprites[currentHealth];
        }
        else
        {
            Debug.LogWarning("Le tableau de sprites n'est pas correctement configuré !");
        }
    }
}
