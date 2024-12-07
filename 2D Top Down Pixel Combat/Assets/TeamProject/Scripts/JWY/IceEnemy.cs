using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceEnemy : Enemy
{
    Rigidbody2D rd2d;
    CapsuleCollider2D EnemyCapsule;

    [SerializeField] float rayLength = 10f; // 레이캐스트 길이
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float attackDistance = 2f;
    [SerializeField] float attackCooldown = 2f;
    [SerializeField] float idleTimeAfterAttack = 3f;

    private Transform target;
    private Animator animator;

    private bool isWalking = false;
    private bool isAttacking = false;
    private bool canAttack = true;
    private bool hasDetectedPlayer = false; // 플레이어 발견 여부

    IceEnemy()
    {
        hp = 15; // 초기 HP 설정
    }

    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        EnemyCapsule = GetComponent<CapsuleCollider2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }

    private void StopMoving()
    {
        rd2d.velocity = new Vector2(0, rd2d.velocity.y);
        animator.ResetTrigger("isWander");
        isWalking = false;
    }

    private void MoveTowardPlayer()
    {
        if (!isAttacking)
        {
            isWalking = true;
            animator.SetTrigger("isWander");

            float moveDirection = target.position.x > transform.position.x ? 1 : -1;
            Vector2 enemyVelocity = new Vector2(moveSpeed * moveDirection, rd2d.velocity.y);
            rd2d.velocity = enemyVelocity;

            FlipEnemyFacing(moveDirection);
        }
    }

    private IEnumerator AttackPlayer()
    {
        isAttacking = true;
        canAttack = false;

        StopMoving();
        animator.SetTrigger("isAttack");

        yield return new WaitForSeconds(attackCooldown);

        animator.ResetTrigger("isAttack");
        isAttacking = false;

        yield return new WaitForSeconds(idleTimeAfterAttack);
        canAttack = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerMovement>().Damage(5);
        }
    }

    void FlipEnemyFacing(float moveDirection)
    {
        transform.localScale = new Vector2(-(Mathf.Sign(moveDirection)), 1f);
    }

    void Update()
    {
        if (hp <= 0)
        {
            Die();
            return;
        }

        if (target == null) return;

        // 플레이어가 적보다 아래에 있고 아직 발견되지 않은 경우 행동 중지
        if (target.position.y < transform.position.y && !hasDetectedPlayer)
        {
            StopMoving();
            return;
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, target.position - transform.position, rayLength, LayerMask.GetMask("Player"));
        Debug.DrawRay(transform.position, (target.position - transform.position).normalized * rayLength, Color.blue);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            hasDetectedPlayer = true; // 플레이어를 발견함
            float distanceToPlayer = Vector2.Distance(transform.position, target.position);

            if (distanceToPlayer <= attackDistance && canAttack)
            {
                if (!isAttacking)
                {
                    StartCoroutine(AttackPlayer());
                }
            }
            else if (!isAttacking)
            {
                MoveTowardPlayer();
            }
        }
        else
        {
            if (!hasDetectedPlayer) StopMoving();
        }
    }
}

