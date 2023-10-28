using UnityEngine;

public class ForSP0 : MonoBehaviour
{
    public GameObject scorPlus;
    public static bool durum = false;


    void Update()
    {
        if (durum == true)
        {
            Instantiate(scorPlus, transform);
            durum = false;
        }
    }
}