using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 
/// Bu S�n�f Karakteri Ve Karakter �le �lgili Olan ��levleri Kontrol Ediyor.
/// 
/// 
/// >*>*> De�i�kenlerin A��klamalar� <*<*< \\\
/// [Header("Basic Variables")]
///     -> [ walkingEffectSpriteRNDR ]
///     Karakterin Arkas�nda Olan Y�r�me Efektini Kapat�p A�mak ��in Vard�r. Karakter Z�play�nca False Olurki Havada Y�r�me Efekti Olmas�n.
///     
///     -> [ characterANMTR ]
///     Karakterin Animat�r De�i�keni.
///     
///     -> [ rb ]
///     Karakterin Rigidbody2D De�i�keni.
/// 
/// 
/// [Header("Power Potion Variables")]
///     -> [ powerPotionSprites ]
///     G�� �ksirinin Sprite'lar�. Sa� Alttaki �ksir �i�esinin G�rselini De�i�tirirken Kullan�lan �ksir Sprite'lar�.
///     
///     -> [ potionEmptySprite ]
///     Ad� �zerinde Belli Oluyor, Yukar�daki De�i�ken �le Ayn� ��lev.
///     
///     -> [ powerPotionIcon ]
///     Sa� Alttaki �ksir �isesi. Buna Sprite'lar Aktar�l�yor.
///     
///     -> [ powerPotion ]
///     Sahneye Spawn Olan �ksir, Oyuncu Yakalayabilirse �ksiri ��mi� Olur.
///     
///     -> [ powerPotionSpawnControllerValue ]
///     Oyuncu 600 Skora Ula�t���nda �ksir Spawn Olur. �ksir Her Spawn Olduktan Sonra 2500 Artarki �ksir Habire Spawn Olmas�n.
/// 
/// 
/// [Header("Blindness Potion Variables")]
///     -> [ blindnessPotion ]
///     Sahneye Spawn Olan �ksir, Oyuncu Yakalayabilirse �ksiri ��mi� Olur.
///     
///     -> [ blindnessEffect ]
///     K�rl�k Efekti, Oyuncu �ksiri ��ince Bu Obje Aktif Olarak Oyuncunun G�r�� Alan�n� K�s�tlar.
///     
///     -> [ blindnessPotionSpawnControllerValue ]
///     K�rl�k �ksirinin Spawn Olup Olamayaca��n� Bu Metot Belirler. E�er True De�er D�nd�r�yorsa �ksir Spawn Olabilir.
/// 
/// 
/// [Header("Health Potion Variables")]
///     -> [ healthPotion ]
///     Sahneye Spawn Olan �ksir, Oyuncu Yakalayabilirse �ksiri ��mi� Olur.
///     
///     -> [ heart ]
///     Sol Alttaki Kalbin Prefab'�. �ksir ��ildikten Sonra Bu Kalp Spawn Olur.
///     
///     -> [ healthPotionSpawnControllerValue ]
///     Can �ksirinin Spawn Olup Olamayaca��n� Bu Metot Belirler. E�er True De�er D�nd�r�yorsa �ksir Spawn Olabilir.
/// 
/// 
/// [Header("Fire Ball Variables")]
///     -> [ fireballIconAnmtr ]
///     Orta A�a��daki Ate� Topu G�rselinin Animat�r�d�r. Kullan�c� S�resi Dolmadan Ate� Topu Atmak �sterse Bununla Animasyonu Tetikliyoz.
///     
///     -> [ isTheFireballActive ]
///     Bu De�i�ken Ate� Topunun Bekleme S�resi Bitince True Olur, Ve False Oldu�u S�re Boyunca Oyuncu Ate� Topu Atamaz.
///     
///     -> [ canFireballBeUsed ]
///     Bu De�i�ken Sadece Oyuncu Ko�uyor Veya Z�pl�yorsa True Olur, Ve False Oldu�u S�re Boyunca Oyuncu Ate� Topu Atamaz.
/// 
/// 
/// [Header("Other")]
///     -> [ pauseBtn ]
///     Herhangi Bir �ksir ��ildi�inde Sol �stteki Durdurma Butonunun Pasif Olmas� Laz�mki Animasyon Buga Girmesin. Bu De�i�kende O Durdurma Butonu.
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
        // Temel Ayarlamalar, Ba�lang�� ��in Haz�rl�k.
        chrctrTHIS = this;
        healthPotionSpawnControllerValue = true;
        blindnessPotionSpawnControllerValue = false;
        if (PlayerPrefs.GetInt("isCheatModeActive") == 1 && PlayerPrefs.GetInt("isAlwaysPowerPotion") == 1)
            powerPotionSpawnControllerValue = 50;
        else
            powerPotionSpawnControllerValue = 600;

        // Gerekli ��lemler Yap�ld�ktan Sonra Ba�lang��.
        SetNewScore();
        StartCoroutine(WaitToUsingFireball());
        Time.timeScale = 1.0f;
    }
    private void Update()
    {
        // Z�plama.
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && isOnGround == true)
        {
            rb.AddForce(Vector2.up * 600.0f);

            isOnGround = false;
            walkingEffectSpriteRNDR.enabled = false;
            characterANMTR.SetBool("isJumping", true);

            AudioManager.admgTHIS.PlayOneShotASound("JumpSound");
        }
        
        // K�l��la Sald�rma.
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && isOnGround == true)
            characterANMTR.SetBool("isAttacking", true);
        else
            characterANMTR.SetBool("isAttacking", false);

        // K�l��la Havada Sald�rma.
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && isOnGround == false)
            characterANMTR.SetBool("isJumpAttacking", true);
        else
            characterANMTR.SetBool("isJumpAttacking", false);

        // Ate� Topu Atma.
        if ((Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.RightArrow))
            && GameObject.Find("Canvas/StopPanel") == false)
        {
            if (isTheFireballActive == true && canFireballBeUsed == true)
            {
                // Tekrar Ate� Topu At�lamas�n Diye De�i�ken False Edilir, F�rlatma Animasyonu Tetiklenir Ve
                // Bir Sonraki Ate� Topu ��in Bekleme Metodu Ba�lar. Ate� Topu Animasyonun Eventinde Do�uyor.
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
    // S�re Boyunca S�z�lerek Yava�las�n. Bunu Linear �le Tam Anlam�yla Yapamayaca��m ��in Bu �ekilde Yapt�m.
    private void OnTriggerEnter2D(Collider2D temas1)
    {
        if (temas1.gameObject.name == "BorderLine1")
            rb.gravityScale = 1.5f;
    }


    #region Bu Metotlar, D��manlarla Etkile�im Sonucu Tetikleniyor.
    // Yani, Ad� �st�nde Bir Metot Anl�yorsundur Umar�m.
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


    // Bu Metot Oyuncu Skor Ald�ktan Sonra Ve Skor Animasyon Objesi Bitip Silindikten Sonra Skoru G�ncelliyor.
    internal void SetNewScore()
    {
        switch (PlayerPrefs.GetString("LeanLocalization.CurrentLanguage"))
        {
            case "Turkish":     GameObject.Find("Canvas/Score").GetComponent<TextMeshProUGUI>().text = "Skorun: "        + ScoreManager.smTHIS.score.ToString(); break;
            case "Azerbaijani": GameObject.Find("Canvas/Score").GetComponent<TextMeshProUGUI>().text = "Xal�n�z: "       + ScoreManager.smTHIS.score.ToString(); break;
            case "English":     GameObject.Find("Canvas/Score").GetComponent<TextMeshProUGUI>().text = "Your Score: "    + ScoreManager.smTHIS.score.ToString(); break;
            case "Spanish":     GameObject.Find("Canvas/Score").GetComponent<TextMeshProUGUI>().text = "Tu Puntaje: "    + ScoreManager.smTHIS.score.ToString(); break;
            case "German":      GameObject.Find("Canvas/Score").GetComponent<TextMeshProUGUI>().text = "Ihr Punktzahl: " + ScoreManager.smTHIS.score.ToString(); break;
        }
    }


    // Bu Metot MinusHealth Metoduyla Ba�lant�l�, Oradan Tetikleniyor. Ad� �st�nde Yani �ok Bir �eyi Yok.
    internal void Die()
    {
        // Son Kalbin Yok Edilme Animasyonunun Tetiklenmesi.
        GameObject.Find("Canvas").transform.GetChild(1).GetChild(0).gameObject.GetComponent<Animator>().enabled = true;

        // �l�m Panelinin Ve Fake Panelin Aktif Edilmesi.
        GameObject.Find("Canvas").transform.GetChild(7).gameObject.SetActive(true);
        GameObject.Find("Canvas").transform.GetChild(8).gameObject.SetActive(true);

        // K�rl�k Efektinin Kapatma Ve Oyunu Durdurma Butonunu Pasif Hale Getirme.
        Destroy(blindnessEffect);
        GameObject.Find("Canvas/PauseBTN").SetActive(false);

        // B�t�n D��manlar�n Scriptlerini Yok Etme.
        Transform enemiesPath = GameObject.Find("Enemies").transform;
        Destroy(enemiesPath.GetChild(0).gameObject.GetComponent<Eagle>());
        Destroy(enemiesPath.GetChild(1).gameObject.GetComponent<SolidWall>());
        Destroy(enemiesPath.GetChild(2).gameObject.GetComponent<BrokenWall>());

        // Ku� S�r�s� Aktifse Sesini Kapatma.
        if (GameObject.Find("Eagles(Clone)") == true)
        {
            Destroy(GameObject.Find("Eagles(Clone)").GetComponent<AudioSource>());
        }

        // Ate� Topu Aktifse Silme Ve Sesini Kapatma.
        GameObject fireball = GameObject.Find("Fireball(Clone)");
        if (fireball == true)
        {
            AudioSource soundManager = GameObject.Find("AudioManager/SoundManager").GetComponent<AudioSource>();
            soundManager.enabled = false;
            soundManager.enabled = true;
            Destroy(fireball);
        }

        // Eski En Y�ksek Skoru Ve En Y�ksek Skoru Ayarlama.
        if (PlayerPrefs.GetInt("isCheatModeActive") == 0 && ScoreManager.smTHIS.score > PlayerPrefs.GetInt("bestScore"))
        {
            PlayerPrefs.SetInt("oldBestScore", PlayerPrefs.GetInt("bestScore"));

            PlayerPrefs.SetInt("bestScore", (ScoreManager.smTHIS.score - 200));
            if (PlayerPrefs.GetInt("bestScore") < 0)
                PlayerPrefs.SetInt("bestScore", 0);
        }

        // Kapan��.
        Time.timeScale = 0.0f;
        Destroy(gameObject);
    }
    #endregion

    #region Karakterin �zellikleri ��in Olu�turulmu� Metotlar (�ksirler Ve Alev Topu)
    #region �ksirler
    /// <summary>
    /// 
    /// �ksirlerin Spawn Olup Olamayaca��n� Belirleyen Metodlar (CanPowerPotionBeSpawn, CanBlindnessPotionBeSpawn, CanHealthPotionBeSpawn)
    /// [ ScoreManager ] Scriptinden Tetiklenmektedir. Bu Metodlar Oyuncu Bir Skor Ald���nda Tetiklenir.
    /// 
    /// </summary>
    #region Power Potion
    // Bu Metot G�� �ksirinin Aktif Olup Olamayaca��n� De�erlendiriyor. [ powerPotionSpawnControllerValue ] De�i�keni
    // Ba�lang��ta 600'd�r. Ve If'te G�rebilece�in �zere [ score % 600 ] Oldu�unda G�� �ksiri Spawn Olur. G�� �ksiri Her
    // Spawn Olduktan Sonra Bu De�i�kenin De�eri 1500 Artar. De�i�kenin De�eri [ PowerPotionIsDone ] Metodunda Ayarlan�yor.
    internal void CanPowerPotionBeSpawn()
    {
        // Bu Metot ScoreManager S�n�f�ndan Tetikleniyor. Ne Zaman Skor De�eri Artarsa
        // G�� �ksiri Aktif Olmaya Uygun Mu De�il Mi Onu Kontrol Ediyor.
        if (ScoreManager.smTHIS.score % powerPotionSpawnControllerValue == 0
            && PotionManager.isTheAnyPotionActive == false && PotionManager.didThePlayerDrankPowerPotion == false)
        {
            // Sahnede Bir �ksirin Aktif Oldu�unu Belirtme.
            PotionManager.isTheAnyPotionActive = true;

            // Can �ksirini �a��rma.
            Instantiate(powerPotion);
        }
    }

    // A�a��daki B�t�n Metotlar  [ DrinkPowerPotion ]  Animasyonundan Tetikleniyor.
    private void PowerPotion5()
    {
        // �ksirin Kontrol�n� Sa�layabilmek ��in Gerekli De�i�kenler Set Edilir.
        PotionManager.didThePlayerDrankPowerPotion = true;
        PotionManager.isTheAnyPotionAnimActive = false;
        PotionManager.isTheAnyPotionActive = false;

        // Oyunu Durdurma �zelli�ini Geri Aktifle�tirir, �ksir ��ilme Esnas�nda False Oluyor.
        pauseBtn.enabled = true;

        // Oyuncu Tekrardan Z�playabilir.
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.zero);
        isOnGround = true;

        // Y�r�me Efektini Aktif Eder, Sa� Alttaki �ksir �i�esini Doldurur Ve Oyun H�z� Ayarlarlan�r.
        powerPotionIcon.sprite = powerPotionSprites[4];
        walkingEffectSpriteRNDR.enabled = true;
        Time.timeScale = 1.8f;
    }
    private void PowerPotion4()
    {
        // Sa� Alttaki �ksir G�rselini G�nceller, Ses Efekti �al�n�r, Animasyon Tetiklenir.
        powerPotionIcon.sprite = powerPotionSprites[3];
        AudioManager.admgTHIS.PlayOneShotASound("DrinkPotionSound");
        GameObject.Find("Canvas/PowerPotionIcon").gameObject.GetComponent<Animator>().SetTrigger("changePotionSpriteAnim");
    }
    private void PowerPotion3()
    {
        // Sa� Alttaki �ksir G�rselini G�nceller, Ses Efekti �al�n�r, Animasyon Tetiklenir.
        powerPotionIcon.sprite = powerPotionSprites[2];
        AudioManager.admgTHIS.PlayOneShotASound("DrinkPotionSound");
        GameObject.Find("Canvas/PowerPotionIcon").gameObject.GetComponent<Animator>().SetTrigger("changePotionSpriteAnim");
    }
    private void PowerPotion2()
    {
        // Sa� Alttaki �ksir G�rselini G�nceller, Ses Efekti �al�n�r, Animasyon Tetiklenir.
        powerPotionIcon.sprite = powerPotionSprites[1];
        AudioManager.admgTHIS.PlayOneShotASound("DrinkPotionSound");
        GameObject.Find("Canvas/PowerPotionIcon").gameObject.GetComponent<Animator>().SetTrigger("changePotionSpriteAnim");
    }
    private void PowerPotion1()
    {
        // Sa� Alttaki �ksir G�rselini G�nceller, Ses Efekti �al�n�r, Animasyon Tetiklenir.
        powerPotionIcon.sprite = powerPotionSprites[0];
        AudioManager.admgTHIS.PlayOneShotASound("DrinkPotionSound");
        GameObject.Find("Canvas/PowerPotionIcon").gameObject.GetComponent<Animator>().SetTrigger("changePotionSpriteAnim");
    }
    private IEnumerator PowerPotionIsDone()
    {
        // Oyunun Durdurulmas� Engellenir.
        PotionManager.isTheAnyPotionAnimActive = true;
        pauseBtn.enabled = false;

        // G�� �ksirinin Etkisi Bitmeden �nce Oyun H�z�n� Yava�lat�p H�zland�rma.
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

        // Bir Sonraki �ksirin Ne Zaman Spawn Oluca��n� Belirleme.
        if (PlayerPrefs.GetInt("isCheatModeActive") == 1 && PlayerPrefs.GetInt("isAlwaysPowerPotion") == 1)
            powerPotionSpawnControllerValue = ScoreManager.smTHIS.score + 50;
        else
            powerPotionSpawnControllerValue = ScoreManager.smTHIS.score + 1500;

        // K�rl�k �ksirinin Zamanlay�c�s�n� Aktif Edip �a��rma S�recini Ba�latma.
        StartCoroutine(BlindnessPotionTimer());

        // Son Kez PotionInfo Animasyonunu Tetikler Ve �i�eyi Eski Haline �evirir.
        GameObject.Find("Canvas/PowerPotionIcon").gameObject.GetComponent<Animator>().SetTrigger("changePotionSpriteAnim");
        powerPotionIcon.sprite = potionEmptySprite;

        // Oyun Tekrardan Durdurulabilir.
        pauseBtn.enabled = true;

        // Oyun Mekanikleri Eski Haline D�ner Ve Art�k �ksir �zelli�i Biter.
        PotionManager.didThePlayerDrankPowerPotion = false;
        PotionManager.isTheAnyPotionAnimActive = false;
        PotionManager.isTheAnyPotionActive = false;
        Time.timeScale = 1;
    }
    #endregion


    #region Blindness Potion
    // G�� �ksirinin Aktifli�i Bittikten Sonra [ BlindnessPotionTimer ] Metodu Tetiklenir Ve 12 Saniye Sonra 
    // [ CanBlindnessPotionBeSpawn ] Metodunda Ki If True De�eri D�nd�rerek, K�rl�k �ksirinin Spawn Olmas�n� Sa�lar.
    internal void CanBlindnessPotionBeSpawn()
    {
        // K�rl�k �ksiri Aktif Olmaya Uygun Mu De�il Mi Onu Kontrol Ediyor.
        if (blindnessPotionSpawnControllerValue == true
            && PotionManager.isTheAnyPotionActive == false && PotionManager.didThePlayerDrankPowerPotion == false)
        {
            // Sahnede Bir �ksirin Aktif Oldu�unu Belirtme.
            PotionManager.isTheAnyPotionActive = true;

            // K�rl�k �ksirini �a��rma.
            Instantiate(blindnessPotion);
        }
    }
    internal IEnumerator BlindnessPotionTimer()
    {
        blindnessPotionSpawnControllerValue = false;
        yield return new WaitForSeconds(12.0f);
        blindnessPotionSpawnControllerValue = true;
    }

    // Bu Metot  [ AnimEventConroller ]  Scriptinden Tetikleniyor. Karakterin K�rl�k �ksirini ��me Animasyonu Bitince Bu Metot �al���r.
    internal IEnumerator KeepTheBlindnessPotionActive()
    {
        // Olas� Hatalara Kar�� �nlem Ama�l� Bekleme.
        yield return new WaitForSecondsRealtime(0.1f);

        // Oyuncu Tekrardan Z�playabilir.
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.zero);
        isOnGround = true;

        // Oyunu Geri Ba�latma
        Time.timeScale = 1.0f;

        // �ksirin Kontrol�n� Sa�layabilmek ��in Gerekli De�i�kenler Set Edilir
        PotionManager.isTheAnyPotionAnimActive = false;
        PotionManager.isTheAnyPotionActive = false;
        blindnessPotionSpawnControllerValue = false;

        // S�ras�yla: K�rl�k Efektinin Image'i Aktif Edilir Ve Durdurma Butonu Tekrardan Aktifle�ir.
        gameObject.transform.GetChild(3).gameObject.SetActive(true);
        pauseBtn.enabled = true;



        // Zamanlay�c�y� Ba�latma, 15 Saniye Boyunca Karakter K�r Olucak.
        yield return new WaitForSecondsRealtime(15.0f);

        // 10 Saniye Doldu Ve K�rl�k Efektini Kapatma Animasyonu Tetiklendi.
        blindnessEffect.GetComponent<Animator>().SetTrigger("setDisableBlindnessEffect");
    }
    #endregion


    #region Health Potion
    // Can �ksiri [ healthPotionSpawnControllerValue ] De�i�keni True Oldu�unda Ve Oyuncunun Can� 1 Oldu�unda Spawn Olur.
    // Ba�lang��ta Bu De�i�kenin De�eri True Oldu�u ��in Oyuncunun Can� Azal�r Azalmaz Can �ksiri Spawn Olur. Ancak
    // Daha Sonras�nda [ HealthPotionShortTimer ] Metodu �al��arak De�i�ken 1.5dk Sonra True Olur. E�er Oyuncu �ksiri
    // Ka��r�rsa Ve Alamazsa Bu S�re 2dk Olur. [ HealthPotionLongTimer ] Metodu, [ HealthPotion ] Scriptinden Tetiklenmektedir.
    internal void CanHealthPotionBeSpawn()
    {
        // Bu Metot ScoreManager S�n�f�ndan Tetikleniyor. Ne Zaman Skor De�eri Artarsa
        // G�� �ksiri Aktif Olmaya Uygun Mu De�il Mi Onu Kontrol Ediyor.
        if (health == 1 && healthPotionSpawnControllerValue == true
            && PotionManager.isTheAnyPotionActive == false && PotionManager.didThePlayerDrankPowerPotion == false)
        {
            // Hile Kapal�ysa Controller De�i�kenini False Yap.
            if (PlayerPrefs.GetInt("isCheatModeActive") == 1 && PlayerPrefs.GetInt("isAlwaysHealthPotion") == 0)
                healthPotionSpawnControllerValue = false;

            // Sahnede Bir �ksirin Aktif Oldu�unu Belirtme.
            PotionManager.isTheAnyPotionActive = true;

            // Can �ksirini �a��rma.
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

    // Bu Metot  [ DrinkHealthPotion ]  Animasyonundan Tetikleniyor. Can �ksirini ��me Animasyonu Bitince Bu Metot �al���r.
    private void InstantieHearthAndHealthPlus()
    {
        // Oyuncunun Can�n� Artt�rma Ve Kalp �konunu �a��rma.
        health = 2;
        Instantiate(heart, GameObject.Find("Canvas/Hearts").transform);

        // �ksirin Kontrol�n� Sa�layabilmek ��in Gerekli De�i�kenler Set Edilir.
        PotionManager.isTheAnyPotionAnimActive = false;
        PotionManager.isTheAnyPotionActive = false;

        // Oyunu Durdurma �zelli�ini Geri Aktifle�tirir, �ksir ��ilme Esnas�nda False Oluyor.
        pauseBtn.enabled = true;

        // Oyuncu Tekrardan Z�playabilir.
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.zero);
        isOnGround = true;

        // Oyunu Geri Ba�latma.
        Time.timeScale = 1.0f;
    }
    #endregion
    #endregion

    // Bu Metot Ate� Topunu Beklemeye Sokar Ve Oyuncu Sadece 10 Saniyede Bir Ate� Topu Kullanabilir.
    internal IEnumerator WaitToUsingFireball()
    {
        // Hile A��ksa Ate� Topunu Hileli Versiyona G�re Dizayn Etme.
        if (PlayerPrefs.GetInt("isCheatModeActive") == 1)
        {
            if (PlayerPrefs.GetInt("fireballTimerValue") != 0)
            {
                // De�i�ken Atamalar�.
                Image fireballIcon = GameObject.Find("Canvas/FireballIcon").transform.GetChild(1).GetComponent<Image>();
                fireballIcon.fillAmount = 0.0f;

                // Ate� Topunun Sayac� Aktif Hale Getirilir.
                fireballCounter.gameObject.SetActive(true);

                // Ate� Topunun Bekleme S�resi Kay�t Edilir.
                sbyte fireballTimerValue = (sbyte)PlayerPrefs.GetInt("fireballTimerValue");

                // Bekleme K�sm�. Oyuncunun Hileyle Ayarlad��� S�re Boyunca Beklenilir.
                for (sbyte i = (sbyte)PlayerPrefs.GetInt("fireballTimerValue"); i > 0; i--)
                {
                    // Ate� Topunun Sayac�na Bekleme S�resi Yazd�r�l�r Ve Beklenir.
                    fireballCounter.text = i.ToString();
                    yield return new WaitForSeconds(1.0f);

                    // E�er Oyuncu Ayarlardan Ate� Topunun Bekleme S�resini De�i�tirirse D�ng� K�r�l�r Ve Biter.
                    if (fireballTimerValue != PlayerPrefs.GetInt("fireballTimerValue"))
                        break;
                }

                // Sistem D�ng�den ��kt���nda, Ate� Topunun Bekleme S�resinin De�i�tirilme Sebebiyle ��km��sa A�a��daki Kodlar ��lenmez.
                if (fireballTimerValue == PlayerPrefs.GetInt("fireballTimerValue"))
                {
                    fireballCounter.gameObject.SetActive(false);

                    // Fill Amount De�eri Zaten D�ng�den Sonra 1.0f Oluyor Ama Ben Yinede �nlem Ama�l� Burda Tekrar 1.0f Yapt�m.
                    fireballIcon.fillAmount = 1.0f;

                    // Animasyon Tetiklenir, De�i�ken Set Edilir Ve Art�k Ate� Topu Kullan�labilir.
                    fireballIconAnmtr.SetTrigger("fireballEnable");
                    isTheFireballActive = true;
                }
            }
            else
            {
                // Ate� Topunun Sayac�n� Pasif Hale Getirme.
                fireballCounter.gameObject.SetActive(false);

                // Ate� Topunun Texture FillAmount De�erini Fulleme.
                GameObject.Find("Canvas/FireballIcon").transform.GetChild(1).GetComponent<Image>().fillAmount = 1.0f;

                // Animasyon Tetiklenir, De�i�ken Set Edilir Ve Art�k Ate� Topu Kullan�labilir.
                fireballIconAnmtr.SetTrigger("fireballEnable");
                isTheFireballActive = true;
            }
        }
        // Hile Kapal� Oldu�u ��in Ate� Topunu Normal Bir �ekilde �al��t�r�r.
        else
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
            isTheFireballActive = true;
        }
    }
    #endregion
}