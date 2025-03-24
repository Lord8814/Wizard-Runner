using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void PlayGame1()
    {
        // Charge la première scène du jeu
        SceneManager.LoadScene("Level1");
    }

    public void PlayGame2()
    {
        // Charge la première scène du jeu
        SceneManager.LoadScene("Level2");
    }

    public void PlayGame3()
    {
        // Charge la première scène du jeu
        SceneManager.LoadScene("Level3");
    }

    public void PlayGame4()
    {
        // Charge la première scène du jeu
        SceneManager.LoadScene("Level4");
    }

    public void PlayGame5()
    {
        // Charge la première scène du jeu
        SceneManager.LoadScene("Level5");
    }

    public void PlayGame6()
    {
        // Charge la première scène du jeu
        SceneManager.LoadScene("Level6");
    }

    public void PlayGame7()
    {
        // Charge la première scène du jeu
        SceneManager.LoadScene("Level7");
    }

    public void PlayGame8()
    {
        // Charge la première scène du jeu
        SceneManager.LoadScene("Level8");
    }

    public void Levelmanager()
    {
        // Charge la première scène du jeu
        SceneManager.LoadScene("LevelManager");
    }




    public void QuitGame()
    {
        // Quitte le jeu (ne fonctionne que dans un build)
        Debug.Log("Quitter le jeu !");
        Application.Quit();
    }
}
