using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ParticleSystemManager : MonoBehaviour
{
    private static ParticleSystemManager instance;

    public static ParticleSystemManager Instance { get { return instance; } }
    private Dictionary<string, GameObject> particleGameObjects = new Dictionary<string, GameObject>();

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
        string particleName = particleObject.GetComponent<ParticleData>().particleObjectName;
        GameObject createdParticle = Instantiate(particleObject, transform);
        createdParticle.transform.position = location.position;
    }
    public void PlayParticleOnce(GameObject particleObject, Transform location)
    {
        string particleName = particleObject.GetComponent<ParticleData>().particleObjectName;
        if(IsParticlePlaying(particleName))
        {

            return;
        }
        else
        {
            if (particleGameObjects.ContainsKey(particleName))
            {
                particleGameObjects[particleName].transform.position = location.position;
                particleGameObjects[particleName].SetActive(true);
            }
            else
            {
                GameObject createdParticle = Instantiate(particleObject, location);
                createdParticle.transform.position = location.position;
                createdParticle.GetComponent<ParticleData>().isManaged = true;
                particleGameObjects.Add(particleName, createdParticle);
            }    
        }

    }

    public bool IsParticlePlaying(string particleName)
    {
        if(particleGameObjects.ContainsKey(particleName) && particleGameObjects[particleName].activeInHierarchy)
        {
            if(particleGameObjects[particleName].GetComponent<ParticleSystem>().isPlaying)
            {
                return true;
            }

        }
        return false;
    }
}
