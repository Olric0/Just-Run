using System.Collections;
using UnityEngine;


/// <summary>
/// 
/// Bu Script Fýrlatýlan Ateþ Topunu Kontrol Eder. Animasyon Halinde Olan, UI'da Aþaðýda
/// Olan Ateþ Topu Deðil Karýþtýrma! Deðiþkenleri Adý Üstünde Olduðu Ýçin Anlayabilirsin.
/// Normal Ateþ Topu Ýle Bir Alakasý Yok! Bu Sadece Öðretici Modda Çalýþýr Adý Üstünde Yani.
/// 
/// </summary>
public class FireballForTutorial : MonoBehaviour
{
    private bool didTheFireballHitAnyEnemie;



    private IEnumerator Start()
    {
        // Temel Ayarlamalar, Baþlangýç Ýçin Hazýrlýk.
        transform.SetParent(null);
        StartCoroutine(DestroyYourselfAfter2Second());
        AudioManager.admgTHIS.PlayOneShotASound("FireballSound");

        // Bu Deðiþken True Deðerinde Olduðu Süre Boyunca Ateþ Topu Bir Düþmana Çarpmamýþ Demektir. Bu Sebeple Ateþ Topu
        // Sað Tarafa Doðru Ýlerler. Ne Zamanki Ateþ Topu Bir Düþmana Çarpar, Deðiþken False Olur Ve Döngüden Çýkýlýr.
        while (didTheFireballHitAnyEnemie == false)
        {
            gameObject.transform.Translate(Vector2.right * 6.0f * Time.deltaTime);
            yield return null;
        }

        // Ateþ Topu Bir Düþmana Çarptýðý Ýçin Artýk Ýleriye Gitmiyor, Sola Doðru Gidiyor Yoksa Göz Yanýlsamasý Olur.
        while (true)
        {
            gameObject.transform.Translate(Vector2.left * 5.7f * Time.deltaTime);
            yield return null;
        }
    }

    // Ateþ Topu 2 Saniye Ýlerledikten Sonra Kendini Yok Eder.
    private IEnumerator DestroyYourselfAfter2Second()
    {
        yield return new WaitForSeconds(2.0f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D temas)
    {
        if (temas.CompareTag("Enemies"))
        {
            // Yenilen Düþman Sayýsýný Bir Arttýrýr Ve Bir If'e Girerek Oyuncunun
            // Hangi Görevde Ve Görevin Baþarýsýz Olup Olmadýðýný Kontrol Eder.
            TutorialManager tutorialMngr = TutorialManager.tmTHIS;
            tutorialMngr.numberOfEnemiesKilled++;
            if (tutorialMngr.numberOfEnemiesKilled == 4 && tutorialMngr.numberOfTask == 1)
                tutorialMngr.StartCoroutine(tutorialMngr.TypeWriterController1());
            else if (tutorialMngr.numberOfEnemiesKilled == 7 && tutorialMngr.numberOfTask == 2)
                tutorialMngr.StartCoroutine(tutorialMngr.TypeWriterController2());
            else if (tutorialMngr.numberOfEnemiesKilled == 14 && tutorialMngr.numberOfTask == 3)
                tutorialMngr.StartCoroutine(tutorialMngr.TypeWriterController3());
            else if (tutorialMngr.numberOfEnemiesKilled == 25 && tutorialMngr.numberOfTask == 4)
                tutorialMngr.StartCoroutine(tutorialMngr.TypeWriterController4());

            // Patlama Animasyonunu Aktifleþtirme Ve Deðiþken Setlemesi.
            gameObject.transform.position = new Vector2(transform.position.x + 0.35f, transform.position.y + 0.25f);
            gameObject.GetComponent<Animator>().SetTrigger("didTheFireballHitAnyEnemie");
            didTheFireballHitAnyEnemie = true;

            // Ateþ Topunun Fýrlatma Sesini Kapatýp, Patlama Sesini Açma.
            AudioSource soundManager = GameObject.Find("AudioManager/SoundManager").GetComponent<AudioSource>();
            soundManager.enabled = false;
            soundManager.enabled = true;

            // Ateþ Topu, Kuþa Çarpmýþsa Baþka Bir Ses Efektinin Çalýnmasýný Saðlar.
            if (temas.gameObject.name == "Eagle")
                AudioManager.admgTHIS.PlayOneShotASound("ExplosionSound1");
            else
                AudioManager.admgTHIS.PlayOneShotASound("ExplosionSound2");
            
            // Ateþ Topunun Çarptýðý Düþmaný Silme.
            Destroy(temas.gameObject);
        }
    }
}