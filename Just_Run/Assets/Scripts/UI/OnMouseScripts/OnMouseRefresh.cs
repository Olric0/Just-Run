using UnityEngine;


/// <summary>
/// 
/// Bu Scriptin Amac� Oyundaki S�f�rlama Panelini A�makt�r.
/// Oyuncu Ana Men�de Sa� Alttaki Kartal �konuna Basarak Bu Paneli A�abilir.
/// 
/// </summary>
public class OnMouseRefresh : MonoBehaviour
{
    // �mle� Kartal�n �st�ne Gelince Kartal Boyutu B�y�r.
    private void OnMouseEnter() => gameObject.GetComponent<RectTransform>().localScale = new Vector2(1.3f, 1.3f);

    // �mle� Kartal�n �st�nden Ayr�l�nca Kartal Eski Boyutuna Geri D�ner.
    private void OnMouseExit() => gameObject.GetComponent<RectTransform>().localScale = new Vector2(1.0f, 1.0f);

    // Oyuncu �kona T�klayarak S�f�rlama Panelini A�ar.
    private void OnMouseDown()
    {
        // Kartal Eski Boyutuna Geri D�ner.
        gameObject.GetComponent<RectTransform>().localScale = new Vector2(1.0f, 1.0f);

        // Panel A��l�p Ses Efekti �al�n�r.
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