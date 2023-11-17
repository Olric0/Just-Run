using System.Collections;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 
/// Bu Scriptin Amac� Oyundaki Hile Sistemini Kapat�p A�makt�r.
/// Oyuncu Ana Men�de Sol Alttaki Karakter �konuna Basarak Hileyi A��p Kapatabilir.
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



    // �mle� Karakterin �st�ne Gelince Karakterin Boyutu B�y�r.
    private void OnMouseEnter()
    {
        // Bu If'in Amac�, Oyuncu Hileyi A�mak ��in T�klad���nda Animasyon Esnas�nda
        // �mle� �le �konun �st�ne Girip ��karak �konu Buga Sokmas�n� Engellemek.
        if (isCheatAnimPlaying == false)
        {
            RectTransform characterIconTransform = gameObject.GetComponent<RectTransform>();
            characterIconTransform.localScale = new Vector2(2.5f, 2.5f);
            characterIconTransform.anchoredPosition = new Vector2(-492.5f, -325.5f);
            gameObject.GetComponent<PolygonCollider2D>().offset = new Vector2(5.0f, -5.0f);
        }
    }
    // �mle� Karakterin �st�nden Ayr�l�nca Karakter Eski Boyutuna Geri D�ner.
    private void OnMouseExit()
    {
        // Bu If'in Amac�, Oyuncu Hileyi A�mak ��in T�klad���nda Animasyon Esnas�nda
        // �mle� �le �konun �st�ne Girip ��karak �konu Buga Sokmas�n� Engellemek.
        if (isCheatAnimPlaying == false)
        {
            RectTransform characterIconTransform = gameObject.GetComponent<RectTransform>();
            characterIconTransform.localScale = new Vector2(2.1f, 2.1f);
            characterIconTransform.anchoredPosition = new Vector2(-527.0f, -353.0f);
            gameObject.GetComponent<PolygonCollider2D>().offset = new Vector2(0.0f, 0.0f);
        }
    }

    // Oyuncu �kona T�klayarak Hileyi Kapat�r Veya A�ar.
    private void OnMouseDown()
    {
        // Olas� Buglar� Engellemek ��in De�i�ken Ayar�.
        isCheatAnimPlaying = true;

        // Sahnedeki B�t�n Etkile�imli Nesneleri Pasif Etme.
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

        // Hile Aktif Olur Ya Da Pasif Olur. Ard�ndan Karakter �konunun Animasyonu  [ RunKnightIconAnim ]  Metodu �le Tetiklenir.
        // Parametre De�eri  [ TRUE ]  �se Hile Aktif Edilir. Parametre De�eri  [ FALSE ]  �se Hile Pasif Edilir.
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


    // Kamera Animasyonu �le E� Zamanl� Bir �ekilde Karakter �konunun Rengi De�i�ir.
    private IEnumerator RunKnightIconAnim(bool value)
    {
        // Karakterin Renginin Animasyonlu Bir �ekilde K�rm�z�ya D�nmesi.
        Image knightIcon = GetComponent<Image>();
        for (byte i = 255; i >= 105; i--)
        {
            knightIcon.color = new Color32(255, i, i, 255);
            yield return new WaitForSecondsRealtime(0.01f);
        }
        knightIcon.color = new Color32(255, 105, 105, 255);


        // Karakterin Sprite'�n�n De�i�mesi.
        if (value == true)
            knightIcon.sprite = knightCheatStrikeSprite;
        else
            knightIcon.sprite = knightStrikeSprite;


        // Karakterin Renginin Animasyonlu Bir �ekilde Normal Haline D�nmesi.
        for (byte i = 105; i <= 254; i++)
        {
            knightIcon.color = new Color32(255, i, i, 255);
            yield return new WaitForSecondsRealtime(0.01f);
        }
        knightIcon.color = new Color32(255, 255, 255, 255);

        // Hile Ba�l���n�n Animasyonunun Tetiklenmesi.
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

    // A�a��daki 2 Metot  { RunKnightIconAnim }  Metodu Bitince �al���r.
    //    RunCheatTitleAnim      =>  Hile Yaz�s�n�n Animasyonunu Kontrol Eder.
    //    RunSettingsButtonAnim  =>  Ayarlar Butonunun Animasyonunu Kontrol Eder.
    private IEnumerator RunCheatTitleAnim(bool value)
    {
        // Parametre True �se Hile Yaz�s� Aktif Ediliyor.
        if (value == true)
        {
            // Hile Ba�l���n�n Aktif Edilmesi.
            GameObject.Find("Canvas/Titles/CheatTitle").SetActive(true);

            // Hile Ba�l���n�n Ve Ayarlar Butonunun A��lma Animasyonu.
            RectTransform cheatTitleTransform = GameObject.Find("Canvas/Titles/CheatTitle").GetComponent<RectTransform>();
            for (float i = 0.0f; i <= 2.2f; i = i + 0.025f)
            {
                cheatTitleTransform.localScale = new Vector2(i, 2.0f);
                yield return new WaitForSecondsRealtime(0.01f);
            }
            cheatTitleTransform.localScale = new Vector2(2.2f, 2.0f);
        }
        // Parametre False �se Hile Yaz�s� Pasif Ediliyor.
        else
        {
            // Hile Ba�l���n�n Animasyonu.
            RectTransform cheatTitleTransform = GameObject.Find("Canvas/Titles/CheatTitle").GetComponent<RectTransform>();
            for (float i = 2.2f; i >= 0.0f; i = i - 0.025f)
            {
                cheatTitleTransform.localScale = new Vector2(i, 2.0f);
                yield return new WaitForSecondsRealtime(0.01f);
            }
            cheatTitleTransform.localScale = new Vector2(0.0f, 2.0f);

            // Hile Ba�l���n�n Pasif Edilmesi.
            GameObject.Find("Canvas/Titles/CheatTitle").SetActive(false);
        }
    }
    private IEnumerator RunSettingsButtonAnim(bool value)
    {
        if (value == true)
        {
            // Hileli Ayarlar Butonunun Aktif Edilmesi.
            GameObject.Find("Canvas").transform.GetChild(2).gameObject.SetActive(true);

            // Hile Ba�l���n�n Ve Ayarlar Butonunun A��lma Animasyonu.
            Image settingsButtonImage = GameObject.Find("Canvas").transform.GetChild(3).GetComponent<Image>();
            for (byte i = 254; i >= 1; i--)
            {
                settingsButtonImage.color = new Color32(255, 255, 255, i);
                yield return new WaitForSecondsRealtime(0.004f);
            }
            settingsButtonImage.color = new Color32(255, 255, 255, 0);

            // Normal Ayarlar Butonunu Kapat�yor. ��nk� Hileli Ayarlar Butonu Aktif Edildi.
            GameObject.Find("Canvas").transform.GetChild(3).gameObject.SetActive(false);


            // Kapan��.
            yield return new WaitForSecondsRealtime(0.5f);
            Finish(true);
        }
        else
        {
            // Normal Ayarlar Butonunun Aktif Edilmesi.
            GameObject.Find("Canvas").transform.GetChild(3).gameObject.SetActive(true);

            // Hile Ba�l���n�n Ve Ayarlar Butonunun A��lma Animasyonu.
            Image settingsButtonImage = GameObject.Find("Canvas").transform.GetChild(3).GetComponent<Image>();
            for (byte i = 0; i <= 254; i++)
            {
                settingsButtonImage.color = new Color32(255, 255, 255, i);
                yield return new WaitForSecondsRealtime(0.004f);
            }
            settingsButtonImage.color = new Color32(255, 255, 255, 255);

            // Hileli Ayarlar Butonunun Pasif Edilmesi.
            GameObject.Find("Canvas").transform.GetChild(2).gameObject.SetActive(false);

            // Kapan��.
            yield return new WaitForSecondsRealtime(0.5f);
            Finish(false);
        }
    }

    // B�t�n Animasyonlar Bitip Hile A��ld�ktan Veya Kapand�ktan Sonra En Son �al���r.
    private void Finish(bool value)
    {
        // Karakterin Eski Haline D�nmesi.
        RectTransform characterIconTransform = gameObject.GetComponent<RectTransform>();
        characterIconTransform.localScale = new Vector2(2.1f, 2.1f);
        characterIconTransform.anchoredPosition = new Vector2(-527.0f, -353.0f);
        gameObject.GetComponent<PolygonCollider2D>().offset = new Vector2(0.0f, 0.0f);

        // Sahnedeki B�t�n Etkile�imli Nesneleri Tekrardan Aktif Etme.
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

        // Hileyi A�ma Veya Kapatma.
        if (value == true)
        {
            // De�i�ken Atamalar�.
            Transform canvasPath = GameObject.Find("Canvas").transform.GetChild(10);

            // Ayarlar Panelini Hileli Ayarlar Paneline �evirme.
            canvasPath.GetComponent<Image>().sprite = cheatSettingsPanel;
            canvasPath.GetComponent<Image>().SetNativeSize();
            canvasPath.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(-326.0f, 0.0f);
            canvasPath.GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector2(-326.0f, 0.0f);
            canvasPath.GetChild(3).gameObject.SetActive(true);
        }
        else
        {
            // De�i�ken Atamalar�.
            Transform canvasPath = GameObject.Find("Canvas").transform.GetChild(10);

            // Ayarlar Panelini Normal Ayarlar Paneline �evirme.
            canvasPath.GetComponent<Image>().sprite = settingsPanel;
            canvasPath.GetComponent<Image>().SetNativeSize();
            canvasPath.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, 0.0f);
            canvasPath.GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, 0.0f);
            canvasPath.GetChild(3).gameObject.SetActive(false);
        }

        // OnMouseEnter Ve OnMouseExit Metodlar�n�n Tekrar �al���labilmesini Sa�lama.
        isCheatAnimPlaying = false;
    }
}