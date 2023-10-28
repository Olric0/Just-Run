using UnityEngine;
using TMPro;

public class Main : MonoBehaviour
{
    private Rigidbody2D rb;
    public static Animator reyiz0;
    private AudioSource jumpSong;
    public TextMeshProUGUI scorText;

    public GameObject walkingEffect;
    public GameObject deathPanel;
    public GameObject stop;

    public SpriteRenderer potionInfo;
    public SpriteRenderer potionEmpty;
    public SpriteRenderer potionI1;
    public SpriteRenderer potionI2;
    public SpriteRenderer potionI3;
    public SpriteRenderer potionI4;
    public SpriteRenderer potionI5;
    public GameObject potionSprites;

    private byte ySpeed = 6;
    private bool potVarMi;
    private bool yerdeMi;

    public static byte heart = 2;
    public static short scor;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        reyiz0 = GetComponent<Animator>();
        jumpSong = GetComponent<AudioSource>();
    }



    // Update
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
            reyiz0.SetBool("isAttack", true);
        else
            reyiz0.SetBool("isAttack", false);
        // *_*



        // Ýksir Spawn Et
        if (scor != 0 && scor % 200 == 0 && potVarMi == false)
        {
            PowerPOT.isActive = true;
            potVarMi = true;
        }
        //



        // Ölünce Çalýþýr
        if (heart == 0)
        {
            Time.timeScale = 0;
            ForHeart0.damage = true;
            HOBirds.herdOfBirdsSong.Stop();
            deathPanel.SetActive(true);
            stop.SetActive(false);
            Destroy(gameObject);
        }
        //
    }
    // *_*
    
    
    
    // Ýksir Spriteýný Ayarlama
    void Potion5()
    {
        potionInfo.sprite = potionI5.sprite;
        Time.timeScale = 1.5f;
    }
    void Potion4()
    {
        potionInfo.sprite = potionI4.sprite;
    }
    void Potion3()
    {
        potionInfo.sprite = potionI3.sprite;
    }
    void Potion2()
    {
        potionInfo.sprite = potionI2.sprite;
    }
    void Potion1()
    {
        potionInfo.sprite = potionI1.sprite;
    }
    void PotionEmpty()
    {
        potionInfo.sprite = potionEmpty.sprite;
        Destroy(potionSprites);
        PowerPOT.isActive = false;
        PowerPOT.done = true;
        Time.timeScale = 1;
    }
    // *_*



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