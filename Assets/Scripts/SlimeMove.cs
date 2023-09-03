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

    public float moveSpeed;
    public float moveCount;

    float rotateSize;
    int rightOrLeft = 1;
    Vector3 rotateBir;
    void Start()
    {
    }

    void Update()
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

            if(rotateSize <= 0)
            {
                rotateSize = Random.Range(0, 180);
                if (Random.Range(1, 3) % 2 == 0) rightOrLeft *= -1;
                rotateBir = new Vector3(0, rightOrLeft, 0);
                rotateDaley = Random.Range(3, 5);
            }

            transform.Rotate(rotateBir);
            rotateSize --;
        }
        else
        {
            rotateDaley -= Time.deltaTime;
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
}
