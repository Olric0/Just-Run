using UnityEngine;


/// <summary>
/// 
/// Bu Script Canvas'da Olan K�rl�k Efektinin, Karakteri Takip Etmesine Yar�yor.
/// 
/// </summary>
public class BlindnessEffect : MonoBehaviour
{
    [SerializeField] private Transform characterTransform;
    private void Update() => gameObject.transform.position = new Vector2(transform.position.x, (characterTransform.position.y - 0.35f));
}