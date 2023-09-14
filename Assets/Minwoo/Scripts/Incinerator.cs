using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Incinerator : MonoBehaviour
{
    public GameObject ash;
    public Transform exit;
    private void OnCollisionEnter(Collision other)
    {
        Destroy(other.gameObject);
        GameObject instance = Instantiate(ash);
        ash.transform.position = exit.position;
    }
}
