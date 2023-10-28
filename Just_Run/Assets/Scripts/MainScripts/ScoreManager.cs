using UnityEngine;
using TMPro;


/// <summary>
/// 
/// Oyunun Skor Kýsmý Bu Sýnýftan Kontrol Edilir, Skorun Artmasý Veya Azalmasý
/// Sonucunda Oluþacak Olaylarýn Hepsi Bu Sýnýftan Kontrol Edilir.
/// 
/// </summary>
public class ScoreManager : MonoBehaviour
{
    [Header("Basic")]
    [SerializeField] private GameObject scoreAnimObject;
    [SerializeField] private GameObject eagles;
    public static ScoreManager smTHIS;



    private void Start() => smTHIS = this;
    internal void ScorePlus()
    {
        // Skoru Arttýrýp, Skor Artma Animasyon Objesini Spawn Etme.
        Character.chrctrTHIS.score += 50;
        Instantiate(scoreAnimObject, transform).GetComponent<TextMeshProUGUI>().color = new Color32(0, 150, 0, 255);

        // Oyuncunun Hileyi Açýp Açmadýðýna Göre Ýksirler Çaðýrýlýr.
        if (PlayerPrefs.GetInt("isCheatModeActive") == 1 && PlayerPrefs.GetInt("isWithoutAnyPotions") == 0)
        {
            Character.chrctrTHIS.CanPowerPotionBeSpawn();
            Character.chrctrTHIS.CanHealthPotionBeSpawn();

            if (PlayerPrefs.GetInt("isWithoutBlindnessPotion") == 0)
                Character.chrctrTHIS.CanBlindnessPotionBeSpawn();
        }
        else if (PlayerPrefs.GetInt("isCheatModeActive") == 0)
        {
            Character.chrctrTHIS.CanPowerPotionBeSpawn();
            Character.chrctrTHIS.CanHealthPotionBeSpawn();
            Character.chrctrTHIS.CanBlindnessPotionBeSpawn();
        }

        // Eðer Oyuncu Alt+F4 Ýle Oyunu Kapatýrsa Ne Olur Ne Olmaz Diye En Ýyi Skoru Burada Da Set Ediyorum.
        // Evet Performans Kaybý Ama Oyuncunun Elektriði Gider Veya Bilgisayarýna Bir Þey Olursa Skoru Çöp Olmasýn.
        if ((PlayerPrefs.GetInt("isCheatModeActive") == 0) && (Character.chrctrTHIS.score > PlayerPrefs.GetInt("bestScore")))
            PlayerPrefs.SetInt("bestScore", Character.chrctrTHIS.score);
    }
    internal void ScoreMinus()
    {
        if ((PlayerPrefs.GetInt("isCheatModeActive") == 1 && PlayerPrefs.GetInt("isGodModeOn") == 0) || PlayerPrefs.GetInt("isCheatModeActive") == 0)
        {
            // Oyuncunun Skorunu Eksiye Düþürmeden Puanýný Azaltmak Ýçin Bu Switch Var, Kafan Karýþmasýn.
            switch (Character.chrctrTHIS.score)
            {
                default:
                    Instantiate(scoreAnimObject, transform).GetComponent<TextMeshProUGUI>().text = "-200";
                    Character.chrctrTHIS.score -= 200;
                    break;

                case 150:
                    Instantiate(scoreAnimObject, transform).GetComponent<TextMeshProUGUI>().text = "-150";
                    Character.chrctrTHIS.score -= 150;
                    break;
                case 100:
                    Instantiate(scoreAnimObject, transform).GetComponent<TextMeshProUGUI>().text = "-100";
                    Character.chrctrTHIS.score -= 100;
                    break;
                case 50:
                    Instantiate(scoreAnimObject, transform).GetComponent<TextMeshProUGUI>().text = "-50";
                    Character.chrctrTHIS.score -= 50;
                    break;
                case 0:
                    Instantiate(scoreAnimObject, transform).GetComponent<TextMeshProUGUI>().text = "-0";
                    break;
            }

            // Kuþ Sürüsünü Getirme.
            if (GameObject.Find("Eagles(Clone)") == false && Character.chrctrTHIS.health != 0)
            {
                Instantiate(eagles);
                GameObject.Find("Eagles(Clone)").GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("songVolume");
            }
        }
    }
}