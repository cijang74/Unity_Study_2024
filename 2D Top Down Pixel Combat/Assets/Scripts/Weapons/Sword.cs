using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Sword : MonoBehaviour, IWeapon
{
    [SerializeField] private GameObject slashAnimPrefab;
    [SerializeField] private Transform slashAnimSpawnPoint;
    [SerializeField] private float swordAttackCD = .5f;
    [SerializeField] private WeaponInfo weaponInfo;

    private Animator myAnimator;
    private GameObject slashAnim;
    private Transform weaponCollider;

    private void Awake() 
    {
        myAnimator = GetComponent<Animator>();
    }

    private void Start() 
    {
        weaponCollider = PlayerController.Instance.GetWeaponCollider();
        slashAnimSpawnPoint = GameObject.Find("SlashAnimationSpawn").transform; // 나중에 위에처럼 참조해서 바꾸는걸로 수정할것.
    }

    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }

    public void Attack() // 공격 관련 메서드
    {
        // 검 애니메이션 실행되도록 트리거를 Attack으로 설정
        myAnimator.SetTrigger("Attack");
        weaponCollider.gameObject.SetActive(true);

        // slashAnimPrefab을 특정 위치에 인스턴스화한 객체를 slashAnim에 저장
        slashAnim = Instantiate(slashAnimPrefab, slashAnimSpawnPoint.position, quaternion.identity);
        slashAnim.transform.parent = this.transform.parent; // 인스턴스의 부모 위치 = 검의 부모 위치
    }

    private void DoneAttackingAnimEvent() // 검 애니메이터에 적용시킬 메서드
    {
        // 애니메이션이 끝나면 공격 범위 트리거를 false인 상태로 놔둘거임.
        weaponCollider.gameObject.SetActive(false);
    }

    private void MouseFollowWithOffset() // 마우스 포인터 위치에 따라 무기 방향 전환
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);
        // 카메라 기준으로의 플레이어의 위치값을 선정하여 저장

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg; 
        // y를 x로 나눈 탄젠트. 0~6값을 2라디안으로 변경

        if(mousePos.x < playerScreenPoint.x) // 마우스 x좌표값이 플레이어의x좌표값보다 작다면
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0,-180,angle); // 검
            weaponCollider.transform.rotation = Quaternion.Euler(0,-180,0); // 검 충돌범위
            // 쿼터니언 이용하여 뒤집고, 각도 변경
        }
        else
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0,0,angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0,0,0);
        }
    }

    public void SwingUpFlipAnimEvent() // 마우스 포인터 방향에 따라 무기 방향 전환
    {
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(-180,0,0);
        // 내렸다가 다시 올릴때는 내릴때 스프라이트를 기준으로 x축으로 뒤집어야함.

        if(PlayerController.Instance.FacingLeft == true)
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    public void SwingDownFlipAnimEvent()
    {
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(0,0,0);
        // 올렸다가 다시 내릴때는 내릴때 스프라이트를 기준으로 x축으로 뒤집어야함.

        if(PlayerController.Instance.FacingLeft == true)
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    private void Update()
    {
        MouseFollowWithOffset();
    }
}
