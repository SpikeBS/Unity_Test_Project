using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gun_behavior : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    // Start is called before the first frame update
    void Start()
    {
        GlobalEventManager.OnFire += fire;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void fire() 
    {
        GameObject clone = Instantiate(bullet, transform.position, Quaternion.identity);
        if (gameObject.transform.rotation == Quaternion.Euler(0,0,0))
        clone.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 20f, ForceMode2D.Impulse);
        else clone.GetComponent<Rigidbody2D>().AddForce(Vector2.left * 20f, ForceMode2D.Impulse);
        Invoke("setInvisible", 0.5f);
    }

    void setInvisible() 
    {
    gameObject.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
    }
}
