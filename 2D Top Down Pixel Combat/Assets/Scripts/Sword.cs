using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private PlayerControls playerControls;
    private Animator myAnimator;

    private void Awake() 
    {
        playerControls = new PlayerControls();
        myAnimator = GetComponent<Animator>();
    }

    private void OnEnable() // 플레이어 컨트롤을 활성화 할 때 사용
    {
        playerControls.Enable();
    }

    private void Start()
    {
        // 마우스 왼쪽 클릭 시 시작이 수행됨
        // =>: 람다식, 연산자 왼쪽이 파라미터, 연산자 오른쪽이 실행문장
        // 즉, _(전달값 X)를 파라미터로 Attack함수를 실행한 값을 started변수와 더하여 started에 저장
        playerControls.Combat.Attack.started += _ => Attack();
    }

    private void Attack()
    {
        // 검 애니메이션 실행
        myAnimator.SetTrigger("Attack");
    }
}
