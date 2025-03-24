using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Dictionary<string, float> bestTimes = new Dictionary<string, float>();  // Meilleurs temps par niveau
    private float currentLevelTime;  // Temps du niveau actuel
    private string currentLevelName;  // Nom du niveau actuel
    private string lastCompletedLevelName;  // Nom du dernier niveau terminé
    private string playerName;  // Nom du joueur

    private string serverUrl = "https://yourserver.com/api/submit_score";  // URL de l'API pour envoyer les scores

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadBestTimes();  // Charger les meilleurs temps au démarrage
        }
        else
        {
            Destroy(gameObject);  // Assurer qu'il n'y a qu'une seule instance
        }
    }

    // Fonction pour demander le nom du joueur (par exemple via un UI ou une boîte de dialogue)
    public void SetPlayerName(string name)
    {
        playerName = name;
        Debug.Log($"Player name set to: {playerName}");
    }

    public void StartLevelTimer(string levelName)
    {
        currentLevelName = levelName;
        currentLevelTime = 0f;
        Debug.Log($"Timer started for level: {levelName}");
    }

    public void UpdateLevelTimer()
    {
        currentLevelTime += Time.deltaTime;
    }

    public void EndLevelTimer()
    {
        Debug.Log($"Timer ended for level: {currentLevelName} with time: {currentLevelTime}");

        // Sauvegarder le meilleur temps si c'est le nouveau meilleur
        if (!bestTimes.ContainsKey(currentLevelName) || currentLevelTime < bestTimes[currentLevelName])
        {
            bestTimes[currentLevelName] = currentLevelTime;
            SaveBestTime(currentLevelName, currentLevelTime);
            Debug.Log($"New best time saved for level: {currentLevelName}");
        }

        // Envoyer les scores au serveur une fois le niveau terminé
        SubmitScoreToServer();
    }

    public void SetLastCompletedLevel(string levelName)
    {
        lastCompletedLevelName = levelName;
        Debug.Log($"Last completed level set to: {levelName}");
    }

    public string GetLastCompletedLevel()
    {
        return lastCompletedLevelName;
    }

    public float GetBestTime(string levelName)
    {
        return bestTimes.ContainsKey(levelName) ? bestTimes[levelName] : float.MaxValue;
    }

    public float GetCurrentLevelTime()
    {
        return currentLevelTime;
    }

    private void SaveBestTime(string levelName, float time)
    {
        PlayerPrefs.SetFloat($"BestTime_{levelName}", time);
        PlayerPrefs.Save();
    }

    private void LoadBestTimes()
    {
        bestTimes.Clear();

        foreach (var scene in GetAllScenesInBuild())
        {
            if (PlayerPrefs.HasKey($"BestTime_{scene}"))
            {
                bestTimes[scene] = PlayerPrefs.GetFloat($"BestTime_{scene}");
                Debug.Log($"Loaded best time for {scene}: {bestTimes[scene]}");
            }
        }
    }

    public void ReloadBestTimes()
    {
        LoadBestTimes();
    }

    private List<string> GetAllScenesInBuild()
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        List<string> scenes = new List<string>();

        for (int i = 0; i < sceneCount; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = Path.GetFileNameWithoutExtension(scenePath);
            scenes.Add(sceneName);
        }

        return scenes;
    }

    // Méthode pour soumettre le score au serveur
    private void SubmitScoreToServer()
    {
        // Créer une instance de la classe pour envoyer les données via POST
        StartCoroutine(SubmitScoreCoroutine());
    }

    private IEnumerator SubmitScoreCoroutine()
    {
        // URL pour envoyer les données du score et du joueur
        string levelName = currentLevelName;
        float bestTime = bestTimes[currentLevelName];

        // Créer les données JSON
        string jsonData = JsonUtility.ToJson(new ScoreData(playerName, levelName, bestTime));

        // Créer une requête POST
        UnityWebRequest request = UnityWebRequest.Post(serverUrl, jsonData);
        request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData));
        request.SetRequestHeader("Content-Type", "application/json");

        // Attendre la réponse du serveur
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Score submitted successfully!");
        }
        else
        {
            Debug.LogError($"Error submitting score: {request.error}");
        }
    }

    // Classe pour structurer les données envoyées au serveur
    [System.Serializable]
    public class ScoreData
    {
        public string playerName;
        public string levelName;
        public float score;

        public ScoreData(string playerName, string levelName, float score)
        {
            this.playerName = playerName;
            this.levelName = levelName;
            this.score = score;
        }
    }
}
