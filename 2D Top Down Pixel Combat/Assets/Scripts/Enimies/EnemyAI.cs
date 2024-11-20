using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float roamChangeDirFloat = 2f;
    [SerializeField] private float attackRange = 0f;
    [SerializeField] private MonoBehaviour enemyType;
    [SerializeField] private float attackCoolDown = 2f;
    [SerializeField] private bool stopMovingWhileAttacking = false;

    private bool canAttack = true;

    private enum State // 열거자 객체 정의
    {
        Roming,
        Attaking
    }

    // class State <- 위 함수와 같은 역할을 함.
    // {
	//      public static final State Roming = new State();
	//      public static final State "" = new State();
    // }

    private State state;
    private EnemyPathfinding enemyPathfinding;
    private Vector2 roamPosition;
    private float timeRoaming = 0f;

    private void Awake()
    {
        enemyPathfinding = GetComponent<EnemyPathfinding>();
        state = State.Roming; // 처음 state 상태는 Roming상태
    }

    // 코루틴은 시간의 경과에 따른 절차적 단계를 수행하는데에 사용되는 함수
    // 1번째 호출 끝나고 다시 코루틴 호출하면 1번째 호출 리턴 라인 이후부터 시작.
    // 마지막번째 호출이 끝나고 다시 코루틴 호출하면 마지번째의 전 호출 리턴 라인부터
    private void Start()
    {
        roamPosition = GetRomingPosition();
    }
    
    private void Update()
    {
        MovementStateControl();
    }

    private void MovementStateControl()
    // 상태전환 제어
    {
        switch (state) // state에 따라 조건을 만족하는 코드를 실행
        {
            case State.Roming:
                Roaming();
                break;
            
            case State.Attaking:
                Attacking();
                break;
            
            default:
                break;

        }
    }

    private void Roaming()
    {
        timeRoaming += Time.deltaTime;
        enemyPathfinding.Moveto(roamPosition); //Pathfinding 클래스의 Moveto함수에 방향벡터 전달

        if(Vector2.Distance(transform.position, PlayerController.Instance.transform.position) < attackRange)
        // 플레이어가 몬스터 공격범위에 들어왔으면공격
        {
            state = State.Attaking;
        }

        if(timeRoaming > roamChangeDirFloat)
        // 돌아다닌 시간이 방향전환타이머를 초과했으면 다른 방향으로 설정하게 함
        {
            roamPosition = GetRomingPosition();
        }
    }

    private void Attacking()
    {
        if(Vector2.Distance(transform.position, PlayerController.Instance.transform.position) > attackRange)
        // 플레이어가 몬스터 공격범위에서 벗어났으면
        {
            state = State.Roming;
        }

        if(attackRange != 0 && canAttack == true)
        // 원거리공격 가능한 적이라면 원거리공격하기
        {
            canAttack = false;
            (enemyType as IEnemy).Attack();

            if(stopMovingWhileAttacking) // 공격중이 아닐때만 움직일 수 있게 하기
            {
                enemyPathfinding.StopMoving();
            }

            else
            {
                enemyPathfinding.Moveto(roamPosition);
            }

            StartCoroutine(AttackCoolDownRoutine());
        }
    }

    private IEnumerator AttackCoolDownRoutine() // 공격 쿨타임
    {
        yield return new WaitForSeconds(attackCoolDown);
        canAttack = true;
    }

    // private IEnumerator RomingRoutine()
    // {
    //     while(state == State.Roming)
    //     {
    //         Vector2 roamPosition = GetRomingPosition();
    //         enemyPathfinding.Moveto(roamPosition); //Pathfinding 클래스의 Moveto함수에 방향벡터 전달
    //         yield return new WaitForSeconds(roamChangeDirFloat); 
    //         // 2초동안 대기했다가 리턴함
    //     }
    // }

    private Vector2 GetRomingPosition()
    {
        timeRoaming = 0f;
        return new Vector2(Random.Range(-1f, 1f),Random.Range(-1f, 1f));
        // x축, y축에 -1f~1f 사이의 랜덤값 벡터 리턴
    }
}
