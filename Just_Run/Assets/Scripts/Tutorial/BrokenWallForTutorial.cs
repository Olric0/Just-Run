using System.Collections;
using UnityEngine;


/// <summary>
/// 
/// Bu Script Öðretici Moddaki Kýrýk Duvarý Kontrol Eder. Bu Yüzden Bazý Þeyler Farklýdýr. Normal Kýrýk Duvar Ýle Bir Alakasý Yok!
/// 
/// </summary>
public class BrokenWallForTutorial : MonoBehaviour
{
    private IEnumerator Start()
    {
        while (true)
        {
            // Düþmanýn Oyuncuya Doðru Hareket Etmesi
            transform.Translate(Vector2.left * 5.7f * Time.deltaTime);
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D temas)
    {
        // Oyuncu Duvarý Kýrýnca Çalýþýr.
        if (temas.gameObject.name == "HitBox0")
        {
            // Görev Bitti Mi Diye Kontrol Eder.
            TutorialManager.tmTHIS.IsTheTaskOver();

            // Ses Efektini Tetikler Ve Düþmaný Siler.
            AudioManager.admgTHIS.PlayOneShotASound("HitObjectSound");
            Destroy(this.gameObject);
        }
        // Oyuncu Duvarýn Üstünden Zýplayarak Duvarý Geçerse Çalýþýr.
        else if (temas.gameObject.name == "BorderLine0")
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