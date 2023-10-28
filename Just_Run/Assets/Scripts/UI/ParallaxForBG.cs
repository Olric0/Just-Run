using UnityEngine;


/// <summary>
/// 
/// Bu Sýnýf Arka Planýn Parallax Olmasýný Saðlýyor. Parallax Nedir Diyecek Olursan,
/// Sanki Oyuncu Ýleriye Doðru Gidiyormuþ Gibi Bir Görüntü Verir. Ama Aslýnda Oyuncu
/// Ýleriye Gitmez, Arkaplan Geriye Gider. Bu Optimizasyondan Büyük Bir Kârdýr.
/// 
/// </summary>
public class ParallaxForBG : MonoBehaviour
{
    [SerializeField] private Transform camTransform;
    [SerializeField] private float parallaxMoveSpeed;



    private void Update()
    {
        // Arka Plandaki Objeyi Sola Doðru Götürme.
        transform.Translate(Vector2.left * parallaxMoveSpeed * Time.deltaTime);

        // Eðer Sona Gelmiþse Tekrar Baþa Döndürme.
        float transformPosX = transform.position.x + 14.4f;
        if (camTransform.position.x >= transformPosX)
            transform.position = new Vector2(camTransform.position.x + 14.4f, transform.position.y);
    }
}