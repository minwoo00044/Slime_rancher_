using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tear : MonoBehaviour
{
    public GameObject crashEff;
    private void OnCollisionEnter(Collision other)
    {
        if(!other.collider.CompareTag("Player"))
        {
            ParticleSystemManager.Instance.PlayParticleOnce(crashEff, transform);
            transform.GetChild(0).SetParent(null);
            Destroy(gameObject);
        }
    }
}
