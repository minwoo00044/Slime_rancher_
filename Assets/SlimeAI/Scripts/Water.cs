using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up * 2, ForceMode.Force);
        rb.useGravity = false;

    }
    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        rb.useGravity = true;
    }
    private void OnTriggerEnter(Collider other)
    {

        Rigidbody rb = other.GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up, ForceMode.VelocityChange);

    }
}
