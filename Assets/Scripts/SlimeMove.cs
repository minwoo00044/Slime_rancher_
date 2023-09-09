using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SlimeMove : MonoBehaviour
{
    float jumpDaley;
    float moveDaley;
    float rotateDaley;

    public bool onGround = true;

    public float maxJumpHigh = 5f;
    public float minJumpHigh = 3f;

    Vector3 jumpBir;

    public float moveSpeed = 0.005f;
    public float moveCount;

    float rotateSize;
    int rightOrLeft = 1;
    Vector3 rotateBir;

    public float findRange = 5f;
    GameObject lookObject;
    GameObject findObject;

    float hunger = 0;
    GameObject gem;
    GameObject spawnPos;
    public GameObject gemList;
    public GameObject slimeList;

    void Start()
    {
        spawnPos = transform.GetChild(2).gameObject;
        FindMyGem();
    }



    void Update()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, findRange);
        lookObject = null;



        for (int i = 0; i < cols.Length; i++)
        {
            findObject = cols[i].gameObject;
            if(findObject.tag == "Tar")
            {
                lookObject = findObject;
                break;
            }
            if (findObject.tag == "Item" && findObject.transform.GetChild(0).name != transform.GetChild(0).name)
            {
                if (lookObject == null || (lookObject.transform.position - transform.position).magnitude > (findObject.transform.position - transform.position).magnitude)
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
            if (0 > jumpDaley)
            {
                Jump();
            }
        }

        if (hunger > 0)
        {
            if (hunger >= 100)
            {
                GameObject gemGO = Instantiate(gem);
                gemGO.transform.position = spawnPos.transform.position;
                Rigidbody rigidbody = gemGO.GetComponent<Rigidbody>();
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
        if (collision.gameObject.tag == "Food" && hunger == 0)
        {
            Destroy(collision.gameObject);
            lookObject = null;

            hunger = 100;
        }
        if (collision.gameObject.tag == "Item" && collision.gameObject.transform.GetChild(0).name != transform.GetChild(0).name)
        {
            GrowthSlime();
        }
    }
    void FindMyGem()
    {
        for (int i = 0; i < gemList.transform.childCount; i++)
        {
            GameObject thisGem = gemList.transform.GetChild(i).gameObject;
            if (thisGem.transform.GetChild(0).name == transform.GetChild(0).name)
            {
                gem = thisGem;
                break;

            }
        }
    }
    void GrowthSlime()
    {
        for (int i = 0; i < slimeList.transform.childCount; i++)
        {
            GameObject thisSlime = slimeList.transform.GetChild(i).gameObject;
            if (thisSlime.transform.GetChild(0).name == this.transform.GetChild(0).name || thisSlime.transform.GetChild(0).name == lookObject.transform.GetChild(0).name)
            {
                if (thisSlime.transform.GetChild(1).name == this.transform.GetChild(0).name || thisSlime.transform.GetChild(1).name == lookObject.transform.GetChild(0).name)
                {
                    Destroy(lookObject);
                    GameObject bigSlime = Instantiate(thisSlime);
                    bigSlime.transform.position = spawnPos.transform.position;
                    Rigidbody slimeStert = bigSlime.GetComponent<Rigidbody>();
                    slimeStert.AddForce(Vector3.up*5,ForceMode.Impulse);
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
