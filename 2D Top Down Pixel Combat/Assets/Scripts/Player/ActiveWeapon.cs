using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWeapon : Singleton<ActiveWeapon>
{
    // 무기 객체에 접근하여 해당 무기 스크립트를 읽어와 IWeapon인터페이스를 구현시켜주는 스크립트

    public MonoBehaviour CurrentActiveWeapon{ get; private set; }
    // 활성화시킬 무기스크립트를 입력받는다

    private PlayerControls playerControls;
    private float timeBetweenAttacks; //공격 쿨타임 변수

    private bool attackButtonDown, isAttacking = false;

    protected override void Awake() 
    {
        base.Awake();
        playerControls = new PlayerControls();
    }

    private void OnEnable() // 플레이어 컨트롤을 활성화 할 때 사용
    {
        playerControls.Enable();
    }

    private void Start()
    {
        // 마우스 왼쪽 클릭 시 시작이 수행됨
        // =>: 람다식, 연산자 왼쪽이 파라미터, 연산자 오른쪽이 실행문장
        // 즉, _(전달값 X)를 파라미터로하여 연산자 뒤 함수를 실행한 값을 덧붙여준다.
        playerControls.Combat.Attack.started += _ => StartAttacking();
        playerControls.Combat.Attack.canceled += _ => StopAttacking();

        AttackCoolDown();
        //시작하자마자 공격버튼 못누르게 쿨타임 줌
    }

    public void NewWeapon(MonoBehaviour newWeapon)
    {
        // 무기를 바꿀때 기존 선택된 무기 오브젝트를 삭제하고 새로 만들때 사용할 함수
        CurrentActiveWeapon = newWeapon;
        
        AttackCoolDown(); 
        // 무기 사이를 왔다갔다 하면서 쿨타임 없애는거 방지하기 위해 현재 장착 무기의 쿨타임 변수만큼 쿨타임 줌
        
        timeBetweenAttacks = (CurrentActiveWeapon as IWeapon).GetWeaponInfo().weaponCooldown;
        // CurrentActiveWeapon 변수를 IWeapon형식으로 형변환 시킨다.
    }

    public void WeaponNull()
    {
        // 무기를 바꿀때 빈 인벤토리를 선택하면 실행되는 함수
        CurrentActiveWeapon = null;
    }

    private void AttackCoolDown()
    {
        isAttacking = true;
        StopAllCoroutines(); // 해당 스크립트에 구현된 코루틴중 실행되고 있는 모든 코루틴 비활성화(안전용)
        StartCoroutine(TimeBetweenAttacksRoutine());
    }

    private IEnumerator TimeBetweenAttacksRoutine()
    {
        // 공격 쿨타임 가지는 함수
        yield return new WaitForSeconds(timeBetweenAttacks);
        isAttacking = false;
    }

    private void StartAttacking()
    {
        attackButtonDown = true;
    }

    private void StopAttacking()
    {
        attackButtonDown = false;
    }

    private void Update()
    {
        Attack();
    }

    private void Attack()
    {
        if(attackButtonDown && !isAttacking && CurrentActiveWeapon)
        // 쿨다운상태거나 지금 무기를 들고있지 않으면 실행안됨
        {
            AttackCoolDown();
            (CurrentActiveWeapon as IWeapon).Attack();
            // as: 형변환, currentActiveWeapon스크립트가 IWeapon인터페이스에 있는
            // 함수를 구현하였다면 currentActiveWeapon스크립트의 Attack()메서드를 실행시킨다.
        }
    }
}
