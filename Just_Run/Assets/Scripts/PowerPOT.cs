using UnityEngine;

public class PowerPOT : MonoBehaviour
{
    public GameObject hitBox1;
    public GameObject hitBox2;
    public GameObject potionSprites;
    public SpriteRenderer spriteRen;
    public static bool isActive;
    public static bool done;


    void Update()
    {
        if (isActive == true)
        {
            transform.Translate(Vector2.left * 6 * Time.deltaTime);
        }
        else
        {
            hitBox1.SetActive(true);
            hitBox2.SetActive(false);
            if (done == true)
            {
                Destroy(hitBox2);
                Destroy(potionSprites);
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D temas)
    {
        if (temas.gameObject.name == "HitBox1")
        {
            hitBox1.SetActive(false);
            hitBox2.SetActive(true);
            spriteRen.enabled = false;


            Main.reyiz0.SetBool("isDrink", true);
            Time.timeScale = 0;
        }
        else
        {
            Main.reyiz0.SetBool("isDrink", false);
        }
    }
}