using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            string levelName = SceneManager.GetActiveScene().name;

            GameManager.Instance.SetLastCompletedLevel(levelName);
            GameManager.Instance.EndLevelTimer();

            SceneManager.LoadScene("EndLevel"); // Assurez-vous que "EndLevel" est bien le nom exact de votre scène de fin
        }
    }
}
