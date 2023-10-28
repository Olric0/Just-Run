using System.Collections;
using UnityEngine;


/// <summary>
/// 
/// Bu Script Oyundaki K�r�k Duvar� Kontrol Eder.
/// 
/// </summary>
public class BrokenWall : MonoBehaviour
{
    [SerializeField] private Transform solidWallTransform;



    private IEnumerator Start()
    {
        while (true)
        {
            // D��man�n Oyuncuya Do�ru Hareket Etmesi.
            transform.Translate(Vector2.left * 5.7f * Time.deltaTime);

            // K�r�k Duvar, Sa�lam Duvar �le �ak��acak Kadar Yak�n M� Diye Kontrol Etme. E�er Yak�nsa Pozisyon Yenilenir.
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

    // K�r�k Duvar Tekrar Spawn Olurken +8...+13 Kordinatlar� Aras�nda Bir Yerde Random Spawn Olur
    private IEnumerator SetRandomPos()
    {
        // Pozisyonun Yenilenmesi.
        transform.position = new Vector2(Random.Range(8.0f, 13.0f), transform.position.y);

        // Bu K�s�m E�er K�r�k Duvar, Sa�lam Duvara 5 Birim Kadar Yak�nsa Tekrar Spawn Etmeye Yarar.
        // While D�ng�s�n�n ��inde ��nk� Yine Spawn Oldu�unda Ayn� �ey Olabilir, Ne Zamanki �ki Duvar Aras�nda
        // Yeterli Mesafe Var O Zaman D�ng�den ��kar. E�er Bunu Silersen Duvarlar �� ��e Do�ar Ve Oyun Bozulur.
        while (Vector2.Distance(transform.position, solidWallTransform.position) >= -5.0f
                && Vector2.Distance(transform.position, solidWallTransform.position) <= 5.0f)
        {
            transform.position = new Vector2(Random.Range(8.0f, 13.0f), transform.position.y);
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D temas)
    {
        // Oyuncu Duvar� K�r�nca �al���r.
        if (temas.gameObject.name == "HitBox0")
        {
            StartCoroutine(SetRandomPos());

            ScoreManager.smTHIS.ScorePlus();
            AudioManager.admgTHIS.PlayOneShotASound("HitObjectSound");
        }
        // Oyuncu Duavara �arp�nca �al���r.
        else if (temas.gameObject.name == "HitBox1")
        {
            StartCoroutine(SetRandomPos());

            Character.chrctrTHIS.MinusHealth();
            ScoreManager.smTHIS.ScoreMinus();
            AudioManager.admgTHIS.PlayOneShotASound("BumpSound");
        }
        // G�� �ksiri ��ilmi� Oldu�u Halde Oyuncu K�r�k Duvara �arp�nca �al���r.
        else if (temas.gameObject.name == "HitBox2")
        {
            StartCoroutine(SetRandomPos());

            ScoreManager.smTHIS.ScorePlus();
            AudioManager.admgTHIS.PlayOneShotASound("BumpSound");
        }
        // K�r�k Duvar� Tekrar Spawn Eder.
        else if (temas.gameObject.name == "BorderLine0")
        {
            StartCoroutine(SetRandomPos());
        }
    }
}