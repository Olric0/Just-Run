using UnityEngine;

public class ForHeart1 : MonoBehaviour
{
    private Animator reyiz;
    public static bool damage = false;

    void Start()
    {
        reyiz = GetComponent<Animator>();
        reyiz.speed = 0;
    }

    void Update()
    {
        if (damage == true)
            reyiz.speed = 1;
    }
}