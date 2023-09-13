using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//목적 : 폭탄이 물체에 부딪히면 폭탄이 이펙트를 만들고 파괴된다.
public class BombAction : MonoBehaviour
{
    public GameObject bombEffect;
    public float explosionRadius = 5f;
    public float power = 5;
    public float additionalUpwardForce = 3f;
    public float staminaReduce;
    public GameObject gunPos;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && Player.Instance.isEquipImpact)
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
        Collider[] cols = Physics.OverlapSphere(gunPos.transform.position, explosionRadius, 1 << 6);

        foreach (Collider col in cols)
        {
            Rigidbody rigidbody = col.GetComponent<Rigidbody>();

            if (rigidbody != null)
            {
                // 물체와 폭발 중심 간의 상대 위치를 계산
                Vector3 pushDirection = col.transform.position - gunPos.transform.position;

                // 살짝 위로 튕기도록 상승 힘을 추가
                Vector3 upwardForce = Vector3.up * additionalUpwardForce;

                // 물체를 밀어내는 힘을 적용
                rigidbody.AddForce((pushDirection.normalized * power) + upwardForce, ForceMode.Impulse);
            }
        }
    }


}
