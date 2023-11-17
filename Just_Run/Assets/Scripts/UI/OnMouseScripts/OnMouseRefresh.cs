using UnityEngine;


/// <summary>
/// 
/// Bu Scriptin Amacý Oyundaki Sýfýrlama Panelini Açmaktýr.
/// Oyuncu Ana Menüde Sað Alttaki Kartal Ýkonuna Basarak Bu Paneli Açabilir.
/// 
/// </summary>
public class OnMouseRefresh : MonoBehaviour
{
    // Ýmleç Kartalýn Üstüne Gelince Kartal Boyutu Büyür.
    private void OnMouseEnter() => gameObject.GetComponent<RectTransform>().localScale = new Vector2(1.3f, 1.3f);

    // Ýmleç Kartalýn Üstünden Ayrýlýnca Kartal Eski Boyutuna Geri Döner.
    private void OnMouseExit() => gameObject.GetComponent<RectTransform>().localScale = new Vector2(1.0f, 1.0f);

    // Oyuncu Ýkona Týklayarak Sýfýrlama Panelini Açar.
    private void OnMouseDown()
    {
        // Kartal Eski Boyutuna Geri Döner.
        gameObject.GetComponent<RectTransform>().localScale = new Vector2(1.0f, 1.0f);

        // Panel Açýlýp Ses Efekti Çalýnýr.
        GameObject.Find("Canvas").transform.GetChild(8).gameObject.SetActive(true);
        GameObject.Find("Canvas").transform.GetChild(9).gameObject.SetActive(true);
        AudioManager.admgTHIS.PlayOneShotASound("ClickSound1");

        // Arkaplanda Kalan Butonlar Pasif Hale Getirilir.
        GameObject.Find("Canvas/PlayBTN").GetComponent<BoxCollider2D>().enabled = false;
        GameObject.Find("Canvas/Icons/Eagle").GetComponent<PolygonCollider2D>().enabled = false;
        GameObject.Find("Canvas/Icons/Knight").GetComponent<PolygonCollider2D>().enabled = false;
        GameObject.Find("Canvas/BestScoreTexts").GetComponent<BoxCollider2D>().enabled = false;
        for (sbyte i = 0; i < 3; i++)
            GameObject.Find("Canvas/SocialMediaURLS").transform.GetChild(i).GetComponent<BoxCollider2D>().enabled = false;
    }
}