using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    [SerializeField] float Speed = 10f;
    [SerializeField] float jumpSpeed = 10f;
    [SerializeField] float hp = 10f;

    CapsuleCollider2D myCapusleCollider;

    bool canDoubleJump;

    private Animator animator; 

    private bool isWalking = false;
    private bool isJump = false;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myCapusleCollider = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Run();
        FlipSprite();
    }

    public void Damage(int damage)
    {
        hp -= damage;
        Debug.Log("Now Player HP is " + hp);
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        Debug.Log(moveInput);
    }

    void OnJump(InputValue value)
    {  
        if(value.isPressed)
        {
            //jump 애니메이션 추가
            isJump = true;
            animator.SetTrigger("isJump");
            //

            if(myCapusleCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                myRigidbody.velocity += new Vector2(0f, jumpSpeed);
                canDoubleJump = true;
            }
            else if(canDoubleJump)
            {
                myRigidbody.velocity += new Vector2(0f, jumpSpeed);
                canDoubleJump = false;
            }

            //jump 애니메이션 중지
            StartCoroutine(ResetJumpTriggerAfterDelay(1.0f));
        }
        else{
            isJump = false;
        }

    }
    // 점프를 누르면 1초만 재생
    private IEnumerator ResetJumpTriggerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        animator.ResetTrigger("isJump");
    }

    void Run()
    {
        if (Mathf.Abs(moveInput.x) > Mathf.Epsilon)
        {
            isWalking = true;
        }
        else{
            isWalking = false;
        }

        // 기본 중력
        Vector2 playerVelocity = new Vector2(moveInput.x * Speed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;

        animator.SetBool("isWalking", isWalking);
    }

    void FlipSprite()
    {
        bool playerHasHoizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

        if (playerHasHoizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
    }

}
