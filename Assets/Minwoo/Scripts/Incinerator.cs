using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Incinerator : MonoBehaviour
{
    public GameObject ash;
    public Transform exit;
    public GameObject explosionEff;
    private void OnCollisionEnter(Collision other)
    {
        ParticleSystemManager.Instance.PlayParticle(explosionEff, other.transform);
        foreach(Renderer render in other.transform.GetComponentsInChildren<Renderer>())
        {
            if (render.gameObject.GetComponent<ParticleSystem>() == null)
            {
                // ��ƼŬ �ý����� �ƴ� ��쿡�� ������ ��Ȱ��ȭ
                render.enabled = false;
            }
        }
        Destroy(other.gameObject, explosionEff.GetComponent<ParticleSystem>().main.duration);
        GameObject instance = Instantiate(ash);
        ash.transform.position = exit.position;
    }
}
