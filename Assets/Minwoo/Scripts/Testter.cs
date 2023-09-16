using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testter : MonoBehaviour
{
    public GameObject test;
    void Update()
    {
        if(Input.GetKey(KeyCode.N))
        {
            ParticleSystemManager.Instance.PlayParticle(test, transform);
        }
        else if (Input.GetKey(KeyCode.M))
        {
            ParticleSystemManager.Instance.PlayParticleOnce(test, transform);
        }
    }
}
