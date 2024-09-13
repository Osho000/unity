using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_movement : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 5f;
    CharacterController characterController;
    Vector2 moveDirection;
    float moveX;
    float moveY;
    Animator animator;
    float moveXidle;
    float moveYidle;
    Vector3 direction;
    public float cooldown = 3f;
    private float lastAttack = -9999f;

    public GameObject bulletPref;



    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

  
    void Update()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;
        if (moveX != 0 || moveY != 0)
        {
            Animate(moveX, moveY, 1);
            moveXidle = moveX;
            moveYidle = moveY;
            Debug.Log(moveX + " " + moveY);
        }
        else 
        {

            Animate(moveXidle, moveYidle, 0);


        }
        if (Input.GetMouseButtonDown(0))
            Attack();
        else
            animator.SetBool("isAttack", false);
    }


    private void FixedUpdate()
    {
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        
    }

    void Animate(float moveX, float moveY, int layer)
    {
        
        switch (layer)
        {
            case 0: animator.SetLayerWeight(0, 1);
                    animator.SetLayerWeight(1, 0);
                break;
            case 1: animator.SetLayerWeight(1, 1);
                    animator.SetLayerWeight(0, 0);
                break;
        }
        animator.SetFloat("MoveX", moveX);
        animator.SetFloat("MoveY", moveY);
    }

    void Attack()
    {
        if (Time.time > lastAttack + cooldown)
        {
            animator.SetBool("isAttack", true);
            animator.SetFloat("MoveX", moveXidle);
            animator.SetFloat("MoveY", moveYidle);

            direction = Input.mousePosition;
            direction.z = 0f;
            direction = Camera.main.ScreenToWorldPoint(direction);
            direction = direction - transform.position;

            GameObject bulletInstance = Instantiate(bulletPref, transform.position, Quaternion.identity);
            bulletInstance.GetComponent<Rigidbody2D>().velocity = direction * 5f;

            Destroy(bulletInstance);
            lastAttack = Time.time;
        }
    }
}
