using System.Collections;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 
/// Bu Script ��retici Moddaki Karakterdir Normal Karakter De�ildir! Ve Bu Y�zden Baz� K�s�tlamalar� Vard�r.
/// 
///>*>*> De�i�kenlerin A��klamalar� <*<*< \\\
///     -> [ isPlayerCanUseFireball ]
///     Bu De�i�ken, TutorialManager.cs'nin StartTask Metodundan Tetikleniyor. E�er Oyuncu 4. Taska Gelmi�se Art�k Ate�
///     Topu Aktif Olur, Ve StartTask'daki Bir If �le E�er Oyuncu 4. Taska Gelmi� �se Bu De�i�ken True Olarak Set Ediliyor.
///     Ve Art�k Ate� Topu Aktif Oluyor, Oyuncu Gerekli Ko�ullar Do�rultusunda Art�k Ate� Topunu Kullanabilir.
///     
///     -> [ canFireballBeUsed ]
///     Bu De�i�ken Sadece Oyuncu Ko�uyor Veya Z�pl�yorsa True Olur, Ve False Oldu�u S�re Boyunca Oyuncu Ate� Topu Atamaz.
///     
///     -> [ isFireballActive ]
///     Bunu En Ba�daki De�i�ken �le Kar��t�rma. Bu Da Aktifli�i Belirliyor Ama Bu Ate� Topunun Cooldown'� Bitince True Oluyor
///     Ve Ate� Topu Aktifdir Anlam�na Geliyor. En Ba�daki Komple Ate� Topu Aktif Mi De�il Mi Diye Bak�yor. Ama Bu De�i�ken
///     Ate� Topu Aktif Fakat Oyuncunun Kullanabilmesi ��in Uygun Mu De�il Mi Diye Bak�yor.
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
        // Z�plama Kontrolleri.
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && isOnGround == true)
        {
            rb.AddForce(Vector2.up * 600.0f);

            isOnGround = false;
            walkingEffectSpriteRNDR.enabled = false;
            characterANMTR.SetBool("isJumping", true);

            AudioManager.admgTHIS.PlayOneShotASound("JumpSound");
        }

        // K�l��la Sald�rma.
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            characterANMTR.SetBool("isAttacking", true);
        }
        else
        {
            characterANMTR.SetBool("isAttacking", false);
        }

        // K�l��la Havada Sald�rma.
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && isOnGround == false)
        {
            characterANMTR.SetBool("isJumpAttacking", true);
        }
        else if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
        {
            characterANMTR.SetBool("isJumpAttacking", false);
        }

        // Ate� Topu Atma.
        if ((Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.RightArrow)) 
            && isPlayerCanUseFireball == true && GameObject.Find("QuitPanel") == false)
        {
            if (isFireballActive == true && canFireballBeUsed == true)
            {
                // Tekrar Ate� Topu At�lamas�n Diye De�i�ken False Edilir, F�rlatma Animasyonu Tetiklenir Ve
                // Bir Sonraki Ate� Topu ��in Bekleme Metodu Ba�lar. Ate� Topu Animasyonun Eventinde Do�uyor.
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

    // Ate� Topunu Tekrar Kullanmak ��in Bekleme. (�lk Sefer)
    internal IEnumerator WaitToUsingFireballForFirstTime()
    {
        // De�i�ken Atamalar�.
        Image fireballIcon = GameObject.Find("Canvas/FireballIcon").transform.GetChild(1).GetComponent<Image>();
        fireballIcon.fillAmount = 0.0f;

        // Ate� Topunun Tekrar Aktif Olmas� ��in 1,5 Saniye Beklenir. Bu Sadece Bir Seferli�ine Olur!
        for (short i = 0; i < 100; i++)
        {
            fireballIcon.fillAmount += 0.01f;
            yield return new WaitForSeconds(0.00000001f);
        }

        // Fill Amount De�eri Zaten D�ng�den Sonra 1.0f Oluyor Ama Ben Yinede �nlem Ama�l� Burda Tekrar 1.0f Yapt�m.
        fireballIcon.fillAmount = 1.0f;

        // Animasyon Tetiklenir, De�i�ken Set Edilir Ve Art�k Ate� Topu Kullan�labilir.
        fireballIconAnmtr.SetTrigger("fireballEnable");
        isFireballActive = true;
    }

    // Ate� Topunu Tekrar Kullanmak ��in Bekleme.
    internal IEnumerator WaitToUsingFireball()
    {
        // De�i�ken Atamalar�.
        Image fireballIcon = GameObject.Find("Canvas/FireballIcon").transform.GetChild(1).GetComponent<Image>();
        fireballIcon.fillAmount = 0.0f;

        // Ate� Topunun Tekrar Aktif Olmas� ��in 10 Saniye Beklenir.
        for (short i = 0; i < 1000; i++)
        {
            fireballIcon.fillAmount += 0.001f;
            yield return new WaitForSeconds(0.01f);
        }

        // Fill Amount De�eri Zaten D�ng�den Sonra 1.0f Oluyor Ama Ben Yinede �nlem Ama�l� Burda Tekrar 1.0f Yapt�m.
        fireballIcon.fillAmount = 1.0f;

        // Animasyon Tetiklenir, De�i�ken Set Edilir Ve Art�k Ate� Topu Kullan�labilir.
        fireballIconAnmtr.SetTrigger("fireballEnable");
        isFireballActive = true;
    }


    // Zemin Kontrol�.
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

    // Havada S�z�lmeyi Ayarl�yor. Bunu Neden Rigidbody'de Ki LinearDrag �le Yapmad�n Dicek Olursak, O S�z�lmeyi
    // Biraz Garip Yap�yor. Ben �stiyorum Ki Karakter Yukar� H�zl� ��ks�n, A�a��ya H�zl� �nsan Ama Havada Oldu�u
    // S�re Boyunca S�z�lerek Yava�las�n. Bunu Linear �le Tam Anlam�yla Yapamayaca��m ��in Kendim Yapt�m.
    private void OnTriggerEnter2D(Collider2D temas1)
    {
        if (temas1.gameObject.name == "BorderLine1")
        {
            rb.gravityScale = 1.5f;
        }
    }
}