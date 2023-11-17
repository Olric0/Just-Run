using UnityEngine;
using TMPro;


/// <summary>
/// 
/// Oyunun Skor K�sm� Bu S�n�ftan Kontrol Edilir, Skorun Artmas� Veya Azalmas�
/// Sonucunda Olu�acak Olaylar�n Hepsi Bu S�n�ftan Kontrol Edilir.
/// 
/// 
/// >*>*> De�i�kenlerin A��klamalar� <*<*< \\\
///     -> [ scoreAnimObject ]
///     Oyuncunun Skoru Artt���nda Veya Azald���nda Ekran�n Sa� �st�nde ��kan Animasyondur.
///     
///     -> [ eagles ]
///     Oyuncu Bir D��mana �arpt���nda, Oyuncunun Arkas�ndan Oyuncuyu Kovalayan Ku�lard�r.
///     
///     -> [ score ]
///     Oyuncunun Mevcut Skor De�eridir.
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
        // Skoru Artt�r�p, Skor Artma Animasyon Objesini Spawn Etme.
        score += 50;
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
    }
    internal void ScoreMinus()
    {
        if ((PlayerPrefs.GetInt("isCheatModeActive") == 1 && PlayerPrefs.GetInt("isGodModeOn") == 0) || PlayerPrefs.GetInt("isCheatModeActive") == 0)
        {
            // Oyuncunun Skorunu Eksiye D���rmeden Puan�n� Azaltmak ��in Bu Switch Var, Kafan Kar��mas�n.
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

            // Ku� S�r�s�n� Getirme.
            if (GameObject.Find("Eagles(Clone)") == false && Character.chrctrTHIS.health != 0)
            {
                Instantiate(eagles);
                GameObject.Find("Eagles(Clone)").GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("songVolume");
            }
        }
    }
}