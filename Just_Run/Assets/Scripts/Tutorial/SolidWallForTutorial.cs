using System.Collections;
using UnityEngine;


/// <summary>
/// 
/// Bu Script ��retici Moddaki Sa�lam Duvar� Kontrol Eder. Bu Y�zden Baz� �eyler Farkl�d�r. Normal Sa�lam Duvar �le Bir Alakas� Yok!
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
        // Oyuncu Duvar�n �st�nden Z�playarak Duvar� Ge�erse �al���r.
        if (temas.gameObject.name == "BorderLine0")
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