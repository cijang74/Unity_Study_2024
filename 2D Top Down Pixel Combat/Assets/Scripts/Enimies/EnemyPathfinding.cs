using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    private Rigidbody2D rb;
    private Vector2 moveDir;
    private Knockback knockback;
    private SpriteRenderer spriteRenderer;

    private void Awake() 
    {
        knockback = GetComponent<Knockback>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Moveto(Vector2 targetPosition) // 다른 클래스에서 이동 방향을 입력받는 용도
    {
        moveDir = targetPosition;
    }

    private void FixedUpdate() // 물리나 플레이어 입력에 주로 사용되는 고정 업데이트
    {
        if(knockback.GettingKnockedBack) // 지금 넉벡중이면 고정 업데이트 하지 말고 꺼져
        {
            return;
        }

        rb.MovePosition(rb.position + moveDir * (moveSpeed * Time.fixedDeltaTime)); 
        // 움직이려는 객체의 최근 포지션 + (이동벡터값 * (이동속도 * 프레임))

        // 움직이는 방향에 따라 스프라이트 뒤집어주기
        if(moveDir.x < 0)
        {
            spriteRenderer.flipX = true;
        }

        else
        {
            spriteRenderer.flipX = false;
        }
    }

    public void StopMoving()
    {
        moveDir = Vector3.zero;
        // 방향벡터를 0으로 설정함
    }
}
