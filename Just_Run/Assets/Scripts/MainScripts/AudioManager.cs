using UnityEngine;


/// <summary>
/// 
/// Bu Sýnýf Oyunun Ses Kýsmýný Kontrol Ediyor. Bir Ses Efekti Çalýnacaðý Zaman X Script'inden Bu Script'e Ulaþýp
/// [ PlayOneShotASound ] Metotunu Çalýþtýrabilirsin. String Bir Þekilde Ses Efektinin Adýný Ýstiyor Senden, Büyük
/// Küçük Harf Önemli Deðil Sadece Ayný Harfleri Yaz. Deðiþkenlerin Açýklamalarýný Yazmadým Çünkü Ne Olduklarý Belli.
/// 
/// </summary>
public class AudioManager : MonoBehaviour
{
    [Header("Main Variables")]
    [SerializeField] private AudioSource soundManagerAudioSRC;
    public static AudioManager admgTHIS;

    [Header("Recorded Sounds")]
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip bumpSound;
    [SerializeField] private AudioClip hitObjectSound;
    [SerializeField] private AudioClip drinkPotionSound;
    [SerializeField] private AudioClip drinkBlindnessPotionSound;
    [SerializeField] private AudioClip drinkHealthPotionSound;
    [SerializeField] private AudioClip fireballSound;
    [SerializeField] private AudioClip explosionSound1;
    [SerializeField] private AudioClip explosionSound2;
    [SerializeField] private AudioClip clickSound1;
    [SerializeField] private AudioClip clickSound2;
    [SerializeField] private AudioClip errorSound;



    private void Start() => admgTHIS = this;
    internal void PlayOneShotASound(string soundName)
    {
        switch (soundName.ToLower())
        {
            case "jumpsound":
                soundManagerAudioSRC.PlayOneShot(jumpSound);
                break;
            case "bumpsound":
                soundManagerAudioSRC.PlayOneShot(bumpSound);
                break;
            case "hitobjectsound":
                soundManagerAudioSRC.PlayOneShot(hitObjectSound);
                break;
            case "drinkpotionsound":
                soundManagerAudioSRC.PlayOneShot(drinkPotionSound);
                break;
            case "drinkblindnesspotionsound":
                soundManagerAudioSRC.PlayOneShot(drinkBlindnessPotionSound);
                break;
            case "drinkhealthpotionsound":
                soundManagerAudioSRC.PlayOneShot(drinkHealthPotionSound);
                break;
            case "fireballsound":
                soundManagerAudioSRC.PlayOneShot(fireballSound);
                break;
            case "explosionsound1":
                soundManagerAudioSRC.PlayOneShot(explosionSound1);
                break;
            case "explosionsound2":
                soundManagerAudioSRC.PlayOneShot(explosionSound2);
                break;
            case "clicksound1":
                soundManagerAudioSRC.PlayOneShot(clickSound1);
                break;
            case "clicksound2":
                soundManagerAudioSRC.PlayOneShot(clickSound2);
                break;
            case "errorsound":
                soundManagerAudioSRC.PlayOneShot(errorSound);
                break;
        }
    }
}