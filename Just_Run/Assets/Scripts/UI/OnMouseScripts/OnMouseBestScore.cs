using UnityEngine;


/// <summary>
/// 
/// Bu Script Sayesinde Oyuncu Ana Menüde Fareyi  [ En Ýyi Skor: ... ]  Yazýsýnýn
/// Üzerine Götürürse, Bir Önceki En Yüksek Skoru Görebilir.
/// 
/// </summary>
public class OnMouseBestScore : MonoBehaviour
{
    private void OnMouseEnter()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(true);
    }
    private void OnMouseExit()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
    }
}