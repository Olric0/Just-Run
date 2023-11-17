using System.Collections;
using UnityEngine;


/// <summary>
/// 
/// Bu Sýnýf Körlük Ýksirini Kontrol Eder. Ýksirin Gidiþ Yönü, Oyuncuya Çarptýmý Gibi Kontroller Buradan Olur.
///     
/// </summary>
public class BlindnessPotion : MonoBehaviour
{
    private IEnumerator Start()
    {
        // Spawn Olmadan Önce, Ýksir Öbür Duvarlarýn Ýçine Giriyor Mu Girmiyor Mu Diye Kontrol Eder
        while (
            (Vector2.Distance(transform.position, GameObject.Find("SolidWall").transform.position) >= -2.0f &&
             Vector2.Distance(transform.position, GameObject.Find("SolidWall").transform.position) <= 2.0f)
             ||
            (Vector2.Distance(transform.position, GameObject.Find("BrokenWall").transform.position) >= -2.0f &&
             Vector2.Distance(transform.position, GameObject.Find("BrokenWall").transform.position) <= 2.0f))
        {
            if (GameObject.Find("Character") == true)
                transform.position = new Vector2(Random.Range(8.0f, 13.0f), transform.position.y);
            yield return null;
        }

        // Spawn Olduktan Sonra Ýksirin Oyuncuya Doðru Gitmesi.
        while (true)
        {
            transform.Translate(Vector2.left * 5.7f * Time.deltaTime);
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D temas)
    {
        // Oyuncu Ýksire Çarparak Ýksiri Ýçer Ve Gerekli Ýþlemler Baþlatýlýr.
        if (temas.gameObject.name == "HitBox1")
        {
            // Kör Etme Ýksirini Pasif Etme.
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<CircleCollider2D>().enabled = false;

            // Kör Etme Ýksirini Ayarlama Ve Ýçme.
            PotionManager.pmTHIS.PotionSpecificSettings("BlindnessPotion");

            // Kapanýþ.
            Destroy(gameObject);
        }
        // Eðer Oyuncu Ýksire Çarpamazsa Ýksiri Kaçýrýr Ve Ýksir Kendini Ýmha Eder.
        else if (temas.gameObject.name == "BorderLine0")
        {
            // Körlük Ýksirinin Tekrar Gelmesini Engelleme.
            Character.chrctrTHIS.blindnessPotionSpawnControllerValue = false;

            // Ýksirin Gittiðini Belirtme.
            PotionManager.isTheAnyPotionActive = false;

            // Kapanýþ.
            Destroy(gameObject);
        }
    }
}