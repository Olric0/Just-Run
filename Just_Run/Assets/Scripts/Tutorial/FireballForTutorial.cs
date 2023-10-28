using System.Collections;
using UnityEngine;


/// <summary>
/// 
/// Bu Script F�rlat�lan Ate� Topunu Kontrol Eder. Animasyon Halinde Olan, UI'da A�a��da
/// Olan Ate� Topu De�il Kar��t�rma! De�i�kenleri Ad� �st�nde Oldu�u ��in Anlayabilirsin.
/// Normal Ate� Topu �le Bir Alakas� Yok! Bu Sadece ��retici Modda �al���r Ad� �st�nde Yani.
/// 
/// </summary>
public class FireballForTutorial : MonoBehaviour
{
    private bool didTheFireballHitAnyEnemie;



    private IEnumerator Start()
    {
        // Temel Ayarlamalar, Ba�lang�� ��in Haz�rl�k.
        transform.SetParent(null);
        StartCoroutine(DestroyYourselfAfter2Second());
        AudioManager.admgTHIS.PlayOneShotASound("FireballSound");

        // Bu De�i�ken True De�erinde Oldu�u S�re Boyunca Ate� Topu Bir D��mana �arpmam�� Demektir. Bu Sebeple Ate� Topu
        // Sa� Tarafa Do�ru �lerler. Ne Zamanki Ate� Topu Bir D��mana �arpar, De�i�ken False Olur Ve D�ng�den ��k�l�r.
        while (didTheFireballHitAnyEnemie == false)
        {
            gameObject.transform.Translate(Vector2.right * 6.0f * Time.deltaTime);
            yield return null;
        }

        // Ate� Topu Bir D��mana �arpt��� ��in Art�k �leriye Gitmiyor, Sola Do�ru Gidiyor Yoksa G�z Yan�lsamas� Olur.
        while (true)
        {
            gameObject.transform.Translate(Vector2.left * 5.7f * Time.deltaTime);
            yield return null;
        }
    }

    // Ate� Topu 2 Saniye �lerledikten Sonra Kendini Yok Eder.
    private IEnumerator DestroyYourselfAfter2Second()
    {
        yield return new WaitForSeconds(2.0f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D temas)
    {
        if (temas.CompareTag("Enemies"))
        {
            // Yenilen D��man Say�s�n� Bir Artt�r�r Ve Bir If'e Girerek Oyuncunun
            // Hangi G�revde Ve G�revin Ba�ar�s�z Olup Olmad���n� Kontrol Eder.
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

            // Patlama Animasyonunu Aktifle�tirme Ve De�i�ken Setlemesi.
            gameObject.transform.position = new Vector2(transform.position.x + 0.35f, transform.position.y + 0.25f);
            gameObject.GetComponent<Animator>().SetTrigger("didTheFireballHitAnyEnemie");
            didTheFireballHitAnyEnemie = true;

            // Ate� Topunun F�rlatma Sesini Kapat�p, Patlama Sesini A�ma.
            AudioSource soundManager = GameObject.Find("AudioManager/SoundManager").GetComponent<AudioSource>();
            soundManager.enabled = false;
            soundManager.enabled = true;

            // Ate� Topu, Ku�a �arpm��sa Ba�ka Bir Ses Efektinin �al�nmas�n� Sa�lar.
            if (temas.gameObject.name == "Eagle")
                AudioManager.admgTHIS.PlayOneShotASound("ExplosionSound1");
            else
                AudioManager.admgTHIS.PlayOneShotASound("ExplosionSound2");
            
            // Ate� Topunun �arpt��� D��man� Silme.
            Destroy(temas.gameObject);
        }
    }
}