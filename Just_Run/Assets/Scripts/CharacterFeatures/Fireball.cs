using System.Collections;
using UnityEngine;


/// <summary>
/// 
/// Bu Script Fýrlatýlan Ateþ Topunu Kontrol Eder. Animasyon Halinde Olan, UI'da Aþaðýda Olan Ateþ Topu
/// Deðil Karýþtýrma! Deðiþkenleri Adý Üstünde Olduðu Ýçin Anlayabilirsin.
/// 
/// </summary>
public class Fireball : MonoBehaviour
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

    // Random Pozisyon Deðiþtirmeyi SolidWall Ve BrokenWall Scriptlerinde Anlatmýþtým. While Döngüsünün Sebebini Fln
    // Aynýsý Burdada Var, Kafan Karýþmasýn. Daha Optimize Olsun Diye Duvarlarýn Pozisyonlarýný Bu Scriptten Set Ediyorum.
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
        // Ateþ Topu Bir Düþmanla Etkileþime Girdiðinde, Switch'e Girer.
        // Ardýndan Çarptýðý Düþmaný Tekrar Spawn Edip Oyuncuya Puan Verir.
        if (temas.CompareTag("Enemies"))
        {
            switch (temas.gameObject.name)
            {
                case "Eagle":
                    // Kartalý Tekrar Spawn Etme.
                    GameObject.Find("Eagle").transform.position = new Vector2(Random.Range(10.0f, 13.0f), 0.5f);
                    break;
                case "SolidWall":
                    // Saðlam Duvarý Tekrar Spawn Etme.
                    StartCoroutine(SetRandomPosForSolidWall());
                    break;
                case "BrokenWall":
                    // Kýrýk Duvarý Tekrar Spawn Etme.
                    StartCoroutine(SetRandomPosForBrokenWall());
                    break;
            }

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
            {
                AudioManager.admgTHIS.PlayOneShotASound("ExplosionSound1");
            }
            else
            {
                AudioManager.admgTHIS.PlayOneShotASound("ExplosionSound2");
            }

            // Oyuncuya Puan Verilir Ve Patlama Animasyonuda Bittikten Sonra Ateþ Topu Kendiliðinden Yok Olur.
            ScoreManager.smTHIS.ScorePlus();
        }
    }
}