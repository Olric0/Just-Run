using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



/// <summary>
/// 
/// Bu Script Oyunun Hile Sistemini Kontrol Eder.
/// Ayrıca Ayarlar Panelindeki Hile Ayarlarıda Bu Scriptteki Metotlardan Yönetilir.
/// 
/// </summary>
public class CheatManager : MonoBehaviour
{
    [Header("Fireball Cheat Variables")]
    [SerializeField] private Slider fireballTimerSlider;
    [SerializeField] private TextMeshProUGUI fireballTimerValueText;

    [Header("Other Cheat Variables")]
    [SerializeField] private Toggle godMode;
    [SerializeField] private Toggle withoutAnyPotions;
    [SerializeField] private Toggle withoutBlindnessPotion;
    [SerializeField] private TextMeshProUGUI withoutBlindnessPotionLabelColor;
    [SerializeField] private Toggle alwaysPowerPotion;
    [SerializeField] private TextMeshProUGUI alwaysPowerPotionLabelColor;
    [SerializeField] private Toggle alwaysHealthPotion;
    [SerializeField] private TextMeshProUGUI alwaysHealthPotionLabelColor;

    [Header("Other")]
    [SerializeField] private Sprite cheatKnightStrikeIcon;
    [SerializeField] private Sprite cheatSettingsButton;
    [SerializeField] private Sprite cheatSettingsPanel;



    private void Start()
    {
        // Eğer Hile Açıksa Sahneyi Hileli Versiyona Göre Hazırlama.
        if (PlayerPrefs.GetInt("isCheatModeActive") == 1)
        {
            // Eğer Oyuncu 0. Sahne İndeksinde İse Ana Sahnededir. Ve Buna Özel Ayarlamalar Yapılır.
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                // Değişken Atamaları.
                Transform canvasPath = GameObject.Find("Canvas").transform;

                // Karakterin Strike İkonunu Hileli İkon İle Değiştirme.
                canvasPath.GetChild(1).GetChild(0).GetComponent<Image>().sprite = cheatKnightStrikeIcon;

                // Hileli Ayarlar Butonunu Aktif Etme.
                canvasPath.GetChild(2).gameObject.SetActive(true);
                canvasPath.GetChild(3).gameObject.SetActive(false);

                // Hile Yazısını Aktif Etme.
                canvasPath.GetChild(0).GetChild(1).gameObject.SetActive(true);

                // Ayarlar Panelini Hileli Ayarlar Paneline Çevirme.
                canvasPath = canvasPath.GetChild(10);
                canvasPath.GetComponent<Image>().sprite = cheatSettingsPanel;
                canvasPath.GetComponent<Image>().SetNativeSize();
                canvasPath.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(-326.0f, 0.0f);
                canvasPath.GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector2(-326.0f, 0.0f);
                canvasPath.GetChild(3).gameObject.SetActive(true);
            }
            // Eğer Oyuncu 0. Sahne İndeksinde Değil İse Oyun Sahnesindedir. Ve Buna Özel Ayarlamalar Yapılır.
            else
            {
                // Değişken Atamaları.
                Transform canvasPath = GameObject.Find("Canvas").transform.GetChild(10);

                // Ayarlar Butonunu Hileli Versiyonu İle Değiştirme.
                GameObject.Find("Canvas").transform.GetChild(8).GetChild(2).gameObject.GetComponent<Image>().sprite = cheatSettingsButton;
                GameObject.Find("Canvas").transform.GetChild(9).GetChild(2).gameObject.GetComponent<Image>().sprite = cheatSettingsButton;

                // Ayarlar Panelini Hileli Ayarlar Paneline Çevirme.
                canvasPath.GetComponent<Image>().sprite = cheatSettingsPanel;
                canvasPath.GetComponent<Image>().SetNativeSize();
                canvasPath.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(-326.0f, 0.0f);
                canvasPath.GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector2(-326.0f, 0.0f);
                canvasPath.GetChild(3).gameObject.SetActive(true);
            }
        }

        // Ateş Topunun Slider'ını Ve Bekleme Yazısını Ayarlama.
        fireballTimerSlider.value = PlayerPrefs.GetInt("fireballTimerValue");
        switch (PlayerPrefs.GetString("LeanLocalization.CurrentLanguage"))
        {
            case "Turkish":     fireballTimerValueText.text = PlayerPrefs.GetInt("fireballTimerValue").ToString() + " Saniye";  break;
            case "Azerbaijani": fireballTimerValueText.text = PlayerPrefs.GetInt("fireballTimerValue").ToString() + " Saniyə";  break;
            case "English":     fireballTimerValueText.text = PlayerPrefs.GetInt("fireballTimerValue").ToString() + " Second"; break;
            case "Spanish":     fireballTimerValueText.text = PlayerPrefs.GetInt("fireballTimerValue").ToString() + " Segundo"; break;
            case "German":      fireballTimerValueText.text = PlayerPrefs.GetInt("fireballTimerValue").ToString() + " Sekunde"; break;
        }

        // Hileleri Karşılaştırma.
        if (PlayerPrefs.GetInt("isGodModeOn") == 0)
            godMode.isOn = false;
        if (PlayerPrefs.GetInt("isWithoutAnyPotions") == 1)
            withoutAnyPotions.isOn = true;
        if (PlayerPrefs.GetInt("isWithoutBlindnessPotion") == 1)
            withoutBlindnessPotion.isOn = true;
        if (PlayerPrefs.GetInt("isAlwaysPowerPotion") == 1)
            alwaysPowerPotion.isOn = true;
        if (PlayerPrefs.GetInt("isAlwaysHealthPotion") == 1)
            alwaysHealthPotion.isOn = true;

        #region Hilelerin Eventlarını Set Etme.
        // Ateş Topu Hilesinin Slider Eventi.
        fireballTimerSlider.onValueChanged.AddListener((x) => SetfireballTimerValue()); ;

        // Ölümsüzlük Hilesinin Toggle Eventi.
        bool val = PlayerPrefs.GetInt("isGodModeOn") == 1 ? val = true : val = false;
        godMode.onValueChanged.AddListener((val) => { OnValueChangedGodMode(val); });

        // Bütün İksirleri Devre Dışı Bırakma Hilesinin Toggle Eventi.
        val = PlayerPrefs.GetInt("isWithoutAnyPotions") == 1 ? val = true : val = false;
        if (val == true)
        {
            // WithoutBlindnessPotion, AlwaysPowerPotion, AlwaysHealthPotion Hilelerini Devre Dışı Bırakma.
            withoutBlindnessPotion.isOn = false;
            withoutBlindnessPotion.interactable = false;
            withoutBlindnessPotionLabelColor.color = new Color32(255, 100, 0, 120);
            alwaysPowerPotion.isOn = false;
            alwaysPowerPotion.interactable = false;
            alwaysPowerPotionLabelColor.color = new Color32(255, 100, 0, 120);
            alwaysHealthPotion.isOn = false;
            alwaysHealthPotion.interactable = false;
            alwaysHealthPotionLabelColor.color = new Color32(255, 100, 0, 120);

            // Devre Dışı Bırakılan Hilelerin Değişken Değerlerini Set Etme.
            PlayerPrefs.SetInt("isWithoutBlindnessPotion", 0);
            PlayerPrefs.SetInt("isAlwaysPowerPotion", 0);
            PlayerPrefs.SetInt("isAlwaysHealthPotion", 0);
        }
        withoutAnyPotions.onValueChanged.AddListener((val) => { OnValueChangedWithoutAnyPotions(val); });

        // Körlük İksirini Devre Dışı Bırakma Hilesinin Toggle Eventi.
        val = PlayerPrefs.GetInt("isWithoutBlindnessPotion") == 1 ? val = true : val = false;
        withoutBlindnessPotion.onValueChanged.AddListener((val) => { OnValueChangedWithoutBlindnessPotion(val); });

        // Süreklü Güç İksiri Hilesinin Toggle Eventi.
        val = PlayerPrefs.GetInt("isAlwaysPowerPotion") == 1 ? val = true : val = false;
        alwaysPowerPotion.onValueChanged.AddListener((val) => { OnValueChangedAlwaysPowerPotion(val); });

        // Süreklü Can İksiri Hilesinin Toggle Eventi.
        val = PlayerPrefs.GetInt("isAlwaysHealthPotion") == 1 ? val = true : val = false;
        alwaysHealthPotion.onValueChanged.AddListener((val) => { OnValueChangedAlwaysHealthPotion(val); });
        #endregion
    }


    #region Ateş Topu Hilesi
    private void SetfireballTimerValue()
    {
        // Ateş Topu Bekleme Süresinin Değerini Hile Değeri İle Set Etme.
        PlayerPrefs.SetInt("fireballTimerValue", ((int)fireballTimerSlider.value));

        // Gerekli Kontrolleri Yapma.
        CheckFireballTimerOptions();
    }
    public void SetFireballTimerPlusValue()
    {
        if (fireballTimerSlider.value < 10)
        {
            // Ateş Topu Bekleme Süresinin Değerini Hile Değeri İle Set Etme.
            PlayerPrefs.SetInt("fireballTimerValue", (PlayerPrefs.GetInt("fireballTimerValue") + 1));

            // Gerekli Kontrolleri Yapma.
            CheckFireballTimerOptions();
        }
        else
        {
            AudioManager.admgTHIS.PlayOneShotASound("ErrorSound");
        }
    }
    public void SetFireballTimerMinusValue()
    {
        if (fireballTimerSlider.value > 0)
        {
            // Ateş Topu Bekleme Süresinin Değerini Hile Değeri İle Set Etme.
            PlayerPrefs.SetInt("fireballTimerValue", (PlayerPrefs.GetInt("fireballTimerValue") - 1));

            // Gerekli Kontrolleri Yapma.
            CheckFireballTimerOptions();
        }
        else
        {
            AudioManager.admgTHIS.PlayOneShotASound("ErrorSound");
        }
    }
    private void CheckFireballTimerOptions()
    {
        // Oyuncu Oyundaysa Ateş Topunun Bekleme Süresinin Yenilenmesi İçin Reset Atma.
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            Character.chrctrTHIS.StopCoroutine(Character.chrctrTHIS.WaitToUsingFireball());
            Character.chrctrTHIS.isTheFireballActive = false;
            Character.chrctrTHIS.StartCoroutine(Character.chrctrTHIS.WaitToUsingFireball());
        }

        // Ateş Topunun Bekleme Süresinin Slider'ını Ve Yazısını Ayarlama.
        fireballTimerSlider.value = PlayerPrefs.GetInt("fireballTimerValue");
        switch (PlayerPrefs.GetString("LeanLocalization.CurrentLanguage"))
        {
            case "Turkish":     fireballTimerValueText.text = PlayerPrefs.GetInt("fireballTimerValue").ToString() + " Saniye";  break;
            case "Azerbaijani": fireballTimerValueText.text = PlayerPrefs.GetInt("fireballTimerValue").ToString() + " Saniyə";  break;
            case "English":     fireballTimerValueText.text = PlayerPrefs.GetInt("fireballTimerValue").ToString() + " Second";  break;
            case "Spanish":     fireballTimerValueText.text = PlayerPrefs.GetInt("fireballTimerValue").ToString() + " Segundo"; break;
            case "German":      fireballTimerValueText.text = PlayerPrefs.GetInt("fireballTimerValue").ToString() + " Sekunde"; break;
        }

        // Ses Efektini Çalma.
        AudioManager.admgTHIS.PlayOneShotASound("ClickSound1");
    }
    #endregion

    #region Diğer Hileler
    private void OnValueChangedGodMode(bool value)
    {
        if (value == true)
        {
            // GodMode Hilesini  [ TRUE ]  Yapıp Ses Efektini Çalma.
            PlayerPrefs.SetInt("isGodModeOn", 1);
            AudioManager.admgTHIS.PlayOneShotASound("ClickSound1");
        }
        else
        {
            // GodMode Hilesini  [ FALSE ]  Yapıp Ses Efektini Çalma.
            PlayerPrefs.SetInt("isGodModeOn", 0);
            AudioManager.admgTHIS.PlayOneShotASound("ClickSound2");
        }
    }
    private void OnValueChangedWithoutAnyPotions(bool value)
    {
        if (value == true)
        {
            // WithoutBlindnessPotion, AlwaysPowerPotion, AlwaysHealthPotion Hilelerini Devre Dışı Bırakma.
            withoutBlindnessPotion.isOn = false;
            withoutBlindnessPotion.interactable = false;
            withoutBlindnessPotionLabelColor.color = new Color32(255, 100, 0, 120);
            alwaysPowerPotion.isOn = false;
            alwaysPowerPotion.interactable = false;
            alwaysPowerPotionLabelColor.color = new Color32(255, 100, 0, 120);
            alwaysHealthPotion.isOn = false;
            alwaysHealthPotion.interactable = false;
            alwaysHealthPotionLabelColor.color = new Color32(255, 100, 0, 120);

            // Devre Dışı Bırakılan Hilelerin Değişken Değerlerini Set Etme.
            PlayerPrefs.SetInt("isWithoutBlindnessPotion", 0);
            PlayerPrefs.SetInt("isAlwaysPowerPotion", 0);
            PlayerPrefs.SetInt("isAlwaysHealthPotion", 0);

            // WithoutAnyPotions Hilesini True Yapıp Ses Efektini Çalma.
            PlayerPrefs.SetInt("isWithoutAnyPotions", 1);
            AudioManager.admgTHIS.PlayOneShotASound("ClickSound1");

            // Sahnede Herhangi Bir Aktif İksir Varsa Devre Dışı Bırakma.
            if (GameObject.Find("PowerPotion(Clone)"))
            {
                Destroy(GameObject.Find("PowerPotion(Clone)").gameObject);
                PotionManager.isTheAnyPotionActive = false;
            }
            if (GameObject.Find("BlindnessPotion(Clone)"))
            {
                Destroy(GameObject.Find("BlindnessPotion(Clone)").gameObject);
                PotionManager.isTheAnyPotionActive = false;
            }
            if (GameObject.Find("HealthPotion(Clone)"))
            {
                Destroy(GameObject.Find("HealthPotion(Clone)").gameObject);
                PotionManager.isTheAnyPotionActive = false;
            }
        }
        else
        {
            // WithoutBlindnessPotion, AlwaysPowerPotion, AlwaysHealthPotion Hilelerini Aktif Yapma.
            withoutBlindnessPotion.interactable = true;
            withoutBlindnessPotionLabelColor.color = new Color32(255, 100, 0, 255);
            alwaysPowerPotion.interactable = true;
            alwaysPowerPotionLabelColor.color = new Color32(255, 100, 0, 255);
            alwaysHealthPotion.interactable = true;
            alwaysHealthPotionLabelColor.color = new Color32(255, 100, 0, 255);

            // WithoutAnyPotions Hilesini  [ FALSE ]  Yapıp Ses Efektini Çalma.
            PlayerPrefs.SetInt("isWithoutAnyPotions", 0);
            AudioManager.admgTHIS.PlayOneShotASound("ClickSound2");
        }
    }
    private void OnValueChangedWithoutBlindnessPotion(bool value)
    {
        if (value == true)
        {
            // WithoutBlindnessPotion Hilesini  [ TRUE ]  Yapıp Ses Efektini Çalma.
            PlayerPrefs.SetInt("isWithoutBlindnessPotion", 1);
            AudioManager.admgTHIS.PlayOneShotASound("ClickSound1");

            // Sahnede Aktif Bir Körlük Efekti İksiri Varsa Yok Etme.
            if (GameObject.Find("BlindnessPotion(Clone)"))
            {
                Destroy(GameObject.Find("BlindnessPotion(Clone)").gameObject);
                PotionManager.isTheAnyPotionActive = false;
            }
        }
        else
        {
            // WithoutBlindnessPotion Hilesini  [ FALSE ]  Yapıp Ses Efektini Çalma.
            PlayerPrefs.SetInt("isWithoutBlindnessPotion", 0);
            AudioManager.admgTHIS.PlayOneShotASound("ClickSound2");
        }
    }
    private void OnValueChangedAlwaysPowerPotion(bool value)
    {
        if (value == true)
        {
            // AlwaysHealthPotion Hilesini  [ FALSE ]  Yapma.
            alwaysHealthPotion.isOn = false;
            PlayerPrefs.SetInt("isAlwaysHealthPotion", 0);

            // AlwaysPowerPotion Hilesini  [ TRUE ]  Yapma.
            PlayerPrefs.SetInt("isAlwaysPowerPotion", 1);
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                Character.chrctrTHIS.powerPotionSpawnControllerValue = (Character.chrctrTHIS.score + 50);
            }

            // Ses Efektini Oynatma.
            AudioManager.admgTHIS.PlayOneShotASound("ClickSound1");
        }
        else
        {
            // AlwaysPowerPotion Hilesini  [ FALSE ]  Yapıp Ses Efektini Çalma.
            PlayerPrefs.SetInt("isAlwaysPowerPotion", 0);
            AudioManager.admgTHIS.PlayOneShotASound("ClickSound2");
        }
    }
    private void OnValueChangedAlwaysHealthPotion(bool value)
    {
        if (value == true)
        {
            // AlwaysPowerPotion Hilesini  [ FALSE ]  Yapma.
            alwaysPowerPotion.isOn = false;
            PlayerPrefs.SetInt("isAlwaysPowerPotion", 0);

            // AlwaysPowerPotion Hilesini  [ TRUE ]  Yapma.
            PlayerPrefs.SetInt("isAlwaysHealthPotion", 1);
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                Character.chrctrTHIS.healthPotionSpawnControllerValue = true;
            }

            // Ses Efektini Oynatma.
            AudioManager.admgTHIS.PlayOneShotASound("ClickSound1");
        }
        else
        {
            // AlwaysHealthPotion Hilesini  [ FALSE ]  Yapıp Ses Efektini Çalma.
            PlayerPrefs.SetInt("isAlwaysHealthPotion", 0);
            AudioManager.admgTHIS.PlayOneShotASound("ClickSound2");
        }
    }
    #endregion
}