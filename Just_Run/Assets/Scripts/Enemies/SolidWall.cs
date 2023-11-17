using System.Collections;
using UnityEngine;


/// <summary>
/// 
/// Bu Script Oyundaki Saðlam Duvarý Kontrol Eder.
/// 
/// </summary>
public class SolidWall : MonoBehaviour
{
    [SerializeField] private Transform brokenWallTransform;
    

    private IEnumerator Start()
    {
        while (true)
        {
            // Düþmanýn Oyuncuya Doðru Hareket Etmesi.
            transform.Translate(Vector2.left * 5.7f * Time.deltaTime);
            yield return null;
        }
    }

    // Saðlam Duvar Tekrar Spawn Olurken +13...+18 Kordinatlarý Arasýnda Bir Yerde Random Spawn Olur.
    private IEnumerator SetRandomPos()
    {
        transform.position = new Vector2(Random.Range(13.0f, 18.0f), transform.position.y);

        // Bu Kýsým Eðer Saðlam Duvar, Kýrýk Duvara 5 Birim Kadar Yakýnsa Tekrar Spawn Etmeye Yarar.
        // While Döngüsünün Ýçinde Çünkü Yine Spawn Olduðunda Ayný Þey Olabilir, Ne Zamanki Ýki Duvar Arasýnda
        // Yeterli Mesafe Var O Zaman Döngüden Çýkar. Eðer Bunu Silersen Duvarlar Ýç Ýçe Doðar Ve Oyun Bozulur.
        while (Vector2.Distance(transform.position, brokenWallTransform.position) >= -5.0f
                && Vector2.Distance(transform.position, brokenWallTransform.position) <= 5.0f)
        {
            transform.position = new Vector2(Random.Range(13.0f, 18.0f), transform.position.y);
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D temas)
    {
        // Oyuncu Duvara Çarptýðýnda Çalýþýr.
        if (temas.gameObject.name == "HitBox1")
        {
            StartCoroutine(SetRandomPos());

            ScoreManager.smTHIS.ScoreMinus();
            Character.chrctrTHIS.MinusHealth();
            AudioManager.admgTHIS.PlayOneShotASound("BumpSound");
        }
        // Güç Ýksiri Ýçilmiþ Olduðu Halde Oyuncu Saðlam Duvara Çarpýnca Çalýþýr.
        else if (temas.gameObject.name == "HitBox2")
        {
            StartCoroutine(SetRandomPos());

            ScoreManager.smTHIS.ScorePlus();
            AudioManager.admgTHIS.PlayOneShotASound("BumpSound");
        }
        // Saðlam Duvarý Tekrar Spawn Eder.
        else if (temas.gameObject.name == "BorderLine0")
        {
            StartCoroutine(SetRandomPos());
        }
    }
}