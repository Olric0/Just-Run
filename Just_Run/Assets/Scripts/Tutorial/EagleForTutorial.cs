using System.Collections;
using UnityEngine;


/// <summary>
/// 
/// Bu Script ��retici Moddaki Kartal� Kontrol Eder. Bu Y�zden Baz� �eyler Farkl�d�r. Normal Kartal �le Bir Alakas� Yok!
/// 
/// >*>*> De�i�kenlerin A��klamalar� <*<*< \\\
///     -> [ eagleDieSprite ]
///     Kartal �ld��� Zaman SpriteRenderer'�n Sprite'�n� Bu De�i�kenin Sprite'� Olarak Set Edilir.
///     
///     -> [ isMovingBirdDown ]
///     Kartal �ld��� Zaman, Cesedin A�a��ya Do�ru �nmesini Kontrol Etmek ��in Kullan�l�r.
/// 
/// </summary>
public class EagleForTutorial : MonoBehaviour
{
    [SerializeField] private Sprite eagleDieSprite;
    private bool isMovingBirdDown;



    private IEnumerator Start()
    {
        while (true)
        {
            // Hareket.
            if (isMovingBirdDown == false)
                transform.Translate(Vector2.left * 7.0f * Time.deltaTime);
            else
                transform.Translate(Vector2.down * 5.5f * Time.deltaTime);

            yield return null;
        }
    }

    // Oyuncu Kartal� �ld�rd�kten Sonra, Kartal�n A�a��ya Do�ru 1.5 Saniye D���p Silinmesini Sa�l�yor.
    private IEnumerator WaitAndDestroyYourSelf()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D temas)
    {
        // Oyuncu Kartal� �ld�r�nce �al���r.
        if (temas.gameObject.name == "HitBox0")
        {
            // G�rev Bitti Mi Diye Kontrol Eder.
            TutorialManager.tmTHIS.IsTheTaskOver();

            // Ku�u Pasif Edip 1.5 Saniye Sonras�nda Silinmesini Sa�lar.
            isMovingBirdDown = true;
            gameObject.GetComponent<Animator>().enabled = false;
            gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            gameObject.GetComponent<SpriteRenderer>().sprite = eagleDieSprite;
            AudioManager.admgTHIS.PlayOneShotASound("HitObjectSound");
            StartCoroutine(WaitAndDestroyYourSelf());
        }
        // Oyuncu Kartal�n Alt�ndan Ka�arak Ku�u Ge�erse �al���r.
        else if (temas.gameObject.name == "BorderLine0")
        {
            // G�rev Bitti Mi Diye Kontrol Eder.
            TutorialManager.tmTHIS.IsTheTaskOver();

            // D��man� Siler.
            Destroy(this.gameObject);
        }
        // Oyuncu Kartala �arpt���nda �al���r.
        else if (temas.gameObject.name == "HitBox1")
        {
            // Oyuncu Yenildi�i ��in G�revi Durdurur.
            TutorialManager.tmTHIS.StartCoroutine(TutorialManager.tmTHIS.StopTask());

            // Ses Efekti Tetiklenip, D��man Silinir.
            AudioManager.admgTHIS.PlayOneShotASound("BumpSound");
            Destroy(this.gameObject);
        }
    }
}