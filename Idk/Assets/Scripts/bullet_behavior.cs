using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet_behavior : MonoBehaviour
{
    // Start is called before the first frame update
    float cooldown = 1f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cooldown -= Time.deltaTime;
        if (cooldown <= 0) Destroy(gameObject);
    }

}
