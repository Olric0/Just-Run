using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PanelCTRL : MonoBehaviour
{
    public GameObject fakePanel;
    public GameObject exitPanel;
    public GameObject stopPanel;
    public GameObject settingsPanel;

    [SerializeField] private Slider volumeSlider;

    void Start()
    {
        QualitySettings.SetQualityLevel(6);

        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 0.5f);
            Load();
        }
        else
        {
            Load();
        }
    }


    // Baþlangýç
    public void GoGame()
    {
        Main.heart = 2;
        Main.scor = 0;

        SceneManager.LoadScene(1);
        Time.timeScale = 1;

        ForHeart0.damage = false;
        ForHeart1.damage = false;
    }
    // *_*

    
    // Stop Panel Ayarlarý
    public void OpenGameMenuSettings()
    {
        settingsPanel.SetActive(true);
    }
    public void ExitGameMenuSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void OpenStopPanel()
    {
        stopPanel.SetActive(true);
        fakePanel.SetActive(true);
        Time.timeScale = 0;
    }
    public void ExitStopPanel()
    {
        stopPanel.SetActive(false);
        fakePanel.SetActive(false);
        Time.timeScale = 1;
    }
    // *_*


    // Ölüm Ekraný
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
    // *_*

    
    // Ana Menü Ayarlar Paneli
    public void Settings()
    {
        fakePanel.SetActive(true);
        settingsPanel.SetActive(true);
    }
    // *_*

    
    // Ana Menü Oyundan Çýkýþ Ayarlarý
    public void Exit()
    {
        fakePanel.SetActive(true);
        exitPanel.SetActive(true);
        Time.timeScale = 0;
    }
    public void ExitNo()
    {
        exitPanel.SetActive(false);
        fakePanel.SetActive(false);
        settingsPanel.SetActive(false);
        Time.timeScale = 1;
    }
    public void ExitYes()
    {
        Application.Quit();
    }
    // *_*


    // Grafik Ayarlarý
    public void Low()
    {
        QualitySettings.SetQualityLevel(1);
    }
    public void Medium()
    {
        QualitySettings.SetQualityLevel(4);
    }
    public void High()
    {
        QualitySettings.SetQualityLevel(6);
    }
    //*_*


    // Ses Ayarlarý
    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        Save();
    }

    private void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }
    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }
    // *_*
}