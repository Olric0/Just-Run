using UnityEngine;

public class ForHeart0 : MonoBehaviour
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

    public void Delete()
    {
        Destroy(gameObject);
    }
}