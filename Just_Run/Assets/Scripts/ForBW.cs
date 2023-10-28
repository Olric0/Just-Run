using UnityEngine;

public class ForBW : MonoBehaviour
{
    public static AudioSource bumpSound;
    public GameObject character;
    public GameObject bird;
    public GameObject solidWall;
    public GameObject panel;

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
            Main.scor += 50;

            BirdCTRL.dieSong.Play();
        }
        // *_*

        // Karaktere Çarpýnca Çalýþýr
        if (temas.gameObject.name == "HitBox1")
        {
            transform.position = new Vector2(xDegeri, -1.21f);
            KusSurusu.reyiz.SetBool("move", true);
            Main.scor -= 200;
            KusSurusu.herdOfBirdsSong.Play();
            ForHeart1.damage = true;
            bumpSound.Play();

            Main.heart--;

            // Oyun Bittiðinde Çalýþýr
            if (Main.heart == 0)
            {
                Destroy(character);
                Destroy(bird);
                Destroy(this);
                Destroy(solidWall);
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
            transform.position = new Vector2(xDegeri, -1.21f);
        }
        // *_*

        // Saðlam Duvarla Temasýný Engeller
        if (temas.gameObject.name == "SolidWall")
        {
            transform.position = new Vector2(xDegeri, -1.21f);
        }
        // *_*
    }
}