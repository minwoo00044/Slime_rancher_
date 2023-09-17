using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tear : MonoBehaviour
{
    public GameObject crashEff;
    private void OnCollisionEnter(Collision other)
    {
        GameObject target = other.gameObject;
        if(!other.collider.gameObject.CompareTag("Player") && !other.collider.gameObject.CompareTag("Water"))
        {
            print(other.gameObject.name);
            ParticleSystemManager.Instance.PlayParticle(crashEff, transform);
            Destroy(gameObject, crashEff.GetComponent<ParticleSystem>().main.duration);
        }
    }
}
