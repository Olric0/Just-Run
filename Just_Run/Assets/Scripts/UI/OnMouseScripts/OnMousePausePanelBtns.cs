using UnityEngine;


/// <summary>
/// 
/// Bu S�n�f StopPanel'deki Butunlar�n Yukar� A�a�� Gidip Gelme Efektini Yap�yor.
/// Mant��� �ok Basit Oldu�u ��in A��klama Yapmad�m.
/// 
/// </summary>
public class OnMousePausePanelBtns : MonoBehaviour
{
    private float xPos;
    private void Start() => xPos = this.gameObject.GetComponent<RectTransform>().anchoredPosition.x;

    private void OnMouseEnter()
    {
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(xPos, 190.0f);
        gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0.0f, -21.0f);
        gameObject.GetComponent<BoxCollider2D>().size = new Vector2(120.0f, 170.0f);
        AudioManager.admgTHIS.PlayOneShotASound("ClickSound1");
    }
    private void OnMouseExit()
    {
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(xPos, 160.0f);
        gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0.0f, 0.0f);
        gameObject.GetComponent<BoxCollider2D>().size = new Vector2(120.0f, 127.0f);
    }
    private void OnDisable()
    {
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(xPos, 160.0f);
        gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0.0f, -32.0f);
        gameObject.GetComponent<BoxCollider2D>().size = new Vector2(120.0f, 192.0f);
    }
}