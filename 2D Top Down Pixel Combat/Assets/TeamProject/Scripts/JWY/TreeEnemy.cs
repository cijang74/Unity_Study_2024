using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeEnemy : Enemy
{
    Rigidbody2D rd2d; // Rigidbody2D 컴포넌트 (물리적 이동 제어)
    CapsuleCollider2D EnemyCapsule; // CapsuleCollider2D 컴포넌트 (적의 충돌 영역)

    [SerializeField] float rayLength = 10f; // 레이캐스트 길이
    [SerializeField] float moveSpeed = 1f; // 적의 이동 속도
    [SerializeField] float attackDistance = 2f; // 공격 가능 거리
    [SerializeField] float attackCooldown = 2f; // 공격 대기 시간

    private Transform target; // 플레이어의 Transform 정보
    private Animator animator; // 애니메이션 제어를 위한 Animator 컴포넌트

    private bool isAttacking = false; // 적이 공격 중인지 여부
    private bool isWalking = false; // 적이 걷고 있는지 여부
    private bool hasDetectedPlayer = false; // 플레이어를 발견했는지 여부

    TreeEnemy()
    {
        hp = 10; // 적의 초기 체력 설정
    }

    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>(); // Rigidbody2D 컴포넌트 가져오기
        EnemyCapsule = GetComponent<CapsuleCollider2D>(); // CapsuleCollider2D 컴포넌트 가져오기
        target = GameObject.FindGameObjectWithTag("Player").transform; // "Player" 태그가 있는 오브젝트의 Transform 가져오기
        animator = GetComponent<Animator>(); // Animator 컴포넌트 가져오기
    }

    void Update()
    {
        if (hp <= 0) // 체력이 0 이하라면
        {
            Die(); // 적 사망 처리
            return;
        }
        if (target == null) return; // 타겟이 없으면 업데이트 중단

        // 플레이어가 적보다 아래에 위치하고 아직 발견되지 않았다면 정지
        if (!hasDetectedPlayer && target.position.y < transform.position.y)
        {
            StopMoving(); // 이동 중지
            return;
        }

        // 레이캐스트를 사용하여 플레이어 탐지
        RaycastHit2D hit = Physics2D.Raycast(transform.position, target.position - transform.position, rayLength, LayerMask.GetMask("Player"));
        Debug.DrawRay(transform.position, (target.position - transform.position).normalized * rayLength, Color.green); // 레이캐스트 디버그 시각화

        if (hit.collider != null && hit.collider.CompareTag("Player")) // 플레이어가 레이캐스트에 감지되었다면
        {
            hasDetectedPlayer = true; // 플레이어 발견 여부 설정

            float distanceToPlayer = Vector2.Distance(transform.position, target.position); // 플레이어와의 거리 계산

            if (distanceToPlayer <= attackDistance) // 공격 가능 거리 안에 있을 경우
            {
                if (!isAttacking) // 현재 공격 중이 아니면
                {
                    StartCoroutine(AttackPlayer()); // 플레이어 공격 시작
                }
            }
            else
            {
                MoveTowardPlayer(); // 플레이어 쪽으로 이동
            }
        }
        else
        {
            if (!hasDetectedPlayer) StopMoving(); // 플레이어를 발견하지 못했으면 정지
        }
    }

    private void StopMoving()
    {
        rd2d.velocity = new Vector2(0, rd2d.velocity.y); // 적의 수평 이동 속도 0으로 설정
        animator.ResetTrigger("isWalking"); // 걷기 애니메이션 트리거 해제
        isWalking = false; // 걷는 상태 해제
    }

    private void MoveTowardPlayer()
    {
        if (!isAttacking) // 공격 중이 아닐 경우에만 이동
        {
            isWalking = true; // 걷는 상태로 설정
            animator.SetTrigger("isWalking"); // 걷기 애니메이션 트리거 설정

            float moveDirection = target.position.x > transform.position.x ? 1 : -1; // 플레이어 방향 계산
            Vector2 enemyVelocity = new Vector2(moveSpeed * moveDirection, rd2d.velocity.y); // 이동 속도 설정
            rd2d.velocity = enemyVelocity; // Rigidbody 속도 설정

            FlipEnemyFacing(moveDirection); // 적의 방향 전환
        }
    }

    private IEnumerator AttackPlayer()
    {
        isAttacking = true; // 공격 중 상태로 설정
        StopMoving(); // 이동 중지

        animator.SetTrigger("isAttacking"); // 공격 애니메이션 트리거 설정

        yield return new WaitForSeconds(attackCooldown); // 공격 대기 시간 대기

        animator.ResetTrigger("isAttacking"); // 공격 애니메이션 트리거 해제
        isAttacking = false; // 공격 상태 해제
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // 플레이어와 충돌했을 경우
        {
            collision.gameObject.GetComponent<PlayerMovement>().Damage(5); // 플레이어에게 데미지 가하기
        }
    }

    void FlipEnemyFacing(float moveDirection)
    {
        transform.localScale = new Vector2(-(Mathf.Sign(moveDirection)), 1f); // 적의 좌우 방향 전환
    }
}