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

    public float maxJumpHigh = 6f;
    public float minJumpHigh = 3f;

    Vector3 jumpBir;

    public float moveSpeed = 0.008f;
    public float moveCount;

    Quaternion lookBir;

    public float findRange = 5f;
    GameObject lookObject;
    GameObject findObject;

    float hunger = 0;
    float eating = 0;
    float gemCount = 0;

    GameObject gem1;
    GameObject gem2;
    GameObject spawnPos;
    public GameObject gemList;
    public GameObject tar;

    bool grip = false;

    Animator animator;
    void Start()
    {
        spawnPos = transform.GetChild(3).gameObject;
        animator = GetComponent<Animator>();
        FindMyGem();
    }

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

        Eat();

    }

    private void OnTriggerEnter(Collider other)
    {
        onGround = true;
        animator.SetTrigger("JumpE");
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
                animator.SetTrigger("JumpS");
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
            if (lookObject.tag == "Tar") Quaternion.LookRotation(new Vector3(lookObject.transform.position.x - transform.position.x, 0, lookObject.transform.position.z - transform.position.z), Vector3.up); 
            else lookBir = lookBir = Quaternion.LookRotation(new Vector3(lookObject.transform.position.x - transform.position.x, 0, lookObject.transform.position.z - transform.position.z), Vector3.up);
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
            if (findObject.tag == "Tar")
            {
                lookObject = findObject;
                break;
            }
            if (findObject.tag == "Item")
            {
                if (findObject.transform.GetChild(0).name != transform.GetChild(1).name && findObject.transform.GetChild(0).name != transform.GetChild(0).name)
                {
                    if (lookObject == null || (lookObject.transform.position - transform.position).magnitude > (findObject.transform.position - transform.position).magnitude)
                    {
                        lookObject = findObject;
                    }
                }
            }
            if (findObject.tag == "Food" && hunger <= 0)
            {
                if (lookObject == null || (lookObject.transform.position - transform.position).magnitude > (findObject.transform.position - transform.position).magnitude)
                {
                    lookObject = findObject;
                }
            }
        }
    }

    private void Eat()
    {
        if (hunger > 0)
        {
            hunger -= Time.deltaTime;
            if(gemCount > 0)
            {
                eating -= Time.deltaTime;
                if (eating <= 0)
                {
                    GameObject gem1GO = Instantiate(gem1);
                    gem1GO.transform.position = spawnPos.transform.position;
                    Rigidbody rigidbody = gem1GO.GetComponent<Rigidbody>();
                    rigidbody.AddForce(Vector3.up * 3, ForceMode.Impulse);
                    GameObject gem2GO = Instantiate(gem2);
                    gem2GO.transform.position = spawnPos.transform.position;
                    rigidbody = gem2GO.GetComponent<Rigidbody>();
                    rigidbody.AddForce(Vector3.up * 3, ForceMode.Impulse);

                    gemCount--;
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
        if (collision.gameObject.tag == "Food" && hunger <= 0)
        {
            Destroy(collision.gameObject);
            lookObject = null;

            eating = 3;
            hunger = 100;
            gemCount++;
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).gameObject.name == collision.gameObject.name) gemCount++;
            }
        }
        if (collision.gameObject.tag == "Item")
        {
            if(collision.gameObject.transform.GetChild(0).name != transform.GetChild(0).name && collision.gameObject.transform.GetChild(0).name != transform.GetChild(1).name)
            {
                Destroy(collision.gameObject);
                lookObject = null;
                GrowthSlime();
            }
            
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
                    i = 0;
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
        slimeStert1.AddForce(Vector3.up * 2, ForceMode.Impulse);

        GameObject tar2 = Instantiate(tar);
        tar2.transform.position = spawnPos.transform.position;
        Rigidbody slimeStert2 = tar2.GetComponent<Rigidbody>();
        slimeStert2.AddForce(Vector3.up * 2, ForceMode.Impulse);

        Destroy(this.gameObject);
    }
}
