using System.Collections;
using UnityEngine;


/// <summary>
/// 
/// Bu Script Oyundaki Kartalý Kontrol Eder.
/// 
/// >*>*> Deðiþkenlerin Açýklamalarý <*<*< \\\
///     -> [ eagleDieSprite ]
///     Kartal Öldüðü Zaman SpriteRenderer'ýn Sprite'ýný Bu Deðiþkenin Sprite'ý Olarak Set Edilir.
///     
///     -> [ isMovingBirdDown ]
///     Kartal Öldüðü Zaman, Cesedin Aþaðýya Doðru Ýnmesini Kontrol Etmek Ýçin Kullanýlýr.
/// 
/// </summary>
public class Eagle : MonoBehaviour
{
    [SerializeField] private Sprite eagleDieSprite;
    private bool isMovingBirdDown;


    // Hýz Ve Yön Ayarlarý
    private IEnumerator Start()
    {
        while (true)
        {
            if (isMovingBirdDown == false)
                transform.Translate(Vector2.left * 7.0f * Time.deltaTime);
            else
                transform.Translate(Vector2.down * 5.5f * Time.deltaTime);
            
            yield return null;
        }
    }

    // Oyuncu Kartalý Öldürdükten Sonra, Kartalýn Aþaðýya Düþüp Tekrar Yukarý Çýkabilmesini Saðlýyor.
    private IEnumerator SpawnBirdAgain()
    {
        yield return new WaitForSeconds(3.5f);

        isMovingBirdDown = false;
        gameObject.GetComponent<Animator>().enabled = true;
        gameObject.GetComponent<CapsuleCollider2D>().enabled = true;
        SetRandomPos();
    }

    // Kartal Tekrar Spawn Olurken +9...+14 Kordinatlarý Arasýnda Bir Yerde Random Spawn Olur.
    internal void SetRandomPos() => transform.position = new Vector2(Random.Range(9.0f, 13.0f), 0.6f);

    private void OnTriggerEnter2D(Collider2D temas)
    {
        // Oyuncu Kartalý Öldürünce Çalýþýr.
        if (temas.gameObject.name == "HitBox0")
        {
            isMovingBirdDown = true;
            gameObject.GetComponent<Animator>().enabled = false;
            gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            gameObject.GetComponent<SpriteRenderer>().sprite = eagleDieSprite;
            StartCoroutine(SpawnBirdAgain());

            ScoreManager.smTHIS.ScorePlus();
            AudioManager.admgTHIS.PlayOneShotASound("HitObjectSound");
        }
        // Oyuncu Kartala Çarptýðýnda Çalýþýr.
        else if (temas.gameObject.name == "HitBox1")
        {
            SetRandomPos();

            ScoreManager.smTHIS.ScoreMinus();
            Character.chrctrTHIS.MinusHealth();
            AudioManager.admgTHIS.PlayOneShotASound("BumpSound");
        }
        // Güç Ýksiri Ýçilmiþ Olduðu Halde Oyuncu Kartala Çarpýnca Çalýþýr.
        else if(temas.gameObject.name == "HitBox2")
        {
            SetRandomPos();

            ScoreManager.smTHIS.ScorePlus();
            AudioManager.admgTHIS.PlayOneShotASound("BumpSound");
        }
        // Kartalý Tekrar Spawn Eder.
        else if (temas.gameObject.name == "BorderLine0")
        {
            SetRandomPos();
        }
    }
}