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

    public float moveSpeed = 0.01f;
    public float moveCount;

    float rotateSize;
    int rightOrLeft = 1;
    Vector3 rotateBir;

    public float findRange = 10f;
    GameObject lookObject;
    GameObject findObject;

    int slimeSize = 2;
    public GameObject tar;

    bool scenePos = false;
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(slimeSize, slimeSize, slimeSize);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent != null)
            if (transform.parent.tag == "Player")
            {
                scenePos = true;
                return;
            }

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

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0), Time.deltaTime * 3);

        if (onGround)
        {
            if (scenePos)
            {
                GameObject reset = Instantiate(this.gameObject);
                reset.transform.position = transform.position;
                Rigidbody slimeStert1 = reset.GetComponent<Rigidbody>();
                slimeStert1.freezeRotation = true;
                slimeStert1.AddForce(Vector3.up, ForceMode.Impulse);
                Destroy(this.gameObject);
            }
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
            transform.localScale = new Vector3(slimeSize, slimeSize, slimeSize)*0.8f;
        }
    }
}
