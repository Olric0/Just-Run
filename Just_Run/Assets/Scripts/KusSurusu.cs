using UnityEngine;

public class KusSurusu : MonoBehaviour
{
    public static Animator reyiz;
    public static AudioSource herdOfBirdsSong;

    void Start()
    {
        reyiz = GetComponent<Animator>();
        herdOfBirdsSong = GetComponent<AudioSource>();
    }

    // Animasyon Bitince Durur
    public void False()
    {
        reyiz.SetBool("move", false);
    }
    // *_*
}