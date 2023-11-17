using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Lean.Localization;


/// <summary>
/// 
/// Oyunun UI Kısmı Komple Buradan Sorumludur. Değişkenlerin Çoğunun Açıklamalarını Yazmadım Çünkü Hepsi Adı Üstünde, Anlayabilirsin.
/// 
/// >*>*> Değişkenlerin Açıklamaları <*<*< \\\
///     > [ settingsPanelIsActive ]
///     Ayarlar Panelini Aç Kapa Yapmak İçin Kullanılır.
///     
///     > [ pausePanelIsActive ]
///     Durdurma Panelini Aç Kapa Yapmak İçin Kullanılır.
/// 
/// </summary>
public class PanelManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject fakePanel;
    [SerializeField] private GameObject quitPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject settingsPanel;

    [Header("For Settings")]
    [SerializeField] private UnityEngine.UI.Slider songVolumeSlider;
    [SerializeField] private UnityEngine.UI.Slider soundEffectsVolumeSlider;
    [SerializeField] private AudioSource songManager;
    [SerializeField] private AudioSource soundManager;

    private bool settingsPanelIsActive;
    private bool pausePanelIsActive;



    // Başlangıç İçin Gerekli Atamalar, Ayarlamalar Vs.
    private void Awake()
    {
        // Oyuncu Oyuna İlk Defa Giriyorsa, Gerekli Olan PlayerPrefs'ler Set Edilir.
        // Oyuncu  [ PLAY BUTONUNA ]  Bastığında, isFirst Değişkeni -1 Olarak Set Edilir.
        if (PlayerPrefs.HasKey("isFirst") == false)
        {
            // Ses Ayarlarının PP'lerini Set Etmek.
            PlayerPrefs.SetFloat("songVolume", 0.05f);
            PlayerPrefs.SetFloat("soundVolume", 0.1f);

            // Dil Ayarlarının PP'lerini Set Etmek.
            PlayerPrefs.SetInt("languageValue", 1);
            PlayerPrefs.SetString("LeanLocalization.CurrentLanguage", "Turkish");

            // Hilelerin PP'lerini Set Etmek.
            PlayerPrefs.SetInt("isCheatModeActive", 0);
            PlayerPrefs.SetInt("fireballTimerValue", 3);
            PlayerPrefs.SetInt("isGodModeOn", 1);
            PlayerPrefs.SetInt("isWithoutAnyPotions", 0);
            PlayerPrefs.SetInt("isWithoutAnyPotions", 0);
            PlayerPrefs.SetInt("isWithoutBlindnessPotion", 0);
            PlayerPrefs.SetInt("isAlwaysPowerPotion", 0);
            PlayerPrefs.SetInt("isAlwaysHealthPotion", 0);

            // Oyuncu Play Butonuna Bastığı Zaman  [ isFirst ]  PP'si  [ -1 ]  Olarak Tanımlanır.
        }

        // Ses Ayarlarını Kontrol Etme.
        songManager.volume = PlayerPrefs.GetFloat("songVolume");
        soundManager.volume = PlayerPrefs.GetFloat("soundVolume");
        songVolumeSlider.value = PlayerPrefs.GetFloat("songVolume");
        soundEffectsVolumeSlider.value = PlayerPrefs.GetFloat("soundVolume");

        // Oyuncu Ana Menüdeyse  [ PauseGameForMainMenuScene() ]  Metodunu Aktif Et Ve Sahnenin Dil Ayarlarını Kontrol Et.
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            StartCoroutine(PauseGameForMainMenuScene());

            // Ana Menü İçin Dil Ayarlarını Kontrol Etme.
            switch (PlayerPrefs.GetInt("languageValue"))
            {
                case 1:
                    LeanLocalization.SetCurrentLanguageAll("Turkish");
                    GameObject.Find("Canvas/BestScoreTexts/BestScoreText").GetComponent<TextMeshProUGUI>().text = "En İyi Skor: " + PlayerPrefs.GetInt("bestScore");
                    GameObject.Find("Canvas/BestScoreTexts/OldBestScoreText").GetComponent<TextMeshProUGUI>().text = "Önceki Rekorun: " + PlayerPrefs.GetInt("oldBestScore");
                    break;
                case 2:
                    LeanLocalization.SetCurrentLanguageAll("Azerbaijani");
                    GameObject.Find("Canvas/BestScoreTexts/BestScoreText").GetComponent<TextMeshProUGUI>().text = "Ən Yaxşı Xal: " + PlayerPrefs.GetInt("bestScore");
                    GameObject.Find("Canvas/BestScoreTexts/OldBestScoreText").GetComponent<TextMeshProUGUI>().text = "Əvvəlki Rekord: " + PlayerPrefs.GetInt("oldBestScore");
                    break;
                case 3:
                    LeanLocalization.SetCurrentLanguageAll("English");
                    GameObject.Find("Canvas/BestScoreTexts/BestScoreText").GetComponent<TextMeshProUGUI>().text = "Best Score: " + PlayerPrefs.GetInt("bestScore");
                    GameObject.Find("Canvas/BestScoreTexts/OldBestScoreText").GetComponent<TextMeshProUGUI>().text = "Previous Record: " + PlayerPrefs.GetInt("oldBestScore");
                    break;
                case 4:
                    LeanLocalization.SetCurrentLanguageAll("Spanish");
                    GameObject.Find("Canvas/BestScoreTexts/BestScoreText").GetComponent<TextMeshProUGUI>().text = "Mejor Puntaje: " + PlayerPrefs.GetInt("bestScore");
                    GameObject.Find("Canvas/BestScoreTexts/OldBestScoreText").GetComponent<TextMeshProUGUI>().text = "Récord Anterior: " + PlayerPrefs.GetInt("oldBestScore");
                    break;
                case 5:
                    LeanLocalization.SetCurrentLanguageAll("German");
                    GameObject.Find("Canvas/BestScoreTexts/BestScoreText").GetComponent<TextMeshProUGUI>().text = "Beste Punktzahl: " + PlayerPrefs.GetInt("bestScore");
                    GameObject.Find("Canvas/BestScoreTexts/OldBestScoreText").GetComponent<TextMeshProUGUI>().text = "Vorige Record: " + PlayerPrefs.GetInt("oldBestScore");
                    break;
            }
        }
        // Oyuncu Oyun Menüsündeyse  [ PauseGameForGameScene() ]  Metodunu Aktif Et Ve Sahnenin Dil Ayarlarını Kontrol Et.
        else
        {
            StartCoroutine(PauseGameForGameScene());

            // Oyun Menüsü İçin Dil Ayarlarını Kontrol Etme.
            switch (PlayerPrefs.GetInt("languageValue"))
            {
                case 1:
                    LeanLocalization.SetCurrentLanguageAll("Turkish");
                    GameObject.Find("Canvas/Score").GetComponent<TextMeshProUGUI>().text = "Skorun: 0";
                    break;
                case 2:
                    LeanLocalization.SetCurrentLanguageAll("Azerbaijani");
                    GameObject.Find("Canvas/Score").GetComponent<TextMeshProUGUI>().text = "Xalınız: 0";
                    break;
                case 3:
                    LeanLocalization.SetCurrentLanguageAll("English");
                    GameObject.Find("Canvas/Score").GetComponent<TextMeshProUGUI>().text = "Your Score: 0";
                    break;
                case 4:
                    LeanLocalization.SetCurrentLanguageAll("Spanish");
                    GameObject.Find("Canvas/Score").GetComponent<TextMeshProUGUI>().text = "Tu Puntaje: 0";
                    break;
                case 5:
                    LeanLocalization.SetCurrentLanguageAll("German");
                    GameObject.Find("Canvas/Score").GetComponent<TextMeshProUGUI>().text = "Ihr Punktzahl: 0";
                    break;
            }
        }

        // Açılış.
        Time.timeScale = 1.0f;
    }


    // Oyunu ESC İle Durdurma Metodları.
    private System.Collections.IEnumerator PauseGameForMainMenuScene()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && quitPanel.activeSelf == false
                && GameObject.Find("RefreshPanel") == false && OnMouseCheat.isCheatAnimPlaying == false)
            {
                settingsPanelIsActive = !settingsPanelIsActive;

                if (settingsPanelIsActive == true)
                    OpenSettingsPanelInMainMenu();
                else
                    CloseSettingsPanelInMainMenu();
            }
            yield return null;
        }
    }
    private System.Collections.IEnumerator PauseGameForGameScene()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && PotionManager.isTheAnyPotionAnimActive == false && GameObject.Find("DeathPanel") == false)
            {
                if (settingsPanelIsActive == false)
                {
                    pausePanelIsActive = !pausePanelIsActive;

                    if (pausePanelIsActive == true)
                        OpenPausePanel();
                    else
                        ClosePausePanel();
                }
                else
                {
                    settingsPanelIsActive = !settingsPanelIsActive;

                    if (settingsPanelIsActive == true)
                        OpenSettingsPanelInGame();
                    else
                        CloseSettingsPanelInGame();
                }
            }
            yield return null;
        }
    }

    // Oyuncu Alt+Tab Gibi Bir İşlev İle Oyundan Çıktığında Oyunu Durdurma Metodu.
    private void OnApplicationFocus(bool focus)
    {
        if (focus == false && SceneManager.GetActiveScene().buildIndex == 1 && Character.chrctrTHIS.health != 0
            && PotionManager.isTheAnyPotionAnimActive == false && settingsPanelIsActive == false)
        {
            // Stop Panel'i Aktifleştirme.
            pausePanelIsActive = true;
            pausePanel.SetActive(true);
            fakePanel.SetActive(true);

            // Kuş Sürüsü Aktifse Sesini Kapatma.
            if (GameObject.Find("Eagles(Clone)") == true)
                GameObject.Find("Eagles(Clone)").GetComponent<AudioSource>().mute = true;

            // Karakteri Pasif Hale Getirme.
            GameObject.Find("Character").GetComponent<Animator>().enabled = false;
            GameObject.Find("Character").GetComponent<Character>().enabled = false;

            // SongManager'ın Animatörünü Zamandan Etkilenip Oyun Durunca Durabilmesi İçin Animatörünün Update Modunu Normal Yapma.
            GameObject.Find("AudioManager/SongManager").GetComponent<Animator>().updateMode = AnimatorUpdateMode.Normal;

            // Kapanış.
            Time.timeScale = 0.0f;
        }
    }


    #region Main Menu
    // Sol Yukardaki Buton İle Ayarlar Panelini Açıp Kapatma.
    public void OpenSettingsPanelInMainMenu()
    {
        TurnOffAllButtonsAndColliders();

        // Ayarlar Panelini Açma Ve Ses Efektini Çalma.
        settingsPanelIsActive = true;
        fakePanel.SetActive(true);
        settingsPanel.SetActive(true);
        AudioManager.admgTHIS.PlayOneShotASound("ClickSound1");
    }
    public void CloseSettingsPanelInMainMenu()
    {
        TurnOnAllButtonsAndColliders();

        // Ayarlar Panelini Kapama Ve Ses Efektini Çalma.
        settingsPanelIsActive = false;
        fakePanel.SetActive(false);
        settingsPanel.SetActive(false);
        AudioManager.admgTHIS.PlayOneShotASound("ClickSound2");
    }


    // Oyunu Başlatma.
    public void Play()
    {
        // Oyuncunun İlk Oynayışıysa, Öğretici Modun Yüklenmesi.
        if (PlayerPrefs.HasKey("isFirst") == false)
        {
            PlayerPrefs.SetInt("isFirst", -1);
            SceneManager.LoadScene("Tutorial");
        }
        else
        {
            SceneManager.LoadScene("Game");
        }
    }


    // Quit Panelinin Kodları.
    public void OpenQuitPanel()
    {
        TurnOffAllButtonsAndColliders();

        // Quit Panelini Açma Ve Ses Efektini Çalma.
        fakePanel.SetActive(true);
        quitPanel.SetActive(true);
        AudioManager.admgTHIS.PlayOneShotASound("ClickSound1");
    }
    public void CloseQuitPanel()
    {
        TurnOnAllButtonsAndColliders();

        // Quit Panelini Kapama Ve Ses Efektini Çalma.
        fakePanel.SetActive(false);
        quitPanel.SetActive(false);
        AudioManager.admgTHIS.PlayOneShotASound("ClickSound2");
    }
    public void QuitTheGame()
    {
        Application.Quit();
    }


    // Oyunda Her Şeyi Sıfırlama Paneli Var. Bu 2 Metod Onun Kodları. Panel OnMouseRefresh.cs Scriptinden Açılıyor.
    public void DeleteAllDatas()
    {
        // Oyundaki Bütün Bilgileri Silme.
        PlayerPrefs.DeleteKey("isFirst");
        PlayerPrefs.SetInt("bestScore", 0);
        PlayerPrefs.SetInt("oldBestScore", 0);

        // Sahneyi Yeniden Başlatma.
        SceneManager.LoadScene("MainMenu");
    }
    public void CloseRefreshPanel()
    {
        TurnOnAllButtonsAndColliders();

        // Panelleri Kapatma Ve Ses Efektini Çalma.
        fakePanel.SetActive(false);
        GameObject.Find("Canvas/RefreshPanel").SetActive(false);
        AudioManager.admgTHIS.PlayOneShotASound("ClickSound2");
    }


    // Orta Aşağıdaki; Instagram, LinkedIn, Github Sosyal Medyalarının Buton Eventi.
    public void OpenUrl(string url)
    {
        Application.OpenURL(url);
    }


    // Ana Menüde Bir Panel Açıldığında, Arkaplanda Kalan Butonları Falan Kapatır.
    private void TurnOffAllButtonsAndColliders()
    {
        // Play Butonunun Ve Url Butonlarının Collider'ını Pasif Yapma.
        GameObject.Find("Canvas/PlayBTN").GetComponent<BoxCollider2D>().enabled = false;
        for (sbyte i = 0; i < 3; i++)
        {
            GameObject.Find("Canvas/SocialMediaURLS").transform.GetChild(i).GetComponent<BoxCollider2D>().enabled = false;
        }

        // Sol Alttaki Karakter Ve Sağ Alttaki Kartalların Colliderlerini Pasif Yapma.
        GameObject.Find("Canvas/Icons/Eagle").GetComponent<PolygonCollider2D>().enabled = false;
        GameObject.Find("Canvas/Icons/Knight").GetComponent<PolygonCollider2D>().enabled = false;

        // Eski En Yüksek Skoru Gösteren Colliderı Pasif Yapma.
        GameObject.Find("Canvas/BestScoreTexts").GetComponent<BoxCollider2D>().enabled = false;
    }
    // Ana Menüde Bir Panel Kapandığında, Arkaplanda Kalan Butonları Geri Açar.
    private void TurnOnAllButtonsAndColliders()
    {
        // Play Butonunun Ve Url Butonlarının Colliderını Aktif Yapma.
        GameObject.Find("Canvas/PlayBTN").GetComponent<BoxCollider2D>().enabled = true;
        for (sbyte i = 0; i < 3; i++)
        {
            GameObject.Find("Canvas/SocialMediaURLS").transform.GetChild(i).GetComponent<BoxCollider2D>().enabled = true;
        }

        // Sol Alttaki Karakter Ve Sağ Alttaki Kartalların Colliderlerini Aktif Yapma.
        GameObject.Find("Canvas/Icons/Eagle").GetComponent<PolygonCollider2D>().enabled = true;
        GameObject.Find("Canvas/Icons/Knight").GetComponent<PolygonCollider2D>().enabled = true;

        // Eski En Yüksek Skoru Gösteren Collideri Aktif Yapma.
        GameObject.Find("Canvas/BestScoreTexts").GetComponent<BoxCollider2D>().enabled = true;
    }
    #endregion

    #region Pause Panel Ayarları
    public void OpenPausePanel()
    {
        // Stop Panel'i Aktifleştirme.
        pausePanelIsActive = true;
        pausePanel.SetActive(true);
        fakePanel.SetActive(true);
        AudioManager.admgTHIS.PlayOneShotASound("ClickSound1");

        // Kuş Sürüsü Aktifse Sesini Kapatma.
        if (GameObject.Find("Eagles(Clone)") == true)
            GameObject.Find("Eagles(Clone)").GetComponent<AudioSource>().mute = true;

        // Karakteri Pasif Hale Getirme.
        GameObject.Find("Character").GetComponent<Animator>().enabled = false;
        GameObject.Find("Character").GetComponent<Character>().enabled = false;

        // SongManager'ın Animatörünü Zamandan Etkilenip Oyun Durunca Durabilmesi İçin Animatörünün Update Modunu Normal Yapma.
        GameObject.Find("AudioManager/SongManager").GetComponent<Animator>().updateMode = AnimatorUpdateMode.Normal;
        Time.timeScale = 0.0f;
    }
    public void ClosePausePanel()
    {
        // Stop Panel'i Kapatma.
        pausePanelIsActive = false;
        pausePanel.SetActive(false);
        fakePanel.SetActive(false);
        AudioManager.admgTHIS.PlayOneShotASound("ClickSound2");

        // Kuş Sürüsü Aktifse Sesini Geri Açma.
        if (GameObject.Find("Eagles(Clone)") == true)
            GameObject.Find("Eagles(Clone)").GetComponent<AudioSource>().mute = false;

        // Karakteri Geri Aktif Hale Getirme.
        GameObject.Find("Character").GetComponent<Animator>().enabled = true;
        GameObject.Find("Character").GetComponent<Character>().enabled = true;

        // SongManager'ın Animatörünü Geri Eski Haline Döndürme.
        GameObject.Find("AudioManager/SongManager").GetComponent<Animator>().updateMode = AnimatorUpdateMode.UnscaledTime;

        // Eğer Karakter İksir İçtiği Bir Zamanda Oyunu Durdurursa, Oyunu Buga Sokmaması İçin Zamanı Ona Göre Kontrol Etme.
        if (PotionManager.didThePlayerDrankPowerPotion == true)
            Time.timeScale = 1.8f;
        else
            Time.timeScale = 1.0f;
    }


    public void OpenSettingsPanelInGame()
    {
        settingsPanelIsActive = true;
        settingsPanel.SetActive(true);
        pausePanel.SetActive(false);
        AudioManager.admgTHIS.PlayOneShotASound("ClickSound1");
    }
    public void CloseSettingsPanelInGame()
    {
        settingsPanelIsActive = false;
        settingsPanel.SetActive(false);

        // Oyuncu Ölmüşse, Ayarları Kapattığında StopPanel'in Açılmaması Gerekir. Yoksa DeathPanel İle Çakışır.
        if (Character.chrctrTHIS.health != 0)
            pausePanel.SetActive(true);

        AudioManager.admgTHIS.PlayOneShotASound("ClickSound2");
    }
    #endregion

    #region Ölüm Ekranı
    public void Restart()
    {
        // Oyuna Restart Atma.
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Game");
    }
    public void GoMainMenu()
    {
        // Ana Menüye Dönme.
        SceneManager.LoadScene("MainMenu");
    }
    #endregion

    #region Ayarlar Paneli

    #region Ses Ayarları
    public void SongVolumePlus()
    {
        songVolumeSlider.value += 0.05f;

        if (songManager.volume >= 1.0f)
            AudioManager.admgTHIS.PlayOneShotASound("ErrorSound");
        else
            AudioManager.admgTHIS.PlayOneShotASound("ClickSound1");
    }
    public void SongVolumeMinus()
    {
        songVolumeSlider.value -= 0.05f;

        if (songManager.volume <= 0.0f)
            AudioManager.admgTHIS.PlayOneShotASound("ErrorSound");
        else
            AudioManager.admgTHIS.PlayOneShotASound("ClickSound1");
    }
    public void ChangeSongVolume()
    {
        songManager.volume = songVolumeSlider.value;
        SaveAllAudioVariables();
    }

    public void SoundEffectsVolumePlus()
    {
        soundEffectsVolumeSlider.value += 0.05f;

        if (soundManager.volume >= 1.0f)
            AudioManager.admgTHIS.PlayOneShotASound("ErrorSound");
        else
            AudioManager.admgTHIS.PlayOneShotASound("ClickSound1");
    }
    public void SoundEffectsVolumeMinus()
    {
        soundEffectsVolumeSlider.value -= 0.05f;

        if (soundManager.volume <= 0.0f)
            AudioManager.admgTHIS.PlayOneShotASound("ErrorSound");
        else
            AudioManager.admgTHIS.PlayOneShotASound("ClickSound1");
    }
    public void ChangeSoundEffectsVolume()
    {
        soundManager.volume = soundEffectsVolumeSlider.value;
        SaveAllAudioVariables();
    }

    // Eğer Bu Metotu Kullanmazsan, Oyuncu Sahne Değiştirdiğinde Ses Ayarları Sıfırlanır.
    private void SaveAllAudioVariables()
    {
        PlayerPrefs.SetFloat("songVolume", songManager.volume);
        PlayerPrefs.SetFloat("soundVolume", soundManager.volume);
    }
    #endregion

    #region Dil Ayarları
    public void LanguageValueMinus()
    {
        // Dil Değerini 1 Düşürüp Switch'e Girme, Ardından Mevcut Dil Değeri Neyse Oyun O Dil Değerine Setlenir.
        PlayerPrefs.SetInt("languageValue", PlayerPrefs.GetInt("languageValue") - 1);
        switch (PlayerPrefs.GetInt("languageValue"))
        {
            case 0:
                AudioManager.admgTHIS.PlayOneShotASound("ErrorSound");
                PlayerPrefs.SetInt("languageValue", 1);
                break;
            case 1:
                LeanLocalization.SetCurrentLanguageAll("Turkish");
                AudioManager.admgTHIS.PlayOneShotASound("ClickSound1");

                settingsPanel.transform.GetChild(3).GetChild(3).GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("fireballTimerValue") + " Saniye";

                if (SceneManager.GetActiveScene().buildIndex == 1)
                {
                    GameObject.Find("Canvas/Score").GetComponent<TextMeshProUGUI>().text = "Skorun: " + ScoreManager.smTHIS.score.ToString();
                }
                else
                {
                    GameObject.Find("Canvas/BestScoreTexts/BestScoreText").GetComponent<TextMeshProUGUI>().text = "En İyi Skor: " + PlayerPrefs.GetInt("bestScore");
                    GameObject.Find("Canvas/BestScoreTexts/OldBestScoreText").GetComponent<TextMeshProUGUI>().text = "Önceki Rekorun: " + PlayerPrefs.GetInt("oldBestScore");
                }
                break;
            case 2:
                LeanLocalization.SetCurrentLanguageAll("Azerbaijani");
                AudioManager.admgTHIS.PlayOneShotASound("ClickSound1");

                settingsPanel.transform.GetChild(3).GetChild(3).GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("fireballTimerValue") + " Saniyə";

                if (SceneManager.GetActiveScene().buildIndex == 1)
                {
                    GameObject.Find("Canvas/Score").GetComponent<TextMeshProUGUI>().text = "Xalınız: " + ScoreManager.smTHIS.score.ToString();
                }
                else
                {
                    GameObject.Find("Canvas/BestScoreTexts/BestScoreText").GetComponent<TextMeshProUGUI>().text = "Ən Yaxşı Xal: " + PlayerPrefs.GetInt("bestScore");
                    GameObject.Find("Canvas/BestScoreTexts/OldBestScoreText").GetComponent<TextMeshProUGUI>().text = "Əvvəlki Rekord: " + PlayerPrefs.GetInt("oldBestScore");
                }
                break;
            case 3:
                LeanLocalization.SetCurrentLanguageAll("English");
                AudioManager.admgTHIS.PlayOneShotASound("ClickSound1");

                settingsPanel.transform.GetChild(3).GetChild(3).GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("fireballTimerValue") + " Second";

                if (SceneManager.GetActiveScene().buildIndex == 1)
                {
                    GameObject.Find("Canvas/Score").GetComponent<TextMeshProUGUI>().text = "Your Score: " + ScoreManager.smTHIS.score.ToString();
                }
                else
                {
                    GameObject.Find("Canvas/BestScoreTexts/BestScoreText").GetComponent<TextMeshProUGUI>().text = "Best Score: " + PlayerPrefs.GetInt("bestScore");
                    GameObject.Find("Canvas/BestScoreTexts/OldBestScoreText").GetComponent<TextMeshProUGUI>().text = "Previous Record: " + PlayerPrefs.GetInt("oldBestScore");
                }
                break;
            case 4:
                LeanLocalization.SetCurrentLanguageAll("Spanish");
                AudioManager.admgTHIS.PlayOneShotASound("ClickSound1");

                settingsPanel.transform.GetChild(3).GetChild(3).GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("fireballTimerValue") + " Segundo";

                if (SceneManager.GetActiveScene().buildIndex == 1)
                {
                    GameObject.Find("Canvas/Score").GetComponent<TextMeshProUGUI>().text = "Tu Puntaje: " + ScoreManager.smTHIS.score.ToString();
                }
                else
                {
                    GameObject.Find("Canvas/BestScoreTexts/BestScoreText").GetComponent<TextMeshProUGUI>().text = "Mejor Puntaje: " + PlayerPrefs.GetInt("bestScore");
                    GameObject.Find("Canvas/BestScoreTexts/OldBestScoreText").GetComponent<TextMeshProUGUI>().text = "Récord Anterior: " + PlayerPrefs.GetInt("oldBestScore");
                }
                break;
        }
    }
    public void LanguageValuePlus()
    {
        // Dil Değerini 1 Arttırıp Switch'e Girme, Ardından Mevcut Dil Değeri Neyse Oyun O Dil Değerine Setlenir.
        PlayerPrefs.SetInt("languageValue", PlayerPrefs.GetInt("languageValue") + 1);
        switch (PlayerPrefs.GetInt("languageValue"))
        {
            case 2:
                LeanLocalization.SetCurrentLanguageAll("Azerbaijani");
                AudioManager.admgTHIS.PlayOneShotASound("ClickSound1");

                settingsPanel.transform.GetChild(3).GetChild(3).GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("fireballTimerValue") + " Saniyə";

                if (SceneManager.GetActiveScene().buildIndex == 1)
                {
                    GameObject.Find("Canvas/Score").GetComponent<TextMeshProUGUI>().text = "Xalınız: " + ScoreManager.smTHIS.score.ToString();
                }
                else
                {
                    GameObject.Find("Canvas/BestScoreTexts/BestScoreText").GetComponent<TextMeshProUGUI>().text = "Ən Yaxşı Xal: " + PlayerPrefs.GetInt("bestScore");
                    GameObject.Find("Canvas/BestScoreTexts/OldBestScoreText").GetComponent<TextMeshProUGUI>().text = "Əvvəlki Rekord: " + PlayerPrefs.GetInt("oldBestScore");
                }
                break;
            case 3:
                LeanLocalization.SetCurrentLanguageAll("English");
                AudioManager.admgTHIS.PlayOneShotASound("ClickSound1");

                settingsPanel.transform.GetChild(3).GetChild(3).GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("fireballTimerValue") + " Second";

                if (SceneManager.GetActiveScene().buildIndex == 1)
                {
                    GameObject.Find("Canvas/Score").GetComponent<TextMeshProUGUI>().text = "Your Score: " + ScoreManager.smTHIS.score.ToString();
                }
                else
                {
                    GameObject.Find("Canvas/BestScoreTexts/BestScoreText").GetComponent<TextMeshProUGUI>().text = "Best Score: " + PlayerPrefs.GetInt("bestScore");
                    GameObject.Find("Canvas/BestScoreTexts/OldBestScoreText").GetComponent<TextMeshProUGUI>().text = "Previous Record: " + PlayerPrefs.GetInt("oldBestScore");
                }
                break;
            case 4:
                LeanLocalization.SetCurrentLanguageAll("Spanish");
                AudioManager.admgTHIS.PlayOneShotASound("ClickSound1");

                settingsPanel.transform.GetChild(3).GetChild(3).GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("fireballTimerValue") + " Segundo";

                if (SceneManager.GetActiveScene().buildIndex == 1)
                {
                    GameObject.Find("Canvas/Score").GetComponent<TextMeshProUGUI>().text = "Tu Puntaje: " + ScoreManager.smTHIS.score.ToString();
                }
                else
                {
                    GameObject.Find("Canvas/BestScoreTexts/BestScoreText").GetComponent<TextMeshProUGUI>().text = "Mejor Puntaje: " + PlayerPrefs.GetInt("bestScore");
                    GameObject.Find("Canvas/BestScoreTexts/OldBestScoreText").GetComponent<TextMeshProUGUI>().text = "Récord Anterior: " + PlayerPrefs.GetInt("oldBestScore");
                }
                break;
            case 5:
                LeanLocalization.SetCurrentLanguageAll("German");
                AudioManager.admgTHIS.PlayOneShotASound("ClickSound1");

                settingsPanel.transform.GetChild(3).GetChild(3).GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("fireballTimerValue") + " Sekunde";

                if (SceneManager.GetActiveScene().buildIndex == 1)
                {
                    GameObject.Find("Canvas/Score").GetComponent<TextMeshProUGUI>().text = "Ihr Punktzahl: " + ScoreManager.smTHIS.score.ToString();
                }
                else
                {
                    GameObject.Find("Canvas/BestScoreTexts/BestScoreText").GetComponent<TextMeshProUGUI>().text = "Beste Punktzahl: " + PlayerPrefs.GetInt("bestScore");
                    GameObject.Find("Canvas/BestScoreTexts/OldBestScoreText").GetComponent<TextMeshProUGUI>().text = "Vorige Record: " + PlayerPrefs.GetInt("oldBestScore");
                }
                break;
            case 6:
                AudioManager.admgTHIS.PlayOneShotASound("ErrorSound");
                PlayerPrefs.SetInt("languageValue", 5);
                break;
        }
    }
    #endregion

    #endregion
}