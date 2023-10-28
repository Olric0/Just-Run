using UnityEngine;


/// <summary>
/// 
/// Bu S�n�f Oyunun Ses K�sm�n� Kontrol Ediyor. Bir Ses Efekti �al�naca�� Zaman X Script'inden Bu Script'e Ula��p
/// [ PlayOneShotASound ] Metotunu �al��t�rabilirsin. String Bir �ekilde Ses Efektinin Ad�n� �stiyor Senden, B�y�k
/// K���k Harf �nemli De�il Sadece Ayn� Harfleri Yaz. De�i�kenlerin A��klamalar�n� Yazmad�m ��nk� Ne Olduklar� Belli.
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