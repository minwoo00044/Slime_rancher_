using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Silo : MonoBehaviour
{
    private List<GameObject> savedItmePool = new List<GameObject>();

    private void OnCollisionEnter(Collision other)
    {
        other.gameObject.SetActive(false);
        savedItmePool.Add(other.gameObject);
    }
}
