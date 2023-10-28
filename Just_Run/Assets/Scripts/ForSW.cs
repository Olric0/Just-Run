using UnityEngine;

public class ForSW : MonoBehaviour
{
    public GameObject character;
    public GameObject bird;
    public GameObject brokenWall;
    public GameObject panel;

    void Update()
    {
        transform.Translate(Vector2.left * 6 * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D temas)
    {
        float xDegeri = Random.Range(20, 25);

        // Karaktere Çarptýðýnda Çalýþýr
        if (temas.gameObject.name == "HitBox1")
        {
            transform.position = new Vector2(xDegeri, -1.21f);
            KusSurusu.reyiz.SetBool("move", true);
            Main.scor -= 200;
            KusSurusu.herdOfBirdsSong.Play();
            ForHeart1.damage = true;
            ForBW.bumpSound.Play();

            Main.heart--;

            // Oyun Bittiðinde Çalýþýr
            if (Main.heart == 0)
            {
                Destroy(character);
                Destroy(bird);
                Destroy(brokenWall);
                Destroy(this);
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
    }
}