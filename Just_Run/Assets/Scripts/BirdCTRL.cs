using UnityEngine;

public class BirdCTRL : MonoBehaviour
{
    private Animator reyiz;
    private PolygonCollider2D coll;
    public static AudioSource dieSong;

    private bool hizCtrl = false;

    void Start()
    {
        reyiz = GetComponent<Animator>();
        coll = GetComponent<PolygonCollider2D>();
        dieSong = GetComponent<AudioSource>();
    }

    // Hýz Ve Yön Ayarlarý
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

        // Öldüðünde Çalýþýr
        if (temas.gameObject.name == "HitBox0")
        {
            coll.enabled = false;
            hizCtrl = true;
            reyiz.SetBool("isDie", true);
            ForSP0.durum = true;
            Main.scor += 50;

            dieSong.Play();
        }
        // *_*

        // Karaktere Çarptýðýnda Çalýþýr
        if (temas.gameObject.name == "HitBox1")
        {
            transform.position = new Vector2(xDegeri, 0.5f);
            HOBirds.reyiz.SetBool("move", true);
            HOBirds.herdOfBirdsSong.Play();
            ForBW.bumpSound.Play();
            ForHeart1.damage = true;

            // Skor Azaltma
            if (Main.scor >= 200)
                Main.scor -= 200;
            else if (Main.scor <= 199)
                Main.scor -= Main.scor;
            // *_*

            Main.heart--;
        }
        // *_*



        // Güç Ýksiri Ýçilmiþ Olduðunda Çalýþýr
        if (temas.gameObject.name == "HitBox2")
        {
            Main.scor += 50;
            ForSP0.durum = true;
            ForBW.bumpSound.Play();
            transform.position = new Vector2(xDegeri, 0.5f);
        }
        // *_*



        // Collider Ayarlamasý
        if (temas.gameObject.name == "SinirCizgisi3")
            coll.enabled = true;
        // *_*



        // Spawn Eder
        if (temas.gameObject.name == "SinirCizgisi0")
            transform.position = new Vector2(xDegeri, 0.5f);
        if (temas.gameObject.name == "SinirCizgisi1")
        {
            hizCtrl = false;
            transform.position = new Vector2(xDegeri, 0.5f);
            reyiz.SetBool("isDie", false);
        }
        // *_*
    }
}