using UnityEngine;


/// <summary>
/// 
/// Bu S�n�f Zeminin Parallax Olmas�n� Sa�l�yor. Parallax Nedir Diyecek Olursan,
/// Sanki Oyuncu �leriye Do�ru Gidiyormu� Gibi Bir G�r�nt� Verir. Ama Asl�nda Oyuncu
/// �leriye Gitmez, Zemin Geriye Gider. Bu Optimizasyondan B�y�k Bir K�rd�r.
/// 
/// </summary>
public class ParallaxForGRND : MonoBehaviour
{
    [SerializeField] private Transform camTransform;

    private void Update()
    {
        // Zemini Sola Do�ru G�t�rme.
        transform.Translate(Vector2.left * 5.7f * Time.deltaTime);

        // E�er Sona Gelmi�se Tekrar Ba�a D�nd�rme.
        float transformPosX = transform.position.x + 14.91f;
        if (camTransform.position.x >= transformPosX)
            transform.position = new Vector2(camTransform.position.x + 14.91f, transform.position.y);
    }
}