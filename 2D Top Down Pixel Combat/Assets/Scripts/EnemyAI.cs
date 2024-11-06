using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private enum State // 열거자 객체 정의
    {
        Roming
    }

    // class State <- 위 함수와 같은 역할을 함.
    // {
	//      public static final State Roming = new State();
	//      public static final State "" = new State();
    // }

    private State state;
    private EnemyPathfinding enemyPathfinding;

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
        StartCoroutine(RomingRoutine());
    }

    private IEnumerator RomingRoutine()
    {
        while(state == State.Roming)
        {
            Vector2 roamPosition = GetRomingPosition();
            enemyPathfinding.Moveto(roamPosition); //Pathfinding 클래스의 Moveto함수에 방향벡터 전달
            yield return new WaitForSeconds(2f); 
            // 2초동안 대기했다가 리턴함
        }
    }

    private Vector2 GetRomingPosition()
    {
        return new Vector2(Random.Range(-1f, 1f),Random.Range(-1f, 1f));
        // x축, y축에 -1f~1f 사이의 랜덤값 벡터 리턴
    }
}
