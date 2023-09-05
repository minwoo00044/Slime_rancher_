using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMove : MonoBehaviour
{
    public float jumpDaley;
    public float moveDaley;
    public float rotateDaley;

    public bool onGround = true;

    public float maxJumpHigh;
    public float minJumpHigh;

    Vector3 jumpBir;

    public float moveSpeed = 0.01f;
    public float moveCount;

    float rotateSize;
    int rightOrLeft = 1;
    Vector3 rotateBir;

    public float findRange = 5f;
    public GameObject lookObject;
    GameObject findObject;

    float hunger = 0;
    public GameObject gem;
    public GameObject gemSpawnPos;


    void Start()
    {
        
    }

    void Update()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, findRange);
        lookObject = null;

        

        for (int i = 0; i < cols.Length; i++)
        {
            findObject = cols[i].gameObject;
            if(findObject.tag == "Item" && findObject.transform.GetChild(0).name != transform.GetChild(1).name)
            {
                if(findObject.transform.GetChild(0).name != transform.GetChild(0).name)
                {
                    lookObject = findObject;
                }
            }
            if (findObject.tag == "Food" && hunger == 0)
            {
                if (lookObject == null || (lookObject.transform.position - transform.position).magnitude > (findObject.transform.position - transform.position).magnitude)
                {
                    lookObject = findObject;
                }
            }

            findObject = null;
        }

        if (lookObject != null)
        {
            Vector3 relativePos = lookObject.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            if (rotation.y > transform.rotation.y +0.01f) transform.Rotate(Vector3.up);
            else if (rotation.y < transform.rotation.y -0.01f) transform.Rotate(Vector3.down);

            print(rotation.y);
            print(transform.rotation.y);
              
            Move();
        }

        else
        {
            if (onGround)
            {
                jumpDaley -= Time.deltaTime;
                if (0 > jumpDaley)
                {
                    jumpDaley = Random.Range(3, 5);
                    Jump();
                }
            }

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

        if (hunger > 0)
        {
            if (hunger >= 100)
            {
                GameObject gemGO = Instantiate(gem);
                gemGO.transform.position = gemSpawnPos.transform.position;
                Rigidbody rigidbody = gemGO.GetComponent<Rigidbody>();
                rigidbody.AddForce(Vector3.up*5, ForceMode.Impulse);
            }
            hunger -= Time.deltaTime;
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

    }
    private void Move()
    {

        transform.Translate(Vector3.forward * moveSpeed);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Food" && hunger == 0)
        {
            Destroy(collision.gameObject);
            lookObject = null;

            hunger = 100;
        }
        if (findObject.tag == "Item" && findObject.transform.GetChild(0).name != transform.GetChild(1).name && findObject.transform.GetChild(0).name != transform.GetChild(0).name)
        {
            Destroy(collision.gameObject);
            lookObject = null;
            
        }
    }
}
