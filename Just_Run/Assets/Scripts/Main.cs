using UnityEngine;
using TMPro;

public class Main : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator reyiz0;
    private AudioSource jumpSong;
    public TextMeshProUGUI scorText;
    public GameObject walkingEffect;

    private byte ySpeed = 6;
    private bool yerdeMi;

    public static byte heart = 2;
    public static short scor;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        reyiz0 = GetComponent<Animator>();
        jumpSong = GetComponent<AudioSource>();

        walkingEffect.SetActive(true);
    }

    void Update()
    {
        // Skor Ayarlarý
        scorText.text = scor.ToString();
        // *_*



        // Zýplama Kontrolleri
        if (Input.GetKeyDown(KeyCode.W) && yerdeMi == true)
        {
            rb.AddForce(Vector2.up * ySpeed * 100);
            walkingEffect.SetActive(false);
            yerdeMi = false;

            jumpSong.Play();
        }
        if (Input.GetKeyDown(KeyCode.Space) && yerdeMi == true)
        {
            rb.AddForce(Vector2.up * ySpeed * 100);
            walkingEffect.SetActive(false);
            yerdeMi = false;

            jumpSong.Play();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && yerdeMi == true)
        {
            rb.AddForce(Vector2.up * ySpeed * 100);
            walkingEffect.SetActive(false);
            yerdeMi = false;

            jumpSong.Play();
        }
        // *_*



        // Saldýr
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return))
        {
            reyiz0.SetBool("isAttack", true);
        }
        else
        {
            reyiz0.SetBool("isAttack", false);
        }
        // *_*
    }





    // Zemin Kontrolü
    void OnCollisionEnter2D(Collision2D temas0)
    {
        if (temas0.gameObject.name == "Zemin0")
        {
            rb.gravityScale = 2;
            yerdeMi = true;
            walkingEffect.SetActive(true);
        }
    }
    // *_*

    
    
    // Süzülme
    void OnTriggerEnter2D(Collider2D temas1)
    {
        if (temas1.gameObject.name == "SinirCizgisi2")
            rb.gravityScale = 1.6f;
    }
    // *_*
}