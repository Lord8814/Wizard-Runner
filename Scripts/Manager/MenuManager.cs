using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        // Charge la première scène du jeu
        SceneManager.LoadScene("Level0");
    }

    public void OpenOptions()
    {
        // Exemple : Affiche un panneau d'options (à implémenter)
        Debug.Log("Options ouvertes !");
    }

    public void QuitGame()
    {
        // Quitte le jeu (ne fonctionne que dans un build)
        Debug.Log("Quitter le jeu !");
        Application.Quit();
    }
}
