using System.Collections;
using UnityEngine;


/// <summary>
/// 
/// Bu S�n�f G�� �ksirini Kontrol Eder. �ksirin Gidi� Y�n�, Oyuncuya �arpt�m� Gibi Kontroller Buradan Olur.
///     
/// </summary>
public class PowerPotion : MonoBehaviour
{
    private IEnumerator Start()
    {
        // Spawn Olmadan �nce, �ksir �b�r Duvarlar�n ��ine Giriyor Mu Girmiyor Mu Diye Kontrol Eder.
        while (
            (Vector2.Distance(transform.position, GameObject.Find("SolidWall").transform.position) >= -2.0f &&
             Vector2.Distance(transform.position, GameObject.Find("SolidWall").transform.position) <= 2.0f)
             ||
            (Vector2.Distance(transform.position, GameObject.Find("BrokenWall").transform.position) >= -2.0f &&
             Vector2.Distance(transform.position, GameObject.Find("BrokenWall").transform.position) <= 2.0f))
        {
            if (GameObject.Find("Character") == true)
            {
                transform.position = new Vector2(Random.Range(8.0f, 13.0f), transform.position.y);
            }
            yield return null;
        }

        // Spawn Olduktan Sonra �ksirin Oyuncuya Do�ru Gitmesi.
        while (true)
        {
            transform.Translate(Vector2.left * 5.7f * Time.deltaTime);
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D temas)
    {
        // Oyuncu �ksire �arparak �ksiri ��er Ve Gerekli ��lemler Ba�lat�l�r.
        if (temas.gameObject.name == "HitBox1")
        {
            // G�� �ksirini Pasif Etme.
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<CircleCollider2D>().enabled = false;

            // G�� �ksirini Ayarlama Ve ��me.
            PotionManager.pmTHIS.PotionSpecificSettings("PowerPotion");

            // Kapan��.
            Destroy(gameObject);
        }
        // E�er Oyuncu �ksire �arpamazsa �ksiri Ka��r�r Ve �ksir Kendini �mha Eder.
        else if (temas.gameObject.name == "BorderLine0")
        {
            // Bir Sonraki G�� �ksirinin Geli� Zaman�n� Hileye Durumuna G�re Belirleme.
            // Hile A��ksa G�� �ksiri Hemen Tekrardan Spawn Olacakt�r.
            if (PlayerPrefs.GetInt("isAlwaysPowerPotion") != 1)
                Character.chrctrTHIS.powerPotionSpawnControllerValue = Character.chrctrTHIS.score + 1000;
            else
                Character.chrctrTHIS.powerPotionSpawnControllerValue = Character.chrctrTHIS.score + 50;

            // �ksirin Gitti�ini Belirtme.
            PotionManager.isTheAnyPotionActive = false;

            // Kapan��.
            Destroy(gameObject);
        }
    }
}