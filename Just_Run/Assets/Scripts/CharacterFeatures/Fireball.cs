using System.Collections;
using UnityEngine;


/// <summary>
/// 
/// Bu Script F�rlat�lan Ate� Topunu Kontrol Eder. Animasyon Halinde Olan, UI'da A�a��da Olan Ate� Topu
/// De�il Kar��t�rma! De�i�kenleri Ad� �st�nde Oldu�u ��in Anlayabilirsin.
/// 
/// </summary>
public class Fireball : MonoBehaviour
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

    // Random Pozisyon De�i�tirmeyi SolidWall Ve BrokenWall Scriptlerinde Anlatm��t�m. While D�ng�s�n�n Sebebini Fln
    // Ayn�s� Burdada Var, Kafan Kar��mas�n. Daha Optimize Olsun Diye Duvarlar�n Pozisyonlar�n� Bu Scriptten Set Ediyorum.
    private IEnumerator SetRandomPosForSolidWall()
    {
        Transform solidWall = GameObject.Find("SolidWall").transform;
        Transform brokenWall = GameObject.Find("BrokenWall").transform;

        solidWall.position = new Vector2(Random.Range(15.0f, 20.0f), solidWall.position.y);
        while (Vector2.Distance(solidWall.position, brokenWall.position) >= -5.0f
                && Vector2.Distance(solidWall.position, brokenWall.position) <= 5.0f)
        {
            solidWall.position = new Vector2(Random.Range(15.0f, 20.0f), solidWall.position.y);
            yield return null;
        }
    }
    private IEnumerator SetRandomPosForBrokenWall()
    {
        Transform brokenWall = GameObject.Find("BrokenWall").transform;
        Transform solidWall = GameObject.Find("SolidWall").transform;

        brokenWall.position = new Vector2(Random.Range(10.0f, 15.0f), brokenWall.position.y);
        while (Vector2.Distance(brokenWall.position, solidWall.position) >= -5.0f
                && Vector2.Distance(brokenWall.position, solidWall.position) <= 5.0f)
        {
            brokenWall.position = new Vector2(Random.Range(14.0f, 15.0f), brokenWall.position.y);
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D temas)
    {
        // Ate� Topu Bir D��manla Etkile�ime Girdi�inde, Switch'e Girer.
        // Ard�ndan �arpt��� D��man� Tekrar Spawn Edip Oyuncuya Puan Verir.
        if (temas.CompareTag("Enemies"))
        {
            switch (temas.gameObject.name)
            {
                case "Eagle":
                    // Kartal� Tekrar Spawn Etme.
                    GameObject.Find("Eagle").transform.position = new Vector2(Random.Range(10.0f, 13.0f), 0.5f);
                    break;
                case "SolidWall":
                    // Sa�lam Duvar� Tekrar Spawn Etme.
                    StartCoroutine(SetRandomPosForSolidWall());
                    break;
                case "BrokenWall":
                    // K�r�k Duvar� Tekrar Spawn Etme.
                    StartCoroutine(SetRandomPosForBrokenWall());
                    break;
            }

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
            {
                AudioManager.admgTHIS.PlayOneShotASound("ExplosionSound1");
            }
            else
            {
                AudioManager.admgTHIS.PlayOneShotASound("ExplosionSound2");
            }

            // Oyuncuya Puan Verilir Ve Patlama Animasyonuda Bittikten Sonra Ate� Topu Kendili�inden Yok Olur.
            ScoreManager.smTHIS.ScorePlus();
        }
    }
}