using UnityEngine;


/// <summary>
/// 
/// Bu Sýnýf Ýksirleri Kontrol Eder. (Power Potion, Blind Potion, Health Potion) Ýksirlerin Aktif Olup
/// Olmadýðýný Belirleme, Ýksir Ýçme Animasyonunun Aktif Olup Olmadýðýný Belirleme, Ýksirin Ýçilip Ýçilmediðini
/// Belirleme Ve Herhangi Bir Ýksiri Aktif Etme (Animasyonlarýný Tetikleme vs.) Görevini Bu Script Yapar.
/// 
/// 
/// >*>*> Deðiþkenlerin Açýklamalarý <*<*< \\\
///     > [ isTheAnyPotionActive ]
///     Herhangi Bir Ýksir Aktif Olduðu Süre Boyunca True'dur. Bak Ýksir Ýçildiðinde Deðil Aktif Olduðu Süre Boyunca,
///     Karýþtýrma! Herhangi Bir Ýksirin Ayný Anda Birden Fazla Kez Doðmasýna Engel Olur.
///     
///     > [ isTheAnyPotionAnimActive ]
///     Oyuncu Herhangi Bir Ýksire Dokunup Ýçme Animasyonunu Baþlattýðý Anda True Olur, Ve Oyuncunun Oyunu Durdurup
///     Animasyonlarý Buga Sokmasýna Engel Olur. Ayrýca Güç Ýksiri Biterkende True Olur. Çünkü Güç Ýksiri Biterken
///     Oyun Kýsa Süreliðine Yavaþlýyor. Ve Bu Esnada Oyun Durdurulursa, Oyunun Hýzý Buga Girer.
///     
///     > [ didThePlayerDrankPowerPotion ]
///     Oyuncu Güç Ýksirini Ýçme Animasyonunu Bitirip, Güç Ýksirini Ýçtikten Sonra; Güç Ýksirinin Efekti Komple Bitene
///     Kadar Aktif Olur. Bu Süre Boyunca Oyuncu Oyunu Durdurursa, Oyun Hýzýný Tekrar Güç Ýksirinin Ýksirin Efekt Hýzý
///     Olan 1.8'den Baþlatmaya Yarar. Kör Etme Ýksirinin Bu Deðiþkenle Bir Olayý Yok.
///     
/// </summary>
public class PotionManager : MonoBehaviour
{
    public static PotionManager pmTHIS;
    public static bool isTheAnyPotionActive;
    public static bool isTheAnyPotionAnimActive;
    public static bool didThePlayerDrankPowerPotion;


    private void Start()
    {
        // Temel Ayarlamalar, Baþlangýç Ýçin Hazýrlýk.
        pmTHIS = this;
        isTheAnyPotionActive = false;
        isTheAnyPotionAnimActive = false;
        didThePlayerDrankPowerPotion = false;
    }
    internal void PotionSpecificSettings(string whatPotionIsThePlayerDrinking)
    {
        // Animasyonlar Ýçin Oyunu Durdurma.
        Time.timeScale = 0.0f;

        // Ýksir Deðiþkenlerini Ayarlama.
        isTheAnyPotionAnimActive = true;

        // Durdurma Butonunu Devre Dýþý Býrakma.
        GameObject.Find("Canvas/PauseBTN").GetComponent<UnityEngine.UI.Button>().enabled = false;

        // Oyuncunun Zýplamasýný Engelleme, Pozisyonunu Düzeltme Ve Yürüme Animasyonunu Kapatma.
        Character.chrctrTHIS.isOnGround = false;
        GameObject.Find("Character").transform.position = new Vector2(-1.2f, -0.74f);
        GameObject.Find("Character").transform.GetChild(3).gameObject.GetComponent<SpriteRenderer>().enabled = false;

        // Eðer Oyuncu Ýksiri Ýçerek Ateþ Topu Atmaya Çalýþýrsa Ateþ Topunu Siler.
        if (GameObject.Find("Fireball(Clone)") == true)
        {
            AudioSource soundManager = GameObject.Find("AudioManager/SoundManager").GetComponent<AudioSource>();
            soundManager.enabled = false;
            soundManager.enabled = true;
            Destroy(GameObject.Find("Fireball(Clone)"));
        }

        // Hangi Ýksirin Ýçildiðini Algýlama Ve Gerekli Animasyonlarý Baþlatma.
        switch (whatPotionIsThePlayerDrinking.ToLower())
        {
            case "power":
            case "powerpotion":
                // Güç Ýksiri Ýçme Animasyonunu Baþlatma.
                Character.chrctrTHIS.characterANMTR.Play("DrinkPowerPotion");

                // Baþlatýlan Animasyonu Desteklemek Ýçin Müzik Efekti Ve Kamera Efektininin Animasyonlarýný Etkinleþtirme.
                GameObject.Find("MainCamera").GetComponent<Animator>().SetTrigger("camShakeForPowerPotion");
                GameObject.Find("AudioManager").transform.GetChild(0).GetComponent<Animator>().SetTrigger("songEffectForPowerPotion");
                break;

            case "blindness":
            case "blindnesspotion":
                // Körlük Ýksirini Ýçme Animasyonunu Baþlatma.
                GameObject.Find("Canvas").transform.GetChild(6).gameObject.SetActive(true);
                Character.chrctrTHIS.characterANMTR.Play("DrinkBlindnessPotion");

                // Baþlatýlan Animasyonu Desteklemek Ýçin Müzik Efekti Ve Kamera Efektininin Animasyonlarýný Etkinleþtirme.
                GameObject.Find("MainCamera").GetComponent<Animator>().SetTrigger("camShakeForBlindnessPotion");
                GameObject.Find("AudioManager").transform.GetChild(0).GetComponent<Animator>().SetTrigger("songEffectForBlindnessPotion");
                AudioManager.admgTHIS.PlayOneShotASound("DrinkBlindnessPotionSound");
                break;

            case "health":
            case "healthpotion":
                // Can Ýksirini Ýçme Animasyonunu Baþlatma.
                Character.chrctrTHIS.characterANMTR.Play("DrinkHealthPotion");

                // Baþlatýlan Animasyonu Desteklemek Ýçin Müzik Efekti Ve Kamera Efektininin Animasyonlarýný Etkinleþtirme.
                GameObject.Find("MainCamera").GetComponent<Animator>().SetTrigger("camShakeForHealthPotion");
                AudioManager.admgTHIS.PlayOneShotASound("DrinkHealthPotionSound");
                break;
        }
    }
}