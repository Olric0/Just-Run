using UnityEngine;


/// <summary>
/// 
/// Bu Script Canvas'da Olan Körlük Efektinin, Karakteri Takip Etmesine Yarýyor.
/// 
/// </summary>
public class BlindnessEffect : MonoBehaviour
{
    [SerializeField] private Transform characterTransform;
    private void Update() => gameObject.transform.position = new Vector2(transform.position.x, (characterTransform.position.y - 0.35f));
}