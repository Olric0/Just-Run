using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelCTRL : MonoBehaviour
{
    public void PlayAgain()
    {
        Main.heart = 2;
        Main.scor = 0;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;

        ForHeart0.damage = false;
        ForHeart1.damage = false;
    }

    public void GoMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void GoGame()
    {
        Main.heart = 2;
        Main.scor = 0;

        SceneManager.LoadScene(1);
        Time.timeScale = 1;

        ForHeart0.damage = false;
        ForHeart1.damage = false;
    }
}