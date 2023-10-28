using UnityEngine;


/// <summary>
/// 
/// Bu Sýnýf Ana Menüdeki Social Media URL Butonlarýnýn (Instagram, LinkedIn, Github) Mause Efektlerini Yapýyor.
/// Mantýðý Çok Basit Olduðu Ýçin Açýklama Yapmadým.
/// 
/// </summary>
public class OnMouseUrlBtn : MonoBehaviour
{
    [SerializeField] private RectTransform[] buttonRectTransforms = new RectTransform[3];
    [SerializeField] private sbyte buttonNumber;

    private void OnMouseEnter()
    {
        buttonRectTransforms[buttonNumber].localScale = new Vector2(1.4f, 1.4f);
        switch (buttonNumber)
        {
            case 0:
                buttonRectTransforms[1].anchoredPosition = new Vector2(20.0f, 50.0f);
                buttonRectTransforms[2].anchoredPosition = new Vector2(120.0f, 50.0f);
                break;
            case 1:
                buttonRectTransforms[0].anchoredPosition = new Vector2(-120.0f, 50.0f);
                buttonRectTransforms[2].anchoredPosition = new Vector2(120.0f, 50.0f);
                break;
            case 2:
                buttonRectTransforms[0].anchoredPosition = new Vector2(-120.0f, 50.0f);
                buttonRectTransforms[1].anchoredPosition = new Vector2(-20.0f, 50.0f);
                break;
        }
    }
    private void OnMouseExit()
    {
        for (sbyte i = 0; i < buttonRectTransforms.Length; i++)
        {
            buttonRectTransforms[i].localScale = new Vector2(1.0f, 1.0f);
            switch (i)
            {
                case 0:
                    buttonRectTransforms[i].anchoredPosition = new Vector2(-100.0f, 50.0f);
                    break;
                case 1:
                    buttonRectTransforms[i].anchoredPosition = new Vector2(0.0f, 50.0f);
                    break;
                case 2:
                    buttonRectTransforms[i].anchoredPosition = new Vector2(100.0f, 50.0f);
                    break;
            }
        }
    }
}