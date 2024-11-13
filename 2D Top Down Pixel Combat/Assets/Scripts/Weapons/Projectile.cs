using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // 화살 발사 스크립트
    [SerializeField] private float moveSpeed = 22f; // 화살속도
    [SerializeField] private GameObject particleOnHitPrefabVFX;

    private WeaponInfo weaponInfo;
    private Vector3 startPosition;

    private void Start() 
    {
        startPosition = transform.position;
    }

    private void Update() 
    {
        MoveProjectile();
        DetectFireDistance();
    }

    public void UpdateWeaponInfo(WeaponInfo weaponInfo)
    {
        // 다른 클래스에서 해당 무기의 정보(데미지 등..)에 접근하도록 해 주는 함수
        this.weaponInfo = weaponInfo;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
        Indestructible indestructible = other.gameObject.GetComponent<Indestructible>();

        if((enemyHealth || indestructible) && !other.isTrigger) 
        // 만약 트리거 콜라이더와 접촉한것이 적, 또는 삭제 가능하지 않은것, 트리거가 아닌 것 이라면
        {
            Instantiate(particleOnHitPrefabVFX, transform.position, transform.rotation); // 삭제 파티클
            Destroy(gameObject);
        }
    }

    private void DetectFireDistance()
    {
        // 사정거리를 벗어나면 화살 인스턴스를 삭제시키는 메서드

        if(Vector3.Distance(transform.position, startPosition) > weaponInfo.weaponRange)
        // 현재 위치와 처음 쐈을때 위치간의 거리를 사정거리와 비교
        {
            Destroy(gameObject);
        }
    }

    private void MoveProjectile()
    {
        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
        // x축 기준 정면방향으로 이동. 
    }
}
