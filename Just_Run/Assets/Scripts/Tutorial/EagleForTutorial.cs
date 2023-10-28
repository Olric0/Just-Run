using System.Collections;
using UnityEngine;


/// <summary>
/// 
/// Bu Script Öðretici Moddaki Kartalý Kontrol Eder. Bu Yüzden Bazý Þeyler Farklýdýr. Normal Kartal Ýle Bir Alakasý Yok!
/// 
/// >*>*> Deðiþkenlerin Açýklamalarý <*<*< \\\
///     -> [ eagleDieSprite ]
///     Kartal Öldüðü Zaman SpriteRenderer'ýn Sprite'ýný Bu Deðiþkenin Sprite'ý Olarak Set Edilir.
///     
///     -> [ isMovingBirdDown ]
///     Kartal Öldüðü Zaman, Cesedin Aþaðýya Doðru Ýnmesini Kontrol Etmek Ýçin Kullanýlýr.
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

    // Oyuncu Kartalý Öldürdükten Sonra, Kartalýn Aþaðýya Doðru 1.5 Saniye Düþüp Silinmesini Saðlýyor.
    private IEnumerator WaitAndDestroyYourSelf()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D temas)
    {
        // Oyuncu Kartalý Öldürünce Çalýþýr.
        if (temas.gameObject.name == "HitBox0")
        {
            // Görev Bitti Mi Diye Kontrol Eder.
            TutorialManager.tmTHIS.IsTheTaskOver();

            // Kuþu Pasif Edip 1.5 Saniye Sonrasýnda Silinmesini Saðlar.
            isMovingBirdDown = true;
            gameObject.GetComponent<Animator>().enabled = false;
            gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            gameObject.GetComponent<SpriteRenderer>().sprite = eagleDieSprite;
            AudioManager.admgTHIS.PlayOneShotASound("HitObjectSound");
            StartCoroutine(WaitAndDestroyYourSelf());
        }
        // Oyuncu Kartalýn Altýndan Kaçarak Kuþu Geçerse Çalýþýr.
        else if (temas.gameObject.name == "BorderLine0")
        {
            // Görev Bitti Mi Diye Kontrol Eder.
            TutorialManager.tmTHIS.IsTheTaskOver();

            // Düþmaný Siler.
            Destroy(this.gameObject);
        }
        // Oyuncu Kartala Çarptýðýnda Çalýþýr.
        else if (temas.gameObject.name == "HitBox1")
        {
            // Oyuncu Yenildiði Ýçin Görevi Durdurur.
            TutorialManager.tmTHIS.StartCoroutine(TutorialManager.tmTHIS.StopTask());

            // Ses Efekti Tetiklenip, Düþman Silinir.
            AudioManager.admgTHIS.PlayOneShotASound("BumpSound");
            Destroy(this.gameObject);
        }
    }
}