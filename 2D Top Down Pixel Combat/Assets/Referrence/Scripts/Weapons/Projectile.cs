using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // 화살 발사 스크립트
    [SerializeField] private float moveSpeed = 22f; // 화살속도
    [SerializeField] private GameObject particleOnHitPrefabVFX;
    [SerializeField] private bool isEnemyProjectile = false;
    [SerializeField] private float projectileRange = 10f;
    
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

    public void UpdateProjectileRange(float projectileRange)
    {
        // 해당 투사체의 사정거리를 설정하는 함수
        this.projectileRange = projectileRange;
    }

    public void UpdateMoveSpeed(float moveSpeed)
    {
        // 해당 투사체의 사정거리를 설정하는 함수
        this.moveSpeed = moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
        Indestructible indestructible = other.gameObject.GetComponent<Indestructible>();
        PlayerHealth player = other.gameObject.GetComponent<PlayerHealth>();

        if((enemyHealth || indestructible || player) && !other.isTrigger) 
        // 만약 트리거 콜라이더와 접촉한것이 적, 또는 삭제 가능하지 않은것, 트리거가 아닌 것 이라면..
        {
            if((player && isEnemyProjectile) || (enemyHealth && !isEnemyProjectile))
            // 플레이어가 적탄환에 맞았을 때 또는 적탄환에 적이 적이 맞은게 아니라면
            // 즉, 오직 총알은 플레이어만 닿았을때 삭제되게 함.
            {
                player?.TakeDamage(1, transform);
                Instantiate(particleOnHitPrefabVFX, transform.position, transform.rotation); // 삭제 파티클
                Destroy(gameObject);
            }

            else if(!other.isTrigger && indestructible) // 지형이나 제거 불가능한 오브젝트와 부딪혔을시
            {
                Instantiate(particleOnHitPrefabVFX, transform.position, transform.rotation); // 삭제 파티클
                Destroy(gameObject);
            }

        }
    }

    private void DetectFireDistance()
    {
        // 사정거리를 벗어나면 화살 인스턴스를 삭제시키는 메서드

        if(Vector3.Distance(transform.position, startPosition) > projectileRange)
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
