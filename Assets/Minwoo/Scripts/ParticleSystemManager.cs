using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ParticleSystemManager : MonoBehaviour
{
    private static ParticleSystemManager instance;

    public static ParticleSystemManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayParticle(GameObject particleObject, Transform location)
    {
        GameObject createdParticle = Instantiate(particleObject, location);
        ParticleSystem particleSystem = createdParticle.GetComponent<ParticleSystem>();
        Destroy(createdParticle, particleSystem.main.duration);
    }
    public bool IsParticlePlaying(GameObject particleObject)
    {
        ParticleSystem particleSystem = particleObject.GetComponent<ParticleSystem>();
        if (particleSystem != null)
        {
            return particleSystem.isPlaying;
        }
        return false;
    }
}
