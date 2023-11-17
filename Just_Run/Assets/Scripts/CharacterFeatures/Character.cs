using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 
/// Bu Sýnýf Karakteri Ve Karakter Ýle Ýlgili Olan Ýþlevleri Kontrol Ediyor.
/// 
/// 
/// >*>*> Deðiþkenlerin Açýklamalarý <*<*< \\\
/// [Header("Basic Variables")]
///     -> [ walkingEffectSpriteRNDR ]
///     Karakterin Arkasýnda Olan Yürüme Efektini Kapatýp Açmak Ýçin Vardýr. Karakter Zýplayýnca False Olurki Havada Yürüme Efekti Olmasýn.
///     
///     -> [ characterANMTR ]
///     Karakterin Animatör Deðiþkeni.
///     
///     -> [ rb ]
///     Karakterin Rigidbody2D Deðiþkeni.
/// 
/// 
/// [Header("Power Potion Variables")]
///     -> [ powerPotionSprites ]
///     Güç Ýksirinin Sprite'larý. Sað Alttaki Ýksir Þiþesinin Görselini Deðiþtirirken Kullanýlan Ýksir Sprite'larý.
///     
///     -> [ potionEmptySprite ]
///     Adý Üzerinde Belli Oluyor, Yukarýdaki Deðiþken Ýle Ayný Ýþlev.
///     
///     -> [ powerPotionIcon ]
///     Sað Alttaki Ýksir Þisesi. Buna Sprite'lar Aktarýlýyor.
///     
///     -> [ powerPotion ]
///     Sahneye Spawn Olan Ýksir, Oyuncu Yakalayabilirse Ýksiri Ýçmiþ Olur.
///     
///     -> [ powerPotionSpawnControllerValue ]
///     Oyuncu 600 Skora Ulaþtýðýnda Ýksir Spawn Olur. Ýksir Her Spawn Olduktan Sonra 2500 Artarki Ýksir Habire Spawn Olmasýn.
/// 
/// 
/// [Header("Blindness Potion Variables")]
///     -> [ blindnessPotion ]
///     Sahneye Spawn Olan Ýksir, Oyuncu Yakalayabilirse Ýksiri Ýçmiþ Olur.
///     
///     -> [ blindnessEffect ]
///     Körlük Efekti, Oyuncu Ýksiri Ýçince Bu Obje Aktif Olarak Oyuncunun Görüþ Alanýný Kýsýtlar.
///     
///     -> [ blindnessPotionSpawnControllerValue ]
///     Körlük Ýksirinin Spawn Olup Olamayacaðýný Bu Metot Belirler. Eðer True Deðer Döndürüyorsa Ýksir Spawn Olabilir.
/// 
/// 
/// [Header("Health Potion Variables")]
///     -> [ healthPotion ]
///     Sahneye Spawn Olan Ýksir, Oyuncu Yakalayabilirse Ýksiri Ýçmiþ Olur.
///     
///     -> [ heart ]
///     Sol Alttaki Kalbin Prefab'ý. Ýksir Ýçildikten Sonra Bu Kalp Spawn Olur.
///     
///     -> [ healthPotionSpawnControllerValue ]
///     Can Ýksirinin Spawn Olup Olamayacaðýný Bu Metot Belirler. Eðer True Deðer Döndürüyorsa Ýksir Spawn Olabilir.
/// 
/// 
/// [Header("Fire Ball Variables")]
///     -> [ fireballIconAnmtr ]
///     Orta Aþaðýdaki Ateþ Topu Görselinin Animatörüdür. Kullanýcý Süresi Dolmadan Ateþ Topu Atmak Ýsterse Bununla Animasyonu Tetikliyoz.
///     
///     -> [ isTheFireballActive ]
///     Bu Deðiþken Ateþ Topunun Bekleme Süresi Bitince True Olur, Ve False Olduðu Süre Boyunca Oyuncu Ateþ Topu Atamaz.
///     
///     -> [ canFireballBeUsed ]
///     Bu Deðiþken Sadece Oyuncu Koþuyor Veya Zýplýyorsa True Olur, Ve False Olduðu Süre Boyunca Oyuncu Ateþ Topu Atamaz.
/// 
/// 
/// [Header("Other")]
///     -> [ pauseBtn ]
///     Herhangi Bir Ýksir Ýçildiðinde Sol Üstteki Durdurma Butonunun Pasif Olmasý Lazýmki Animasyon Buga Girmesin. Bu Deðiþkende O Durdurma Butonu.
/// 
/// </summary>
public class Character : MonoBehaviour
{
    [Header("Basic Variables")]
    [SerializeField] private SpriteRenderer walkingEffectSpriteRNDR;
    [SerializeField] internal Animator characterANMTR;
    [SerializeField] private Rigidbody2D rb;

    [Header("Power Potion Variables")]
    [SerializeField] private Sprite[] powerPotionSprites = new Sprite[5];
    [SerializeField] private Sprite potionEmptySprite;
    [SerializeField] private Image powerPotionIcon;
    [SerializeField] private GameObject powerPotion;
    internal int powerPotionSpawnControllerValue;

    [Header("Blindness Potion Variables")]
    [SerializeField] private GameObject blindnessPotion;
    [SerializeField] private GameObject blindnessEffect;
    internal bool blindnessPotionSpawnControllerValue;

    [Header("Health Potion Variables")]
    [SerializeField] private GameObject healthPotion;
    [SerializeField] private GameObject heart;
    internal bool healthPotionSpawnControllerValue;

    [Header("Fireball Variables")]
    [SerializeField] private TextMeshProUGUI fireballCounter;
    [SerializeField] private Animator fireballIconAnmtr;
    internal bool isTheFireballActive;
    internal bool canFireballBeUsed;

    [Header("Other")]
    [SerializeField] private Button pauseBtn;

    public static Character chrctrTHIS;
    internal byte health = 2;
    internal bool isOnGround;



    private void Start()
    {
        // Temel Ayarlamalar, Baþlangýç Ýçin Hazýrlýk.
        chrctrTHIS = this;
        healthPotionSpawnControllerValue = true;
        blindnessPotionSpawnControllerValue = false;
        if (PlayerPrefs.GetInt("isCheatModeActive") == 1 && PlayerPrefs.GetInt("isAlwaysPowerPotion") == 1)
            powerPotionSpawnControllerValue = 50;
        else
            powerPotionSpawnControllerValue = 600;

        // Gerekli Ýþlemler Yapýldýktan Sonra Baþlangýç.
        SetNewScore();
        StartCoroutine(WaitToUsingFireball());
        Time.timeScale = 1.0f;
    }
    private void Update()
    {
        // Zýplama.
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && isOnGround == true)
        {
            rb.AddForce(Vector2.up * 600.0f);

            isOnGround = false;
            walkingEffectSpriteRNDR.enabled = false;
            characterANMTR.SetBool("isJumping", true);

            AudioManager.admgTHIS.PlayOneShotASound("JumpSound");
        }
        
        // Kýlýçla Saldýrma.
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && isOnGround == true)
            characterANMTR.SetBool("isAttacking", true);
        else
            characterANMTR.SetBool("isAttacking", false);

        // Kýlýçla Havada Saldýrma.
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && isOnGround == false)
            characterANMTR.SetBool("isJumpAttacking", true);
        else
            characterANMTR.SetBool("isJumpAttacking", false);

        // Ateþ Topu Atma.
        if ((Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.RightArrow))
            && GameObject.Find("Canvas/StopPanel") == false)
        {
            if (isTheFireballActive == true && canFireballBeUsed == true)
            {
                // Tekrar Ateþ Topu Atýlamasýn Diye Deðiþken False Edilir, Fýrlatma Animasyonu Tetiklenir Ve
                // Bir Sonraki Ateþ Topu Ýçin Bekleme Metodu Baþlar. Ateþ Topu Animasyonun Eventinde Doðuyor.
                isTheFireballActive = false;
                characterANMTR.SetTrigger("isThrowingTheFireball");
                StartCoroutine(WaitToUsingFireball());
            }
            else if (isTheFireballActive == false && canFireballBeUsed == true)
            {
                if (PlayerPrefs.GetInt("isCheatActive") == 0)
                {
                    fireballIconAnmtr.SetTrigger("fireballDisable");
                    AudioManager.admgTHIS.PlayOneShotASound("ErrorSound");
                }
                else if (PlayerPrefs.GetInt("isCheatActive") == 1 && PlayerPrefs.GetInt("fireballTimerValue") != 0)
                {
                    fireballIconAnmtr.SetTrigger("fireballDisable");
                    AudioManager.admgTHIS.PlayOneShotASound("ErrorSound");
                }
            }
        }
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
    // Süre Boyunca Süzülerek Yavaþlasýn. Bunu Linear Ýle Tam Anlamýyla Yapamayacaðým Ýçin Bu Þekilde Yaptým.
    private void OnTriggerEnter2D(Collider2D temas1)
    {
        if (temas1.gameObject.name == "BorderLine1")
            rb.gravityScale = 1.5f;
    }


    #region Bu Metotlar, Düþmanlarla Etkileþim Sonucu Tetikleniyor.
    // Yani, Adý Üstünde Bir Metot Anlýyorsundur Umarým.
    internal void MinusHealth()
    {
        if ((PlayerPrefs.GetInt("isCheatModeActive") == 1 && PlayerPrefs.GetInt("isGodModeOn") == 0) || PlayerPrefs.GetInt("isCheatModeActive") == 0)
        {
            health--;
            if (health <= 0)
                Die();
            else
                GameObject.Find("Canvas").transform.GetChild(1).GetChild(1).gameObject.GetComponent<Animator>().enabled = true;
        }
    }


    // Bu Metot Oyuncu Skor Aldýktan Sonra Ve Skor Animasyon Objesi Bitip Silindikten Sonra Skoru Güncelliyor.
    internal void SetNewScore()
    {
        switch (PlayerPrefs.GetString("LeanLocalization.CurrentLanguage"))
        {
            case "Turkish":     GameObject.Find("Canvas/Score").GetComponent<TextMeshProUGUI>().text = "Skorun: "        + ScoreManager.smTHIS.score.ToString(); break;
            case "Azerbaijani": GameObject.Find("Canvas/Score").GetComponent<TextMeshProUGUI>().text = "Xalýnýz: "       + ScoreManager.smTHIS.score.ToString(); break;
            case "English":     GameObject.Find("Canvas/Score").GetComponent<TextMeshProUGUI>().text = "Your Score: "    + ScoreManager.smTHIS.score.ToString(); break;
            case "Spanish":     GameObject.Find("Canvas/Score").GetComponent<TextMeshProUGUI>().text = "Tu Puntaje: "    + ScoreManager.smTHIS.score.ToString(); break;
            case "German":      GameObject.Find("Canvas/Score").GetComponent<TextMeshProUGUI>().text = "Ihr Punktzahl: " + ScoreManager.smTHIS.score.ToString(); break;
        }
    }


    // Bu Metot MinusHealth Metoduyla Baðlantýlý, Oradan Tetikleniyor. Adý Üstünde Yani Çok Bir Þeyi Yok.
    internal void Die()
    {
        // Son Kalbin Yok Edilme Animasyonunun Tetiklenmesi.
        GameObject.Find("Canvas").transform.GetChild(1).GetChild(0).gameObject.GetComponent<Animator>().enabled = true;

        // Ölüm Panelinin Ve Fake Panelin Aktif Edilmesi.
        GameObject.Find("Canvas").transform.GetChild(7).gameObject.SetActive(true);
        GameObject.Find("Canvas").transform.GetChild(8).gameObject.SetActive(true);

        // Körlük Efektinin Kapatma Ve Oyunu Durdurma Butonunu Pasif Hale Getirme.
        Destroy(blindnessEffect);
        GameObject.Find("Canvas/PauseBTN").SetActive(false);

        // Bütün Düþmanlarýn Scriptlerini Yok Etme.
        Transform enemiesPath = GameObject.Find("Enemies").transform;
        Destroy(enemiesPath.GetChild(0).gameObject.GetComponent<Eagle>());
        Destroy(enemiesPath.GetChild(1).gameObject.GetComponent<SolidWall>());
        Destroy(enemiesPath.GetChild(2).gameObject.GetComponent<BrokenWall>());

        // Kuþ Sürüsü Aktifse Sesini Kapatma.
        if (GameObject.Find("Eagles(Clone)") == true)
        {
            Destroy(GameObject.Find("Eagles(Clone)").GetComponent<AudioSource>());
        }

        // Ateþ Topu Aktifse Silme Ve Sesini Kapatma.
        GameObject fireball = GameObject.Find("Fireball(Clone)");
        if (fireball == true)
        {
            AudioSource soundManager = GameObject.Find("AudioManager/SoundManager").GetComponent<AudioSource>();
            soundManager.enabled = false;
            soundManager.enabled = true;
            Destroy(fireball);
        }

        // Eski En Yüksek Skoru Ve En Yüksek Skoru Ayarlama.
        if (PlayerPrefs.GetInt("isCheatModeActive") == 0 && ScoreManager.smTHIS.score > PlayerPrefs.GetInt("bestScore"))
        {
            PlayerPrefs.SetInt("oldBestScore", PlayerPrefs.GetInt("bestScore"));

            PlayerPrefs.SetInt("bestScore", (ScoreManager.smTHIS.score - 200));
            if (PlayerPrefs.GetInt("bestScore") < 0)
                PlayerPrefs.SetInt("bestScore", 0);
        }

        // Kapanýþ.
        Time.timeScale = 0.0f;
        Destroy(gameObject);
    }
    #endregion

    #region Karakterin Özellikleri Ýçin Oluþturulmuþ Metotlar (Ýksirler Ve Alev Topu)
    #region Ýksirler
    /// <summary>
    /// 
    /// Ýksirlerin Spawn Olup Olamayacaðýný Belirleyen Metodlar (CanPowerPotionBeSpawn, CanBlindnessPotionBeSpawn, CanHealthPotionBeSpawn)
    /// [ ScoreManager ] Scriptinden Tetiklenmektedir. Bu Metodlar Oyuncu Bir Skor Aldýðýnda Tetiklenir.
    /// 
    /// </summary>
    #region Power Potion
    // Bu Metot Güç Ýksirinin Aktif Olup Olamayacaðýný Deðerlendiriyor. [ powerPotionSpawnControllerValue ] Deðiþkeni
    // Baþlangýçta 600'dür. Ve If'te Görebileceðin Üzere [ score % 600 ] Olduðunda Güç Ýksiri Spawn Olur. Güç Ýksiri Her
    // Spawn Olduktan Sonra Bu Deðiþkenin Deðeri 1500 Artar. Deðiþkenin Deðeri [ PowerPotionIsDone ] Metodunda Ayarlanýyor.
    internal void CanPowerPotionBeSpawn()
    {
        // Bu Metot ScoreManager Sýnýfýndan Tetikleniyor. Ne Zaman Skor Deðeri Artarsa
        // Güç Ýksiri Aktif Olmaya Uygun Mu Deðil Mi Onu Kontrol Ediyor.
        if (ScoreManager.smTHIS.score % powerPotionSpawnControllerValue == 0
            && PotionManager.isTheAnyPotionActive == false && PotionManager.didThePlayerDrankPowerPotion == false)
        {
            // Sahnede Bir Ýksirin Aktif Olduðunu Belirtme.
            PotionManager.isTheAnyPotionActive = true;

            // Can Ýksirini Çaðýrma.
            Instantiate(powerPotion);
        }
    }

    // Aþaðýdaki Bütün Metotlar  [ DrinkPowerPotion ]  Animasyonundan Tetikleniyor.
    private void PowerPotion5()
    {
        // Ýksirin Kontrolünü Saðlayabilmek Ýçin Gerekli Deðiþkenler Set Edilir.
        PotionManager.didThePlayerDrankPowerPotion = true;
        PotionManager.isTheAnyPotionAnimActive = false;
        PotionManager.isTheAnyPotionActive = false;

        // Oyunu Durdurma Özelliðini Geri Aktifleþtirir, Ýksir Ýçilme Esnasýnda False Oluyor.
        pauseBtn.enabled = true;

        // Oyuncu Tekrardan Zýplayabilir.
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.zero);
        isOnGround = true;

        // Yürüme Efektini Aktif Eder, Sað Alttaki Ýksir Þiþesini Doldurur Ve Oyun Hýzý Ayarlarlanýr.
        powerPotionIcon.sprite = powerPotionSprites[4];
        walkingEffectSpriteRNDR.enabled = true;
        Time.timeScale = 1.8f;
    }
    private void PowerPotion4()
    {
        // Sað Alttaki Ýksir Görselini Günceller, Ses Efekti Çalýnýr, Animasyon Tetiklenir.
        powerPotionIcon.sprite = powerPotionSprites[3];
        AudioManager.admgTHIS.PlayOneShotASound("DrinkPotionSound");
        GameObject.Find("Canvas/PowerPotionIcon").gameObject.GetComponent<Animator>().SetTrigger("changePotionSpriteAnim");
    }
    private void PowerPotion3()
    {
        // Sað Alttaki Ýksir Görselini Günceller, Ses Efekti Çalýnýr, Animasyon Tetiklenir.
        powerPotionIcon.sprite = powerPotionSprites[2];
        AudioManager.admgTHIS.PlayOneShotASound("DrinkPotionSound");
        GameObject.Find("Canvas/PowerPotionIcon").gameObject.GetComponent<Animator>().SetTrigger("changePotionSpriteAnim");
    }
    private void PowerPotion2()
    {
        // Sað Alttaki Ýksir Görselini Günceller, Ses Efekti Çalýnýr, Animasyon Tetiklenir.
        powerPotionIcon.sprite = powerPotionSprites[1];
        AudioManager.admgTHIS.PlayOneShotASound("DrinkPotionSound");
        GameObject.Find("Canvas/PowerPotionIcon").gameObject.GetComponent<Animator>().SetTrigger("changePotionSpriteAnim");
    }
    private void PowerPotion1()
    {
        // Sað Alttaki Ýksir Görselini Günceller, Ses Efekti Çalýnýr, Animasyon Tetiklenir.
        powerPotionIcon.sprite = powerPotionSprites[0];
        AudioManager.admgTHIS.PlayOneShotASound("DrinkPotionSound");
        GameObject.Find("Canvas/PowerPotionIcon").gameObject.GetComponent<Animator>().SetTrigger("changePotionSpriteAnim");
    }
    private IEnumerator PowerPotionIsDone()
    {
        // Oyunun Durdurulmasý Engellenir.
        PotionManager.isTheAnyPotionAnimActive = true;
        pauseBtn.enabled = false;

        // Güç Ýksirinin Etkisi Bitmeden Önce Oyun Hýzýný Yavaþlatýp Hýzlandýrma.
        for (float i = 1.8f; i >= 0.6f; i -= 0.05f)
        {
            Time.timeScale = i;
            yield return new WaitForSeconds(0.035f);
        }
        for (float i = 0.6f; i < 1.0f; i += 0.05f)
        {
            Time.timeScale = i;
            yield return new WaitForSeconds(0.03f);
        }

        // Bir Sonraki Ýksirin Ne Zaman Spawn Olucaðýný Belirleme.
        if (PlayerPrefs.GetInt("isCheatModeActive") == 1 && PlayerPrefs.GetInt("isAlwaysPowerPotion") == 1)
            powerPotionSpawnControllerValue = ScoreManager.smTHIS.score + 50;
        else
            powerPotionSpawnControllerValue = ScoreManager.smTHIS.score + 1500;

        // Körlük Ýksirinin Zamanlayýcýsýný Aktif Edip Çaðýrma Sürecini Baþlatma.
        StartCoroutine(BlindnessPotionTimer());

        // Son Kez PotionInfo Animasyonunu Tetikler Ve Þiþeyi Eski Haline Çevirir.
        GameObject.Find("Canvas/PowerPotionIcon").gameObject.GetComponent<Animator>().SetTrigger("changePotionSpriteAnim");
        powerPotionIcon.sprite = potionEmptySprite;

        // Oyun Tekrardan Durdurulabilir.
        pauseBtn.enabled = true;

        // Oyun Mekanikleri Eski Haline Döner Ve Artýk Ýksir Özelliði Biter.
        PotionManager.didThePlayerDrankPowerPotion = false;
        PotionManager.isTheAnyPotionAnimActive = false;
        PotionManager.isTheAnyPotionActive = false;
        Time.timeScale = 1;
    }
    #endregion


    #region Blindness Potion
    // Güç Ýksirinin Aktifliði Bittikten Sonra [ BlindnessPotionTimer ] Metodu Tetiklenir Ve 12 Saniye Sonra 
    // [ CanBlindnessPotionBeSpawn ] Metodunda Ki If True Deðeri Döndürerek, Körlük Ýksirinin Spawn Olmasýný Saðlar.
    internal void CanBlindnessPotionBeSpawn()
    {
        // Körlük Ýksiri Aktif Olmaya Uygun Mu Deðil Mi Onu Kontrol Ediyor.
        if (blindnessPotionSpawnControllerValue == true
            && PotionManager.isTheAnyPotionActive == false && PotionManager.didThePlayerDrankPowerPotion == false)
        {
            // Sahnede Bir Ýksirin Aktif Olduðunu Belirtme.
            PotionManager.isTheAnyPotionActive = true;

            // Körlük Ýksirini Çaðýrma.
            Instantiate(blindnessPotion);
        }
    }
    internal IEnumerator BlindnessPotionTimer()
    {
        blindnessPotionSpawnControllerValue = false;
        yield return new WaitForSeconds(12.0f);
        blindnessPotionSpawnControllerValue = true;
    }

    // Bu Metot  [ AnimEventConroller ]  Scriptinden Tetikleniyor. Karakterin Körlük Ýksirini Ýçme Animasyonu Bitince Bu Metot Çalýþýr.
    internal IEnumerator KeepTheBlindnessPotionActive()
    {
        // Olasý Hatalara Karþý Önlem Amaçlý Bekleme.
        yield return new WaitForSecondsRealtime(0.1f);

        // Oyuncu Tekrardan Zýplayabilir.
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.zero);
        isOnGround = true;

        // Oyunu Geri Baþlatma
        Time.timeScale = 1.0f;

        // Ýksirin Kontrolünü Saðlayabilmek Ýçin Gerekli Deðiþkenler Set Edilir
        PotionManager.isTheAnyPotionAnimActive = false;
        PotionManager.isTheAnyPotionActive = false;
        blindnessPotionSpawnControllerValue = false;

        // Sýrasýyla: Körlük Efektinin Image'i Aktif Edilir Ve Durdurma Butonu Tekrardan Aktifleþir.
        gameObject.transform.GetChild(3).gameObject.SetActive(true);
        pauseBtn.enabled = true;



        // Zamanlayýcýyý Baþlatma, 15 Saniye Boyunca Karakter Kör Olucak.
        yield return new WaitForSecondsRealtime(15.0f);

        // 10 Saniye Doldu Ve Körlük Efektini Kapatma Animasyonu Tetiklendi.
        blindnessEffect.GetComponent<Animator>().SetTrigger("setDisableBlindnessEffect");
    }
    #endregion


    #region Health Potion
    // Can Ýksiri [ healthPotionSpawnControllerValue ] Deðiþkeni True Olduðunda Ve Oyuncunun Caný 1 Olduðunda Spawn Olur.
    // Baþlangýçta Bu Deðiþkenin Deðeri True Olduðu Ýçin Oyuncunun Caný Azalýr Azalmaz Can Ýksiri Spawn Olur. Ancak
    // Daha Sonrasýnda [ HealthPotionShortTimer ] Metodu Çalýþarak Deðiþken 1.5dk Sonra True Olur. Eðer Oyuncu Ýksiri
    // Kaçýrýrsa Ve Alamazsa Bu Süre 2dk Olur. [ HealthPotionLongTimer ] Metodu, [ HealthPotion ] Scriptinden Tetiklenmektedir.
    internal void CanHealthPotionBeSpawn()
    {
        // Bu Metot ScoreManager Sýnýfýndan Tetikleniyor. Ne Zaman Skor Deðeri Artarsa
        // Güç Ýksiri Aktif Olmaya Uygun Mu Deðil Mi Onu Kontrol Ediyor.
        if (health == 1 && healthPotionSpawnControllerValue == true
            && PotionManager.isTheAnyPotionActive == false && PotionManager.didThePlayerDrankPowerPotion == false)
        {
            // Hile Kapalýysa Controller Deðiþkenini False Yap.
            if (PlayerPrefs.GetInt("isCheatModeActive") == 1 && PlayerPrefs.GetInt("isAlwaysHealthPotion") == 0)
                healthPotionSpawnControllerValue = false;

            // Sahnede Bir Ýksirin Aktif Olduðunu Belirtme.
            PotionManager.isTheAnyPotionActive = true;

            // Can Ýksirini Çaðýrma.
            Instantiate(healthPotion);
        }
    }
    internal IEnumerator HealthPotionShortTimer()
    {
        healthPotionSpawnControllerValue = false;
        yield return new WaitForSeconds(90.0f);
        healthPotionSpawnControllerValue = true;
    }
    internal IEnumerator HealthPotionLongTimer()
    {
        healthPotionSpawnControllerValue = false;
        yield return new WaitForSeconds(120.0f);
        healthPotionSpawnControllerValue = true;
    }

    // Bu Metot  [ DrinkHealthPotion ]  Animasyonundan Tetikleniyor. Can Ýksirini Ýçme Animasyonu Bitince Bu Metot Çalýþýr.
    private void InstantieHearthAndHealthPlus()
    {
        // Oyuncunun Canýný Arttýrma Ve Kalp Ýkonunu Çaðýrma.
        health = 2;
        Instantiate(heart, GameObject.Find("Canvas/Hearts").transform);

        // Ýksirin Kontrolünü Saðlayabilmek Ýçin Gerekli Deðiþkenler Set Edilir.
        PotionManager.isTheAnyPotionAnimActive = false;
        PotionManager.isTheAnyPotionActive = false;

        // Oyunu Durdurma Özelliðini Geri Aktifleþtirir, Ýksir Ýçilme Esnasýnda False Oluyor.
        pauseBtn.enabled = true;

        // Oyuncu Tekrardan Zýplayabilir.
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.zero);
        isOnGround = true;

        // Oyunu Geri Baþlatma.
        Time.timeScale = 1.0f;
    }
    #endregion
    #endregion

    // Bu Metot Ateþ Topunu Beklemeye Sokar Ve Oyuncu Sadece 10 Saniyede Bir Ateþ Topu Kullanabilir.
    internal IEnumerator WaitToUsingFireball()
    {
        // Hile Açýksa Ateþ Topunu Hileli Versiyona Göre Dizayn Etme.
        if (PlayerPrefs.GetInt("isCheatModeActive") == 1)
        {
            if (PlayerPrefs.GetInt("fireballTimerValue") != 0)
            {
                // Deðiþken Atamalarý.
                Image fireballIcon = GameObject.Find("Canvas/FireballIcon").transform.GetChild(1).GetComponent<Image>();
                fireballIcon.fillAmount = 0.0f;

                // Ateþ Topunun Sayacý Aktif Hale Getirilir.
                fireballCounter.gameObject.SetActive(true);

                // Ateþ Topunun Bekleme Süresi Kayýt Edilir.
                sbyte fireballTimerValue = (sbyte)PlayerPrefs.GetInt("fireballTimerValue");

                // Bekleme Kýsmý. Oyuncunun Hileyle Ayarladýðý Süre Boyunca Beklenilir.
                for (sbyte i = (sbyte)PlayerPrefs.GetInt("fireballTimerValue"); i > 0; i--)
                {
                    // Ateþ Topunun Sayacýna Bekleme Süresi Yazdýrýlýr Ve Beklenir.
                    fireballCounter.text = i.ToString();
                    yield return new WaitForSeconds(1.0f);

                    // Eðer Oyuncu Ayarlardan Ateþ Topunun Bekleme Süresini Deðiþtirirse Döngü Kýrýlýr Ve Biter.
                    if (fireballTimerValue != PlayerPrefs.GetInt("fireballTimerValue"))
                        break;
                }

                // Sistem Döngüden Çýktýðýnda, Ateþ Topunun Bekleme Süresinin Deðiþtirilme Sebebiyle Çýkmýþsa Aþaðýdaki Kodlar Ýþlenmez.
                if (fireballTimerValue == PlayerPrefs.GetInt("fireballTimerValue"))
                {
                    fireballCounter.gameObject.SetActive(false);

                    // Fill Amount Deðeri Zaten Döngüden Sonra 1.0f Oluyor Ama Ben Yinede Önlem Amaçlý Burda Tekrar 1.0f Yaptým.
                    fireballIcon.fillAmount = 1.0f;

                    // Animasyon Tetiklenir, Deðiþken Set Edilir Ve Artýk Ateþ Topu Kullanýlabilir.
                    fireballIconAnmtr.SetTrigger("fireballEnable");
                    isTheFireballActive = true;
                }
            }
            else
            {
                // Ateþ Topunun Sayacýný Pasif Hale Getirme.
                fireballCounter.gameObject.SetActive(false);

                // Ateþ Topunun Texture FillAmount Deðerini Fulleme.
                GameObject.Find("Canvas/FireballIcon").transform.GetChild(1).GetComponent<Image>().fillAmount = 1.0f;

                // Animasyon Tetiklenir, Deðiþken Set Edilir Ve Artýk Ateþ Topu Kullanýlabilir.
                fireballIconAnmtr.SetTrigger("fireballEnable");
                isTheFireballActive = true;
            }
        }
        // Hile Kapalý Olduðu Ýçin Ateþ Topunu Normal Bir Þekilde Çalýþtýrýr.
        else
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
            isTheFireballActive = true;
        }
    }
    #endregion
}