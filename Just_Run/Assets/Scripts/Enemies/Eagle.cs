using System.Collections;
using UnityEngine;


/// <summary>
/// 
/// Bu Script Oyundaki Kartal� Kontrol Eder.
/// 
/// >*>*> De�i�kenlerin A��klamalar� <*<*< \\\
///     -> [ eagleDieSprite ]
///     Kartal �ld��� Zaman SpriteRenderer'�n Sprite'�n� Bu De�i�kenin Sprite'� Olarak Set Edilir.
///     
///     -> [ isMovingBirdDown ]
///     Kartal �ld��� Zaman, Cesedin A�a��ya Do�ru �nmesini Kontrol Etmek ��in Kullan�l�r.
/// 
/// </summary>
public class Eagle : MonoBehaviour
{
    [SerializeField] private Sprite eagleDieSprite;
    private bool isMovingBirdDown;


    // H�z Ve Y�n Ayarlar�
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

    // Oyuncu Kartal� �ld�rd�kten Sonra, Kartal�n A�a��ya D���p Tekrar Yukar� ��kabilmesini Sa�l�yor.
    private IEnumerator SpawnBirdAgain()
    {
        yield return new WaitForSeconds(3.5f);

        isMovingBirdDown = false;
        gameObject.GetComponent<Animator>().enabled = true;
        gameObject.GetComponent<CapsuleCollider2D>().enabled = true;
        SetRandomPos();
    }

    // Kartal Tekrar Spawn Olurken +9...+14 Kordinatlar� Aras�nda Bir Yerde Random Spawn Olur.
    internal void SetRandomPos() => transform.position = new Vector2(Random.Range(9.0f, 13.0f), 0.6f);

    private void OnTriggerEnter2D(Collider2D temas)
    {
        // Oyuncu Kartal� �ld�r�nce �al���r.
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
        // Oyuncu Kartala �arpt���nda �al���r.
        else if (temas.gameObject.name == "HitBox1")
        {
            SetRandomPos();

            ScoreManager.smTHIS.ScoreMinus();
            Character.chrctrTHIS.MinusHealth();
            AudioManager.admgTHIS.PlayOneShotASound("BumpSound");
        }
        // G�� �ksiri ��ilmi� Oldu�u Halde Oyuncu Kartala �arp�nca �al���r.
        else if(temas.gameObject.name == "HitBox2")
        {
            SetRandomPos();

            ScoreManager.smTHIS.ScorePlus();
            AudioManager.admgTHIS.PlayOneShotASound("BumpSound");
        }
        // Kartal� Tekrar Spawn Eder.
        else if (temas.gameObject.name == "BorderLine0")
        {
            SetRandomPos();
        }
    }
}