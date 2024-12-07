using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float rayLength = 10f; // 레이캐스트 길이
    [SerializeField] private float traceRange = 5f; // 추적 범위
    [SerializeField] private float attackDistance = 2f; // 공격 가능 거리
    [SerializeField] private MonoBehaviour enemyType; // 적 동작 클래스 (IEnemy 인터페이스 구현 필요)
    [SerializeField] private float attackCoolDown = 2f; // 공격 쿨타임
    [SerializeField] private float attackDuration = 1f; // 공격 애니메이션 지속 시간

    private bool canAttack = true; // 공격 가능 여부
    private bool isAttackInProgress = false; // 공격 진행 중 여부

    private State state; // 몬스터 상태
    private EnemyPathfinding enemyPathfinding; // 경로 탐색 관련 컴포넌트

    private enum State 
    {
        Stoping, // 정지 상태
        Tracing, // 추적 상태
        Attacking // 공격 상태
    }

    private Transform playerTransform; // 플레이어 Transform 정보

    private void Awake()
    {
        enemyPathfinding = GetComponent<EnemyPathfinding>(); // EnemyPathfinding 컴포넌트 가져오기
        state = State.Stoping; // 초기 상태 설정
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // 플레이어 Transform 설정
    }

    private void Update()
    {
        // 상태에 따라 동작 수행
        switch (state)
        {
            case State.Stoping:
                Stoping();
                break;
            case State.Tracing:
                Tracing();
                break;
            case State.Attacking:
                Attacking();
                break;
        }
    }

    private void Stoping()
    {
        // 정지 상태: 애니메이션 및 이동 멈춤
        (enemyType as IEnemy)?.Stop();
        enemyPathfinding.StopMoving();

        // 플레이어가 추적 범위 안에 들어오면 추적 상태로 전환
        if (IsPlayerInRaycast(traceRange))
        {
            state = State.Tracing;
        }
    }

    private void Tracing()
    {
        // 추적 상태: 플레이어를 향해 이동
        (enemyType as IEnemy)?.Trace();

        // 플레이어가 공격 범위 안에 들어오면 공격 상태로 전환
        if (IsPlayerInRaycast(attackDistance))
        {
            state = State.Attacking;
        }
        // 플레이어가 추적 범위를 벗어나면 정지 상태로 전환
        else if (!IsPlayerInRaycast(traceRange))
        {
            state = State.Stoping;
        }
    }

    private void Attacking()
    {
        // 공격 중일 때 다른 동작 수행하지 않음
        if (isAttackInProgress) return;

        // 플레이어가 공격 범위를 벗어나면 추적 상태로 전환
        if (!IsPlayerInRaycast(attackDistance))
        {
            StartCoroutine(SwitchToTraceAfterAttack());
            return;
        }

        // 공격 수행
        if (canAttack)
        {
            isAttackInProgress = true; // 공격 진행 플래그 활성화
            canAttack = false; // 공격 불가 상태로 변경
            (enemyType as IEnemy)?.Attack(); // 공격 수행
            enemyPathfinding.StopMoving(); // 이동 멈춤
            StartCoroutine(FinishAttack()); // 공격 종료 처리
            StartCoroutine(AttackCoolDownRoutine()); // 쿨다운 시작
        }
    }

    // 플레이어가 레이캐스트에 감지되는지 확인
    private bool IsPlayerInRaycast(float range)
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized; // 레이 방향 계산
        Debug.DrawRay(transform.position, direction * range, Color.red); // 레이 디버그 시각화

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, range, LayerMask.GetMask("Player"));
        return hit.collider != null && hit.collider.CompareTag("Player");
    }

    private IEnumerator SwitchToTraceAfterAttack()
    {
        // 공격 종료될 때까지 대기
        while (isAttackInProgress) yield return null;
        state = State.Tracing; // 추적 상태로 전환
    }

    private IEnumerator FinishAttack()
    {
        // 공격 애니메이션 지속 시간 대기
        yield return new WaitForSeconds(attackDuration);
        isAttackInProgress = false; // 공격 진행 플래그 해제
    }

    private IEnumerator AttackCoolDownRoutine()
    {
        // 공격 쿨타임 대기
        yield return new WaitForSeconds(attackCoolDown);
        canAttack = true; // 공격 가능 상태로 전환
    }
}