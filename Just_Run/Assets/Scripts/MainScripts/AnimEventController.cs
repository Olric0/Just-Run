using UnityEngine;


/// <summary>
/// 
/// Bu Script Sadece Animasyonlarýn Eventlarýna Metot Atamak Ýçin Kullanýlýr. Baþka Bir Yerde Kullanýlmýyor.
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

    // Bu Metot Ateþ Topunu Spawn Eder. Sadece Karakterde Vardýr, [ ThrowTheFireball ve ThrowTheFireballForTutorial ] Animasyonlarýnda Tetiklenir.
    public void InstantieFireball() => Instantiate(fireball, GameObject.Find("Character").transform);

    // Körlük Ýksirinin Ýçilme Animasyonu Bittikten Sonra, Körlük Efekti Aktif Tutmak Ýçin Vardýr. Bu Metot Sadece DrinkBlindnessPotion Animation'unda Tetiklenir.
    public void FinishBlindnessPotionEffect() => Character.chrctrTHIS.StartCoroutine(Character.chrctrTHIS.KeepTheBlindnessPotionActive());

    // Bu Metot Çoðu Objede Var, Bir Animasyon Olup Bittikten Sonra Objenin Yok Olmasýný Tetikliyor. Adý Üstünde Zaten, Anlamýþsýndýr.
    public void DestroyYourSelf() => Destroy(this.gameObject);

    // Bu Metot Çoðu Objede Var, Bir Animasyon Olup Bittikten Sonra Objenin Pasif Olmasýný Saðlýyor. Adý Üstünde Zaten, Anlamýþsýndýr.
    public void SetActiveFalse() => gameObject.SetActive(false);
}