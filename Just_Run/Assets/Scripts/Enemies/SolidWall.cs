using System.Collections;
using UnityEngine;


/// <summary>
/// 
/// Bu Script Oyundaki Sa�lam Duvar� Kontrol Eder.
/// 
/// </summary>
public class SolidWall : MonoBehaviour
{
    [SerializeField] private Transform brokenWallTransform;
    

    private IEnumerator Start()
    {
        while (true)
        {
            // D��man�n Oyuncuya Do�ru Hareket Etmesi.
            transform.Translate(Vector2.left * 5.7f * Time.deltaTime);
            yield return null;
        }
    }

    // Sa�lam Duvar Tekrar Spawn Olurken +13...+18 Kordinatlar� Aras�nda Bir Yerde Random Spawn Olur.
    private IEnumerator SetRandomPos()
    {
        transform.position = new Vector2(Random.Range(13.0f, 18.0f), transform.position.y);

        // Bu K�s�m E�er Sa�lam Duvar, K�r�k Duvara 5 Birim Kadar Yak�nsa Tekrar Spawn Etmeye Yarar.
        // While D�ng�s�n�n ��inde ��nk� Yine Spawn Oldu�unda Ayn� �ey Olabilir, Ne Zamanki �ki Duvar Aras�nda
        // Yeterli Mesafe Var O Zaman D�ng�den ��kar. E�er Bunu Silersen Duvarlar �� ��e Do�ar Ve Oyun Bozulur.
        while (Vector2.Distance(transform.position, brokenWallTransform.position) >= -5.0f
                && Vector2.Distance(transform.position, brokenWallTransform.position) <= 5.0f)
        {
            transform.position = new Vector2(Random.Range(13.0f, 18.0f), transform.position.y);
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D temas)
    {
        // Oyuncu Duvara �arpt���nda �al���r.
        if (temas.gameObject.name == "HitBox1")
        {
            StartCoroutine(SetRandomPos());

            ScoreManager.smTHIS.ScoreMinus();
            Character.chrctrTHIS.MinusHealth();
            AudioManager.admgTHIS.PlayOneShotASound("BumpSound");
        }
        // G�� �ksiri ��ilmi� Oldu�u Halde Oyuncu Sa�lam Duvara �arp�nca �al���r.
        else if (temas.gameObject.name == "HitBox2")
        {
            StartCoroutine(SetRandomPos());

            ScoreManager.smTHIS.ScorePlus();
            AudioManager.admgTHIS.PlayOneShotASound("BumpSound");
        }
        // Sa�lam Duvar� Tekrar Spawn Eder.
        else if (temas.gameObject.name == "BorderLine0")
        {
            StartCoroutine(SetRandomPos());
        }
    }
}