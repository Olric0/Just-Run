using UnityEngine;
using TMPro;


/// <summary>
/// 
/// Oyunun Skor K�sm� Bu S�n�ftan Kontrol Edilir, Skorun Artmas� Veya Azalmas�
/// Sonucunda Olu�acak Olaylar�n Hepsi Bu S�n�ftan Kontrol Edilir.
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
        // Skoru Artt�r�p, Skor Artma Animasyon Objesini Spawn Etme.
        Character.chrctrTHIS.score += 50;
        Instantiate(scoreAnimObject, transform).GetComponent<TextMeshProUGUI>().color = new Color32(0, 150, 0, 255);

        // Oyuncunun Hileyi A��p A�mad���na G�re �ksirler �a��r�l�r.
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

        // E�er Oyuncu Alt+F4 �le Oyunu Kapat�rsa Ne Olur Ne Olmaz Diye En �yi Skoru Burada Da Set Ediyorum.
        // Evet Performans Kayb� Ama Oyuncunun Elektri�i Gider Veya Bilgisayar�na Bir �ey Olursa Skoru ��p Olmas�n.
        if ((PlayerPrefs.GetInt("isCheatModeActive") == 0) && (Character.chrctrTHIS.score > PlayerPrefs.GetInt("bestScore")))
            PlayerPrefs.SetInt("bestScore", Character.chrctrTHIS.score);
    }
    internal void ScoreMinus()
    {
        if ((PlayerPrefs.GetInt("isCheatModeActive") == 1 && PlayerPrefs.GetInt("isGodModeOn") == 0) || PlayerPrefs.GetInt("isCheatModeActive") == 0)
        {
            // Oyuncunun Skorunu Eksiye D���rmeden Puan�n� Azaltmak ��in Bu Switch Var, Kafan Kar��mas�n.
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

            // Ku� S�r�s�n� Getirme.
            if (GameObject.Find("Eagles(Clone)") == false && Character.chrctrTHIS.health != 0)
            {
                Instantiate(eagles);
                GameObject.Find("Eagles(Clone)").GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("songVolume");
            }
        }
    }
}