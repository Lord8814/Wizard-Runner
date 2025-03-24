using UnityEngine;
using TMPro;

public class LevelTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText; // Référence au texte TMP
    public Transform player; // Référence au joueur ou à son transform
    public float movementThreshold = 0.1f; // Seuil pour détecter un mouvement

    private Vector3 lastPlayerPosition; // Dernière position du joueur
    private bool isTimerRunning = false; // État du chrono

    private void Start()
    {
        // Initialiser la position du joueur
        lastPlayerPosition = player.position;

        // Démarrer le chrono (pas encore compté)
        GameManager.Instance.StartLevelTimer(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    private void Update()
    {
        // Vérifie si le joueur a bougé
        if (!isTimerRunning && PlayerHasMoved())
        {
            isTimerRunning = true; // Le chrono démarre
        }

        // Si le chrono est actif, met à jour le temps
        if (isTimerRunning)
        {
            GameManager.Instance.UpdateLevelTimer();

            // Afficher le temps formaté
            float time = GameManager.Instance.GetCurrentLevelTime();
            timerText.text = FormatTime(time);
        }
    }

    private bool PlayerHasMoved()
    {
        // Calculer la distance entre la dernière position et la position actuelle
        float distance = Vector3.Distance(lastPlayerPosition, player.position);

        // Mettre à jour la dernière position
        lastPlayerPosition = player.position;

        // Retourne vrai si le joueur a dépassé le seuil
        return distance > movementThreshold;
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int milliseconds = Mathf.FloorToInt((time % 1) * 1000); // Extraire les millisecondes
        return $"{minutes:00}:{seconds:00}:{milliseconds:000}";
    }
}
