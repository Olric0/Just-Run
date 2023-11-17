using UnityEngine;


/// <summary>
/// 
/// ��reticinin Men�s�nde Bir �ey Olmad��� ��in Bu Script Sadece Oyuncunun ESC'ye
/// Basarak ��k�� Panelini A��p Kapamas�na Yar�yor. Pek Bir �eyi Yok Yani.
/// 
/// </summary>
public class TutorialMenu : MonoBehaviour
{
    [SerializeField] private GameObject quitPanel;
    [SerializeField] private GameObject fakePanel;
    private bool quitPanelIsActive;


    private System.Collections.IEnumerator Start()
    {
        // Sesleri Ayarlama.
        Transform audioManager = GameObject.Find("AudioManager").transform;
        audioManager.GetChild(0).GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("songVolume");
        audioManager.GetChild(1).GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("soundVolume");
        audioManager.GetChild(2).GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("soundVolume");

        // ESC �le Paneli A��p Kapatma.
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                quitPanelIsActive = !quitPanelIsActive;

                if (quitPanelIsActive == true)
                {
                    // Karakteri Pasif Hale Getirme.
                    GameObject character = GameObject.Find("Character");
                    if (character == true)
                    {
                        character.GetComponent<Animator>().enabled = false;
                        character.GetComponent<CharacterForTutorial>().enabled = false;
                    }

                    // Panelleri A��p Ses Efektini �alma.
                    quitPanel.SetActive(true);
                    fakePanel.SetActive(true);
                    AudioManager.admgTHIS.PlayOneShotASound("ClickSound1");

                    // Oyunu Durdurma.
                    Time.timeScale = 0.0f;
                }
                else
                {
                    QuitAnswerIsNo();
                }
            }
            yield return null;
        }
    }

    // Oyuncu Alt+Tab Gibi Bir ��lev �le Oyundan ��kt���nda Oyunu Durdurma Metodu.
    private void OnApplicationFocus(bool focus)
    {
        if (focus == false)
        {
            // Karakteri Pasif Hale Getirme.
            GameObject character = GameObject.Find("Character");
            if (character == true)
            {
                character.GetComponent<Animator>().enabled = false;
                character.GetComponent<CharacterForTutorial>().enabled = false;
            }

            // Panelleri A��p Ses Efektini �alma.
            quitPanel.SetActive(true);
            fakePanel.SetActive(true);
            AudioManager.admgTHIS.PlayOneShotASound("ClickSound1");

            // Oyunu Durdurma.
            quitPanelIsActive = true;
            Time.timeScale = 0.0f;
        }
    }

    // Quit Panel Ayarlar�.
    public void QuitAnswerIsYes()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    public void QuitAnswerIsNo()
    {
        // Karakteri Aktif Hale Getirme.
        GameObject character = GameObject.Find("Character");
        if (character == true)
        {
            character.GetComponent<Animator>().enabled = true;
            character.GetComponent<CharacterForTutorial>().enabled = true;
        }

        // Panelleri Kapat�p Ses Efektini �alma.
        quitPanel.SetActive(false);
        fakePanel.SetActive(false);
        AudioManager.admgTHIS.PlayOneShotASound("ClickSound2");

        // Oyunu Devam Ettirme.
        quitPanelIsActive = false;
        Time.timeScale = 1.0f;
    }
}