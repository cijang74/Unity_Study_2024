using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private GameObject slashAnimPrefab;
    [SerializeField] private Transform slashAnimSpawnPoint;
    [SerializeField] private Transform weaponCollider;
    [SerializeField] private float swordAttackCD = .5f;

    private PlayerControls playerControls;
    private Animator myAnimator;
    private PlayerController playerController;
    private ActiveWeapon activeWeapon;
    private bool attackButtonDown, isAttacking = false;

    private GameObject slashAnim;

    private void Awake() 
    {
        playerController = GetComponentInParent<PlayerController>();
        activeWeapon = GetComponentInParent<ActiveWeapon>();
        // 칼의 부모 오브젝트(플레이어)의 컴포넌트에 접근
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
        // 즉, _(전달값 X)를 파라미터로하여 연산자 뒤 함수를 실행한 값을 덧붙여준다.
        playerControls.Combat.Attack.started += _ => StartAttacking();
        playerControls.Combat.Attack.canceled += _ => StopAttacking();
    }

    private void Attack() // 공격 관련 메서드
    {
        if(attackButtonDown == true && !isAttacking)
        {
            isAttacking = true;
            // 검 애니메이션 실행되도록 트리거를 Attack으로 설정
            myAnimator.SetTrigger("Attack");
            weaponCollider.gameObject.SetActive(true);

            // slashAnimPrefab을 특정 위치에 인스턴스화한 객체를 slashAnim에 저장
            slashAnim = Instantiate(slashAnimPrefab, slashAnimSpawnPoint.position, quaternion.identity);
            slashAnim.transform.parent = this.transform.parent; // 인스턴스의 부모 위치 = 검의 부모 위치
            StartCoroutine(AttackCDRoutine());
        }
    }

    private void StartAttacking()
    {
        attackButtonDown = true;
    }

    private void StopAttacking()
    {
        attackButtonDown = false;
    }

    private IEnumerator AttackCDRoutine()
    {
        // 공격 쿨타임함수
        yield return new WaitForSeconds(swordAttackCD); 
        // swordAttackCD초동안 대기했다가 리턴함
        isAttacking = false;
    }

    private void DoneAttackingAnimEvent() // 검 애니메이터에 적용시킬 메서드
    {
        // 애니메이션이 끝나면 공격 범위 트리거를 false인 상태로 놔둘거임.
        weaponCollider.gameObject.SetActive(false);
    }

    private void MouseFollowWithOffset() // 마우스 포인터 위치에 따라 무기 방향 전환
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(playerController.transform.position);
        // 카메라 기준으로의 플레이어의 위치값을 선정하여 저장

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg; 
        // y를 x로 나눈 탄젠트. 0~6값을 2라디안으로 변경

        if(mousePos.x < playerScreenPoint.x) // 마우스 x좌표값이 플레이어의x좌표값보다 작다면
        {
            activeWeapon.transform.rotation = Quaternion.Euler(0,-180,angle); // 검
            weaponCollider.transform.rotation = Quaternion.Euler(0,-180,0); // 검 충돌범위
            // 쿼터니언 이용하여 뒤집고, 각도 변경
        }
        else
        {
            activeWeapon.transform.rotation = Quaternion.Euler(0,0,angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0,0,0);
        }
    }

    public void SwingUpFlipAnimEvent() // 마우스 포인터 방향에 따라 무기 방향 전환
    {
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(-180,0,0);
        // 내렸다가 다시 올릴때는 내릴때 스프라이트를 기준으로 x축으로 뒤집어야함.

        if(playerController.FacingLeft == true)
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    public void SwingDownFlipAnimEvent()
    {
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(0,0,0);
        // 올렸다가 다시 내릴때는 내릴때 스프라이트를 기준으로 x축으로 뒤집어야함.

        if(playerController.FacingLeft == true)
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    private void Update()
    {
        MouseFollowWithOffset();
        Attack();
    }
}
