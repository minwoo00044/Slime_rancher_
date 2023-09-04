using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//목적 : 폭탄이 물체에 부딪히면 폭탄이 이펙트를 만들고 파괴된다.
public class BombAction : MonoBehaviour
{
    public GameObject bombEffect;
    public float explosionRadius = 5f;
    public float power = 5;
    public float staminaReduce;
    public GameObject gunPos;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if(bombEffect != null)
            {
                bombEffect.GetComponent<ParticleSystem>().Play();
            }
            Action();
            Player.Instance.stamina -= staminaReduce;
        }
    }
    private void Action()
    {
        Vector3 pushDirection = gunPos.transform.up;
        Collider[] cols = Physics.OverlapSphere(gunPos.transform.position, explosionRadius, 1 << 6);
        for (int i = 0; i < cols.Length; i++)
        {
            Rigidbody rigidbody = cols[i].GetComponent<Rigidbody>();
            rigidbody.AddForce(pushDirection * power, ForceMode.Impulse);
        }
    }
}
