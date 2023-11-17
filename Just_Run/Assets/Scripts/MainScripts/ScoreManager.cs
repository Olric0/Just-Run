using UnityEngine;
using TMPro;


/// <summary>
/// 
/// Oyunun Skor Kýsmý Bu Sýnýftan Kontrol Edilir, Skorun Artmasý Veya Azalmasý
/// Sonucunda Oluþacak Olaylarýn Hepsi Bu Sýnýftan Kontrol Edilir.
/// 
/// 
/// >*>*> Deðiþkenlerin Açýklamalarý <*<*< \\\
///     -> [ scoreAnimObject ]
///     Oyuncunun Skoru Arttýðýnda Veya Azaldýðýnda Ekranýn Sað Üstünde Çýkan Animasyondur.
///     
///     -> [ eagles ]
///     Oyuncu Bir Düþmana Çarptýðýnda, Oyuncunun Arkasýndan Oyuncuyu Kovalayan Kuþlardýr.
///     
///     -> [ score ]
///     Oyuncunun Mevcut Skor Deðeridir.
///     
/// </summary>
public class ScoreManager : MonoBehaviour
{
    [SerializeField] private GameObject scoreAnimObject;
    [SerializeField] private GameObject eagles;
    public static ScoreManager smTHIS;
    internal int score;


    private void Start() => smTHIS = this;
    internal void ScorePlus()
    {
        // Skoru Arttýrýp, Skor Artma Animasyon Objesini Spawn Etme.
        score += 50;
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
    }
    internal void ScoreMinus()
    {
        if ((PlayerPrefs.GetInt("isCheatModeActive") == 1 && PlayerPrefs.GetInt("isGodModeOn") == 0) || PlayerPrefs.GetInt("isCheatModeActive") == 0)
        {
            // Oyuncunun Skorunu Eksiye Düþürmeden Puanýný Azaltmak Ýçin Bu Switch Var, Kafan Karýþmasýn.
            switch (score)
            {
                default:
                    score -= 200;
                    Instantiate(scoreAnimObject, transform).GetComponent<TextMeshProUGUI>().text = "-200";
                    break;

                case 150:
                    score -= 150;
                    Instantiate(scoreAnimObject, transform).GetComponent<TextMeshProUGUI>().text = "-150";
                    break;
                case 100:
                    score -= 100;
                    Instantiate(scoreAnimObject, transform).GetComponent<TextMeshProUGUI>().text = "-100";
                    break;
                case 50:
                    score -= 50;
                    Instantiate(scoreAnimObject, transform).GetComponent<TextMeshProUGUI>().text = "-50";
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