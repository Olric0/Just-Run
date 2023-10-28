using System.Collections;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 
/// Bu Script Öðretici Moddaki Karakterdir Normal Karakter Deðildir! Ve Bu Yüzden Bazý Kýsýtlamalarý Vardýr.
/// 
///>*>*> Deðiþkenlerin Açýklamalarý <*<*< \\\
///     -> [ isPlayerCanUseFireball ]
///     Bu Deðiþken, TutorialManager.cs'nin StartTask Metodundan Tetikleniyor. Eðer Oyuncu 4. Taska Gelmiþse Artýk Ateþ
///     Topu Aktif Olur, Ve StartTask'daki Bir If Ýle Eðer Oyuncu 4. Taska Gelmiþ Ýse Bu Deðiþken True Olarak Set Ediliyor.
///     Ve Artýk Ateþ Topu Aktif Oluyor, Oyuncu Gerekli Koþullar Doðrultusunda Artýk Ateþ Topunu Kullanabilir.
///     
///     -> [ canFireballBeUsed ]
///     Bu Deðiþken Sadece Oyuncu Koþuyor Veya Zýplýyorsa True Olur, Ve False Olduðu Süre Boyunca Oyuncu Ateþ Topu Atamaz.
///     
///     -> [ isFireballActive ]
///     Bunu En Baþdaki Deðiþken Ýle Karýþtýrma. Bu Da Aktifliði Belirliyor Ama Bu Ateþ Topunun Cooldown'ý Bitince True Oluyor
///     Ve Ateþ Topu Aktifdir Anlamýna Geliyor. En Baþdaki Komple Ateþ Topu Aktif Mi Deðil Mi Diye Bakýyor. Ama Bu Deðiþken
///     Ateþ Topu Aktif Fakat Oyuncunun Kullanabilmesi Ýçin Uygun Mu Deðil Mi Diye Bakýyor.
///     
/// </summary>
public class CharacterForTutorial : MonoBehaviour
{
    [Header("Basic Variables")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer walkingEffectSpriteRNDR;
    [SerializeField] internal Animator characterANMTR;

    [Header("Fire Ball Variables")]
    [SerializeField] private Animator fireballIconAnmtr;
    internal bool isPlayerCanUseFireball;
    internal bool canFireballBeUsed;
    private bool isFireballActive;

    public static CharacterForTutorial chrctrTHIS;
    private bool isOnGround;



    private void Start() => chrctrTHIS = this;
    private void Update()
    {
        // Zýplama Kontrolleri.
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && isOnGround == true)
        {
            rb.AddForce(Vector2.up * 600.0f);

            isOnGround = false;
            walkingEffectSpriteRNDR.enabled = false;
            characterANMTR.SetBool("isJumping", true);

            AudioManager.admgTHIS.PlayOneShotASound("JumpSound");
        }

        // Kýlýçla Saldýrma.
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            characterANMTR.SetBool("isAttacking", true);
        }
        else
        {
            characterANMTR.SetBool("isAttacking", false);
        }

        // Kýlýçla Havada Saldýrma.
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && isOnGround == false)
        {
            characterANMTR.SetBool("isJumpAttacking", true);
        }
        else if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
        {
            characterANMTR.SetBool("isJumpAttacking", false);
        }

        // Ateþ Topu Atma.
        if ((Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.RightArrow)) 
            && isPlayerCanUseFireball == true && GameObject.Find("QuitPanel") == false)
        {
            if (isFireballActive == true && canFireballBeUsed == true)
            {
                // Tekrar Ateþ Topu Atýlamasýn Diye Deðiþken False Edilir, Fýrlatma Animasyonu Tetiklenir Ve
                // Bir Sonraki Ateþ Topu Ýçin Bekleme Metodu Baþlar. Ateþ Topu Animasyonun Eventinde Doðuyor.
                isFireballActive = false;
                characterANMTR.SetTrigger("isThrowingTheFireballForTutorial");
                StartCoroutine(WaitToUsingFireball());
            }
            else if (isFireballActive == false || canFireballBeUsed == false)
            {
                fireballIconAnmtr.SetTrigger("fireballDisable");
                AudioManager.admgTHIS.PlayOneShotASound("ErrorSound");
            }
        }
    }

    // Ateþ Topunu Tekrar Kullanmak Ýçin Bekleme. (Ýlk Sefer)
    internal IEnumerator WaitToUsingFireballForFirstTime()
    {
        // Deðiþken Atamalarý.
        Image fireballIcon = GameObject.Find("Canvas/FireballIcon").transform.GetChild(1).GetComponent<Image>();
        fireballIcon.fillAmount = 0.0f;

        // Ateþ Topunun Tekrar Aktif Olmasý Ýçin 1,5 Saniye Beklenir. Bu Sadece Bir Seferliðine Olur!
        for (short i = 0; i < 100; i++)
        {
            fireballIcon.fillAmount += 0.01f;
            yield return new WaitForSeconds(0.00000001f);
        }

        // Fill Amount Deðeri Zaten Döngüden Sonra 1.0f Oluyor Ama Ben Yinede Önlem Amaçlý Burda Tekrar 1.0f Yaptým.
        fireballIcon.fillAmount = 1.0f;

        // Animasyon Tetiklenir, Deðiþken Set Edilir Ve Artýk Ateþ Topu Kullanýlabilir.
        fireballIconAnmtr.SetTrigger("fireballEnable");
        isFireballActive = true;
    }

    // Ateþ Topunu Tekrar Kullanmak Ýçin Bekleme.
    internal IEnumerator WaitToUsingFireball()
    {
        // Deðiþken Atamalarý.
        Image fireballIcon = GameObject.Find("Canvas/FireballIcon").transform.GetChild(1).GetComponent<Image>();
        fireballIcon.fillAmount = 0.0f;

        // Ateþ Topunun Tekrar Aktif Olmasý Ýçin 10 Saniye Beklenir.
        for (short i = 0; i < 1000; i++)
        {
            fireballIcon.fillAmount += 0.001f;
            yield return new WaitForSeconds(0.01f);
        }

        // Fill Amount Deðeri Zaten Döngüden Sonra 1.0f Oluyor Ama Ben Yinede Önlem Amaçlý Burda Tekrar 1.0f Yaptým.
        fireballIcon.fillAmount = 1.0f;

        // Animasyon Tetiklenir, Deðiþken Set Edilir Ve Artýk Ateþ Topu Kullanýlabilir.
        fireballIconAnmtr.SetTrigger("fireballEnable");
        isFireballActive = true;
    }


    // Zemin Kontrolü.
    private void OnCollisionEnter2D(Collision2D temas0)
    {
        if (temas0.gameObject.name == "GroundCollider")
        {
            rb.gravityScale = 2.0f;
            isOnGround = true;
            walkingEffectSpriteRNDR.enabled = true;
            characterANMTR.SetBool("isJumping", false);
        }
    }

    // Havada Süzülmeyi Ayarlýyor. Bunu Neden Rigidbody'de Ki LinearDrag Ýle Yapmadýn Dicek Olursak, O Süzülmeyi
    // Biraz Garip Yapýyor. Ben Ýstiyorum Ki Karakter Yukarý Hýzlý Çýksýn, Aþaðýya Hýzlý Ýnsan Ama Havada Olduðu
    // Süre Boyunca Süzülerek Yavaþlasýn. Bunu Linear Ýle Tam Anlamýyla Yapamayacaðým Ýçin Kendim Yaptým.
    private void OnTriggerEnter2D(Collider2D temas1)
    {
        if (temas1.gameObject.name == "BorderLine1")
        {
            rb.gravityScale = 1.5f;
        }
    }
}