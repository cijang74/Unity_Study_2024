using UnityEngine;

public class Tree : MonoBehaviour, IEnemy
{
    private Transform target; // 플레이어의 Transform 정보
    private Animator animator; // 애니메이션 제어를 위한 Animator 컴포넌트
    private EnemyPathfinding enemyPathfinding; // 경로 탐색용 컴포넌트

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        enemyPathfinding = GetComponent<EnemyPathfinding>();
    }

    public void Attack()
    {
        AttackPlayer();
    }

    public void Trace()
    {
        MoveTowardPlayer();
    }

    private void AttackPlayer()
    {
        animator.ResetTrigger("isWalking");
        animator.SetTrigger("isAttacking");
    }

    private void MoveTowardPlayer()
    {
        animator.ResetTrigger("isAttacking");
        animator.SetTrigger("isWalking");

        float moveDirection = target.position.x > transform.position.x ? 1 : -1;
        Vector2 moveVector = new Vector2(moveDirection, 0f);

        enemyPathfinding.Moveto(moveVector);
    }

    public void Stop()
    {
        animator.ResetTrigger("isWalking");
        animator.ResetTrigger("isAttacking");
    }
}