using UnityEngine;

public class HOBirds : MonoBehaviour
{
    public static Animator reyiz;
    public static AudioSource herdOfBirdsSong;

    void Start()
    {
        reyiz = GetComponent<Animator>();
        herdOfBirdsSong = GetComponent<AudioSource>();
    }
}