using UnityEngine;


/// <summary>
/// 
/// Bu S�n�f Arka Plan�n Parallax Olmas�n� Sa�l�yor. Parallax Nedir Diyecek Olursan,
/// Sanki Oyuncu �leriye Do�ru Gidiyormu� Gibi Bir G�r�nt� Verir. Ama Asl�nda Oyuncu
/// �leriye Gitmez, Arkaplan Geriye Gider. Bu Optimizasyondan B�y�k Bir K�rd�r.
/// 
/// </summary>
public class ParallaxForBG : MonoBehaviour
{
    [SerializeField] private Transform camTransform;
    [SerializeField] private float parallaxMoveSpeed;



    private void Update()
    {
        // Arka Plandaki Objeyi Sola Do�ru G�t�rme.
        transform.Translate(Vector2.left * parallaxMoveSpeed * Time.deltaTime);

        // E�er Sona Gelmi�se Tekrar Ba�a D�nd�rme.
        float transformPosX = transform.position.x + 14.4f;
        if (camTransform.position.x >= transformPosX)
            transform.position = new Vector2(camTransform.position.x + 14.4f, transform.position.y);
    }
}