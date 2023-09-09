
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    
    public float speed = 10f;
    public float runMultiple;
    float runSpeed;
    
    public float jumpPower = 10;
    CharacterController characterController;
    float gravity = -20f;
    float yVelocity = 0;
    public bool isJumping = false;

    public bool isStaminaReduce = false;
    bool isRunning = false; 
    public float staminaReduce;

    public bool isEquipJumpPack = false;
    public float jumpPackPower;
    public float flyLimit;

    public GameObject gun;
    private Vector3 gunOriginalPosition;
    //�ʿ� �Ӽ�: �𵨸� ������Ʈ�� �ִϸ�����
    Animator animator;
    private void Start()
    {
        gunOriginalPosition = gun.transform.localPosition;
        characterController = GetComponent<CharacterController>();
        animator = gameObject.GetComponentInChildren<Animator>();
        runSpeed = speed * runMultiple;

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // Toggle the running state
            isRunning = !isRunning;

            // Set the animator parameter and adjust speed based on the running state
            animator.SetBool("isRun", isRunning);
            speed = isRunning ? runSpeed : speed / runMultiple;
            isStaminaReduce = isRunning;
        }

        // Optionally, you can reduce stamina while the player is running here
        if (isRunning)
        {
            Player.Instance.stamina -= staminaReduce * Time.deltaTime;
        }
        if (isJumping && characterController.collisionFlags == CollisionFlags.Below)
        {
            isJumping = false;
        }
        //�ٴڿ� ��������� ���� �ӵ� �ʱ�ȭ
        else if (characterController.collisionFlags == CollisionFlags.Below)
        {
            yVelocity = 0;
        }
        //�Ϲ� ����
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            yVelocity = jumpPower;
            isJumping = true;
        }
        if (Input.GetButton("Jump") && isEquipJumpPack)
        {
            isStaminaReduce = true;
            Player.Instance.stamina -= staminaReduce * Time.deltaTime;
            if (transform.position.y < flyLimit)
            {

                yVelocity = jumpPower;
                yVelocity += jumpPackPower * Time.deltaTime;

            }
        }
        else if (Input.GetButtonUp("Jump"))
        {
            isStaminaReduce = false;
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        //�̵����� ����

        Vector3 dir = new Vector3(h, 0, v);
        dir = Camera.main.transform.TransformDirection(dir);


        yVelocity = yVelocity + gravity * Time.deltaTime;
        dir.y = yVelocity;

        //transform.position += dir * speed * Time.deltaTime;

        characterController.Move(dir * speed * Time.deltaTime);

        //animator.SetFloat("MoveMotion", dir.magnitude);
    }

}
