using System.Collections;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 
/// Bu Scriptin Amacý Oyundaki Hile Sistemini Kapatýp Açmaktýr.
/// Oyuncu Ana Menüde Sol Alttaki Karakter Ýkonuna Basarak Hileyi Açýp Kapatabilir.
/// 
/// </summary>
public class OnMouseCheat : MonoBehaviour
{
    [Header("Icon Sprites")]
    [SerializeField] private Sprite knightCheatStrikeSprite;
    [SerializeField] private Sprite knightStrikeSprite;
    [SerializeField] private Sprite cheatSettingsPanel;
    [SerializeField] private Sprite settingsPanel;
    public static bool isCheatAnimPlaying;



    // Ýmleç Karakterin Üstüne Gelince Karakterin Boyutu Büyür.
    private void OnMouseEnter()
    {
        // Bu If'in Amacý, Oyuncu Hileyi Açmak Ýçin Týkladýðýnda Animasyon Esnasýnda
        // Ýmleç Ýle Ýkonun Üstüne Girip Çýkarak Ýkonu Buga Sokmasýný Engellemek.
        if (isCheatAnimPlaying == false)
        {
            RectTransform characterIconTransform = gameObject.GetComponent<RectTransform>();
            characterIconTransform.localScale = new Vector2(2.5f, 2.5f);
            characterIconTransform.anchoredPosition = new Vector2(-492.5f, -325.5f);
            gameObject.GetComponent<PolygonCollider2D>().offset = new Vector2(5.0f, -5.0f);
        }
    }
    // Ýmleç Karakterin Üstünden Ayrýlýnca Karakter Eski Boyutuna Geri Döner.
    private void OnMouseExit()
    {
        // Bu If'in Amacý, Oyuncu Hileyi Açmak Ýçin Týkladýðýnda Animasyon Esnasýnda
        // Ýmleç Ýle Ýkonun Üstüne Girip Çýkarak Ýkonu Buga Sokmasýný Engellemek.
        if (isCheatAnimPlaying == false)
        {
            RectTransform characterIconTransform = gameObject.GetComponent<RectTransform>();
            characterIconTransform.localScale = new Vector2(2.1f, 2.1f);
            characterIconTransform.anchoredPosition = new Vector2(-527.0f, -353.0f);
            gameObject.GetComponent<PolygonCollider2D>().offset = new Vector2(0.0f, 0.0f);
        }
    }

    // Oyuncu Ýkona Týklayarak Hileyi Kapatýr Veya Açar.
    private void OnMouseDown()
    {
        // Olasý Buglarý Engellemek Ýçin Deðiþken Ayarý.
        isCheatAnimPlaying = true;

        // Sahnedeki Bütün Etkileþimli Nesneleri Pasif Etme.
        gameObject.GetComponent<PolygonCollider2D>().enabled = false;
        GameObject.Find("Canvas").transform.GetChild(2).GetComponent<Button>().enabled = false;
        GameObject.Find("Canvas").transform.GetChild(3).GetComponent<Button>().enabled = false;
        GameObject.Find("Canvas/QuitBTN").GetComponent<Button>().enabled = false;
        GameObject.Find("Canvas/PlayBTN").GetComponent<Button>().enabled = false;
        GameObject.Find("Canvas/PlayBTN").GetComponent<BoxCollider2D>().enabled = false;
        GameObject.Find("Canvas/Icons/Eagle").GetComponent<PolygonCollider2D>().enabled = false;
        GameObject.Find("Canvas/BestScoreTexts").GetComponent<BoxCollider2D>().enabled = false;
        for (sbyte i = 0; i < 3; i++)
        {
            GameObject.Find("Canvas/SocialMediaURLS").transform.GetChild(i).GetComponent<BoxCollider2D>().enabled = false;
        }

        // Kamera Animasyonunu Tetikleme.
        GameObject.Find("MainCamera").GetComponent<Animator>().SetTrigger("cameraCheatAnim");

        // Hile Aktif Olur Ya Da Pasif Olur. Ardýndan Karakter Ýkonunun Animasyonu  [ RunKnightIconAnim ]  Metodu Ýle Tetiklenir.
        // Parametre Deðeri  [ TRUE ]  Ýse Hile Aktif Edilir. Parametre Deðeri  [ FALSE ]  Ýse Hile Pasif Edilir.
        if (PlayerPrefs.GetInt("isCheatModeActive") == 0)
        {
            PlayerPrefs.SetInt("isCheatModeActive", 1);
            StartCoroutine(RunKnightIconAnim(true));
        }
        else
        {
            PlayerPrefs.SetInt("isCheatModeActive", 0);
            StartCoroutine(RunKnightIconAnim(false));
        }
    }


    // Kamera Animasyonu Ýle Eþ Zamanlý Bir Þekilde Karakter Ýkonunun Rengi Deðiþir.
    private IEnumerator RunKnightIconAnim(bool value)
    {
        // Karakterin Renginin Animasyonlu Bir Þekilde Kýrmýzýya Dönmesi.
        Image knightIcon = GetComponent<Image>();
        for (byte i = 255; i >= 105; i--)
        {
            knightIcon.color = new Color32(255, i, i, 255);
            yield return new WaitForSecondsRealtime(0.01f);
        }
        knightIcon.color = new Color32(255, 105, 105, 255);


        // Karakterin Sprite'ýnýn Deðiþmesi.
        if (value == true)
            knightIcon.sprite = knightCheatStrikeSprite;
        else
            knightIcon.sprite = knightStrikeSprite;


        // Karakterin Renginin Animasyonlu Bir Þekilde Normal Haline Dönmesi.
        for (byte i = 105; i <= 254; i++)
        {
            knightIcon.color = new Color32(255, i, i, 255);
            yield return new WaitForSecondsRealtime(0.01f);
        }
        knightIcon.color = new Color32(255, 255, 255, 255);

        // Hile Baþlýðýnýn Animasyonunun Tetiklenmesi.
        if (value == true)
        {
            StartCoroutine(RunCheatTitleAnim(true));
            StartCoroutine(RunSettingsButtonAnim(true));
        }
        else
        {
            StartCoroutine(RunCheatTitleAnim(false));
            StartCoroutine(RunSettingsButtonAnim(false));
        }
    }

    // Aþaðýdaki 2 Metot  { RunKnightIconAnim }  Metodu Bitince Çalýþýr.
    //    RunCheatTitleAnim      =>  Hile Yazýsýnýn Animasyonunu Kontrol Eder.
    //    RunSettingsButtonAnim  =>  Ayarlar Butonunun Animasyonunu Kontrol Eder.
    private IEnumerator RunCheatTitleAnim(bool value)
    {
        // Parametre True Ýse Hile Yazýsý Aktif Ediliyor.
        if (value == true)
        {
            // Hile Baþlýðýnýn Aktif Edilmesi.
            GameObject.Find("Canvas/Titles/CheatTitle").SetActive(true);

            // Hile Baþlýðýnýn Ve Ayarlar Butonunun Açýlma Animasyonu.
            RectTransform cheatTitleTransform = GameObject.Find("Canvas/Titles/CheatTitle").GetComponent<RectTransform>();
            for (float i = 0.0f; i <= 2.2f; i = i + 0.025f)
            {
                cheatTitleTransform.localScale = new Vector2(i, 2.0f);
                yield return new WaitForSecondsRealtime(0.01f);
            }
            cheatTitleTransform.localScale = new Vector2(2.2f, 2.0f);
        }
        // Parametre False Ýse Hile Yazýsý Pasif Ediliyor.
        else
        {
            // Hile Baþlýðýnýn Animasyonu.
            RectTransform cheatTitleTransform = GameObject.Find("Canvas/Titles/CheatTitle").GetComponent<RectTransform>();
            for (float i = 2.2f; i >= 0.0f; i = i - 0.025f)
            {
                cheatTitleTransform.localScale = new Vector2(i, 2.0f);
                yield return new WaitForSecondsRealtime(0.01f);
            }
            cheatTitleTransform.localScale = new Vector2(0.0f, 2.0f);

            // Hile Baþlýðýnýn Pasif Edilmesi.
            GameObject.Find("Canvas/Titles/CheatTitle").SetActive(false);
        }
    }
    private IEnumerator RunSettingsButtonAnim(bool value)
    {
        if (value == true)
        {
            // Hileli Ayarlar Butonunun Aktif Edilmesi.
            GameObject.Find("Canvas").transform.GetChild(2).gameObject.SetActive(true);

            // Hile Baþlýðýnýn Ve Ayarlar Butonunun Açýlma Animasyonu.
            Image settingsButtonImage = GameObject.Find("Canvas").transform.GetChild(3).GetComponent<Image>();
            for (byte i = 254; i >= 1; i--)
            {
                settingsButtonImage.color = new Color32(255, 255, 255, i);
                yield return new WaitForSecondsRealtime(0.004f);
            }
            settingsButtonImage.color = new Color32(255, 255, 255, 0);

            // Normal Ayarlar Butonunu Kapatýyor. Çünkü Hileli Ayarlar Butonu Aktif Edildi.
            GameObject.Find("Canvas").transform.GetChild(3).gameObject.SetActive(false);


            // Kapanýþ.
            yield return new WaitForSecondsRealtime(0.5f);
            Finish(true);
        }
        else
        {
            // Normal Ayarlar Butonunun Aktif Edilmesi.
            GameObject.Find("Canvas").transform.GetChild(3).gameObject.SetActive(true);

            // Hile Baþlýðýnýn Ve Ayarlar Butonunun Açýlma Animasyonu.
            Image settingsButtonImage = GameObject.Find("Canvas").transform.GetChild(3).GetComponent<Image>();
            for (byte i = 0; i <= 254; i++)
            {
                settingsButtonImage.color = new Color32(255, 255, 255, i);
                yield return new WaitForSecondsRealtime(0.004f);
            }
            settingsButtonImage.color = new Color32(255, 255, 255, 255);

            // Hileli Ayarlar Butonunun Pasif Edilmesi.
            GameObject.Find("Canvas").transform.GetChild(2).gameObject.SetActive(false);

            // Kapanýþ.
            yield return new WaitForSecondsRealtime(0.5f);
            Finish(false);
        }
    }

    // Bütün Animasyonlar Bitip Hile Açýldýktan Veya Kapandýktan Sonra En Son Çalýþýr.
    private void Finish(bool value)
    {
        // Karakterin Eski Haline Dönmesi.
        RectTransform characterIconTransform = gameObject.GetComponent<RectTransform>();
        characterIconTransform.localScale = new Vector2(2.1f, 2.1f);
        characterIconTransform.anchoredPosition = new Vector2(-527.0f, -353.0f);
        gameObject.GetComponent<PolygonCollider2D>().offset = new Vector2(0.0f, 0.0f);

        // Sahnedeki Bütün Etkileþimli Nesneleri Tekrardan Aktif Etme.
        gameObject.GetComponent<PolygonCollider2D>().enabled = true;
        GameObject.Find("Canvas").transform.GetChild(2).GetComponent<Button>().enabled = true;
        GameObject.Find("Canvas").transform.GetChild(3).GetComponent<Button>().enabled = true;
        GameObject.Find("Canvas/QuitBTN").GetComponent<Button>().enabled = true;
        GameObject.Find("Canvas/PlayBTN").GetComponent<Button>().enabled = true;
        GameObject.Find("Canvas/PlayBTN").GetComponent<BoxCollider2D>().enabled = true;
        GameObject.Find("Canvas/Icons/Eagle").GetComponent<PolygonCollider2D>().enabled = true;
        GameObject.Find("Canvas/BestScoreTexts").GetComponent<BoxCollider2D>().enabled = true;
        for (sbyte i = 0; i < 3; i++)
            GameObject.Find("Canvas/SocialMediaURLS").transform.GetChild(i).GetComponent<BoxCollider2D>().enabled = true;

        // Hileyi Açma Veya Kapatma.
        if (value == true)
        {
            // Deðiþken Atamalarý.
            Transform canvasPath = GameObject.Find("Canvas").transform.GetChild(10);

            // Ayarlar Panelini Hileli Ayarlar Paneline Çevirme.
            canvasPath.GetComponent<Image>().sprite = cheatSettingsPanel;
            canvasPath.GetComponent<Image>().SetNativeSize();
            canvasPath.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(-326.0f, 0.0f);
            canvasPath.GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector2(-326.0f, 0.0f);
            canvasPath.GetChild(3).gameObject.SetActive(true);
        }
        else
        {
            // Deðiþken Atamalarý.
            Transform canvasPath = GameObject.Find("Canvas").transform.GetChild(10);

            // Ayarlar Panelini Normal Ayarlar Paneline Çevirme.
            canvasPath.GetComponent<Image>().sprite = settingsPanel;
            canvasPath.GetComponent<Image>().SetNativeSize();
            canvasPath.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, 0.0f);
            canvasPath.GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, 0.0f);
            canvasPath.GetChild(3).gameObject.SetActive(false);
        }

        // OnMouseEnter Ve OnMouseExit Metodlarýnýn Tekrar Çalýþýlabilmesini Saðlama.
        isCheatAnimPlaying = false;
    }
}