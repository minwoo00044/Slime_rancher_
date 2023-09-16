using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleData : MonoBehaviour
{
    public string particleObjectName;
    public bool isManaged;

    private ParticleSystem myParticleSystem;
    private float timer;

    private void OnEnable()
    {
        myParticleSystem = GetComponent<ParticleSystem>();  
        timer = myParticleSystem.main.duration;
        myParticleSystem.Play();
        StartCoroutine(Toggle(timer));
    }

    IEnumerator Toggle(float time)
    {
        yield return new WaitForSeconds(time);
        if(isManaged)
        {
            gameObject.SetActive(false);
            transform.SetParent(null);
        }
        else
            Destroy(gameObject,timer);
    }

}
