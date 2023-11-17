using System.Collections;
using UnityEngine;


/// <summary>
/// 
/// Bu Script ��retici Moddaki K�r�k Duvar� Kontrol Eder. Bu Y�zden Baz� �eyler Farkl�d�r. Normal K�r�k Duvar �le Bir Alakas� Yok!
/// 
/// </summary>
public class BrokenWallForTutorial : MonoBehaviour
{
    private IEnumerator Start()
    {
        while (true)
        {
            // D��man�n Oyuncuya Do�ru Hareket Etmesi
            transform.Translate(Vector2.left * 5.7f * Time.deltaTime);
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D temas)
    {
        // Oyuncu Duvar� K�r�nca �al���r.
        if (temas.gameObject.name == "HitBox0")
        {
            // G�rev Bitti Mi Diye Kontrol Eder.
            TutorialManager.tmTHIS.IsTheTaskOver();

            // Ses Efektini Tetikler Ve D��man� Siler.
            AudioManager.admgTHIS.PlayOneShotASound("HitObjectSound");
            Destroy(this.gameObject);
        }
        // Oyuncu Duvar�n �st�nden Z�playarak Duvar� Ge�erse �al���r.
        else if (temas.gameObject.name == "BorderLine0")
        {
            // G�rev Bitti Mi Diye Kontrol Eder.
            TutorialManager.tmTHIS.IsTheTaskOver();

            // D��man� Siler.
            Destroy(this.gameObject);
        }
        // Oyuncu Duavara �arp�nca �al���r.
        else if (temas.gameObject.name == "HitBox1")
        {
            // Oyuncu Yenildi�i ��in G�revi Durdurur.
            TutorialManager.tmTHIS.StartCoroutine(TutorialManager.tmTHIS.StopTask());

            // Ses Efektini Tetikler Ve D��man� Siler.
            AudioManager.admgTHIS.PlayOneShotASound("BumpSound");
            Destroy(this.gameObject);
        }
    }
}