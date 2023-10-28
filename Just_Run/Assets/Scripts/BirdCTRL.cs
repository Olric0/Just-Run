using UnityEngine;

public class BirdCTRL : MonoBehaviour
{
    private Animator reyiz;
    public static AudioSource dieSong;
    public GameObject character;
    public GameObject brokenWall;
    public GameObject solidWall;
    public GameObject panel;

    private bool hizCtrl = false;

    void Start()
    {
        reyiz = GetComponent<Animator>();

        dieSong = GetComponent<AudioSource>();
    }

    // H�z Ve Y�n Ayarlar�
    void Update()
    {
        if (hizCtrl == false)
            transform.Translate(Vector2.left * 6 * Time.deltaTime);

        if (hizCtrl == true)
        {
            transform.Translate(Vector2.left * 0 * Time.deltaTime);
            transform.Translate(Vector2.down * 6 * Time.deltaTime);
        }

    }
    // *_*

    
    
    void OnTriggerEnter2D(Collider2D temas)
    {
        float xDegeri = Random.Range(8, 13);

        // �ld���nde �al���r
        if (temas.gameObject.name == "HitBox0")
        {
            hizCtrl = true;
            reyiz.SetBool("isDie", true);
            Main.scor += 50;

            dieSong.Play();
        }
        // *_*

        // Karaktere �arpt���nda �al���r
        if (temas.gameObject.name == "HitBox1")
        {
            transform.position = new Vector2(xDegeri, 0.5f);
            KusSurusu.reyiz.SetBool("move", true);

            if (Main.scor >= 200)
                Main.scor -= 200;

            ForBW.bumpSound.Play();
            KusSurusu.herdOfBirdsSong.Play();
            ForHeart1.damage = true;

            Main.heart--;

            // Oyun Bitti�inde �al���r
            if (Main.heart == 0)
            {
                Destroy(character);
                Destroy(this);
                Destroy(solidWall);
                Destroy(brokenWall);
                Time.timeScale = 0;
                ForHeart0.damage = true;
                KusSurusu.herdOfBirdsSong.Stop();
                panel.SetActive(true);
            }
            // *_*
        }
        // *_*



        // Spawn Eder
        if (temas.gameObject.name == "SinirCizgisi0")
        {
            transform.position = new Vector2(xDegeri, 0.5f);
        }
        if (temas.gameObject.name == "SinirCizgisi1")
        {
            hizCtrl = false;
            transform.position = new Vector2(xDegeri, 0.5f);
            reyiz.SetBool("isDie", false);
        }
        // *_*
    }
}