using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : MonoBehaviour
{
    float timer;
    public GameObject product;
    bool isHarvested;
    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        if(isHarvested)
        {
            timer += Time.deltaTime;
            if(timer > 720f)
            {
                isHarvested = false;
                timer = 0f;
                Instantiate(product, transform.position, Quaternion.identity);
            }
        }
        
    }
}
