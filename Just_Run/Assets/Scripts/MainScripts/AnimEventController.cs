using UnityEngine;


/// <summary>
/// 
/// Bu Script Sadece Animasyonlar�n Eventlar�na Metot Atamak ��in Kullan�l�r. Ba�ka Bir Yerde Kullan�lm�yor.
/// 
/// </summary>
public class AnimEventController : MonoBehaviour
{
    [SerializeField] private GameObject fireball;


    // Bu Metot [ ScorePlus ve ScoreMinus ] Anim Objelerinde Tetikleniyor.
    public void CheckNewScore()
    {
        Character.chrctrTHIS.SetNewScore();
        Destroy(this.gameObject);
    }

    // Bu Metot Ate� Topunu Spawn Eder. Sadece Karakterde Vard�r, [ ThrowTheFireball ve ThrowTheFireballForTutorial ] Animasyonlar�nda Tetiklenir.
    public void InstantieFireball() => Instantiate(fireball, GameObject.Find("Character").transform);

    // K�rl�k �ksirinin ��ilme Animasyonu Bittikten Sonra, K�rl�k Efekti Aktif Tutmak ��in Vard�r. Bu Metot Sadece DrinkBlindnessPotion Animation'unda Tetiklenir.
    public void FinishBlindnessPotionEffect() => Character.chrctrTHIS.StartCoroutine(Character.chrctrTHIS.KeepTheBlindnessPotionActive());

    // Bu Metot �o�u Objede Var, Bir Animasyon Olup Bittikten Sonra Objenin Yok Olmas�n� Tetikliyor. Ad� �st�nde Zaten, Anlam��s�nd�r.
    public void DestroyYourSelf() => Destroy(this.gameObject);

    // Bu Metot �o�u Objede Var, Bir Animasyon Olup Bittikten Sonra Objenin Pasif Olmas�n� Sa�l�yor. Ad� �st�nde Zaten, Anlam��s�nd�r.
    public void SetActiveFalse() => gameObject.SetActive(false);
}