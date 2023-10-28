using UnityEngine;
using UnityEngine.UI;
using TMPro;


/// <summary>
/// 
/// Bu Sýnýf Ana Menüdeki Play Butonunun Mause Efektlerini Yapýyor.
/// Mantýðý Çok Basit Olduðu Ýçin Açýklama Yapmadým.
/// 
/// </summary>
public class OnMousePlayBtn : MonoBehaviour
{
    [SerializeField] private Sprite redSword;
    [SerializeField] private Sprite greenSword;
    [SerializeField] private Sprite playButton0;
    [SerializeField] private Sprite playButton1;
    [SerializeField] private Image sword0Image;
    [SerializeField] private Image sword1Image;
    [SerializeField] private RectTransform sword0RTrnsfrm;
    [SerializeField] private RectTransform sword1RTrnsfrm;
    [SerializeField] private TextMeshProUGUI buttonText;


    private void OnMouseEnter()
    {
        buttonText.fontSize = 85;
        gameObject.GetComponent<Image>().sprite = playButton1;

        sword0Image.sprite = redSword;
        sword1Image.sprite = redSword;
        sword0RTrnsfrm.anchoredPosition = new Vector2(-282.0f, +18.0f);
        sword1RTrnsfrm.anchoredPosition = new Vector2(+282.0f, +18.0f);

        AudioManager.admgTHIS.PlayOneShotASound("ClickSound2");
    }
    private void OnMouseExit()
    {
        buttonText.fontSize = 90;
        gameObject.GetComponent<Image>().sprite = playButton0;

        sword0Image.sprite = greenSword;
        sword1Image.sprite = greenSword;
        sword0RTrnsfrm.anchoredPosition = new Vector2(-312.0f, +48.0f);
        sword1RTrnsfrm.anchoredPosition = new Vector2(+312.0f, +48.0f);
    }
}