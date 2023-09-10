using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarMove : MonoBehaviour
{
    float jumpDaley;
    float moveDaley;
    float rotateDaley;

    public bool onGround = true;

    public float maxJumpHigh =4f;
    public float minJumpHigh = 2f;

    Vector3 jumpBir;

    public float moveSpeed = 0.006f;
    public float moveCount;

    float rotateSize;
    int rightOrLeft = 1;
    Vector3 rotateBir;

    public float findRange = 10f;
    GameObject lookObject;
    GameObject findObject;

    int slimeSize = 1;
    public GameObject tar;
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(slimeSize, slimeSize, slimeSize) * 0.8f;
    }

    // Update is called once per frame
    void Update()
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

        if (lookObject != null)
        {
            Vector3 relativePos = new Vector3(lookObject.transform.position.x - transform.position.x, 0, lookObject.transform.position.z - transform.position.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(relativePos, Vector3.up), Time.deltaTime * 3);

            Move();
        }
        else
        {
            if (!onGround)
            {
                Move();
            }
            else if (moveDaley <= 0 && moveCount >= 0)
            {
                Move();
                moveCount -= Time.deltaTime;
                if (moveCount <= 0)
                {
                    moveCount = Random.Range(3, 10);
                    moveDaley = Random.Range(5, 10);
                }
            }
            else
            {
                moveDaley -= Time.deltaTime;
            }

            if (rotateDaley <= 0)
            {

                if (rotateSize <= 0)
                {
                    rotateSize = Random.Range(0, 180);
                    if (Random.Range(1, 3) % 2 == 0) rightOrLeft *= -1;
                    rotateBir = new Vector3(0, rightOrLeft, 0);
                    rotateDaley = Random.Range(3, 5);
                }

                transform.Rotate(rotateBir);
                rotateSize--;
            }
            else
            {
                rotateDaley -= Time.deltaTime;
            }
        }
        if (onGround)
        {
            jumpDaley -= Time.deltaTime;
            if (jumpDaley < 0)
            {
                Jump();
            }
        }

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
        jumpBir = new Vector3(0, Random.Range(minJumpHigh, maxJumpHigh), 0);

        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.AddForce(jumpBir, ForceMode.Impulse);
        jumpDaley = Random.Range(3, 5);

    }
    private void Move()
    {
        transform.Translate(Vector3.forward * moveSpeed);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Slime")
        {
            collision.gameObject.SetActive(false);
            lookObject = null;
            slimeSize++;
            if (slimeSize == 3)
            {
                GameObject tarGO = Instantiate(tar);
                tarGO.transform.position = transform.position;
                Rigidbody rigidbody = tarGO.GetComponent<Rigidbody>();
                rigidbody.AddForce(Vector3.up * 3, ForceMode.Impulse);
                slimeSize = 1;
            }
            transform.localScale = new Vector3(slimeSize, slimeSize, slimeSize)*0.8f;
        }
    }
}
