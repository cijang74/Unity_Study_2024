using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    private Rigidbody2D rb;
    private Vector2 moveDir;

    private void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Moveto(Vector2 targetPosition) // 다른 클래스에서 이동 방향을 입력받는 용도
    {
        moveDir = targetPosition;
    }

    private void FixedUpdate() // 물리나 플레이어 입력에 주로 사용되는 고정 업데이트
    {
        rb.MovePosition(rb.position + moveDir * (moveSpeed * Time.fixedDeltaTime)); 
        // 움직이려는 객체의 최근 포지션 + (이동벡터값 * (이동속도 * 프레임))
    }
}