using UnityEngine;


/// <summary>
/// 
/// Bu S�n�f �ksirleri Kontrol Eder. (Power Potion, Blind Potion, Health Potion) �ksirlerin Aktif Olup
/// Olmad���n� Belirleme, �ksir ��me Animasyonunun Aktif Olup Olmad���n� Belirleme, �ksirin ��ilip ��ilmedi�ini
/// Belirleme Ve Herhangi Bir �ksiri Aktif Etme (Animasyonlar�n� Tetikleme vs.) G�revini Bu Script Yapar.
/// 
/// 
/// >*>*> De�i�kenlerin A��klamalar� <*<*< \\\
///     > [ isTheAnyPotionActive ]
///     Herhangi Bir �ksir Aktif Oldu�u S�re Boyunca True'dur. Bak �ksir ��ildi�inde De�il Aktif Oldu�u S�re Boyunca,
///     Kar��t�rma! Herhangi Bir �ksirin Ayn� Anda Birden Fazla Kez Do�mas�na Engel Olur.
///     
///     > [ isTheAnyPotionAnimActive ]
///     Oyuncu Herhangi Bir �ksire Dokunup ��me Animasyonunu Ba�latt��� Anda True Olur, Ve Oyuncunun Oyunu Durdurup
///     Animasyonlar� Buga Sokmas�na Engel Olur. Ayr�ca G�� �ksiri Biterkende True Olur. ��nk� G�� �ksiri Biterken
///     Oyun K�sa S�reli�ine Yava�l�yor. Ve Bu Esnada Oyun Durdurulursa, Oyunun H�z� Buga Girer.
///     
///     > [ didThePlayerDrankPowerPotion ]
///     Oyuncu G�� �ksirini ��me Animasyonunu Bitirip, G�� �ksirini ��tikten Sonra; G�� �ksirinin Efekti Komple Bitene
///     Kadar Aktif Olur. Bu S�re Boyunca Oyuncu Oyunu Durdurursa, Oyun H�z�n� Tekrar G�� �ksirinin �ksirin Efekt H�z�
///     Olan 1.8'den Ba�latmaya Yarar. K�r Etme �ksirinin Bu De�i�kenle Bir Olay� Yok.
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
        // Temel Ayarlamalar, Ba�lang�� ��in Haz�rl�k.
        pmTHIS = this;
        isTheAnyPotionActive = false;
        isTheAnyPotionAnimActive = false;
        didThePlayerDrankPowerPotion = false;
    }
    internal void PotionSpecificSettings(string whatPotionIsThePlayerDrinking)
    {
        // Animasyonlar ��in Oyunu Durdurma.
        Time.timeScale = 0.0f;

        // �ksir De�i�kenlerini Ayarlama.
        isTheAnyPotionAnimActive = true;

        // Durdurma Butonunu Devre D��� B�rakma.
        GameObject.Find("Canvas/PauseBTN").GetComponent<UnityEngine.UI.Button>().enabled = false;

        // Oyuncunun Z�plamas�n� Engelleme, Pozisyonunu D�zeltme Ve Y�r�me Animasyonunu Kapatma.
        Character.chrctrTHIS.isOnGround = false;
        GameObject.Find("Character").transform.position = new Vector2(-1.2f, -0.74f);
        GameObject.Find("Character").transform.GetChild(3).gameObject.GetComponent<SpriteRenderer>().enabled = false;

        // E�er Oyuncu �ksiri ��erek Ate� Topu Atmaya �al���rsa Ate� Topunu Siler.
        if (GameObject.Find("Fireball(Clone)") == true)
        {
            AudioSource soundManager = GameObject.Find("AudioManager/SoundManager").GetComponent<AudioSource>();
            soundManager.enabled = false;
            soundManager.enabled = true;
            Destroy(GameObject.Find("Fireball(Clone)"));
        }

        // Hangi �ksirin ��ildi�ini Alg�lama Ve Gerekli Animasyonlar� Ba�latma.
        switch (whatPotionIsThePlayerDrinking.ToLower())
        {
            case "power":
            case "powerpotion":
                // G�� �ksiri ��me Animasyonunu Ba�latma.
                Character.chrctrTHIS.characterANMTR.Play("DrinkPowerPotion");

                // Ba�lat�lan Animasyonu Desteklemek ��in M�zik Efekti Ve Kamera Efektininin Animasyonlar�n� Etkinle�tirme.
                GameObject.Find("MainCamera").GetComponent<Animator>().SetTrigger("camShakeForPowerPotion");
                GameObject.Find("AudioManager").transform.GetChild(0).GetComponent<Animator>().SetTrigger("songEffectForPowerPotion");
                break;

            case "blindness":
            case "blindnesspotion":
                // K�rl�k �ksirini ��me Animasyonunu Ba�latma.
                GameObject.Find("Canvas").transform.GetChild(6).gameObject.SetActive(true);
                Character.chrctrTHIS.characterANMTR.Play("DrinkBlindnessPotion");

                // Ba�lat�lan Animasyonu Desteklemek ��in M�zik Efekti Ve Kamera Efektininin Animasyonlar�n� Etkinle�tirme.
                GameObject.Find("MainCamera").GetComponent<Animator>().SetTrigger("camShakeForBlindnessPotion");
                GameObject.Find("AudioManager").transform.GetChild(0).GetComponent<Animator>().SetTrigger("songEffectForBlindnessPotion");
                AudioManager.admgTHIS.PlayOneShotASound("DrinkBlindnessPotionSound");
                break;

            case "health":
            case "healthpotion":
                // Can �ksirini ��me Animasyonunu Ba�latma.
                Character.chrctrTHIS.characterANMTR.Play("DrinkHealthPotion");

                // Ba�lat�lan Animasyonu Desteklemek ��in M�zik Efekti Ve Kamera Efektininin Animasyonlar�n� Etkinle�tirme.
                GameObject.Find("MainCamera").GetComponent<Animator>().SetTrigger("camShakeForHealthPotion");
                AudioManager.admgTHIS.PlayOneShotASound("DrinkHealthPotionSound");
                break;
        }
    }
}