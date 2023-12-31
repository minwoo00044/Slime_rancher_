using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Slime" || other.gameObject.tag == "Tar" || other.gameObject.tag == "Item" || other.gameObject.tag == "Food")
        {
            other.gameObject.layer = 0;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Slime"||other.gameObject.tag == "Tar"|| other.gameObject.tag == "Item"|| other.gameObject.tag == "Food")
        {
            Rigidbody rigidbody = other.gameObject.GetComponent<Rigidbody>();
            rigidbody.AddForce(transform.forward *3, ForceMode.Force);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Slime" || other.gameObject.tag == "Tar" || other.gameObject.tag == "Item" || other.gameObject.tag == "Food")
        {
            other.gameObject.layer = 6;
        }
    }
}
