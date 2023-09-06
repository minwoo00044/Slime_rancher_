using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BigSlimeMove : MonoBehaviour
{
    float jumpDaley;
    float moveDaley;
    float rotateDaley;

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
    GameObject lookObject;
    GameObject findObject;

    float hunger = 0;
    GameObject gem1;
    GameObject gem2;
    GameObject spawnPos;
    public GameObject gemList;
    public GameObject tar;

    void Start()
    {
        spawnPos = transform.GetChild(3).gameObject;
        FindMyGem();
    }

    void Update()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, findRange);
        lookObject = null;



        for (int i = 0; i < cols.Length; i++)
        {
            findObject = cols[i].gameObject;
            if (findObject.tag == "Tar")
            {
                lookObject = findObject;
                break;
            }
            if (findObject.tag == "Item" && findObject.transform.GetChild(0).name != transform.GetChild(1).name && findObject.transform.GetChild(0).name != transform.GetChild(0).name)
            {
                if (lookObject == null || (lookObject.transform.position - transform.position).magnitude > (findObject.transform.position - transform.position).magnitude)
                {
                    lookObject = findObject;
                }
            }
            if (findObject.tag == "Food" && hunger <= 0)
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
            Vector3 relativePos = new Vector3(lookObject.transform.position.x - transform.position.x, 0, lookObject.transform.position.z - transform.position.z);
            if (lookObject.tag == "Tar")
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(-relativePos, Vector3.up), Time.deltaTime * 3);
            else transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(relativePos, Vector3.up), Time.deltaTime * 3);
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

        if (hunger > 0)
        {
            if (hunger >= 100)
            {
                GameObject gem1GO = Instantiate(gem1);
                gem1GO.transform.position = spawnPos.transform.position;
                Rigidbody rigidbody = gem1GO.GetComponent<Rigidbody>();
                rigidbody.AddForce(Vector3.up * 5, ForceMode.Impulse);
                GameObject gem2GO = Instantiate(gem2);
                gem2GO.transform.position = spawnPos.transform.position;
                rigidbody = gem2GO.GetComponent<Rigidbody>();
                rigidbody.AddForce(Vector3.up * 5, ForceMode.Impulse);
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
        jumpDaley = Random.Range(3, 5);

    }
    private void Move()
    {

        transform.Translate(Vector3.forward * moveSpeed);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Food" && hunger <= 0)
        {
            Destroy(collision.gameObject);
            lookObject = null;

            hunger = 100;
        }
        if (collision.gameObject.tag == "Item" && collision.gameObject.transform.GetChild(0).name != transform.GetChild(0).name)
        {
            Destroy(collision.gameObject);
            lookObject = null;
            GrowthSlime();
        }
    }
    void FindMyGem()
    {
        int countNum = 0;
        for (int i = 0; i < gemList.transform.childCount; i++)
        {
            GameObject thisGem = gemList.transform.GetChild(i).gameObject;
            if (thisGem.transform.GetChild(0).name == this.transform.GetChild(countNum).name)
            {
                if (gem1 == null)
                {
                    gem1 = thisGem;
                    countNum++;
                }
                else
                {
                    gem2 = thisGem;
                    break;
                }
            }
        }
    }
    void GrowthSlime()
    {
        GameObject tar1 = Instantiate(tar);
        tar1.transform.position = spawnPos.transform.position;
        Rigidbody slimeStert1 = tar1.GetComponent<Rigidbody>();
        slimeStert1.AddForce(Vector3.up * 3, ForceMode.Impulse);

        GameObject tar2 = Instantiate(tar);
        tar2.transform.position = spawnPos.transform.position;
        Rigidbody slimeStert2 = tar2.GetComponent<Rigidbody>();
        slimeStert2.AddForce(Vector3.up * 3, ForceMode.Impulse);

        Destroy(this.gameObject);
    }
}
