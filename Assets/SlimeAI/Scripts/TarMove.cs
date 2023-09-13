using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarMove : MonoBehaviour
{
    float jumpDaley;
    float moveDaley;
    float rotateDaley;

    public bool onGround = true;

    public float maxJumpHigh = 4f;
    public float minJumpHigh = 2f;

    Vector3 jumpBir;

    public float moveSpeed = 0.01f;
    public float moveCount;

    Quaternion lookBir;

    public float findRange = 10f;
    GameObject lookObject;
    GameObject findObject;

    int slimeSize = 2;
    public GameObject tar;

    bool grip = false;
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(slimeSize, slimeSize, slimeSize);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent != null && transform.parent.tag == "Player")
        {
            grip = true;
            return;
        }
        ResetSlime(grip);

        FindObject();

        Move();

        Rotate();

        Jump();


    }

    private void OnTriggerStay(Collider other)
    {
        onGround = true;
    }
    private void OnTriggerExit(Collider other)
    {
        onGround = false;
    }

    private void Jump()
    {
        if (onGround)
        {
            if (jumpDaley < 0)
            {
                jumpBir = new Vector3(transform.forward.x, Random.Range(minJumpHigh, maxJumpHigh), transform.forward.z);
                Rigidbody rigidbody = GetComponent<Rigidbody>();
                rigidbody.AddForce(jumpBir, ForceMode.Impulse);
                jumpDaley = Random.Range(3, 5);
            }
            jumpDaley -= Time.deltaTime;
        }

    }

    private void Move()
    {
            if (moveDaley <= 0 && moveCount >= 0)
            {
                transform.Translate(Vector3.forward * moveSpeed);
                moveCount -= Time.deltaTime;
                if (moveCount <= 0)
                {
                    moveCount = Random.Range(3, 5);
                    moveDaley = Random.Range(10, 20);
                }
            }
        moveDaley -= Time.deltaTime;
    }

    private void Rotate()
    {
        if (lookObject != null)
        {
            lookBir = Quaternion.LookRotation(new Vector3(lookObject.transform.position.x - transform.position.x, 0, lookObject.transform.position.z - transform.position.z), Vector3.up);
            moveDaley = 0;
        }
        else
        {
            if (rotateDaley <= 0)
            {
                lookBir = Quaternion.Euler(0, transform.rotation.eulerAngles.y + Random.Range(-100, 100), 0);

                rotateDaley = Random.Range(3, 10);
            }
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, lookBir, Time.deltaTime * 3);
        rotateDaley -= Time.deltaTime;
    }

    private void FindObject()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, findRange);
        lookObject = null;

        for (int i = 0; i < cols.Length; i++)
        {
            findObject = cols[i].gameObject;
            if (findObject.tag == "Slime")
            {
                if (lookObject == null || (lookObject.transform.position - transform.position).magnitude > (findObject.transform.position - transform.position).magnitude)
                {
                    lookObject = findObject;
                }
            }
        }
    }

    private void ResetSlime(bool call)
    {
        if (call)
        {
            GameObject reset = Instantiate(this.gameObject);
            reset.transform.position = transform.position;
            Rigidbody slimeStert1 = reset.GetComponent<Rigidbody>();
            slimeStert1.freezeRotation = true;
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 4)
        {
            Destroy(this.gameObject);
        }
        if (collision.gameObject.tag == "Slime")
        {
            Destroy(collision.gameObject);
            lookObject = null;
            slimeSize++;
            if (slimeSize == 4)
            {
                GameObject tarGO = Instantiate(tar);
                tarGO.transform.position = transform.position;
                Rigidbody rigidbody = tarGO.GetComponent<Rigidbody>();
                rigidbody.AddForce(Vector3.up * 2, ForceMode.Impulse);
                slimeSize = 2;
            }
            transform.localScale = new Vector3(slimeSize, slimeSize, slimeSize);
        }
    }
}
