using System.Collections;
using UnityEngine;


/// <summary>
/// 
/// Bu Script Öðretici Moddaki Saðlam Duvarý Kontrol Eder. Bu Yüzden Bazý Þeyler Farklýdýr. Normal Saðlam Duvar Ýle Bir Alakasý Yok!
/// 
/// </summary>
public class SolidWallForTutorial : MonoBehaviour
{
    private IEnumerator Start()
    {
        while (true)
        {
            // Hareket.
            transform.Translate(Vector2.left * 5.7f * Time.deltaTime);
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D temas)
    {
        // Oyuncu Duvarýn Üstünden Zýplayarak Duvarý Geçerse Çalýþýr.
        if (temas.gameObject.name == "BorderLine0")
        {
            // Görev Bitti Mi Diye Kontrol Eder.
            TutorialManager.tmTHIS.IsTheTaskOver();

            // Düþmaný Siler.
            Destroy(this.gameObject);
        }
        // Oyuncu Duavara Çarpýnca Çalýþýr.
        else if (temas.gameObject.name == "HitBox1")
        {
            // Oyuncu Yenildiði Ýçin Görevi Durdurur.
            TutorialManager.tmTHIS.StartCoroutine(TutorialManager.tmTHIS.StopTask());

            // Ses Efektini Tetikler Ve Düþmaný Siler.
            AudioManager.admgTHIS.PlayOneShotASound("BumpSound");
            Destroy(this.gameObject);
        }
    }
}