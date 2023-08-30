using System.Collections;
using System.Collections.Generic;
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
    public int hp = 10;
    int maxHp = 10;
    public float stamina = 100;
    float maxStamina = 100;
    public GameObject hitImage;
    float currentTime;

    //필요 속성: 모델링 오브젝트의 애니메이터
    //Animator animator;
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        //animator = gameObject.GetComponentInChildren<Animator>();
        maxHp = hp;
        maxStamina= stamina;
        runSpeed = speed * runMultiple;
    }
    // Update is called once per frame
    void Update()
    {

        //입력
        //if (GameManager.Instance.gameState != GameManager.GameState.Start)
        //    return;
        if(Input.GetKey(KeyCode.LeftShift))
        {
            speed = runSpeed;
            stamina -= 1 * Time.deltaTime;
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = speed / runMultiple;
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
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            yVelocity = jumpPower;
            isJumping = true;
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

}
