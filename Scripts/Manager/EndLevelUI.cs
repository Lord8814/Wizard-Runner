using UnityEngine;
using TMPro;

public class EndLevelUI : MonoBehaviour
{
    public TextMeshProUGUI currentTimeText; // Texte pour afficher le temps actuel
    public TextMeshProUGUI bestTimeText;    // Texte pour afficher le meilleur temps

    private void Start()
    {
        // Recharge les meilleurs temps pour s'assurer qu'ils sont à jour
        GameManager.Instance.ReloadBestTimes();

        // Récupérer le nom du dernier niveau terminé
        string levelName = GameManager.Instance.GetLastCompletedLevel();
        Debug.Log($"Last completed level: {levelName}");

        // Obtenir les temps
        float currentTime = GameManager.Instance.GetCurrentLevelTime();
        float bestTime = GameManager.Instance.GetBestTime(levelName);

        // Afficher les temps
        currentTimeText.text = "Your Time: " + FormatTime(currentTime);
        bestTimeText.text = "Best Time: " + (bestTime == float.MaxValue ? "N/A" : FormatTime(bestTime));
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int milliseconds = Mathf.FloorToInt((time % 1) * 1000);
        return $"{minutes:00}:{seconds:00}:{milliseconds:000}";
    }
}
