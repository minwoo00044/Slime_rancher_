using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

public class SlimeMove : MonoBehaviour
{
    float jumpDaley;
    float moveDaley;
    float rotateDaley;

    public bool onGround = true;

    public float maxJumpHigh = 5f;
    public float minJumpHigh = 3f;

    Vector3 jumpBir;

    public float moveSpeed = 0.009f;
    public float moveCount;

    public float findRange = 5f;
    GameObject lookObject;
    GameObject findObject;

    Quaternion lookBir;

    float hunger = 0;
    float eating = 0;
    public float gemCount = 0;

    GameObject gem;
    GameObject spawnPos;
    public GameObject gemList;
    public GameObject slimeList;

    Animator animator;
    void Start()
    {
        animator = transform.GetComponent<Animator>();
        spawnPos = transform.GetChild(2).gameObject;
        FindMyGem();
    }



    void Update()
    {
        FindObject();

        Move();

        Jump();

        Rotate();

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
                if (findObject.transform.GetChild(0).name != transform.GetChild(0).name)
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
            if (lookObject.tag == "Tar") lookBir = Quaternion.LookRotation(-(new Vector3(lookObject.transform.position.x - transform.position.x, 0, lookObject.transform.position.z - transform.position.z)), Vector3.up);
            else lookBir = Quaternion.LookRotation(new Vector3(lookObject.transform.position.x - transform.position.x, 0, lookObject.transform.position.z - transform.position.z), Vector3.up);
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
    private void Eat()
    {
        if (hunger > 0)
        {
            hunger -= Time.deltaTime;
            if (gemCount > 0)
            {
                eating -= Time.deltaTime;
                if (eating <= 0)
                {
                    GameObject gemGO = Instantiate(gem);
                    gemGO.transform.position = spawnPos.transform.position;
                    Rigidbody rigidbody = gemGO.GetComponent<Rigidbody>();
                    rigidbody.AddForce(Vector3.up * 3, ForceMode.Impulse);

                    gemCount--;
                }
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 4)
        {
            Destroy(this.gameObject);
        }
        if (collision.gameObject.tag == "Food" && hunger == 0)
        {
            Destroy(collision.gameObject);
            lookObject = null;

            eating = 3;
            hunger = 100;
            gemCount++;
            for(int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).gameObject.name == collision.gameObject.name) gemCount++;
            }
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
            if (lookObject.transform.childCount < 1)
                return;
            if (thisSlime.transform.GetChild(0).name == this.transform.GetChild(0).name || thisSlime.transform.GetChild(0).name == lookObject.transform.GetChild(0).name)
            {
                if (thisSlime.transform.GetChild(1).name == this.transform.GetChild(0).name || thisSlime.transform.GetChild(1).name == lookObject.transform.GetChild(0).name)
                {
                    Destroy(lookObject);
                    GameObject bigSlime = Instantiate(thisSlime);
                    bigSlime.transform.position = spawnPos.transform.position;
                    Rigidbody slimeStert = bigSlime.GetComponent<Rigidbody>();
                    slimeStert.AddForce(Vector3.up * 2, ForceMode.Impulse);
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
