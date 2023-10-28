using UnityEngine;


/// <summary>
/// 
/// Bu Sýnýf Zeminin Parallax Olmasýný Saðlýyor. Parallax Nedir Diyecek Olursan,
/// Sanki Oyuncu Ýleriye Doðru Gidiyormuþ Gibi Bir Görüntü Verir. Ama Aslýnda Oyuncu
/// Ýleriye Gitmez, Zemin Geriye Gider. Bu Optimizasyondan Büyük Bir Kârdýr.
/// 
/// </summary>
public class ParallaxForGRND : MonoBehaviour
{
    [SerializeField] private Transform camTransform;

    private void Update()
    {
        // Zemini Sola Doðru Götürme.
        transform.Translate(Vector2.left * 5.7f * Time.deltaTime);

        // Eðer Sona Gelmiþse Tekrar Baþa Döndürme.
        float transformPosX = transform.position.x + 14.91f;
        if (camTransform.position.x >= transformPosX)
            transform.position = new Vector2(camTransform.position.x + 14.91f, transform.position.y);
    }
}