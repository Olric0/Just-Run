using UnityEngine;


/// <summary>
/// 
/// Bu Script Sayesinde Oyuncu Ana Men�de Fareyi  [ En �yi Skor: ... ]  Yaz�s�n�n
/// �zerine G�t�r�rse, Bir �nceki En Y�ksek Skoru G�rebilir.
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