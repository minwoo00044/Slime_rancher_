
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
    public float staminaReduce;

    public bool isEquipJumpPack = false;
    public float jumpPackPower;
    public float flyLimit;

    public GameObject gun;
    public Transform originTf;
    public Transform runGunTf;

    private Camera cam;
    private Vector3 originalPosition;
    //필요 속성: 모델링 오브젝트의 애니메이터
    //Animator animator;
    private void Start()
    {
        cam = Camera.main;
        characterController = GetComponent<CharacterController>();
        //animator = gameObject.GetComponentInChildren<Animator>();
        runSpeed = speed * runMultiple;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            gun.transform.position = runGunTf.position;
            gun.transform.rotation = runGunTf.rotation;
            speed = runSpeed;
            Player.Instance.stamina -= staminaReduce * Time.deltaTime;
            isStaminaReduce = true;
            StartCoroutine(ShakeCamera());
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            gun.transform.position = originTf.position;
            gun.transform.rotation = originTf.rotation;
            speed = speed / runMultiple;
            isStaminaReduce = false;
        }
        if (isJumping && characterController.collisionFlags == CollisionFlags.Below)
        {
            isJumping = false;
        }
        //바닥에 닿아있을때 수직 속도 초기화
        else if (characterController.collisionFlags == CollisionFlags.Below)
        {
            yVelocity = 0;
        }
        //일반 점프
        if (Input.GetButtonDown("Jump") && !isJumping && !isEquipJumpPack)
        {
            yVelocity = jumpPower;
            isJumping = true;
        }
        else if (Input.GetButton("Jump") && isEquipJumpPack)
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
        //이동방향 설정

        Vector3 dir = new Vector3(h, 0, v);
        dir = Camera.main.transform.TransformDirection(dir);


        yVelocity = yVelocity + gravity * Time.deltaTime;
        dir.y = yVelocity;

        //transform.position += dir * speed * Time.deltaTime;

        characterController.Move(dir * speed * Time.deltaTime);

        //animator.SetFloat("MoveMotion", dir.magnitude);
    }
    IEnumerator ShakeCamera()
    {
        yield return null;
    }
}
