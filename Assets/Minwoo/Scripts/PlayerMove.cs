
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
    public bool isRunning = false; 
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
        if (Player.Instance.isStop)
            return;
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if(Player.Instance.stamina > 0)
            {
                ToggleRun();
            }
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
            Jump();
        }
        else if (Input.GetButtonUp("Jump"))
        {
            EndJump();
        }

        // Check for jump pack usage
        if (Input.GetButton("Jump") && isEquipJumpPack)
        {
            UseJumpPack();
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

    public void ToggleRun()
    {
        // Toggle the running state
        isRunning = !isRunning;

        // Set the animator parameter and adjust speed based on the running state
        animator.SetBool("isRun", isRunning);
        speed = isRunning ? runSpeed : speed / runMultiple;
        isStaminaReduce = isRunning;
    }

    private void Jump()
    {
        yVelocity = jumpPower;
        isJumping = true;
    }

    private void EndJump()
    {
        isJumping = false;
        isStaminaReduce = false;
    }
    private void UseJumpPack()
    {
        if (Player.Instance.stamina > 1)
        {
            isStaminaReduce = true;
            isJumping = true;
            Player.Instance.stamina -= staminaReduce * Time.deltaTime;

            if (transform.position.y < flyLimit)
            {
                yVelocity = jumpPower + jumpPackPower * Time.deltaTime;
            }
        }
        else
        {
            return;
        }
    }
}
