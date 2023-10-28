using System.Collections;
using UnityEngine;


/// <summary>
/// 
/// Bu Script Oyundaki Kýrýk Duvarý Kontrol Eder.
/// 
/// </summary>
public class BrokenWall : MonoBehaviour
{
    [SerializeField] private Transform solidWallTransform;



    private IEnumerator Start()
    {
        while (true)
        {
            // Düþmanýn Oyuncuya Doðru Hareket Etmesi.
            transform.Translate(Vector2.left * 5.7f * Time.deltaTime);

            // Kýrýk Duvar, Saðlam Duvar Ýle Çakýþacak Kadar Yakýn Mý Diye Kontrol Etme. Eðer Yakýnsa Pozisyon Yenilenir.
            while (Vector2.Distance(transform.position, solidWallTransform.position) >= -5.0f
                && Vector2.Distance(transform.position, solidWallTransform.position) <= 5.0f)
            {
                if (GameObject.Find("Character") == true)
                {
                    transform.position = new Vector2(Random.Range(8.0f, 13.0f), transform.position.y);
                }
                yield return null;
            }

            yield return null;
        }
    }

    // Kýrýk Duvar Tekrar Spawn Olurken +8...+13 Kordinatlarý Arasýnda Bir Yerde Random Spawn Olur
    private IEnumerator SetRandomPos()
    {
        // Pozisyonun Yenilenmesi.
        transform.position = new Vector2(Random.Range(8.0f, 13.0f), transform.position.y);

        // Bu Kýsým Eðer Kýrýk Duvar, Saðlam Duvara 5 Birim Kadar Yakýnsa Tekrar Spawn Etmeye Yarar.
        // While Döngüsünün Ýçinde Çünkü Yine Spawn Olduðunda Ayný Þey Olabilir, Ne Zamanki Ýki Duvar Arasýnda
        // Yeterli Mesafe Var O Zaman Döngüden Çýkar. Eðer Bunu Silersen Duvarlar Ýç Ýçe Doðar Ve Oyun Bozulur.
        while (Vector2.Distance(transform.position, solidWallTransform.position) >= -5.0f
                && Vector2.Distance(transform.position, solidWallTransform.position) <= 5.0f)
        {
            transform.position = new Vector2(Random.Range(8.0f, 13.0f), transform.position.y);
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D temas)
    {
        // Oyuncu Duvarý Kýrýnca Çalýþýr.
        if (temas.gameObject.name == "HitBox0")
        {
            StartCoroutine(SetRandomPos());

            ScoreManager.smTHIS.ScorePlus();
            AudioManager.admgTHIS.PlayOneShotASound("HitObjectSound");
        }
        // Oyuncu Duavara Çarpýnca Çalýþýr.
        else if (temas.gameObject.name == "HitBox1")
        {
            StartCoroutine(SetRandomPos());

            Character.chrctrTHIS.MinusHealth();
            ScoreManager.smTHIS.ScoreMinus();
            AudioManager.admgTHIS.PlayOneShotASound("BumpSound");
        }
        // Güç Ýksiri Ýçilmiþ Olduðu Halde Oyuncu Kýrýk Duvara Çarpýnca Çalýþýr.
        else if (temas.gameObject.name == "HitBox2")
        {
            StartCoroutine(SetRandomPos());

            ScoreManager.smTHIS.ScorePlus();
            AudioManager.admgTHIS.PlayOneShotASound("BumpSound");
        }
        // Kýrýk Duvarý Tekrar Spawn Eder.
        else if (temas.gameObject.name == "BorderLine0")
        {
            StartCoroutine(SetRandomPos());
        }
    }
}