using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowSpawnPoint;

    private Animator myAnimator;

    readonly int FIRE_HASH = Animator.StringToHash("Fire");
    // 읽기전용 상수 FIRE_HASH는 "FIRE"이라는 문자열을 해쉬형으로 바꾼 value값을 가진 해쉬임
    // 성능 향상할때 쓸 수있으나 굳이굳이 싶기도 함

    private void Awake() 
    {
        myAnimator = GetComponent<Animator>();
    }

    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }
    
    public void Attack()
    {
        myAnimator.SetTrigger(FIRE_HASH);
        GameObject newArrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, ActiveWeapon.Instance.transform.rotation);
        newArrow.GetComponent<Projectile>().UpdateWeaponInfo(weaponInfo);
        // 화살의 인스턴스화
    }
}
