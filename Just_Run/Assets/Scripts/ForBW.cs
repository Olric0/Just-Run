using UnityEngine;

public class ForBW : MonoBehaviour
{
    public static AudioSource bumpSound;

    void Start()
    {
        bumpSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        transform.Translate(Vector2.left * 6 * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D temas)
    {
        float xDegeri = Random.Range(10, 15);

        // Kýrýldýðýnda Çalýþýr
        if (temas.gameObject.name == "HitBox0")
        {
            transform.position = new Vector2(xDegeri, -1.21f);
            ForSP0.durum = true;
            Main.scor += 50;

            BirdCTRL.dieSong.Play();
        }
        // *_*



        // Karaktere Çarpýnca Çalýþýr
        if (temas.gameObject.name == "HitBox1")
        {
            transform.position = new Vector2(xDegeri, -1.21f);
            HOBirds.reyiz.SetBool("move", true);
            HOBirds.herdOfBirdsSong.Play();
            ForHeart1.damage = true;
            bumpSound.Play();


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
            bumpSound.Play();
            transform.position = new Vector2(xDegeri, -1.21f);
        }
        // *_*



        // Spawn Eder
        if (temas.gameObject.name == "SinirCizgisi0")
            transform.position = new Vector2(xDegeri, -1.21f);
        // *_*



        // Saðlam Duvarla Temasýný Engeller
        if (temas.gameObject.name == "SolidWall")
            transform.position = new Vector2(xDegeri, -1.21f);
        // *_*
    }
}